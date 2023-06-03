using ASM.Services.Interfaces;
using ASM.Services.Models.Mepa;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
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

        public async Task<PaymentLinkResponse> CreatePreferenceSdk(Guid sellerId)
        {
            PaymentLinkResponse response = new();
            var settings = await settingsService.GetAppSettingsAsync();
            MercadoPagoConfig.AccessToken = settings.MePaToken;

            var request = new PreferenceRequest
            {
                ExternalReference = sellerId.ToString(),
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title = settings?.VendlyItem?.Title ?? "Vendly Plano Mensal",
                        Quantity = 1,
                        CurrencyId = "BRL",
                        UnitPrice = 39.90M
                    },
                },
                Metadata = new Dictionary<string, object> { { "sellerId", sellerId } }
            };


            var client = new PreferenceClient();
            var preference = await client.CreateAsync(request);
            response.init_point = preference.InitPoint;
            response.price = settings?.VendlyItem?.Price ?? 39.90;

            return response;
        }

        public async Task<PaymentLinkResponse> GetPreference(string preferenceId)
        {
            PaymentLinkResponse response = new();
            var settings = await settingsService.GetAppSettingsAsync();
            MercadoPagoConfig.AccessToken = settings.MePaToken;

            var client = new PreferenceClient();
            var preference = await client.GetAsync(preferenceId);
            response.init_point = preference.InitPoint;
            response.price = settings?.VendlyItem?.Price ?? 39.90;

            return response;
        }

        public async Task<PaymentLinkResponse> CreatePreference(Guid sellerId)
        {
            var settings = await settingsService.GetAppSettingsAsync();
            var accessToken = settings.MePaToken;
            var response = new PaymentLinkResponse();
            response.Success = false;

            RestRequest request = new RestRequest($"/checkout/preferences", Method.POST);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            var body = new PaymentLinkParams
            {
                ExternalReference = sellerId.ToString(),
                Items = new List<PaymentItem>
                {
                    new PaymentItem
                    {
                        currency_id = "BRL",
                        quantity = 1,
                        title = settings?.VendlyItem?.Title ?? "Vendly Plano Mensal",
                        unit_price = settings?.VendlyItem?.Price ?? 39.90
                    }
                },
                Metadata = new Metadata
                {
                    sellerId = sellerId
                },
            };

            request.AddJsonBody(body);

            var result = await restClient.ExecuteAsync<PaymentLinkResponse>(request);
            if (result.IsSuccessful)
            {
                response = result.Data;
                response.Success = true;
                response.price = settings?.VendlyItem?.Price ?? 39.90;
            }
            else
            {
                response.Success = false;
                response.Message = result.Content;
            }

            return response;
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
