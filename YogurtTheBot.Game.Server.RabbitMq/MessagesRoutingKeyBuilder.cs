using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Server.RabbitMq
{
    public class MessagesRoutingKeyBuilder
    {
        public string SocialNetwork { get; set; } = "*";

        public GameMessageType? MessageType { get; set; }

        public MessagesRoutingKeyBuilder WithSocialNetwork(string network) =>
            new MessagesRoutingKeyBuilder
            {
                SocialNetwork = network,
                MessageType = MessageType
            };

        public MessagesRoutingKeyBuilder WithMessageType(GameMessageType messageType) =>
            new MessagesRoutingKeyBuilder
            {
                SocialNetwork = SocialNetwork,
                MessageType = messageType
            };
        
        public string Build()
        {
            string messageTypeString = MessageType.HasValue 
                    ? MessageType!.ToString()
                    : "*";
            
            return $"{SocialNetwork}.{messageTypeString}";
        }
    }
}