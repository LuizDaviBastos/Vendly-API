﻿using ASM.Data.Entities;
using ASM.Data.Interfaces;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using RestSharp;
using System.Net;

namespace ASM.Services
{
    public class MeliService : IMeliService
    {
        private string accessToken = string.Empty;
        private readonly RestClient restClient;
        private readonly IUnitOfWork unitOfWork;
        private readonly AsmConfiguration asmConfiguration;
        private readonly ISellerService sellerService;

        public MeliService(IUnitOfWork unitOfWork, AsmConfiguration asmConfiguration, ISellerService sellerService)
        {
            restClient = new RestClient("https://api.mercadolibre.com");
            this.unitOfWork = unitOfWork;
            this.asmConfiguration = asmConfiguration;
            this.sellerService = sellerService;
        }

        public string GetAuthUrl(string countryId)
        {
            var authUrl = $"{{0}}/authorization?response_type=code&client_id={asmConfiguration.AppId}&redirect_uri={asmConfiguration.RedirectUrl}";
            return string.Format(authUrl, asmConfiguration.Countries?[countryId.ToUpper()]);
        }

        public async Task<AccessToken> GetAccessTokenAsync(string code)
        {
            AccessToken accessToken = new AccessToken();
            accessToken.Success = false;

            RestRequest request = new RestRequest("/oauth/token", Method.POST);
            request.AddJsonBody(new
            {
                grant_type = "authorization_code",
                client_id = asmConfiguration.AppId,
                client_secret = asmConfiguration.SecretKey,
                code = code,
                redirect_uri = asmConfiguration.RedirectUrl
            });

            var result = await restClient.ExecuteAsync<AccessToken>(request);
            if (result.IsSuccessful)
            {
                accessToken = result.Data;
                accessToken.Success = true;
            }
            else
            {
                accessToken.Success = false;
                accessToken.Message = result.Content;
            }

            return accessToken;
        }

        public async Task<Order> GetOrderDetailsAsync(NotificationTrigger notification, bool tryAgain = true)
        {
            var order = new Order();
            order.Success = false;

            if (!SetAccessToken(notification.user_id, out MeliAccount? meliAccount))
            {
                order.Message = "Seller not found";
                return order;
            }

            RestRequest request = new RestRequest($"/orders/{notification.OrderId}", Method.GET);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            var result = await restClient.ExecuteAsync<Order>(request);
            if (result.IsSuccessful)
            {
                order = result.Data;
                order.Success = true;
            }
            else if (tryAgain && result.StatusCode == HttpStatusCode.Forbidden || result.StatusCode == HttpStatusCode.BadRequest)
            {
                return await RefreshTokenAndTryAgain(meliAccount!.RefreshToken, async () => await GetOrderDetailsAsync(notification, false));
            }
            else
            {
                order.Success = false;
                order.Message = result.Content;
            }

            return order;
        }

        public async Task<bool> SendMessageToBuyerAsync(SendMessage sendMessage, bool tryAgain = true)
        {
            if (!SetAccessToken(sendMessage.MeliSellerId, out MeliAccount? meliAccount)) return false;

            RestRequest request = new RestRequest($"/messages/packs/{sendMessage.PackId}/sellers/{sendMessage.MeliSellerId}", Method.POST);
            request.AddHeader("Authorization", $"Bearer {this.accessToken}");
            request.AddQueryParameter("tag", "post_sale")
            .AddJsonBody(new
            {
                from = new
                {
                    user_id = sendMessage.MeliSellerId
                },
                to = new
                {
                    user_id = sendMessage.BuyerId
                },
                text = sendMessage.Message
            });

            var result = await restClient.ExecuteAsync(request);
            if (result.IsSuccessful)
            {
                return true;
            }
            else if (tryAgain && result.StatusCode == HttpStatusCode.Forbidden || result.StatusCode == HttpStatusCode.BadRequest)
            {
                return await RefreshTokenAndTryAgain(meliAccount!.RefreshToken, async () => await SendMessageToBuyerAsync(sendMessage, false));
            }

            return false;
        }

        public async Task<bool> IsFirstSellerMessage(SendMessage sendMessage, bool tryAgain = true)
        {
            if (!SetAccessToken(sendMessage.MeliSellerId, out MeliAccount meliAccount)) return false;

            RestRequest restRequest = new RestRequest($"/messages/packs/{sendMessage.PackId}/sellers/{sendMessage.MeliSellerId}", Method.GET);
            restRequest.AddHeader("Authorization", $"Bearer {this.accessToken}")
                .AddParameter("tag", "post_sale");

            var result = await restClient.ExecuteAsync<MessagesResponse>(restRequest);
            if (result.IsSuccessful)
            {
                return !result.Data.messages.Any(x => x.from.user_id == sendMessage.MeliSellerId);
            }
            else if (tryAgain && result.StatusCode == HttpStatusCode.Forbidden || result.StatusCode == HttpStatusCode.BadRequest || result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return await RefreshTokenAndTryAgain(meliAccount.RefreshToken, async () => await IsFirstSellerMessage(sendMessage, false));
            }

            throw new Exception(result.Content);
        }

        public async Task<SellerInfo> GetSellerInfoByMeliSellerId(long meliSellerId, bool tryAgain = true)
        {
            if (!SetAccessToken(meliSellerId, out MeliAccount? meliAccount)) throw new Exception($"SetAccessToken Error{(meliAccount == null || meliAccount?.Id == Guid.Empty ? ". Seller not found" : "")}");

            RestRequest restRequest = new RestRequest($"/users/me", Method.GET);
            restRequest.AddHeader("Authorization", $"Bearer {this.accessToken}");

            var result = await restClient.ExecuteAsync<SellerInfo>(restRequest);
            if (result.IsSuccessful)
            {
                result.Data.Success = true;
                return result.Data;
            }
            else if (tryAgain && result.StatusCode == HttpStatusCode.Forbidden || result.StatusCode == HttpStatusCode.BadRequest || result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return await RefreshTokenAndTryAgain(meliAccount!.RefreshToken, async () => await GetSellerInfoByMeliSellerId(meliSellerId, false));
            }

            throw new Exception(result.Content);
        }

        private async Task<TResult> RefreshTokenAndTryAgain<TResult>(string refreshToken, Func<Task<TResult>> func)
        {
            var accessToken = await this.RefreshAccessTokenAsync(refreshToken);
            this.accessToken = accessToken.access_token;

            await sellerService.UpdateTokenMeliAccount(accessToken);
            return await func();
        }

        private async Task<AccessToken> RefreshAccessTokenAsync(string refreshToken)
        {
            AccessToken accessToken = new AccessToken();
            accessToken.Success = false;

            RestRequest request = new RestRequest("/oauth/token", Method.POST);
            request.AddJsonBody(new
            {
                grant_type = "refresh_token",
                client_id = asmConfiguration.AppId,
                client_secret = asmConfiguration.SecretKey,
                refresh_token = refreshToken,
            });

            var result = await restClient.ExecuteAsync<AccessToken>(request);
            if (result.IsSuccessful)
            {
                accessToken = result.Data;
                accessToken.Success = true;
            }
            else
            {
                accessToken.Success = false;
                accessToken.Message = result.Content;
            }

            return accessToken;
        }

        private bool SetAccessToken(long meliSellerId, out MeliAccount? meliAccount)
        {
            meliAccount = unitOfWork.MeliAccountRepository.GetByMeliSellerId(meliSellerId);

            if (meliAccount == null) return false;

            this.accessToken = meliAccount.AccessToken;
            return true;
        }
    }


}
