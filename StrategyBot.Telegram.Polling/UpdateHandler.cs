using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using StrategyBot.Game.Data.Abstractions;
using StrategyBot.Game.Logic;
using StrategyBot.Game.Logic.Entities;
using StrategyBot.Telegram.Polling.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StrategyBot.Telegram.Polling
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly TelegramBotClient _bot;
        private readonly GameContext _gameContext;
        private readonly IRepository<PlayerToTelegramChat> _playersToChat;

        public UpdateHandler(GameContext gameContext, TelegramBotClient bot)
        {
            _bot = bot;
            _gameContext = gameContext;
            _playersToChat = _gameContext.UnitOfWork.GetRepository<PlayerToTelegramChat>();
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
        
        private async Task BotOnMessageReceived(Message message)
        {
            ObjectId? key = (await _playersToChat.GetFirstOrDefault(p => p.ChatId == message.Chat.Id))?.Key;

            await _gameContext.ProcessMessage(new IncomingMessage
            {
                Text = message.Text,
                PlayerId = key ?? ObjectId.GenerateNewId()
            });
        }

        private Task UnknownUpdateHandlerAsync(Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");

            return Task.CompletedTask;
        }
    }
}