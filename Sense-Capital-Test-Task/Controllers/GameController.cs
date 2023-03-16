using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SenseCapital.Model;
using SenseCapital.Service;

namespace Sense_Capital_Test_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        protected string AccessToken
        {
            get
            {
                Request.Headers.TryGetValue("AccessToken", out var headerValue);
                return headerValue;
            }
        }

        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public IActionResult GetGames()
        {
            var games = _gameService.GetGames();

            return Ok(games);
        }

        [HttpGet("{id}")]
        public IActionResult GetGameById(string id)
        {
            var game = _gameService.GetGameById(id);

            if (game != null) return Ok(game);
            return BadRequest();
        }

        [HttpPost]
        public IActionResult PostTurn(Game game)
        {
            if (string.IsNullOrEmpty(AccessToken)) return Unauthorized("Send AccessToken in Header");

            game.KeyOfFirstPlayer = AccessToken;
            var id = _gameService.CreateGame(game);

            return Ok(id);
        }

        [HttpPost(@"{id:length(24)}/accept")]
        public IActionResult PostTurn(string id)
        {
            if (string.IsNullOrEmpty(AccessToken)) return Unauthorized("Send AccessToken in Header");

            var result = _gameService.AcceptGame(id, AccessToken);

            if (result) return Ok(result);
            return BadRequest();
        }


        [HttpPut("{id}")]
        public IActionResult PutTurnById(string id, Game game)
        {
            game.BsonId = ObjectId.Parse(id);
            var results = _gameService.GameLogic(game, AccessToken);

            return Ok(results);
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteGame(string id)
        {
            var result = _gameService.DeleteGame(id);
            if (result) return Ok(result);

            return BadRequest();
        }
    }
}
