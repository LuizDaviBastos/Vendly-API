using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
using ASM.Services.Helpers;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.EntityFrameworkCore;

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
                var sellerExist = await unitOfWork.SellerRepository.GetByEmailAsync(seller.Email);
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
            var sellerExist = await unitOfWork.SellerRepository.MeliSellerExist(accessToken.user_id);
            if (sellerExist == null)
            {
                var entity = unitOfWork.SellerRepository.GetQueryable().Where(x => x.Id == sellerId).Include(x => x.MeliAccounts).FirstOrDefault();
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

            return ($"Já existe uma conta com email {sellerExist.Email.HideEmail()} vinculada a essa conta do mercado livre.", false);
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

        public async Task<LoginResponse> Login(string email, string password)
        {
            var entity = await unitOfWork.SellerRepository.GetQueryable().Where(x => x.Email.ToLower() == email.ToLower() && x.Password == password)
                .Select(x => new
                {
                    Seller = x,
                    HasMeliAccount = x.MeliAccounts.Any()
                }).FirstOrDefaultAsync();

            if(entity != null)
            {
                return new()
                {
                    Message = "Success",
                    Success = true,
                    HasMeliAccount = entity.HasMeliAccount,
                    Data = entity.Seller
                };
            }

            return new()
            {
                Success = false,
                Message = "Email ou senha incorreto"
            };
        }

        public async Task<Seller?> GetSellerInfo(Guid sellerId)
        {
            return await unitOfWork.SellerRepository.GetQueryable().Where(x => x.Id == sellerId)
                .Include(x => x.Messages)
                .Include(x => x.MeliAccounts).FirstOrDefaultAsync();
        }
    }
}
