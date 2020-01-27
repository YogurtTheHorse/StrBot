using System.Threading.Tasks;
using MongoDB.Bson;
using StrategyBot.Game.Data.Abstractions;
using StrategyBot.Game.Logic.Entities;
using StrategyBot.Game.Logic.Models;
using StrategyBot.Game.Logic.Screens;

namespace StrategyBot.Game.Logic
{
    public class GameContext
    {
        private readonly IGameCommunicator _gameCommunicator;
        private readonly IScreenController _screenController;
        private readonly IMongoRepository<PlayerData> _playersData;
        private readonly IMongoRepository<PlayerInfo> _playersInfo;

        public GameContext(
            IGameCommunicator gameCommunicator,
            IScreenController screenController,
            IMongoRepository<PlayerData> playersData,
            IMongoRepository<PlayerInfo> playersInfo
        )
        {
            _gameCommunicator = gameCommunicator;
            _screenController = screenController;
            _playersData = playersData;
            _playersInfo = playersInfo;
        }

        public async Task<ObjectId> CreatePlayer(string socialId, string replyBackQueue)
        {
            var playerInfo = new PlayerInfo
            {
                Key = ObjectId.GenerateNewId(),
                SocialId = socialId,
                ReplyQueueName = replyBackQueue
            };
            await _playersInfo.Insert(playerInfo);
            
            var playerData = new PlayerData
            {
                Key = playerInfo.Key
            };
            await _playersData.Insert(playerData);

            return playerInfo.Key;
        }

        public async Task ProcessMessage(IncomingMessage message)
        {
            PlayerData playerData = await _playersData.GetById(message.PlayerId);
            
            IScreen screen = _screenController.GetCurrentPlayerScreen(playerData);
            await screen.ProcessMessage(message, playerData);
        }
    }
}