using MongoDB.Driver;
using SenseCapital.Model;

namespace SenseCapital.Service
{
    public class GameService
    {
        private readonly MongoClient _client;
        public GameService(MongoClient client)
        {
            _client = client;
        }

        public int CreateGame(Game game)
        {
            int lastGameId = _client.GetDatabase("tick_crossdb").GetCollection<Game>("GamesCollection").AsQueryable().Count();
            game.Id = lastGameId + 1;
            _client.GetDatabase("tick_crossdb").GetCollection<Game>("GamesCollection").InsertOne(game);
            return game.Id;
        }



    }
}