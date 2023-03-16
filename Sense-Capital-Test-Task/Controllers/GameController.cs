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
        protected string AccessToken
        {
            get { Request.Headers.TryGetValue("AccessToken", out var headerValue);
                return headerValue;
            }

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
            if (String.IsNullOrEmpty(AccessToken)) return Unauthorized();
            game.KeyOfFirstPlayer = AccessToken;
         var id = _gameService.CreateGame(game);

            return Ok(id);


        }

        [HttpPost("{id:int}/accept")]

        public IActionResult PostTurn(int id)
        {
            if (String.IsNullOrEmpty(AccessToken)) return Unauthorized();
           var f = _gameService.AcceptGame(id, AccessToken);
            if (f) return Ok(f);
            return BadRequest();

        }




        [HttpPut("{id:int}")]

        public IActionResult PutTurnById(int Id, Game game)
        {
            return Ok();


        }
        
        [HttpDelete("{id:int}")]
        public IActionResult ClearField(int id)
        {
            var i = _gameService.DeleteGame(id);
            if (i) return Ok();
            return BadRequest();


        }

    }
}
