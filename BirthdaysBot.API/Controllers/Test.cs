using BirthdaysBot.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace BirthdaysBot.API.Controllers
{
    [ApiController]
    [Route("webhook")]
    public class Test : ControllerBase
    {
        private readonly IUpdateHandler _updateHandler;

        public Test(IUpdateHandler updateHandler)
        {
            _updateHandler = updateHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            await _updateHandler.Execute(update);
            return Ok();
        }
    }
}



//private readonly ServiceTest _serviceTest;

//public Test(ServiceTest serviceTest)
//{
//    _serviceTest = serviceTest;
//}

//[HttpPost]
//public async Task<IActionResult> Post([FromBody] Update update)
//{
//    await _serviceTest.HandleUpdate(update);
//    return Ok();
//}

//private readonly ITelegramBotClient _botClient;

//public Test(ITelegramBotClient botClient)
//{
//    _botClient = botClient;
//}

//[HttpPost]
//public async Task<IActionResult> Post([FromBody] Update update)
//{
//    if (update?.Message?.Text != null)
//    {
//        var chatId = update.Message.Chat.Id;
//        var userMessage = update.Message.Text;

//        await _botClient.SendMessage(
//            chatId: chatId,
//            text: $"Вы написали: {userMessage}"
//        );
//    }
//    return Ok();
//}


//[HttpGet]
//public async Task<ActionResult> TestData()
//{
//    var data = await GetTestData();

//    return data == null ? NotFound() : Ok(data);
//}

//private async Task<IEnumerable<int>> GetTestData()
//{
//    List<int> data = new List<int>()
//    {
//        1, 2, 3, 4, 5, 6, 7, 8, 9, 10
//    };

//    return data;
//}