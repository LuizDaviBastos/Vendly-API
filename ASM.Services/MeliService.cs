using ASM.Core.Repositories;
using ASM.Data.Entities;
using ASM.Services.Helpers;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using RestSharp;

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

       /* private Dictionary<string, string> Countries = new Dictionary<string, string>(new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("AR","https://auth.mercadolibre.com.ar"),
            new KeyValuePair<string, string>("BR","https://auth.mercadolivre.com.br"),
        });*/

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

            return accessToken;
        }

        public async Task<Order> GetOrderDetailsAsync(NotificationTrigger notification, bool tryAgain = true)
        {
            var order = new Order();
            order.Success = false;

            var seller = sellerRepository.GetQueryable(x => x.SellerId == notification.user_id).FirstOrDefault();
            if (seller == null) return order;

            this.accessToken = seller.AccessToken;

            RestRequest request = new RestRequest($"/orders/{notification.OrderId}", Method.GET);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            var result = await restClient.ExecuteAsync<Order>(request);
            if (result.IsSuccessful)
            {
                order = result.Data;
                order.Success = true;
            }
            else if (tryAgain)
            {
                return await RefreshTokenAndTryAgain(seller.RefreshToken, seller.SellerId, async () => await GetOrderDetailsAsync(notification, false));
            }

            return order;
        }

        public async Task<bool> SendMessageToBuyerAsync(SendMessage sendMessage, bool tryAgain = true)
        {
            var seller = sellerRepository.GetQueryable(x => x.SellerId == sendMessage.SellerId).FirstOrDefault();
            if (seller == null) return false;

            this.accessToken = seller.AccessToken;

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
            else if (tryAgain)
            {
                return await RefreshTokenAndTryAgain(seller.RefreshToken, seller.SellerId, async () => await SendMessageToBuyerAsync(sendMessage, false));
            }

            return false;
        }

        public async Task<TResult> RefreshTokenAndTryAgain<TResult>(string refreshToken, long sellerId, Func<Task<TResult>> func)
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

    }


}
