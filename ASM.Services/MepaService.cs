using ASM.Data.Entities;
using ASM.Services.Interfaces;
using ASM.Services.Models.Mepa;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource;
using MercadoPago.Resource.Preference;
using RestSharp;

namespace ASM.Services
{
    public class MepaService : IMepaService
    {
        private readonly RestClient restClient;
        private readonly ISettingsService settingsService;

        public MepaService(ISettingsService settingsService)
        {
            restClient = new RestClient("https://api.mercadopago.com");
            this.settingsService = settingsService;
        }

        public async Task<PaymentLinkResponse> CreatePreference(Guid sellerId, Guid userPaymentId, SubscriptionPlan subscriptionPlan, bool isBinary = false)
        {
            PaymentLinkResponse response = new();
            var settings = await settingsService.GetAppSettingsAsync();
            MercadoPagoConfig.AccessToken = settings.MePaToken;

            var request = new PreferenceRequest
            {
                ExternalReference = sellerId.ToString(),
                NotificationUrl = settings.NotificationPaymentLink,
                BinaryMode = isBinary,
                Expires = true,
                ExpirationDateFrom = DateTime.UtcNow,
                ExpirationDateTo = DateTime.UtcNow.AddDays(6),
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title = subscriptionPlan.Name,
                        Quantity = 1,
                        CurrencyId = "BRL",
                        UnitPrice =  subscriptionPlan.Price
                    },
                },
                Metadata = new Dictionary<string, object> { { "sellerId", sellerId }, { "userPaymentId", userPaymentId } }
            };

            var client = new PreferenceClient();
            var preference = await client.CreateAsync(request);
            response.init_point = preference.InitPoint;
            response.price = subscriptionPlan.Price;
            response.Success = true;
            response.preferenceId = preference.Id;

            return response;
        }

        public async Task<PaymentLinkResponse> GetPreference(string preferenceId)
        {
            PaymentLinkResponse response = new();
            var settings = await settingsService.GetAppSettingsAsync();
            MercadoPagoConfig.AccessToken = settings.MePaToken;

            var client = new PreferenceClient();
            var preference = await client.GetAsync(preferenceId);
            response.Success = !string.IsNullOrEmpty(preference.Id);
            if (response.Success == true)
            {
                response.init_point = preference.InitPoint;
                response.price = preference.Items.First().UnitPrice;
            }
            else
            {
                response.Message = "Preferência não encontrada";
            }
            return response;
        }

        public async Task<ResultsResourcesPage<MercadoPago.Resource.Payment.Payment>> GetLastPayments(Guid sellerId, int limit = 3)
        {
            PaymentLinkResponse response = new();
            var settings = await settingsService.GetAppSettingsAsync();
            MercadoPagoConfig.AccessToken = settings.MePaToken;

            var client = new PaymentClient();
            var paymentResponse = await client.SearchAsync(new MercadoPago.Client.SearchRequest
            {
                Filters = new Dictionary<string, object>
                {
                    { "sort", "date_created" },
                    { "criteria", "desc" },
                    { "external_reference",  sellerId.ToString() }
                },
                Limit = limit,
            });

            return paymentResponse;
        }

        public async Task<PaymentResponse> GetPaymentInformation(string paymentId)
        {
            var response = new PaymentResponse();
            response.Success = false;

            var settings = await settingsService.GetAppSettingsAsync();
            var accessToken = settings.MePaToken;

            RestRequest request = new RestRequest($"/v1/payments/{paymentId}", Method.GET);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            var result = await restClient.ExecuteAsync<PaymentResponse>(request);
            if (result.IsSuccessful)
            {
                response = result.Data;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = result.Content;
            }

            return response;
        }
    }


}
