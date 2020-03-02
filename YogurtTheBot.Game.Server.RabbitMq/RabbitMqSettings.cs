namespace YogurtTheBot.Game.Server.RabbitMq
{
    public class RabbitMqSettings
    {
        public string Hostname { get; set; }
        
        public string ServersQueue { get; set; }
        
        public string MessagesExchange { get; set; }
    }
}