using System;
using System.Collections.Concurrent;
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

            StartListening();
        }

        private void StartListening()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqSettings.Value.Hostname
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

        public void Dispose()
        {
            Channel.Close();
            _connection.Close();
        }

        private BlockingCollection<MessageToSocialNetwork> GetUserAnswers(string userId) =>
            _answers
                .GetOrAdd(
                    userId,
                    _ => new BlockingCollection<MessageToSocialNetwork>()
                );

        public void HandleAnswer(MessageToSocialNetwork answer)
        {
            BlockingCollection<MessageToSocialNetwork> userAnswers = GetUserAnswers(answer.PlayerSocialId);

            userAnswers.Add(answer);            
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
    }
}