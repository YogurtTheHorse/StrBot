using System.Text.Json;

namespace YogurtTheBot.Alice.Models
{
    public class AliceRequest
    {
        public MetaModel Meta { get; set; }

        public RequestModel Request { get; set; }

        public SessionModel Session { get; set; }

        public string Version { get; set; }
    }
}