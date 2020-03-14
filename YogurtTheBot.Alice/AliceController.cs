using Microsoft.AspNetCore.Mvc;
using YogurtTheBot.Alice.Models;

namespace YogurtTheBot.Alice
{
    [ApiController]
    [Route("alice")]
    public class AliceController : Controller
    {
        [HttpPost]
        public AliceResponse WebHook([FromBody] AliceRequest request)
        {
            return new AliceResponse
            {
                Session = request.Session,
                Response = new ResponseModel
                {
                    Text = "asdasd"
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