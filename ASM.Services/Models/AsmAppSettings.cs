using Google.Cloud.Firestore;

namespace ASM.Services.Models
{
    [FirestoreData]
    public class AsmAppSettings 
    {
        [FirestoreProperty(Name = "urlBaseApi")]
        public string? UrlBaseApi { get; set; }

        [FirestoreProperty(Name = "createAccountMeliUrl")]
        public string? CreateAccountMeliUrl { get; set; }

        [FirestoreProperty(Name = "redirectUrl")]
        public string? RedirectUrl { get; set; }
    }
}
