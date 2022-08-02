using Newtonsoft.Json;

namespace BusinessesInterface.Models
{

    public class DeviceUsage
    {
        [JsonProperty(PropertyName = "id")] // need for random generation of moltipul entries in our DB 
        public string? Id { get; set; }

        [JsonProperty(PropertyName = "buisnessId")] //need for filtering entries for each buisness , field in device table
        public string? BuisnessId { get; set; }

        [JsonProperty(PropertyName = "socketNumber")] //int, uniqe //change to socket number , field in device table
        public string? DeviceId { get; set; }

        [JsonProperty(PropertyName = "deviceId")] //string, in iothub //name of device by hadas , field in device table
        public string? HardwareId { get; set; }

        [JsonProperty(PropertyName = "currentLocation")] // dont need as we have buisnessId
        public string? CurrentLocation { get; set; }

        [JsonProperty(PropertyName = "timeStamp")] // to fetch from activation of a socket 
        public DateTime TimeStamp { get; set; }
    }
}

