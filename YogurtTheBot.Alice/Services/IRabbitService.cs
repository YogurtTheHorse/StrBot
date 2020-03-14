using RabbitMQ.Client;
using YogurtTheBot.Game.Server.RabbitMq;

namespace YogurtTheBot.Alice.Services
{
    public interface IRabbitService
    {
        void Listen();
        
        MessageToSocialNetwork HandleUserMessage(MessageFromSocialNetwork messageFromSocialNetwork);
    }
}