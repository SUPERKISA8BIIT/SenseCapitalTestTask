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

        public bool DeleteGame(int id)
        {
            var filter = Builders<Game>.Filter.Eq("Id", id);
            var result = _client.GetDatabase("tick_crossdb").GetCollection<Game>("GamesCollection").DeleteOne(filter);
            if(result.DeletedCount > 0 ) return true;
            return false;
           
        }

        public List<Game> GetGames()
        {
            var dbList = _client.GetDatabase("tick_crossdb").GetCollection<Game>("GamesCollection").AsQueryable();
            return (List<Game>)dbList;
        }

        public Game GetGameById(int id)
        {
           // var filter = Builders<Game>.Filter.Eq("Id", id);
            var result = _client.GetDatabase("tick_crossdb").GetCollection<Game>("GamesCollection").AsQueryable();
            var k = result.FirstOrDefault();
            return k ;
          //  var result = _client.GetDatabase("tick_crossdb").GetCollection<Game>("GamesCollection").AsQuerya
        }

    }
}