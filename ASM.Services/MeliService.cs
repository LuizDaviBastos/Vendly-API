using ASM.Data.Entities;
using ASM.Data.Interfaces;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Text;

namespace ASM.Services
{
    public class MeliService : IMeliService
    {
        private string accessToken = string.Empty;
        private readonly RestClient restClient;
        private readonly IUnitOfWork unitOfWork;
        private readonly AsmConfiguration asmConfiguration;
        private readonly ISellerService sellerService;
        private readonly IStorageService storageService;
        private readonly ISettingsService settingsService;

        private bool NeedRefreshToken(IRestResponse result) => result.StatusCode == HttpStatusCode.Forbidden ||
            result.StatusCode == HttpStatusCode.BadRequest || result.StatusCode == HttpStatusCode.Unauthorized;

        public MeliService(IUnitOfWork unitOfWork, AsmConfiguration asmConfiguration, ISellerService sellerService, IStorageService storageService, ISettingsService settingsService)
        {
            restClient = new RestClient("https://api.mercadolibre.com");
            this.unitOfWork = unitOfWork;
            this.asmConfiguration = asmConfiguration;
            this.sellerService = sellerService;
            this.storageService = storageService;
            this.settingsService = settingsService; 
        }

        public async Task<string> GetAuthUrl(string countryId, StateUrl? state)
        {
            var settings = await this.settingsService.GetAppSettings();
            string? redirectUrl = settings.RedirectUrl ?? asmConfiguration.RedirectUrl;
            string stateBase64 = string.Empty;
            if (state != null) stateBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(state)));

            var authUrl = $"{{0}}/authorization?response_type=code&client_id={asmConfiguration.AppId}&redirect_uri={redirectUrl}&state={stateBase64}";
            return string.Format(authUrl, asmConfiguration.Countries?[countryId.ToUpper()]);
        }

        public async Task<AccessToken> GetAccessTokenAsync(string code)
        {
            var settings = await this.settingsService.GetAppSettings();
            AccessToken accessToken = new AccessToken();
            accessToken.Success = false;

            RestRequest request = new RestRequest("/oauth/token", Method.POST);
            request.AddJsonBody(new
            {
                grant_type = "authorization_code",
                client_id = asmConfiguration.AppId,
                client_secret = asmConfiguration.SecretKey,
                code = code,
                redirect_uri = settings.RedirectUrl ?? asmConfiguration.RedirectUrl
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

            if (!SetAccessToken(notification.user_id, out MeliAccount meliAccount))
            {
                order.Message = "Seller not found";
                return order;
            }

            RestRequest request = new RestRequest($"/orders/{notification.TopicId}", Method.GET);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            var result2 = await restClient.ExecuteAsync(request);
            var result = await restClient.ExecuteAsync<Order>(request);
            if (result.IsSuccessful)
            {
                order = result.Data;
                order.Success = true;
            }
            else if (tryAgain && NeedRefreshToken(result))
            {
                return await RefreshTokenAndTryAgain(meliAccount.RefreshToken, async () => await GetOrderDetailsAsync(notification, false));
            }
            else
            {
                order.Success = false;
                order.Message = result.Content;
            }

            return order;
        }

        public async Task<OrderFeedback> GetFeedbackDetailsAsync(NotificationTrigger notification, bool tryAgain = true)
        {
            var feedback = new OrderFeedback();
            feedback.Success = false;

            if (!SetAccessToken(notification.user_id, out MeliAccount meliAccount))
            {
                feedback.Message = "Seller not found";
                return feedback;
            }

            RestRequest request = new RestRequest($"/orders/{notification.TopicId}/feedback", Method.GET);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            var result = await restClient.ExecuteAsync<OrderFeedback>(request);
            if (result.IsSuccessful)
            {
                feedback = result.Data;
                feedback.Success = true;
            }
            else if (tryAgain && NeedRefreshToken(result))
            {
                return await RefreshTokenAndTryAgain(meliAccount!.RefreshToken, async () => await GetFeedbackDetailsAsync(notification, false));
            }
            else
            {
                feedback.Success = false;
                feedback.Message = result.Content;
            }

            return feedback;
        }

        public async Task<ShipmentResponse> GetShipmentDetails(NotificationTrigger notification, bool tryAgain = true)
        {
            var shipment = new ShipmentResponse();
            shipment.Success = false;

            if (!SetAccessToken(notification.user_id, out MeliAccount meliAccount))
            {
                shipment.Message = "Seller not found";
                return shipment;
            }

            RestRequest request = new RestRequest($"/shipments/{notification.TopicId}", Method.GET);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddParameter("x-format-new", "true");

            var result = await restClient.ExecuteAsync<ShipmentResponse>(request);
            if (result.IsSuccessful)
            {
                shipment = result.Data;
                shipment.Success = true;
            }
            else if (tryAgain && NeedRefreshToken(result))
            {
                await RefreshTokenAndTryAgain(meliAccount!.RefreshToken, async () => await GetShipmentDetails(notification, false));
            }
            else
            {
                shipment.Success = false;
                shipment.Message = result.Content;
            }

            return shipment;
        }

        public async Task<(bool, string)> SendMessageToBuyerAsync(SendMessage sendMessage, bool tryAgain = true)
        {
            if (!SetAccessToken(sendMessage.MeliSellerId, out MeliAccount meliAccount)) return (false, $"Erro ao obter MeliAccount. MeliSellerId: {sendMessage.MeliSellerId}");

            if (!(sendMessage.Attachments?.Any() ?? false)) sendMessage.Attachments = null;

            RestRequest request = new RestRequest($"/messages/packs/{sendMessage.PackId}/sellers/{sendMessage.MeliSellerId}", Method.POST);
            request.AddHeader("Authorization", $"Bearer {this.accessToken}");
            request.AddQueryParameter("tag", "post_sale");

            var body = new SendMessageRequest(sendMessage.MeliSellerId, sendMessage.BuyerId, sendMessage.Message, sendMessage.Attachments);
            request.AddJsonBody(body);

            var result = await restClient.ExecuteAsync(request);
            if (result.IsSuccessful)
            {
                return (true, string.Empty);
            }
            else if (tryAgain && NeedRefreshToken(result))
            {
                return await RefreshTokenAndTryAgain(meliAccount!.RefreshToken, async () => await SendMessageToBuyerAsync(sendMessage, false));
            }

            return (true, result.Content);
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
            else if (tryAgain && NeedRefreshToken(result))
            {
                return await RefreshTokenAndTryAgain(meliAccount.RefreshToken, async () => await IsFirstSellerMessage(sendMessage, false));
            }

            throw new Exception(result.Content);
        }

        public async Task<SellerInfo> GetMeliSellerInfo(long meliSellerId, bool tryAgain = true)
        {
            if (!SetAccessToken(meliSellerId, out MeliAccount? meliAccount)) throw new Exception($"SetAccessToken Error{(meliAccount == null || meliAccount?.Id == Guid.Empty ? ". Seller not found" : "")}");

            RestRequest restRequest = new RestRequest($"/users/me", Method.GET);
            restRequest.AddHeader("Authorization", $"Bearer {this.accessToken}");

            var result = await restClient.ExecuteAsync<SellerInfo>(restRequest);
            if (result.IsSuccessful)
            {
                var sellerItems = await GetSellerItems(meliSellerId);
                result.Data.ProdutosCount = sellerItems?.paging?.total ?? 0;
                result.Data.Success = true;
                return result.Data;
            }
            else if (tryAgain && NeedRefreshToken(result))
            {
                return await RefreshTokenAndTryAgain(meliAccount!.RefreshToken, async () => await GetMeliSellerInfo(meliSellerId, false));
            }

            throw new Exception(result.Content);
        }

        public async Task<IList<SaveAttachmentResponse>> SaveAttachments(Guid sellerId, SellerMessage sellerMessage)
        {
            List<SaveAttachmentResponse> response = new();
            if (!sellerMessage.Attachments.Any()) return response;
            foreach (var attachment in sellerMessage.Attachments)
            {
                var attachContent = await storageService.Download(sellerId, sellerMessage.Type, attachment.Name);

                RestRequest restRequest = new RestRequest($"/messages/attachments", Method.POST);
                restRequest.AddHeader("Authorization", $"Bearer {this.accessToken}")
                    .AddHeader("content-type", "multipart/form-data")
                    .AddParameter("tag", "post_sale")
                    .AddParameter("site_id", "MLB");

                restRequest.AddFileBytes("file", attachContent.ToArray(), attachment.Name);

                var requestResponse = await restClient.ExecuteAsync<SaveAttachmentResponse>(restRequest);
                if (requestResponse.IsSuccessful)
                {
                    response.Add(requestResponse.Data);
                }
            }

            return response;
        }

        private async Task<SellerItems?> GetSellerItems(long meliSellerId, bool tryAgain = true)
        {
           if (!SetAccessToken(meliSellerId, out MeliAccount? meliAccount)) throw new Exception($"SetAccessToken Error{(meliAccount == null || meliAccount?.Id == Guid.Empty ? ". Seller not found" : "")}");

            RestRequest restRequest = new RestRequest($"/sites/MLB/search?seller_id={meliSellerId}", Method.GET);
            restRequest.AddHeader("Authorization", $"Bearer {this.accessToken}");

            var result = await restClient.ExecuteAsync<SellerItems>(restRequest);
            if (result.IsSuccessful)
            {
                result.Data.Success = true;
                return result.Data;
            }
            else if (tryAgain && NeedRefreshToken(result))
            {
                return await RefreshTokenAndTryAgain(meliAccount!.RefreshToken, async () => await GetSellerItems(meliSellerId, false));
            }
            return null;
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

        private bool SetAccessToken(long meliSellerId, out MeliAccount meliAccount)
        {
            meliAccount = unitOfWork.MeliAccountRepository.GetByMeliSellerId(meliSellerId);

            if (meliAccount == null) return false;

            this.accessToken = meliAccount.AccessToken;
            return true;
        }

        public async Task<(bool, (MeliAccount, string))> RevokeMeliAccount(MeliAccount meli, bool tryAgain = true)
        {
            if (!SetAccessToken(meli.MeliSellerId, out MeliAccount? meliAccount)) throw new Exception($"SetAccessToken Error{(meliAccount == null || meliAccount?.Id == Guid.Empty ? ". Seller not found" : "")}");

            RestRequest restRequest = new RestRequest($"/users/{meli.MeliSellerId}/applications/{asmConfiguration.AppId}", Method.DELETE);
            restRequest.AddHeader("Authorization", $"Bearer {this.accessToken}");

            //var result1 = await restClient.ExecuteAsync(restRequest);
            var result = await restClient.ExecuteAsync<RevokeResponse>(restRequest);
            if (result.IsSuccessful)
            {
                return (true, (meliAccount, ""));
            }
            else if (tryAgain && NeedRefreshToken(result))
            {
                return await RefreshTokenAndTryAgain(meliAccount!.RefreshToken, async () => await RevokeMeliAccount(meli, false));
            }

            return (false, (meliAccount, result.Data.msg));
        }

        public async Task<Dictionary<bool, (MeliAccount, string)>> RevokeMeliAccounts(IEnumerable<MeliAccount> meliAccounts, bool tryAgain = true)
        {
            Dictionary<bool, (MeliAccount, string)> status = new();

            if (meliAccounts?.Any() ?? false)
            {
                foreach (var meli in meliAccounts)
                {
                    var result = await this.RevokeMeliAccount(meli, tryAgain);
                    status.Add(result.Item1, result.Item2);
                }
            }

            return status;
        }
    }


}
