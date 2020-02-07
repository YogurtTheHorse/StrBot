using System.Threading.Tasks;
using MongoDB.Bson;
using StrategyBot.Game.Core.Communications;
using StrategyBot.Game.Data.Abstractions;

namespace StrategyBot.Game.Core
{
    public class GameContext<T> where T : PlayerDataBase, new()
    {
        private readonly IMessageProcessor<T> _messageProcessor;
        private readonly IMongoRepository<PlayerInfo> _playersState;
        private readonly IMongoRepository<T> _playersData;

        public GameContext(
            IMessageProcessor<T> messageProcessor,
            IMongoRepository<PlayerInfo> playersState,
            IMongoRepository<T> playersData
        )
        {
            _messageProcessor = messageProcessor;
            _playersState = playersState;
            _playersData = playersData;
        }

        public async Task<ObjectId> CreatePlayer(string socialId, string replyBackQueue)
        {
            var playerState = new PlayerInfo
            {
                Key = ObjectId.GenerateNewId(),
                SocialId = socialId,
                ReplyQueueName = replyBackQueue
            };
            await _playersState.Insert(playerState);

            var playerData = new T
            {
                Key = playerState.Key
            };
            await _playersData.Insert(playerData);
            
            return playerState.Key;
        }

        public async Task ProcessMessage(IncomingMessage message)
        {
            PlayerInfo playerInfo = await _playersState.GetById(message.PlayerId);
            T playerData = await _playersData.GetById(message.PlayerId);
            
            await _messageProcessor.ProcessMessage(message, playerInfo, playerData);
        }
    }
}