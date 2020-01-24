using StrategyBot.Game.Data.Mongo;

namespace StrategyBot.Telegram.Polling.Models
{
    public class PlayerToTelegramChat : MongoModel
    {
        public int ChatId { get; set; }
    }
}