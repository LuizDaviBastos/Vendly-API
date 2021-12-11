using ASM.Core.Repositories;
using ASM.Data.Entities;
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
        private readonly IRepository<Seller> sellerRepository;
        private readonly AsmConfiguration asmConfiguration;

        public MeliService(IRepository<Seller> sellerRepository, AsmConfiguration asmConfiguration)
        {
            restClient = new RestClient("https://api.mercadolibre.com");
            this.sellerRepository = sellerRepository;
            this.asmConfiguration = asmConfiguration;
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

                sellerRepository.AddOrUpdate(new Seller
                {
                    AccessToken = accessToken.access_token,
                    SellerId = accessToken.user_id,
                    RefreshToken = accessToken.refresh_token
                });
            }
            else
            {
                accessToken.Success = false;
                accessToken.Message = result.Content;
            }

            return accessToken;
        }

        public async Task<AccessToken> RefreshAccessTokenAsync(string refreshToken, long sellerId)
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

        public async Task<Order> GetOrderDetailsAsync(NotificationTrigger notification, bool tryAgain = true)
        {
            var order = new Order();
            order.Success = false;

            if (!SetAccessToken(notification.user_id, out Seller seller)) return order;

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
                return await RefreshTokenAndTryAgain(seller.RefreshToken, seller.SellerId, async () => await GetOrderDetailsAsync(notification, false));
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
            if (!SetAccessToken(sendMessage.SellerId, out Seller seller)) return false;

            RestRequest request = new RestRequest($"/messages/packs/{sendMessage.PackId}/sellers/{sendMessage.SellerId}", Method.POST);
            request.AddHeader("Authorization", $"Bearer {this.accessToken}")
            .AddParameter("application_id", asmConfiguration.AppId)
            .AddJsonBody(new
            {
                from = new
                {
                    user_id = sendMessage.SellerId
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
                return await RefreshTokenAndTryAgain(seller.RefreshToken, seller.SellerId, async () => await SendMessageToBuyerAsync(sendMessage, false));
            }

            return false;
        }

        public async Task<bool> IsFirstSellerMessage(SendMessage sendMessage, bool tryAgain = true)
        {
            if (!SetAccessToken(sendMessage.SellerId, out Seller seller)) return false;

            RestRequest restRequest = new RestRequest($"/messages/packs/{sendMessage.PackId}/sellers/{sendMessage.SellerId}", Method.GET);
            restRequest.AddHeader("Authorization", $"Bearer {this.accessToken}")
                .AddParameter("mark_as_read", false);

            var result = await restClient.ExecuteAsync<MessagesResponse>(restRequest);
            if (result.IsSuccessful)
            {
                return !result.Data.messages.Any(x => x.from.user_id == sendMessage.SellerId);
            }
            else if(tryAgain && result.StatusCode == HttpStatusCode.Forbidden || result.StatusCode == HttpStatusCode.BadRequest || result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return await RefreshTokenAndTryAgain(seller.RefreshToken, seller.SellerId, async () => await IsFirstSellerMessage(sendMessage, false));
            }

            throw new Exception(result.Content);
        }

        public async Task<SellerInfo> GetSellerInfo(string accessToken, bool tryAgain = true)
        {
            if (!SetAccessToken(accessToken, out Seller seller)) throw new Exception($"SetAccessToken Error{(seller.Id == 0 ? ". Seller not found" : "")}");

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
                return await RefreshTokenAndTryAgain(seller.RefreshToken, seller.SellerId, async () => await GetSellerInfo(this.accessToken, false));
            }
            
            throw new Exception(result.Content);
        }

        private async Task<TResult> RefreshTokenAndTryAgain<TResult>(string refreshToken, long sellerId, Func<Task<TResult>> func)
        {
            var accessToken = await this.RefreshAccessTokenAsync(refreshToken, sellerId);
            this.accessToken = accessToken.access_token;

            sellerRepository.AddOrUpdate(new Seller
            {
                AccessToken = accessToken.access_token,
                SellerId = accessToken.user_id,
                RefreshToken = accessToken.refresh_token
            });

            return await func();
        }

        private bool SetAccessToken(long sellerId, out Seller seller)
        {   
            seller = sellerRepository.GetQueryable(x => x.SellerId == sellerId).Select(x => new Seller 
            { 
                AccessToken = x.AccessToken, 
                RefreshToken = x.RefreshToken
            }).FirstOrDefault() ?? new Seller();
            
            if (seller == null) return false;

            seller.SellerId = sellerId;
            this.accessToken = seller.AccessToken;

            return true;
        }


        private bool SetAccessToken(string accessToken, out Seller seller)
        {
            seller = sellerRepository.GetQueryable(x => x.AccessToken == accessToken).Select(x => new Seller
            {
                RefreshToken = x.RefreshToken,
                SellerId = x.SellerId,
                AccessToken = x.AccessToken
            }).FirstOrDefault();

            if (seller == null) return false;

            seller.SellerId = seller.SellerId;
            this.accessToken = seller.AccessToken;

            return true;
        }

    }


}
