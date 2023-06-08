using ASM.Data.Enums;
using ASM.Services;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ASM.PreFunction.Functions
{
    public class ProcessPayments
    {
        private readonly IStorageService storageService;
        private readonly IMeliService meliService;
        private readonly IMepaService mepaService;
        private readonly ISellerService sellerService;
        private readonly FcmService fcmService;
        private readonly PaymentService paymentService;

        public ProcessPayments(IStorageService storageService, IMeliService meliService, ISellerService sellerService, IMepaService mepaService, FcmService fcmService, PaymentService paymentService)
        {
            this.storageService = storageService;
            this.meliService = meliService;
            this.sellerService = sellerService;
            this.mepaService = mepaService;
            this.fcmService = fcmService;
            this.paymentService = paymentService;
        }

        [FunctionName("ProcessPayments")]
        public async Task Run([QueueTrigger("process-payments")] NotificationTriggerPayments notification, ILogger log)
        {
            if (notification.type == "payment" && notification.action == "payment.created")
            {
                if (string.IsNullOrEmpty(notification?.data?.id))
                {
                    log.LogError("Não foi possivel processar o pagamento. Id do pagamento não encontrado.");
                    return;
                }

                var paymentInformations = await mepaService.GetPaymentInformation(notification.data.id);
                if (!paymentInformations.Success ?? true)
                {
                    log.LogError(paymentInformations.Message);
                    return;
                }

                if (paymentInformations.status == "approved" || paymentInformations.status == "authorized")
                {
                    var sellerId = paymentInformations.metadata.sellerId;
                    var userPaymentId = paymentInformations.metadata.userPaymentId;
                    if (!sellerId.HasValue || sellerId == Guid.Empty || !userPaymentId.HasValue)
                    {
                        log.LogError("Falha ao processar o pagamento. SellerId ou UserPaymentId não encontrado.");
                        return;
                    }

                    var status = await paymentService.PaymentProcessed(userPaymentId.Value);
                    if (status) return;

                    var seller = await sellerService.GetSellerOnly(sellerId.Value);
                    if(seller == null)
                    {
                        log.LogError($"Falha ao processar o pagamento. Seller não encontrado com o Id: {sellerId}");
                        return;
                    }

                    var lastPayment = paymentInformations.date_approved?.ToUniversalTime() ?? DateTime.UtcNow;
                    DateTime? createdDate = paymentInformations.date_created;
                    decimal? price = paymentInformations.transaction_amount;

                    await paymentService.SubscribeAgainRoutineAsync(sellerId.Value, lastPayment, createdDate, price, userPaymentId.Value);
                }
            }
        }
    }
}
