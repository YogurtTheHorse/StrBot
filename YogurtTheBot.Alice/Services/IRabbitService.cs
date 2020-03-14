using RabbitMQ.Client;
using YogurtTheBot.Game.Server.RabbitMq;

namespace YogurtTheBot.Alice.Services
{
    public interface IRabbitService
    {
        IModel Channel { get; }

        void HandleAnswer(MessageToSocialNetwork answer);
        
        MessageToSocialNetwork HandleUserMessage(MessageFromSocialNetwork messageFromSocialNetwork);
    }
}