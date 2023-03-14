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
            return Ok();


        }

    }
}
