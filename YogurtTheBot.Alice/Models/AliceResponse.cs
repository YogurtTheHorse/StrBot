using System.Text.Json;

namespace YogurtTheBot.Alice.Models
{
    public class AliceResponse
    {
        public ResponseModel Response { get; set; }

        public SessionModel Session { get; set; }

        public string Version { get; set; } = "1.0";
    }
}