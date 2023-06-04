using Google.Cloud.Firestore;

namespace ASM.Services.Models.Settings
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

        [FirestoreProperty(Name = "Html")]
        public Html? Html { get; set; }

        [FirestoreProperty(Name = "VendlyItem")]
        public VendlyItem? VendlyItem { get; set; }

        [FirestoreProperty(Name = "MePaToken")]
        public string? MePaToken { get; set; }

        [FirestoreProperty(Name = "FcmServerKey")]
        public string? FcmServerKey { get; set; }

        [FirestoreProperty(Name = "notificationPaymentLink")]
        public string? NotificationPaymentLink { get; set; }
    }
}
