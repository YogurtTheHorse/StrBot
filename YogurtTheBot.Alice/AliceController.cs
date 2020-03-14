using System.Linq;
using Microsoft.AspNetCore.Mvc;
using YogurtTheBot.Alice.Models;
using YogurtTheBot.Alice.Services;
using YogurtTheBot.Game.Server.RabbitMq;

namespace YogurtTheBot.Alice
{
    [ApiController]
    [Route("alice")]
    public class AliceController : Controller
    {
        private readonly IRabbitService _rabbit;
        public AliceController(IRabbitService rabbit)
        {
            _rabbit = rabbit;
        }
        
        [HttpPost]
        public AliceResponse WebHook([FromBody] AliceRequest request)
        {
            MessageToSocialNetwork answer = _rabbit.HandleUserMessage(new MessageFromSocialNetwork
            {
                Locale = request.Meta.Locale == "ru-RU" ? "ru" : request.Meta.Locale,
                Text = request.Request.OriginalUtterance,
                PlayerSocialId = request.Session.UserId,
                ReplyBackQueueName = "alice"
            });
            
            return new AliceResponse
            {
                Session = request.Session,
                Response = new ResponseModel
                {
                    Text = answer.Text,
                    Buttons = answer
                        .Suggestions
                        .Select(s => new ButtonModel
                        {
                            Title = s.Text
                        })
                        .ToArray()
                }
            };
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}