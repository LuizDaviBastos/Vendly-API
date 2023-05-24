using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Services.Models
{
    public class ShipmentResponse : RequestResponseBase
    {
        public SnapshotPacking snapshot_packing { get; set; }
        public string receiver_id { get; set; }
        public double base_cost { get; set; }
        public StatusHistory status_history { get; set; }
        public object return_details { get; set; }
        public string sender_id { get; set; }
        public string mode { get; set; }
        public string order_cost { get; set; }
        public object carrier_id { get; set; }
        public object service_id { get; set; }
        public List<ShippingItem> shipping_items { get; set; }
        public string tracking_number { get; set; }
        public CostComponents cost_components { get; set; }
        public string id { get; set; }
        public object tracking_method { get; set; }
        public DateTime last_updated { get; set; }
        public List<object> items_types { get; set; }
        public string comments { get; set; }
        public object substatus { get; set; }
        public DateTime date_created { get; set; }
        public object date_first_printed { get; set; }
        public string created_by { get; set; }
        public ShippingOption shipping_option { get; set; }
        public List<string> tags { get; set; }
        public SenderAddress sender_address { get; set; }
        public object return_tracking_number { get; set; }
        public string site_id { get; set; }
        public object carrier_info { get; set; }
        public string market_place { get; set; }
        public ReceiverAddress receiver_address { get; set; }
        public object customer_id { get; set; }
        public string order_id { get; set; }
        public string status { get; set; }


    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class City
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class CostComponents
    {
        public int? special_discount { get; set; }
    }

    public class Country
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Municipality
    {
        public object id { get; set; }
        public object name { get; set; }
    }

    public class Neighborhood
    {
        public object id { get; set; }
        public string name { get; set; }
    }

    public class ReceiverAddress
    {
        public Country country { get; set; }
        public string address_line { get; set; }
        public List<string> types { get; set; }
        public double scoring { get; set; }
        public object agency { get; set; }
        public City city { get; set; }
        public string geolocation_type { get; set; }
        public double latitude { get; set; }
        public Municipality municipality { get; set; }
        public int? location_id { get; set; }
        public string street_name { get; set; }
        public string zip_code { get; set; }
        public string geolocation_source { get; set; }
        public string delivery_preference { get; set; }
        public object intersection { get; set; }
        public string street_number { get; set; }
        public string receiver_name { get; set; }
        public object comment { get; set; }
        public int? id { get; set; }
        public State state { get; set; }
        public Neighborhood neighborhood { get; set; }
        public DateTime geolocation_last_updated { get; set; }
        public string receiver_phone { get; set; }
        public double longitude { get; set; }
    }

    public class Root
    {
    }

    public class SenderAddress
    {
        public Country country { get; set; }
        public string address_line { get; set; }
        public List<string> types { get; set; }
        public double scoring { get; set; }
        public object agency { get; set; }
        public City city { get; set; }
        public string geolocation_type { get; set; }
        public int? latitude { get; set; }
        public Municipality municipality { get; set; }
        public object location_id { get; set; }
        public string street_name { get; set; }
        public string zip_code { get; set; }
        public string geolocation_source { get; set; }
        public object intersection { get; set; }
        public string street_number { get; set; }
        public string comment { get; set; }
        public int? id { get; set; }
        public State state { get; set; }
        public Neighborhood neighborhood { get; set; }
        public DateTime geolocation_last_updated { get; set; }
        public int? longitude { get; set; }
    }

    public class ShippingItem
    {
        public object domain_id { get; set; }
        public int? quantity { get; set; }
        public object dimensions_source { get; set; }
        public string description { get; set; }
        public string id { get; set; }
        public string user_product_id { get; set; }
        public object dimensions { get; set; }
    }

    public class ShippingOption
    {
        public double cost { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public double list_cost { get; set; }
        public string currency_id { get; set; }
        public Speed speed { get; set; }
    }

    public class SnapshotPacking
    {
        public object snapshot_id { get; set; }
        public string pack_hash { get; set; }
    }

    public class Speed
    {
        public object schedule { get; set; }
        public int? shipping { get; set; }
        public object handling { get; set; }
    }

    public class State
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class StatusHistory
    {
        public DateTime date_shipped { get; set; }
        public object date_delivered { get; set; }
    }



}
