namespace YogurtTheBot.Game.Server.RabbitMq
{
    public class MessageFromSocialNetwork
    {
        public string Text { get; set; }
        
        public string ReplyBackQueueName { get; set; }
        
        public string PlayerSocialId { get; set; }
    }
}