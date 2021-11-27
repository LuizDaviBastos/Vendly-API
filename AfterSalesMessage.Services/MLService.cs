using MercadoLibre.AspNetCore.SDK;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfterSalesMessage.Services
{
    public class MLService
    {
        private Meli meli = new Meli(MLConstants.AppId, MLConstants.SecretKey);
        private RestClient restClient = new RestClient();
        public string accessToken = string.Empty;

        public string GetAuthUrl()
        {
            var url = meli.GetAuthUrl(MLConstants.AuthUrl, MLConstants.RedirectUrl);
            return url;
        }

        public string GetAcesssToken(string code)
        {
            return "";
        }
    }

    public class MLConstants
    {
        public const string SecretKey = "JOZNSOfzJAHBcOW09g4RuSnmrGYX4NEd";
        public const long AppId = 728065690975054;
        public const string RedirectUrl = "https://www.google.com.br";
        public static string AuthUrl { get => string.Format("https://auth.mercadolivre.com.br/authorization?response_type=code&client_id={0}&redirect_uri={1}", AppId, RedirectUrl); }
    }
}
