using MongoDB.Bson;
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
            return dbList.ToList();
        }

        public Game GetGameById(int id)
        {
            var filter = Builders<Game>.Filter.Eq("Id", id);
            var result = _client.GetDatabase("tick_crossdb").GetCollection<Game>("GamesCollection").Find(filter).Limit(1).Single();
            return result;      
        }

        public bool AcceptGame(int id, string accessToken)
        {
            var filter = Builders<Game>.Filter.Eq("Id", id);
            var cake = Builders<Game>.Update.Set(x => x.KeyOfSecondPlayer, accessToken);

            var result = _client.GetDatabase("tick_crossdb").GetCollection<Game>("GamesCollection").FindOneAndUpdate<Game>(filter, cake);

            return result.KeyOfSecondPlayer == accessToken;
        }

        public Game GameLogic(Game g, string accessToken)
        {
            if (g.Field.Length != 3 || g.Field.Any(x => x.Length != 3)) throw new Exception();
            var prevTurn = GetGameById(g.Id);
            var fullFieldCount = 0;
            var changedCellsCount = 0;
            for(var i = 0; i < g.Field.Length; i++)
            {
                for (var j = 0; j < g.Field.Length; j++)
                {
                    if (prevTurn.Field[i][j] != null) fullFieldCount++;
                    if( (prevTurn.Field[i][j] != g.Field[i][j]) && prevTurn.Field[i][j] == null) changedCellsCount++;
                    if ((prevTurn.Field[i][j] != g.Field[i][j]) && prevTurn.Field[i][j] != null) throw new Exception(); 
                }
            }

            if ((fullFieldCount % 2 == 0) && accessToken != g.KeyOfFirstPlayer) throw new Exception();
            else if ((fullFieldCount % 2 != 0) && accessToken != g.KeyOfSecondPlayer) throw new Exception();
            if (changedCellsCount != 1) throw new Exception();
            if(fullFieldCount % 2 == 0) g.IsFinished = CheckWinner(false, g.Field);
            else g.IsFinished = CheckWinner(true, g.Field);
           
            var filter = Builders<Game>.Filter.Eq("Id", g.Id);
            var cake = Builders<Game>.Update.Set(x => x.KeyOfSecondPlayer, accessToken);

            var result = _client.GetDatabase("tick_crossdb").GetCollection<Game>("GamesCollection").ReplaceOne(filter, g);
            return g;
        }

        private static bool CheckWinner(bool? team, bool?[][] field)
        {

            // * _ _
            // _ * _
            // _ _ *
            bool trigger = true;
            for (int i = 0; i < 3; i++)
            {
                if (field[i] [i] != team)
                    trigger = false;
            }
            if (trigger) return true;
           
            // _ _ *
            // _ * _
            // * _ _
            trigger = true;
            for (int i = 0; i < 3; i++)
            {
                if (field[i] [2 - i] != team)
                    trigger = false;
            }
            if (trigger) return true;

            // _ * _
            // _ * _
            // _ * _

            for (int i = 0; i < 3; i++)
            {
                bool triggerJ = true;
                for (int j = 0; j < 3; j++)
                {
                    if (field[i][j] != team)
                        triggerJ = false;
                }
                if (triggerJ) return true;
            }
            // _ _ _
            // * * *
            // _ _ _
            for (int j = 0; j < 3; j++)
            {
                bool triggerI = true;
                for (int i = 0; i < 3; i++)
                {
                    if (field[i][j] != team)
                        triggerI = false;
                }
                if (triggerI) return true;
            }
            return false;
        }
    }
}