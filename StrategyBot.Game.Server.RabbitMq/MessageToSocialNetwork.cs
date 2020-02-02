using StrategyBot.Game.Data;

namespace StrategyBot.Game.Server.RabbitMq
{
    public class MessageToSocialNetwork : GameAnswer
    {
        public string PlayerSocialId { get; set; }
    }
}