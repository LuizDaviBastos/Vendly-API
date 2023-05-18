using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
using ASM.Services.Helpers;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Servers;
using System.Security.Cryptography.X509Certificates;

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
            var meliAccount = await unitOfWork.MeliAccountRepository.GetQueryable().Include(x => x.Messages)
                .FirstOrDefaultAsync(x => x.MeliSellerId == meliSellerId);
            var message = meliAccount?.Messages?.FirstOrDefault(x => x.Type == messageType);
            if(message != null)
            {
                var attachments = await unitOfWork.MessageRepository.GetAttachments(message.Id);
                message.Attachments = attachments;
            }
            return message;
        }

        public async Task<Seller?> GetSellerInfo(Guid sellerId)
        {
            return await unitOfWork.SellerRepository.GetQueryable().Where(x => x.Id == sellerId)
                .Include(x => x.MeliAccounts).FirstOrDefaultAsync();
        }

        public async Task<bool> HasMeliAccount(Guid sellerId)
        {
            return await unitOfWork.MeliAccountRepository.GetQueryable().Where(x => x.SellerId == sellerId).AnyAsync();
        }

        public async Task<(string, bool)> SendEmailConfirmationCode(Guid sellerId)
        {
            var entity = unitOfWork.SellerRepository.Get(sellerId);
            if (entity != null)
            {
                //TODO
                // 1 - generate code
                // 2 - save code in database
                // 2 - send confirmation code to email
                entity.ConfirmationCode = "123456";
                unitOfWork.SellerRepository.Update(entity);
                await unitOfWork.CommitAsync();
                return ("", true);
            }
            return ("Usuário não encontrado", false);
        }

        public async Task<(string, bool)> ConfirmEmailAsync(Guid sellerId, string code)
        {
            var entity = unitOfWork.SellerRepository.Get(sellerId);
            if(entity != null)
            {
                if(entity.ConfirmationCode == code)
                {
                    entity.EmailConfirmed = true;
                    entity.ConfirmationCode = null;
                    unitOfWork.SellerRepository.Update(entity);
                    await unitOfWork.CommitAsync();
                    return ("", true);
                } 
                else
                {
                    return ("Código inválido", false);
                }
                
            }
            return ("Usuário não encontrado", false);
        }
    }
}
