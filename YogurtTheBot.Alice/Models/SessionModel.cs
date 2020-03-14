namespace YogurtTheBot.Alice.Models
{
    public class SessionModel {
        public bool New { get; set; }

        public string SessionId { get; set; }

        public int MessageId { get; set; }

        public string SkillId { get; set; }

        public string UserId { get; set; }
    }
}