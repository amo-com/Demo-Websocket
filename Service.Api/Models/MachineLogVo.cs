namespace Service.Api.Models
{
    public class MachineLogVo
    {
        public string Topic { get; set; }
        public string Ref { get; set; }
        public PayloadVo Payload { get; set; }
        public string JoinRef { get; set; }
        public string Event { get; set; }
    }
}
