namespace ASM.Imp.Helpers
{
    public class MLConstants
    {
        public const string SecretKey = "JOZNSOfzJAHBcOW09g4RuSnmrGYX4NEd";
        public const long AppId = 728065690975054;
        public const string RedirectUrl = "https://88a3-177-85-44-254.ngrok.io/api/Auth/GetAccessToken";
        public static string AuthUrl { get => string.Format("https://auth.mercadolivre.com.br/authorization?response_type=code&client_id={0}&redirect_uri={1}", AppId, RedirectUrl); }
    }
}
