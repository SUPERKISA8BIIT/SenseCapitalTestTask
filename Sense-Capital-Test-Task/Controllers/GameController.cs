using Microsoft.AspNetCore.Mvc;
using SenseCapital.Model;

namespace Sense_Capital_Test_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetGames()
        {
            return Ok();


        }
        [HttpGet("{id:int}")]
        public IActionResult GetGamesById(int id)
        {
            return Ok();

        }

        [HttpPost]
        public IActionResult PostTurn(Game game)
        {
            return Ok();


        }

        [HttpPut("{id:int}")]

        public IActionResult PutTurnById(int Id, Game game)
        {
            return Ok();


        }
        //
        [HttpDelete("{id:int}")]
        public IActionResult ClearField(int id)
        {
            return Ok();


        }

    }
}
