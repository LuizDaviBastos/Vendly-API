using Google.Cloud.Firestore;
using System.Net.Mail;

namespace ASM.Services.Models.Settings
{
    [FirestoreData]
    public class SmtpSettings
    {
        [FirestoreProperty(Name = "Host")]
        public string? Host { get; set; }

        [FirestoreProperty(Name = "Port")]
        public int? Port { get; set; }

        [FirestoreProperty(Name = "UserName")]
        public string? UserName { get; set; }

        [FirestoreProperty(Name = "Password")]
        public string? Password { get; set; }

        [FirestoreProperty(Name = "Email")]
        public string? Email { get; set; }

        [FirestoreProperty(Name = "Name")]
        public string? Name { get; set; }

        [FirestoreProperty(Name = "ElasticEmailApiKey")]
        public string? ElasticEmailApiKey { get; set; }
    }

}
