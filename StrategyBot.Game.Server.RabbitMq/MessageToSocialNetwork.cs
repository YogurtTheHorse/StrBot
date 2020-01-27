using StrategyBot.Game.Interface;

namespace StrategyBot.Game.Server.RabbitMq
{
    public class MessageToSocialNetwork : GameAnswer
    {
        public string PlayerSocialId { get; set; }
    }
}