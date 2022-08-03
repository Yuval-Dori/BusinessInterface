using Newtonsoft.Json;

namespace BusinessesInterface.Models
{

    public class Address
    {
        [JsonProperty(PropertyName = "id")] 
        public string? Id { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string? AddressName { get; set; }

        [JsonProperty(PropertyName = "devices")]
        public Device[]? Devices { get; set; }
    }

    public class Device
    {
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }

        [JsonProperty(PropertyName = "deviceID")]
        public string? DeviceID { get; set; }

        [JsonProperty(PropertyName = "history")]
        public long[]? History { get; set; }
    }
}

