using Microsoft.AspNetCore.Mvc;
using SenseCapital.Model;
using SenseCapital.Service;

namespace Sense_Capital_Test_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public IActionResult GetGames()
        {
         var r = _gameService.GetGames();

            return Ok(r);


        }

        [HttpGet("{id:int}")]
        public IActionResult GetGameById(int id)
        {
            var r = _gameService.GetGameById(id);
            if (r != null) return Ok(r);
            return BadRequest();


        }

        [HttpPost]
        public IActionResult PostTurn(Game game)
        {
         var id = _gameService.CreateGame(game);

            return Ok(id);


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
            var i = _gameService.DeleteGame(id);
            if (i) return Ok();
            return BadRequest();


        }

    }
}
