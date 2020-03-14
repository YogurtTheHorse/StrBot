namespace YogurtTheBot.Alice.Models
{
    public class ResponseModel {
        public string Text { get; set; }

        public string Tts { get; set; }

        public bool EndSession { get; set; }

        public ButtonModel[] Buttons { get; set; }
    }
}