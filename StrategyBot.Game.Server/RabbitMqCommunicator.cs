using System.Threading.Tasks;
using RabbitMQ.Client;
using StrategyBot.Game.Data.Abstractions;
using StrategyBot.Game.Logic;
using StrategyBot.Game.Logic.Models;
using StrategyBot.Game.Server.RabbitMq;

namespace StrategyBot.Game.Server
{
    public class RabbitMqCommunicator : IGameCommunicator
    {
        private readonly RabbitMqSettings _rabbitMqSettings;
        private readonly IModel _rabbitMqChannel;
        private readonly IMongoRepository<PlayerInfo> _players;

        public RabbitMqCommunicator(RabbitMqSettings rabbitMqSettings, IModel rabbitMqChannel, IMongoUnitOfWork mongoUnitOfWork)
        {
            _rabbitMqSettings = rabbitMqSettings;
            _rabbitMqChannel = rabbitMqChannel;
            _players = mongoUnitOfWork.GetRepository<PlayerInfo>();
        }

        public async Task Answer(GameAnswer message, GameMessageType messageType = GameMessageType.RegularAnswer)
        {
            PlayerInfo playerInfo = await _players.GetById(message.PlayerId);

            _rabbitMqChannel.BasicPublish(
                _rabbitMqSettings.MessagesExchange,
                new MessagesRoutingKeyBuilder()
                    .WithSocialNetwork(playerInfo.ReplyQueueName)
                    .WithMessageType(messageType)
                    .Build(),
                null,
                new MessageToSocialNetwork
                {
                    Text = message.Text,
                    PlayerId = message.PlayerId,
                    PlayerSocialId = playerInfo.SocialId
                }.EncodeObject()
            );
        }
    }
}