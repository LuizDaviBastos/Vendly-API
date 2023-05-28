using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Services.Models;

namespace ASM.Services.Interfaces
{
    public interface ISellerService
    {
        Task<(string, bool)> SaveAccount(Seller seller);
        Task<(string, bool)> AddMeliAccount(Guid sellerId, AccessToken accessToken);
        Task<(MeliAccount?, bool)> UpdateTokenMeliAccount(AccessToken accessToken);
        Task<SellerMessage?> GetMessageByMeliSellerId(long meliSellerId, MessageType messageType);
        Task<Seller?> GetSellerAndMeliAccounts(Guid sellerId);
        Task<bool> HasMeliAccount(Guid sellerId);
        Task<(string, bool)> ConfirmEmailAsync(Guid sellerId, string code);
        Task<(string, bool)> SendEmailConfirmationCode(Guid sellerId);
        public Task<SellerOrder> GetSellerOrder(Guid sellerId, long meliSellerId, long orderId, MessageType type);
        public Task<SellerOrder?> GetSellerOrder(long meliSellerId, long orderId, MessageType type);
        public Task<SellerOrder> SaveOrUpdateOrderMessageStatus(Guid sellerId, long meliSellerId, long orderId, MessageType type, bool status);
        Task DeleteAccount(Seller user);
        public Task<(string, bool)> SendEmailRecoveryPassword(string email);
        public Task<(string, bool)> RecoveryPassword(Guid sellerId, string code, string newPassword);
        public Task<Seller?> GetSellerOnly(Guid sellerId);
        public Task<PaymentInformation> UpdateBillingInformation(Guid sellerId, BillingStatus status, DateTime expireIn, DateTime lastPayment);
        public Task<PaymentHistory> AddPaymentHistory(Guid sellerId, decimal price, DateTime createdDate, string metaData = null);
    }
}
