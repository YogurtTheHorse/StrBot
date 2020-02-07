using System.Collections.Generic;
using StrategyBot.Game.Data.Mongo;

namespace StrategyBot.Game.Core
{
    public class PlayerData : MongoModel
    {
        public Dictionary<string, object> TemporaryVariables { get; set; } = new Dictionary<string, object>();

        public int AttackShips { get; set; } = 0;

        public int DefenceShips { get; set; } = 0;
    }
}