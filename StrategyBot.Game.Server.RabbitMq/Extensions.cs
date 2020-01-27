using System.Text.Json;
using RabbitMQ.Client;

namespace StrategyBot.Game.Server.RabbitMq
{
    public static class Extensions
    {
        public static byte[] EncodeObject<T>(this T obj) => JsonSerializer.SerializeToUtf8Bytes(obj);

        public static T DecodeObject<T>(this byte[] bytes) => JsonSerializer.Deserialize<T>(bytes);

        public static void SetupServerQueue(this IModel channel, RabbitMqSettings rabbitMqSettings)
        {
            channel.ExchangeDeclare(
                rabbitMqSettings.MessagesExchange,
                ExchangeType.Topic
            );

            channel.QueueDeclare(
                queue: rabbitMqSettings.ServersQueue,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            channel.QueueBind(rabbitMqSettings.ServersQueue, rabbitMqSettings.MessagesExchange, rabbitMqSettings.ServersQueue);
        }
    }
}