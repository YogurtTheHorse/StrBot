using StrategyBot.Game.Logic;

namespace StrategyBot.Game.Server.RabbitMq
{
    public class MessageToSocialNetwork : GameAnswer
    {
        public string PlayerSocialId { get; set; }
    }
}