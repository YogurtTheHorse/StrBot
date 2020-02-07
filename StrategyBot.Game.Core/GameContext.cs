using System.Threading.Tasks;
using MongoDB.Bson;
using StrategyBot.Game.Data.Abstractions;
using StrategyBot.Game.Logic.Communications;

namespace StrategyBot.Game.Logic
{
    public class GameContext
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly IMongoRepository<PlayerState> _playersState;
        private readonly IMongoRepository<PlayerData> _playersData;

        public GameContext(
            IMessageProcessor messageProcessor,
            IMongoRepository<PlayerState> playersState,
            IMongoRepository<PlayerData> playersData
        )
        {
            _messageProcessor = messageProcessor;
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
            
            await _messageProcessor.ProcessMessage(message, playerState, playerData);
        }
    }
}