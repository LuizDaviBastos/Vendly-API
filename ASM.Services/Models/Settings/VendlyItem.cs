using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Services.Models.Settings
{
    [FirestoreData]
    public class VendlyItem
    {
        [FirestoreProperty(Name = "title")]
        public string? Title { get; set; }

        [FirestoreProperty(Name = "currency_id")]
        public string? CurrencyId { get; set; }

        [FirestoreProperty(Name = "price")]
        public decimal? Price { get; set; }
    }
}
