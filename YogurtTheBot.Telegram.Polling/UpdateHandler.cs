using System;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using YogurtTheBot.Game.Server.RabbitMq;

namespace YogurtTheBot.Telegram.Polling
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly RabbitMqSettings _rabbitMqSettings;
        private readonly IModel _channel;
    
        public UpdateHandler(IModel channel, RabbitMqSettings rabbitMqSettings)
        {
            _channel = channel;
            _rabbitMqSettings = rabbitMqSettings;
        }
        
        public async Task HandleUpdate(Update update, CancellationToken cancellationToken)
        {
            Task handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(update.Message),
                UpdateType.EditedMessage => BotOnMessageReceived(update.Message),
                
                _ => UnknownUpdateHandlerAsync(update)
            };
    
            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleError(exception, cancellationToken);
            }
        }
        
        public async Task HandleError(Exception exception, CancellationToken cancellationToken)
        {
            string errorMessage = exception switch
            {
                ApiRequestException apiRequestException => "Telegram API Error:\n" +
                                                           $"[{apiRequestException.ErrorCode}]\n" +
                                                           $"{apiRequestException.Message}",
    
                _ => exception.ToString()
            };
    
            await Console.Error.WriteLineAsync(errorMessage);
    
        }
    
        public UpdateType[] AllowedUpdates => new[]
        {
            UpdateType.Message,
            UpdateType.EditedMessage,
            UpdateType.Unknown
        };
        
        private Task BotOnMessageReceived(Message message)
        {
            _channel.BasicPublish(
                _rabbitMqSettings.MessagesExchange,
                _rabbitMqSettings.ServersQueue,
                null,
                new MessageFromSocialNetwork
                {
                    Text = message.Text,
                    PlayerSocialId = message.Chat.Id.ToString(),
                    ReplyBackQueueName = "telegram"
                }.EncodeObject()
            );
            
            return Task.CompletedTask;
        }
    
        private Task UnknownUpdateHandlerAsync(Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
    
            return Task.CompletedTask;
        }
    }
}