namespace ASM.Imp.Helpers
{
    public class MLConstants
    {
        public const string SecretKey = "aDRd0VLWNbzH5qy4ZVNDyrWi0eqh3PCb";
        public const long AppId = 8710476337491405;
        public const string RedirectUrl = "https://fe56-177-85-44-254.ngrok.io/api/Auth/GetAccessToken";
        private static string AuthUrl { get => $"{{0}}/authorization?response_type=code&client_id={AppId}&redirect_uri={RedirectUrl}"; }
        public static string GetAuthUrlByCountryId(string countryId)
        {
            var auth = string.Format(AuthUrl, Countries[countryId.ToUpper()]);
            return auth;
        }

        private static Dictionary<string, string> Countries = new Dictionary<string, string>(new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("AR","https://auth.mercadolibre.com.ar"),
            new KeyValuePair<string, string>("BR","https://auth.mercadolivre.com.br"),
        });
    }
}
