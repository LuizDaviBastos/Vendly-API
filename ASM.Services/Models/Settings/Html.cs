using Google.Cloud.Firestore;

namespace ASM.Services.Models.Settings
{
    [FirestoreData]
    public class Html
    {
        [FirestoreProperty(Name = "htmlEmailCodeNewUser")]
        public string? HtmlEmailCodeNewUser { get; set; }

        [FirestoreProperty(Name = "htmlEmailCode")]
        public string? HtmlEmailCode { get; set; }

        [FirestoreProperty(Name = "htmlRecoveryPassword")]
        public string? HtmlRecoveryPassword { get; set; }
    }
}
