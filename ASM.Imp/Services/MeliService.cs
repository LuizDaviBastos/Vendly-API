using ASM.Core.Models;
using ASM.Core.Services;
using ASM.Imp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ASM.Imp.Services
{
    public class MeliService : IMeliService
    {
        private readonly RestClient restClient;
        private string accessToken = string.Empty;
        public MeliService()
        {
            restClient = new RestClient("https://api.mercadolibre.com");
        }

        public string GetAuthUrl(string countryId)
        {
            return MLConstants.GetAuthUrlByCountryId(countryId);
        }

        public async Task<AccessToken> GetAccessTokenAsync(string code)
        {
            AccessToken accessToken = new AccessToken();
            accessToken.Success = false;

            RestRequest request = new RestRequest("/oauth/token", Method.POST);
            request.AddJsonBody(new
            {
                grant_type = "authorization_code",
                client_id = MLConstants.AppId,
                client_secret = MLConstants.SecretKey,
                code = code,
                redirect_uri = MLConstants.RedirectUrl
            });

            var result = await restClient.ExecuteAsync<AccessToken>(request);
            if (result.IsSuccessful)
            {
                accessToken = result.Data;
                accessToken.Success = true;
            }
            else
            {
                var content = JsonConvert.DeserializeObject<JObject>(result.Content);
                if((int?)content["status"] == 400)
                {
                    //todo refresh token
                }
            }

            return accessToken;
        }

        public async Task<AccessToken> RefreshAccessTokenAsync(string refreshToken)
        {
            AccessToken accessToken = new AccessToken();
            accessToken.Success = false;

            RestRequest request = new RestRequest("/oauth/token", Method.POST);
            request.AddJsonBody(new
            {
                grant_type = "refresh_token",
                client_id = MLConstants.AppId,
                client_secret = MLConstants.SecretKey,
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

        public async Task<Order> GetOrderDetailsAsync(NotificationTrigger notification)
        {
            var order = new Order();
            order.Success = false;
            RestRequest request = new RestRequest($"/orders/{notification.OrderId}", Method.GET);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            //TODO get accessToken by SellerId 1030856711

            var result = await restClient.ExecuteAsync<Order>(request);
            if (result.IsSuccessful)
            {
                order = result.Data;
                order.Success = true;
            }

            return order;
        }

        public async Task SendMessageToBuyerAsync(SendMessage sendMessage)
        {
            RestRequest request = new RestRequest($"/messages/packs/{sendMessage.PackId}/sellers/{sendMessage.SellerId}", Method.POST);
            request.AddParameter("application_id", MLConstants.AppId)
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
        }
    }

    
}
