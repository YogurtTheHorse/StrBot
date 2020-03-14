namespace YogurtTheBot.Alice.Models
{
    public class RequestModel
    {
        public string Command { get; set; }

        public RequestType Type { get; set; }

        public string OriginalUtterance { get; set; }

        public object Payload { get; set; }
    }
}