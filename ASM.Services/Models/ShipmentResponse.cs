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
        public DateTime last_updated { get; set; }
        public List<object> items_types { get; set; }
        public string substatus { get; set; }
        public DateTime date_created { get; set; }
        public Origin origin { get; set; }
        public Destination destination { get; set; }
        public Source source { get; set; }
        public List<string> tags { get; set; }
        public int declared_value { get; set; }
        public Logistic logistic { get; set; }
        public Sibling sibling { get; set; }
        public LeadTime lead_time { get; set; }
        public object external_reference { get; set; }
        public string tracking_number { get; set; }
        public long id { get; set; }
        public string tracking_method { get; set; }
        public string status { get; set; }
        public Dimensions dimensions { get; set; }

    }

    public class Agency
    {
        public object carrier_id { get; set; }
        public object phone { get; set; }
        public object agency_id { get; set; }
        public object description { get; set; }
        public object type { get; set; }
        public object open_hours { get; set; }
    }

    public class Buffering
    {
        public object date { get; set; }
    }

    public class City
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Country
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Destination
    {
        public object comments { get; set; }
        public int receiver_id { get; set; }
        public string receiver_name { get; set; }
        public ShippingAddress shipping_address { get; set; }
        public string type { get; set; }
        public string receiver_phone { get; set; }
    }

    public class Dimensions
    {
        public int height { get; set; }
        public int width { get; set; }
        public int length { get; set; }
        public int weight { get; set; }
    }

    public class EstimatedDeliveryExtended
    {
        public DateTime date { get; set; }
        public int offset { get; set; }
    }

    public class EstimatedDeliveryFinal
    {
        public DateTime date { get; set; }
        public int offset { get; set; }
    }

    public class EstimatedDeliveryLimit
    {
        public DateTime date { get; set; }
        public int offset { get; set; }
    }

    public class EstimatedDeliveryTime
    {
        public DateTime date { get; set; }
        public DateTime pay_before { get; set; }
        public object schedule { get; set; }
        public string unit { get; set; }
        public Offset offset { get; set; }
        public int shipping { get; set; }
        public TimeFrame time_frame { get; set; }
        public int handling { get; set; }
        public string type { get; set; }
    }

    public class EstimatedHandlingLimit
    {
        public DateTime date { get; set; }
    }

    public class EstimatedScheduleLimit
    {
        public object date { get; set; }
    }

    public class LeadTime
    {
        public object processing_time { get; set; }
        public int cost { get; set; }
        public EstimatedScheduleLimit estimated_schedule_limit { get; set; }
        public string cost_type { get; set; }
        public EstimatedDeliveryFinal estimated_delivery_final { get; set; }
        public Buffering buffering { get; set; }
        public int list_cost { get; set; }
        public EstimatedDeliveryLimit estimated_delivery_limit { get; set; }
        public object priority_class { get; set; }
        public string delivery_promise { get; set; }
        public ShippingMethod shipping_method { get; set; }
        public string delivery_type { get; set; }
        public EstimatedHandlingLimit estimated_handling_limit { get; set; }
        public int service_id { get; set; }
        public EstimatedDeliveryTime estimated_delivery_time { get; set; }
        public long option_id { get; set; }
        public EstimatedDeliveryExtended estimated_delivery_extended { get; set; }
        public string currency_id { get; set; }
    }

    public class Logistic
    {
        public string mode { get; set; }
        public string type { get; set; }
        public string direction { get; set; }
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

    public class Offset
    {
        public DateTime date { get; set; }
        public int shipping { get; set; }
    }

    public class Origin
    {
        public ShippingAddress shipping_address { get; set; }
        public string type { get; set; }
        public int sender_id { get; set; }
    }

    public class ShippingAddress
    {
        public Country country { get; set; }
        public string address_line { get; set; }
        public List<string> types { get; set; }
        public double scoring { get; set; }
        public Agency agency { get; set; }
        public City city { get; set; }
        public string geolocation_type { get; set; }
        public int latitude { get; set; }
        public int address_id { get; set; }
        public Municipality municipality { get; set; }
        public object location_id { get; set; }
        public string street_name { get; set; }
        public string zip_code { get; set; }
        public string geolocation_source { get; set; }
        public object intersection { get; set; }
        public string street_number { get; set; }
        public string comment { get; set; }
        public State state { get; set; }
        public Neighborhood neighborhood { get; set; }
        public DateTime geolocation_last_updated { get; set; }
        public int longitude { get; set; }
        public string delivery_preference { get; set; }
    }

    public class ShippingMethod
    {
        public string name { get; set; }
        public string deliver_to { get; set; }
        public int id { get; set; }
        public string type { get; set; }
    }

    public class Sibling
    {
        public object reason { get; set; }
        public object sibling_id { get; set; }
        public object description { get; set; }
        public object source { get; set; }
        public object date_created { get; set; }
        public object last_updated { get; set; }
    }

    public class SnapshotPacking
    {
        public string snapshot_id { get; set; }
        public string pack_hash { get; set; }
    }

    public class Source
    {
        public string site_id { get; set; }
        public string market_place { get; set; }
        public object customer_id { get; set; }
        public object application_id { get; set; }
    }

    public class State
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class TimeFrame
    {
        public object from { get; set; }
        public object to { get; set; }
    }


}
