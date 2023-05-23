namespace ASM.Services.Models
{
    public class SellerItems : RequestResponseBase
    {
        public string site_id { get; set; }
        //public SellerSellerItem seller { get; set; }
        //public string country_default_time_zone { get; set; }
        public Paging paging { get; set; }
        //public List<Result> results { get; set; }
        //public Sort sort { get; set; }
        //public List<AvailableSort> available_sorts { get; set; }
        //public List<object> filters { get; set; }
        //public List<AvailableFilter> available_filters { get; set; }
    }


    public class Attribute
    {
        public string id { get; set; }
        public string name { get; set; }
        public string value_id { get; set; }
        public string value_name { get; set; }
        public string attribute_group_id { get; set; }
        public string attribute_group_name { get; set; }
        public object value_struct { get; set; }
        public List<Value> values { get; set; }
        public object source { get; set; }
        public string value_type { get; set; }
    }

    public class AvailableFilter
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public List<Value> values { get; set; }
    }

    public class AvailableSort
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Cancellations
    {
        public string period { get; set; }
        public double rate { get; set; }
        public int value { get; set; }
    }


    public class Claims
    {
        public string period { get; set; }
        public double rate { get; set; }
        public int value { get; set; }
    }


    public class DelayedHandlingTime
    {
        public string period { get; set; }
        public int rate { get; set; }
        public int value { get; set; }
    }

    public class DifferentialPricing
    {
        public int id { get; set; }
    }

    public class Installments
    {
        public int quantity { get; set; }
        public double amount { get; set; }
        public double rate { get; set; }
        public string currency_id { get; set; }
    }

    public class Metrics
    {
        public Sales sales { get; set; }
        public Claims claims { get; set; }
        public DelayedHandlingTime delayed_handling_time { get; set; }
        public Cancellations cancellations { get; set; }
    }

    public class Paging
    {
        public int? total { get; set; }
        public int? primary_results { get; set; }
        public int? offset { get; set; }
        public int? limit { get; set; }
    }


    public class Result
    {
        public string id { get; set; }
        public string title { get; set; }
        public string condition { get; set; }
        public string thumbnail_id { get; set; }
        public string catalog_product_id { get; set; }
        public string listing_type_id { get; set; }
        public string permalink { get; set; }
        public string buying_mode { get; set; }
        public string site_id { get; set; }
        public string category_id { get; set; }
        public string domain_id { get; set; }
        public string thumbnail { get; set; }
        public string currency_id { get; set; }
        public int order_backend { get; set; }
        public double price { get; set; }
        public object original_price { get; set; }
        public object sale_price { get; set; }
        public int sold_quantity { get; set; }
        public int available_quantity { get; set; }
        public object official_store_id { get; set; }
        public bool use_thumbnail_id { get; set; }
        public bool accepts_mercadopago { get; set; }
        public List<string> tags { get; set; }
        public Shipping shipping { get; set; }
        public DateTime stop_time { get; set; }
        public SellerSellerItem seller { get; set; }
        public SellerAddress seller_address { get; set; }
        public Address address { get; set; }
        public List<Attribute> attributes { get; set; }
        public Installments installments { get; set; }
        public object winner_item_id { get; set; }
        public bool catalog_listing { get; set; }
        public object discounts { get; set; }
        public List<object> promotions { get; set; }
        public object inventory_id { get; set; }
        public DifferentialPricing differential_pricing { get; set; }
    }

    public class Sales
    {
        public string period { get; set; }
        public int completed { get; set; }
    }

    public class SellerSellerItem
    {
        public int id { get; set; }
        public string nickname { get; set; }
        public bool car_dealer { get; set; }
        public bool real_estate_agency { get; set; }
        public bool _ { get; set; }
        public DateTime registration_date { get; set; }
        public List<string> tags { get; set; }
        public string permalink { get; set; }
        public SellerReputation seller_reputation { get; set; }
        public string car_dealer_logo { get; set; }
    }

    public class SellerAddress
    {
        public string comment { get; set; }
        public string address_line { get; set; }
        public object id { get; set; }
        public object latitude { get; set; }
        public object longitude { get; set; }
        public Country country { get; set; }
        public State state { get; set; }
        public City city { get; set; }
    }


    public class Sort
    {
        public string id { get; set; }
        public string name { get; set; }
    }


    public class Value
    {
        public string id { get; set; }
        public string name { get; set; }
        public object @struct { get; set; }
        public object source { get; set; }
        public int results { get; set; }
    }




}
