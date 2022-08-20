using Newtonsoft.Json;

namespace Service.Api.Models
{
    public class PayloadVo
    {
        [JsonProperty("machine_id")]
        public string Machine_Id { get; set; }
        public string Id { get; set; }
        public string TimeStamp { get; set; }
        public MachineStatus Status { get; set; }
    }
}
