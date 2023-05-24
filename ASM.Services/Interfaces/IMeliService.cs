using ASM.Data.Entities;
using ASM.Services.Models;

namespace ASM.Services.Interfaces
{
    public interface IMeliService
    {
        public Task<string> GetAuthUrl(string countryId, StateUrl? state);
        public Task<AccessToken> GetAccessTokenAsync(string code);
        public Task<Order> GetOrderDetailsAsync(NotificationTrigger notification, bool tryAgain = true);
        public Task<(bool, string)> SendMessageToBuyerAsync(SendMessage sendMessage, bool tryAgain = true);
        public Task<bool> IsFirstSellerMessage(SendMessage sendMessage, bool tryAgain = true);
        public Task<SellerInfo> GetMeliSellerInfo(long sellerId, bool tryAgain = true);
        public Task<IList<SaveAttachmentResponse>> SaveAttachments(Guid sellerId, SellerMessage sellerMessage);
        public Task<OrderFeedback> GetFeedbackDetailsAsync(NotificationTrigger notification, bool tryAgain = true);
        public Task<ShipmentResponse> GetShipmentDetails(NotificationTrigger notification, bool tryAgain = true);
        public Task<Dictionary<bool, (MeliAccount, string)>> RevokeMeliAccounts(IEnumerable<MeliAccount> meliAccounts, bool tryAgain = true);
        public Task<(bool, (MeliAccount, string))> RevokeMeliAccount(MeliAccount meli, bool tryAgain = true);
        public Task<ShipmentResponse> UpdateShipmentStatus(long meliSellerId, long shipmentId, string shipmentStatus, string shipmentSubStatus, bool tryAgain = true);
        public Task<ShipmentResponseByOrder> GetShipmentDetailsByOrder(NotificationTrigger notification, bool tryAgain = true);
    }
}
