﻿using ASM.Data.Entities;
using ASM.Services.Models;

namespace ASM.Services.Interfaces
{
    public interface IMeliService
    {
        public string GetAuthUrl(string countryId, StateUrl state);
        public Task<AccessToken> GetAccessTokenAsync(string code);
        public Task<Order> GetOrderDetailsAsync(NotificationTrigger notification, bool tryAgain = true);
        public Task<bool> SendMessageToBuyerAsync(SendMessage sendMessage, bool tryAgain = true);
        public Task<bool> IsFirstSellerMessage(SendMessage sendMessage, bool tryAgain = true);
        public Task<SellerInfo> GetMeliSellerInfo(long sellerId, bool tryAgain = true);
        public Task<IList<SaveAttachmentResponse>> SaveAttachments(Guid sellerId, SellerMessage sellerMessage);
        public Task<OrderFeedback> GetFeedbackDetailsAsync(NotificationTrigger notification, bool tryAgain = true);
    }
}
