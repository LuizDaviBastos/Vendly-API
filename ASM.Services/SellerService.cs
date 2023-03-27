using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.EntityFrameworkCore;
using static ASM.Services.Models.MessagesResponse;

namespace ASM.Services
{
    public class SellerService : ISellerService
    {
        private readonly IUnitOfWork unitOfWork;
        public SellerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<(string, bool)> SaveAccount(Seller seller)
        {
            if (!string.IsNullOrEmpty(seller.Email))
            {
                var sellerExist = await unitOfWork.SellerRepository.GetQueryable().FirstOrDefaultAsync(x => x.Email.ToLower() == seller.Email.ToLower());
                if (sellerExist == null)
                {
                    unitOfWork.SellerRepository.Add(seller);
                    await unitOfWork.CommitAsync();
                    return ("success", true);
                }

                return ($"Já existe uma conta com email: ${seller.Email}", false);
            }

            return ($"Email é obrigatório.", false);
        }

        public async Task<(string, bool)> AddMeliAccount(Guid sellerId, AccessToken accessToken)
        {
            var sellerExist = await unitOfWork.SellerRepository.GetQueryable().Select(x => new Seller { Email = x.Email })
                .FirstOrDefaultAsync(x => x.MeliAccounts.Any(x => x.MeliSellerId == accessToken.user_id));
            if (sellerExist == null)
            {
                var entity = unitOfWork.SellerRepository.Get(sellerId);
                MeliAccount account = new()
                {
                    AccessToken = accessToken.access_token,
                    MeliSellerId = accessToken.user_id,
                    RefreshToken = accessToken.refresh_token,
                };
                entity.MeliAccounts.Add(account);
                unitOfWork.SellerRepository.Update(entity);
                await unitOfWork.CommitAsync();

                return ("success", true);
            }

            return ($"Já existe uma conta com email: ${sellerExist.Email} vinculada a essa conta do mercado livre.", false);
        }

        public async Task<(MeliAccount?, bool)> UpdateTokenMeliAccount(AccessToken accessToken)
        {
            var entity = unitOfWork.MeliAccountRepository.GetByMeliSellerId(accessToken.user_id);
            if (entity != null)
            {
                entity.AccessToken = accessToken.access_token;
                entity.RefreshToken = accessToken.refresh_token;

                unitOfWork.MeliAccountRepository.Update(entity);
                await unitOfWork.CommitAsync();

                return (entity, true);
            }

            return (null, false);
        }

        public async Task<SellerMessage?> GetMessageByMeliSellerId(long meliSellerId, MessageType messageType)
        {
            var seller = unitOfWork.SellerRepository.GetQueryable()
                .Select(x => new Seller { Messages = x.Messages, MeliAccounts = x.MeliAccounts})
                .FirstOrDefault(x => x.MeliAccounts.Any(y => y.MeliSellerId == meliSellerId));

            if (seller != null)
            {
                return seller?.Messages?.FirstOrDefault(x => x.Type == messageType);
            }

            return null;
        }
    }
}
