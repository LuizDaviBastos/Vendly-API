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
        Task<Seller?> GetSellerInfo(Guid sellerId);
        Task<bool> HasMeliAccount(Guid sellerId);
        Task<(string, bool)> ConfirmEmailAsync(Guid sellerId, string code);
        Task<(string, bool)> SendEmailConfirmationCode(Guid sellerId);
    }
}
