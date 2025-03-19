namespace BirthdaysBot.API.Controllers
{
    [ApiController]
    [Route("webhook")]
    public class TelegramController : ControllerBase
    {
        private readonly IUpdateHandler _updateHandler;

        public TelegramController(IUpdateHandler updateHandler)
        {
            _updateHandler = updateHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update? update)
        {
            if (update == null)
            {
                return BadRequest("Update cannot be null.");
            }

            await _updateHandler.Execute(update);

            return Ok();
        }
    }
}