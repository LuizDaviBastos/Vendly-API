﻿using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
using ASM.Services.Helpers;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASM.Services
{
    public class SellerService : ISellerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;
        private readonly ISettingsService settingsService;
        private readonly UserManager<Seller> userManager;

        public SellerService(IUnitOfWork unitOfWork, IEmailService emailService, ISettingsService settingsService, UserManager<Seller> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
            this.settingsService = settingsService;
            this.userManager = userManager;
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
            if (message != null)
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
                string code = Utils.GetRandomCode().ToString();
                var settings = await settingsService.GetAppSettings();

                string body, title;

                if (!entity.EmailConfirmed)
                {
                    title = "Complete seu cadastro";
                    body = settings.HtmlEmailCodeNewUser.Replace(@"{{codigo}}", code);
                }
                else
                {
                    title = "Confirme seu email";
                    body = settings.HtmlEmailCode.Replace(@"{{codigo}}", code);
                }

                await emailService.SendEmail(entity.Email, body, title);

                entity.ConfirmationCode = code;
                unitOfWork.SellerRepository.Update(entity);
                await unitOfWork.CommitAsync();
                return ("", true);
            }
            return ("Usuário não encontrado", false);
        }

        public async Task<(string, bool)> ConfirmEmailAsync(Guid sellerId, string code)
        {
            var entity = unitOfWork.SellerRepository.Get(sellerId);
            if (entity != null)
            {
                if (entity.ConfirmationCode == code)
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

        public async Task<(string, bool)> SendEmailRecoveryPassword(string email)
        {
            var entity = await unitOfWork.SellerRepository.GetByEmailAsync(email);
            if (entity == null) return ("Usuario não encontrado.", false);
            else if(!entity.EmailConfirmed) return ("Seu e-mail não foi confirmado.", false);

            var settings = await settingsService.GetAppSettings();

            string body, title;
            string code = await userManager.GeneratePasswordResetTokenAsync(entity);
            string encodedCode = Utils.GetBase64String(code);

            string url = $"{settings.UrlBaseApi}/api/auth/RedirectConfirmRecoveryPassword?sellerId={entity.Id}&code={encodedCode}";
            title = "Recupere sua senha";
            body = settings.HtmlRecoveryPassword.Replace("{{name}}", entity.FirstName).Replace("{{url}}", url);

            await emailService.SendEmail(entity.Email, body, title);

            return ("", true);
        }

        public async Task<(string, bool)> RecoveryPassword(Guid sellerId, string encodedCode, string newPassword)
        {
            var entity = unitOfWork.SellerRepository.Get(sellerId);
            if (entity == null) return ("Usuario não encontrado.", false);
            else if (!entity.EmailConfirmed) return ("Seu e-mail não foi confirmado.", false);

            string code = Utils.GetFromBase64String(encodedCode);
            IdentityResult? result = await userManager.ResetPasswordAsync(entity, code, newPassword);
            if(!result?.Succeeded ?? false) return ("Token de recuperação inválido.", false);

            return ("", true);
        }

        public async Task<SellerOrder> GetSellerOrder(Guid sellerId, long meliSellerId, long orderId, MessageType type)
        {
            var order = await unitOfWork.SellerOrderRepository.GetQueryable()
               .Where(x => x.MeliSellerId == meliSellerId && x.OrderId == orderId).FirstOrDefaultAsync();

            if (order == null)
            {
                order = await SaveOrUpdateOrderMessageStatus(sellerId, meliSellerId, orderId, type, false);
            }

            return order;
        }

        public async Task<SellerOrder?> GetSellerOrder(long meliSellerId, long orderId, MessageType type)
        {
            SellerOrder? order = await unitOfWork.SellerOrderRepository.GetQueryable()
               .Where(x => x.MeliSellerId == meliSellerId && x.OrderId == orderId).FirstOrDefaultAsync();

            return order;
        }

        public async Task<SellerOrder> SaveOrUpdateOrderMessageStatus(Guid sellerId, long meliSellerId, long orderId, MessageType type, bool status)
        {
            var order = await unitOfWork.SellerOrderRepository.GetQueryable()
                .Where(x => x.MeliSellerId == meliSellerId && x.OrderId == orderId).FirstOrDefaultAsync();
            if (order == null)
            {
                order = new SellerOrder
                {
                    OrderId = orderId,
                    MeliSellerId = meliSellerId,
                    SellerId = sellerId
                };
                UpdateMessageStatus(order, type, status);
                unitOfWork.SellerOrderRepository.Add(order);
            }
            else
            {
                UpdateMessageStatus(order, type, status);
                unitOfWork.SellerOrderRepository.Update(order);
            }

            await unitOfWork.CommitAsync();
            return order;
        }

        private void UpdateMessageStatus(SellerOrder order, MessageType type, bool status)
        {
            switch (type)
            {
                case MessageType.AfterSeller:
                    order.AfterSellerMessageStatus = status;
                    break;
                case MessageType.Shipping:
                    order.ShippingMessageStatus = status;
                    break;
                case MessageType.Delivered:
                    order.DeliveredMessageStatus = status;
                    break;
                default:
                    break;
            }
        }

        public async Task DeleteAccount(Seller user)
        {
            unitOfWork.SellerRepository.Delete(user.Id);
            await unitOfWork.CommitAsync();
        }
    }
}
