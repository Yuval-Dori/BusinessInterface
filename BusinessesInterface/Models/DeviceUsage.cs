using Newtonsoft.Json;

namespace BusinessesInterface.Models
{

    public class Address
    {
        [JsonProperty(PropertyName = "id")] 
        public string? Id { get; set; }

        [JsonProperty(PropertyName = "devices")]
        public Device[]? Devices { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string? Password { get; set; }
    }

    public class Device
    {
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }

        [JsonProperty(PropertyName = "deviceID")]
        public string? DeviceID { get; set; }

        [JsonProperty(PropertyName = "history")]
        public long[]? History { get; set; }

        [JsonProperty(PropertyName = "historyDateTime")]
        public DateTime[]? HistoryDateTime { get; set; }
    }

    public class TimeSearch
    {
        [JsonProperty(PropertyName = "timeToSearch")]
        public DateTime TimeToSearch { get; set; }
    }
}

