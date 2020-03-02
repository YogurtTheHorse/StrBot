namespace YogurtTheBot.Game.Data.Mongo
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; }
        
        public string Database { get; set; }

        public bool EnumAsString { get; set; } = false;
    }
}