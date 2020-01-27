using System.Collections.Generic;
using StrategyBot.Game.Data.Mongo;

namespace StrategyBot.Game.Logic.Models
{
    public class PlayerData : MongoModel
    {
        public Dictionary<string, object> TemporaryVariables { get; set; } = new Dictionary<string, object>();
    }
}