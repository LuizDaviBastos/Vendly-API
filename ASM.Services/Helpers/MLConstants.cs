namespace ASM.Services.Helpers
{
    public class MLConstants
    {
        public static string GetAuthUrl(string countryId, long appId, string redirectUrl)
        {
            var authUrl = $"{{0}}/authorization?response_type=code&client_id={appId}&redirect_uri={redirectUrl}";
            var auth = string.Format(authUrl, Countries[countryId.ToUpper()]);
            return auth;
        }

        public static Dictionary<string, string> Countries = new Dictionary<string, string>(new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("AR","https://auth.mercadolibre.com.ar"),
            new KeyValuePair<string, string>("BR","https://auth.mercadolivre.com.br"),
        });
    }
}
