using System.Threading.Tasks;
using MongoDB.Bson;
using StrategyBot.Game.Data.Abstractions;
using StrategyBot.Game.Interface.Entities;
using StrategyBot.Game.Interface.Models;
using StrategyBot.Game.Interface.Screens;

namespace StrategyBot.Game.Interface
{
    public class GameContext
    {
        private readonly IScreenController _screenController;
        private readonly IMongoRepository<PlayerState> _playersState;
        private readonly IMongoRepository<PlayerData> _playersData;

        public GameContext(
            IScreenController screenController,
            IMongoRepository<PlayerState> playersState,
            IMongoRepository<PlayerData> playersData
        )
        {
            _screenController = screenController;
            _playersState = playersState;
            _playersData = playersData;
        }

        public async Task<ObjectId> CreatePlayer(string socialId, string replyBackQueue)
        {
            var playerState = new PlayerState
            {
                Key = ObjectId.GenerateNewId(),
                SocialId = socialId,
                ReplyQueueName = replyBackQueue
            };
            await _playersState.Insert(playerState);

            var playerData = new PlayerData
            {
                Key = playerState.Key
            };
            await _playersData.Insert(playerData);
            
            return playerState.Key;
        }

        public async Task ProcessMessage(IncomingMessage message)
        {
            PlayerState playerState = await _playersState.GetById(message.PlayerId);
            PlayerData playerData = await _playersData.GetById(message.PlayerId);
            
            IScreen screen = _screenController.GetCurrentPlayerScreen(playerState);
            await screen.ProcessMessage(message, playerState, playerData);
        }
    }
}