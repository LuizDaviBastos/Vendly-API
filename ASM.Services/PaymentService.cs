using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
using ASM.Services.Helpers;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using ASM.Services.Models.Mepa;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ASM.Services
{
    public class PaymentService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;
        private readonly ISettingsService settingsService;
        private readonly UserManager<Seller> userManager;
        private readonly IMepaService mepaService;
        private readonly FcmService fcmService;
        private readonly PushNotificationService pushNotificationService;

        public PaymentService(IUnitOfWork unitOfWork, IEmailService emailService, ISettingsService settingsService, UserManager<Seller> userManager, IMepaService mepaService, FcmService fcmService, PushNotificationService pushNotificationService)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
            this.settingsService = settingsService;
            this.userManager = userManager;
            this.mepaService = mepaService;
            this.fcmService = fcmService;
            this.pushNotificationService = pushNotificationService;
        }
        public async Task<PaymentLinkResponse> GetNewPaymentLink(Guid sellerId, Guid subscriptionPlanId, bool isBinary = false)
        {
            Guid userPaymentId = Guid.NewGuid();
            SubscriptionPlan subscription = await GetSubscriptionPlanAsync(x => x.Id == subscriptionPlanId);
            if (subscription != null)
            {
                var mepaLink = await mepaService.CreatePreference(sellerId, userPaymentId, subscription, isBinary);
                await AddOrUpdatePaymentHistory(sellerId, subscription.Price.Value, DateTime.UtcNow, userPaymentId, subscriptionPlanId, PaymentStatus.Pending);
                mepaLink.Success = true;
                return mepaLink;
            }
            return new PaymentLinkResponse
            {
                Success = false,
                Message = "Plano não encontrado."
            };
        }
        public async Task<PaymentInformation?> GetPaymentInformation(Guid sellerId)
        {
            return await unitOfWork.BillingInformationRepository.GetQueryable().Where(x => x.SellerId == sellerId)
                .Include(x => x.SubscriptionPlan).FirstOrDefaultAsync();
        }

        public async Task<SubscriptionPlan> GetSubscriptionPlanAsync(Expression<Func<SubscriptionPlan, bool>> expression)
        {
            return await unitOfWork.SubscriptionPlanRepository.GetQueryable().FirstOrDefaultAsync(expression);
        }

        public async Task<List<SubscriptionPlan>> GetSubscriptionPlanAsync(bool includeFreePlans = false)
        {
            return await unitOfWork.SubscriptionPlanRepository.GetQueryable().Where(x => x.IsFree == false && x.IsFree == includeFreePlans).ToListAsync();
        }

        public async Task<bool> PaymentProcessed(Guid userPaymentId)
        {
            var payment = await unitOfWork.PaymentHistoryRepository.GetQueryable().FirstOrDefaultAsync(x => x.UserPaymentId == userPaymentId);
            return payment != null && payment.Status == PaymentStatus.Success;
        }

        public async Task<PaymentHistory> AddOrUpdatePaymentHistory(Guid sellerId, decimal price, DateTime createdDate, Guid userPaymentId, Guid subscriptionPlanId, PaymentStatus status, string metaData = default)
        {
            var entity = await GetPaymentHistoryItem(sellerId, userPaymentId);
            if (entity == null)
            {
                entity = new PaymentHistory
                {
                    SellerId = sellerId,
                    CreatedDate = createdDate,
                    Price = price,
                    UserPaymentId = userPaymentId,
                    SubscriptionPlanId = subscriptionPlanId,
                    Status = status
                };
                if (metaData != default) entity.MetaData = metaData;
                unitOfWork.PaymentHistoryRepository.Add(entity);
            }
            else
            {
                entity.Price = price;
                entity.MetaData = metaData;
                entity.SubscriptionPlanId = subscriptionPlanId;
                entity.Status = status;
                if (metaData != default) entity.MetaData = metaData;
                unitOfWork.PaymentHistoryRepository.Update(entity);
            }

            await unitOfWork.CommitAsync();
            return entity;
        }

        public async Task SubscribeAgainRoutineAsync(Guid sellerId, DateTime? dateApproved, DateTime? dateCreated, decimal? price, Guid userPaymentId)
        {
            var currentBilling = await GetPaymentInformation(sellerId);
            var paymentHistory = await GetPaymentHistoryItem(sellerId, userPaymentId);

            DateTime baseDate = DateTime.UtcNow;
            var lastPayment = dateApproved?.ToUniversalTime() ?? DateTime.UtcNow;
            DateTime createdDate = dateCreated?.ToUniversalTime() ?? DateTime.UtcNow;

            if (currentBilling?.ExpireIn > DateTime.UtcNow)
            {
                baseDate = currentBilling.ExpireIn.Value;
            }

            await UpdateBillingInformation(sellerId, baseDate, lastPayment, paymentHistory.SubscriptionPlanId.Value);
            await AddOrUpdatePaymentHistory(sellerId, price.Value, createdDate, userPaymentId, paymentHistory.SubscriptionPlanId.Value, PaymentStatus.Success);
            await pushNotificationService.SendPushNotificationAsync(sellerId, "Vendly", "Pagamento efetuado!");
        }

        public async Task<ExpiredResponse> ExpirateDateValid(Guid sellerId, bool searchPayment = true)
        {
            ExpiredResponse response = new();
            response.IsFreePeriod = false;
            response.NotExpired = false;

            var paymentInfo = await GetPaymentInformation(sellerId);
            if (paymentInfo == null) return response;

            var notExpirated = (paymentInfo.ExpireIn > DateTime.UtcNow);
            response.NotExpired = notExpirated;
            response.IsFreePeriod = paymentInfo.SubscriptionPlan?.IsFree ?? false;

            if (!notExpirated && searchPayment)
            {
                var payments = await mepaService.GetLastPayments(sellerId);
                var paid = payments.Results.FirstOrDefault(x => x.DateApproved > paymentInfo.ExpireIn);
                if (paid != null && (paid.Status == "approved" || paid.Status == "authorized"))
                {
                    Guid userPaymentId = Guid.Parse(paid.Metadata["user_payment_id"].ToString());
                    await SubscribeAgainRoutineAsync(sellerId, paid.DateApproved, paid.DateCreated, paid.TransactionAmount, userPaymentId);
                    response.NotExpired = true;
                    return response;
                }
            }

            return response;
        }

        public async Task<ExpiredResponse> ExpirateDateValid(long meliSellerId, bool searchPayment = true)
        {
            ExpiredResponse response = new();
            response.IsFreePeriod = false;
            response.NotExpired = false;

            Guid? sellerId = await unitOfWork.MeliAccountRepository.GetQueryable()
                .Where(x => x.MeliSellerId == meliSellerId).Include(x => x.Seller)
                .Select(x => x.Seller.Id)
                .FirstOrDefaultAsync();
            if (!sellerId.HasValue) return response;

            return await ExpirateDateValid(sellerId.Value, searchPayment);
        }

        public async Task<List<PaymentHistory>> GetPaymentHistory(Guid sellerId)
        {
            return await unitOfWork.PaymentHistoryRepository.GetQueryable()
                .Include(x => x.SubscriptionPlan).Where(x => x.SellerId == sellerId).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        private async Task<PaymentHistory?> GetPaymentHistoryItem(Guid sellerId, Guid userPaymentId)
        {
            return await unitOfWork.PaymentHistoryRepository.GetQueryable().FirstOrDefaultAsync(x => x.UserPaymentId == userPaymentId && x.SellerId == sellerId);
        }

        private async Task<PaymentInformation> UpdateBillingInformation(Guid sellerId, DateTime baseDate, DateTime lastPayment, Guid subscribeId)
        {
            var subscribePlan = await GetSubscriptionPlanAsync(x => x.Id == subscribeId);
            var billing = await unitOfWork.BillingInformationRepository.GetQueryable().Where(x => x.SellerId == sellerId).FirstOrDefaultAsync();
            if (billing == null)
            {
                billing = new PaymentInformation
                {
                    SellerId = sellerId,
                    LastPayment = lastPayment
                };

                billing.SetSubscription(subscribePlan, baseDate);
                unitOfWork.BillingInformationRepository.Add(billing);
            }
            else
            {
                billing.LastPayment = lastPayment;
                billing.SetSubscription(subscribePlan, baseDate);
                unitOfWork.BillingInformationRepository.Update(billing);
            }
            await unitOfWork.CommitAsync();
            return billing;
        }
    }
}
