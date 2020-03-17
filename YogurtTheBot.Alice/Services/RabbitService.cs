using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using YogurtTheBot.Game.Server.RabbitMq;

namespace YogurtTheBot.Alice.Services
{
    public class RabbitService : IDisposable, IRabbitService
    {
        private readonly ConcurrentDictionary<string, BlockingCollection<MessageToSocialNetwork>> _answers;
        private readonly IOptions<RabbitMqSettings> _rabbitMqSettings;
        private IConnection _connection;
        public IModel Channel { get; private set; }

        public RabbitService(IOptions<RabbitMqSettings> rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings;
            _answers = new ConcurrentDictionary<string, BlockingCollection<MessageToSocialNetwork>>();

            SetupRabbit();
        }

        public void Dispose()
        {
            Channel.Close();
            _connection.Close();
        }

        public MessageToSocialNetwork HandleUserMessage(MessageFromSocialNetwork messageFromSocialNetwork)
        {
            BlockingCollection<MessageToSocialNetwork> userAnswers = GetUserAnswers(messageFromSocialNetwork.PlayerSocialId);
            
            Channel.BasicPublish(
                _rabbitMqSettings.Value.MessagesExchange,
                _rabbitMqSettings.Value.ServersQueue,
                null,
                messageFromSocialNetwork.EncodeObject()
            );

            // TODO: Add timeout
            return userAnswers.Take();
        }

        public void Listen()
        {
            var consumer = new EventingBasicConsumer(Channel);

            consumer.Received += (channel, ea) =>
            {
                HandleAnswer(ea.Body.DecodeObject<MessageToSocialNetwork>());
                Channel.BasicAck(ea.DeliveryTag, false);
            };

            Channel.BasicConsume("alice", false, consumer);
        }

        private BlockingCollection<MessageToSocialNetwork> GetUserAnswers(string userId) =>
            _answers
                .GetOrAdd(
                    userId,
                    _ => new BlockingCollection<MessageToSocialNetwork>()
                );

        private void HandleAnswer(MessageToSocialNetwork answer)
        {
            BlockingCollection<MessageToSocialNetwork> userAnswers = GetUserAnswers(answer.PlayerSocialId);

            userAnswers.Add(answer);                
        }

        private void SetupRabbit()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqSettings.Value.Hostname,
                UserName = _rabbitMqSettings.Value.Username,
                Password = _rabbitMqSettings.Value.Password
            };

            _connection = factory.CreateConnection();
            Channel = _connection.CreateModel();

            Channel.SetupServerQueue(_rabbitMqSettings.Value);

            Channel.QueueDeclare("alice", true, false, false);
            Channel.QueueBind(
                "alice",
                _rabbitMqSettings.Value.MessagesExchange,
                new MessagesRoutingKeyBuilder()
                    .WithSocialNetwork("alice")
                    .Build()
            );
        }
    }
}