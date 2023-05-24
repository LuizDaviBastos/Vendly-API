using ASM.Services.Models.Settings;
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

        [FirestoreProperty(Name = "smtpSettings")]
        public SmtpSettings? SmtpSettings { get; set; }

        [FirestoreProperty(Name = "htmlEmailCodeNewUser")]
        public string? HtmlEmailCodeNewUser { get; set; }

        [FirestoreProperty(Name = "htmlEmailCode")]
        public string? HtmlEmailCode { get; set; }
    }
}
