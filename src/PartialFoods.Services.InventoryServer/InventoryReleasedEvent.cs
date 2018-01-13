using Newtonsoft.Json;

namespace PartialFoods.Services.InventoryServer
{
    public class InventoryReleasedEvent
    {
        [JsonProperty("sku")]
        public string SKU { get; set; }

        [JsonProperty("quantity")]
        public uint Quantity { get; set; }

        [JsonProperty("released_on")]
        public ulong ReservedOn { get; set; }

        [JsonProperty("order_id")]
        public string OrderID { get; set; }

        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("event_id")]
        public string EventID { get; set; }
    }
}