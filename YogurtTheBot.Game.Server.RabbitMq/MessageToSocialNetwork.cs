using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Server.RabbitMq
{
    public class MessageToSocialNetwork : GameAnswer
    {
        public string PlayerSocialId { get; set; }
    }
}