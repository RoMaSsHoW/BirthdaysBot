using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BirthdaysBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> TestData()
        {
            var data = await GetTestData();

            return data == null ? NotFound() : Ok(data);
        }

        private async Task<IEnumerable<int>> GetTestData()
        {
            List<int> data = new List<int>()
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            };

            return data;
        }
    }
}
