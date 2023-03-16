using MongoDB.Bson;
using MongoDB.Driver;
using SenseCapital.Model;

namespace SenseCapital.Service
{
    public class GameService
    {
        protected IMongoCollection<Game> _gameCollection => _client.GetDatabase("tick_crossdb").GetCollection<Game>("GamesCollection");

        private readonly MongoClient _client;
        public GameService(MongoClient client)
        {
            _client = client;
        }      
      

        public string CreateGame(Game game)
        {
            _gameCollection.InsertOne(game);

            return game.BsonId.ToString();
        }

        public bool DeleteGame(string id)
        {
            var filter = Builders<Game>.Filter.Eq(x => x.BsonId, ObjectId.Parse(id));

            var result = _gameCollection.DeleteOne(filter);

            if(result.DeletedCount > 0 ) return true;
            return false;           
        }

        public List<Game> GetGames()
        {
            var dbList = _gameCollection.AsQueryable();

            return dbList.ToList();
        }

        public Game GetGameById(string id)
        {
            var filter = Builders<Game>.Filter.Eq(x => x.BsonId, ObjectId.Parse(id));

            var result = _gameCollection.Find(filter).Limit(1).Single();

            return result;      
        }

        public bool AcceptGame(string id, string accessToken)
        {
            var filter = Builders<Game>.Filter.Eq(x => x.BsonId, ObjectId.Parse(id));
            var acceptedGame = Builders<Game>.Update.Set(x => x.KeyOfSecondPlayer, accessToken);

            var result = _gameCollection.UpdateOne(filter, acceptedGame);

            return result.ModifiedCount > 0;
        }

        public Game GameLogic(Game newGameVersion, string accessToken)
        {
            //--- Validating --//
            if (newGameVersion.Field.Length != 3 || newGameVersion.Field.Any(x => x.Length != 3)) throw new Exception();

            var prevGameVerision = GetGameById(newGameVersion.BsonId.ToString());

            var fullFieldCount = 0;
            var changedCellsCount = 0;
            for(var i = 0; i < newGameVersion.Field.Length; i++)
            {
                for (var j = 0; j < newGameVersion.Field.Length; j++)
                {
                    if (prevGameVerision.Field[i][j] != null)
                        fullFieldCount++;
                    if( (prevGameVerision.Field[i][j] != newGameVersion.Field[i][j]) && prevGameVerision.Field[i][j] == null)
                        changedCellsCount++;
                    if ((prevGameVerision.Field[i][j] != newGameVersion.Field[i][j]) && prevGameVerision.Field[i][j] != null)
                        throw new Exception(); 
                }
            }

            if ((fullFieldCount % 2 == 0) && accessToken != prevGameVerision.KeyOfFirstPlayer) throw new Exception();
            else if ((fullFieldCount % 2 != 0) && accessToken != prevGameVerision.KeyOfSecondPlayer) throw new Exception();
            if (changedCellsCount != 1) throw new Exception();
            //--- ------ --//

            if (fullFieldCount % 2 == 0) newGameVersion.IsFinished = CheckWinner(false, newGameVersion.Field);
            else newGameVersion.IsFinished = CheckWinner(true, newGameVersion.Field);

            var filter = Builders<Game>.Filter.Eq(x => x.BsonId, newGameVersion.BsonId);
            var newTurn = Builders<Game>.Update
                .Set(x => x.Field, newGameVersion.Field)
                .Set(x => x.IsFinished, newGameVersion.IsFinished);

            var result = _gameCollection.FindOneAndUpdate(filter, newTurn);
            return newGameVersion;
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

            //check if draw
            var c = 0;
            for (int j = 0; j < 3; j++)
            {
              
                for (int i = 0; i < 3; i++)
                {
                    if (field[i][j] != null)
                        c++;
                }
                if (c == 9) return true;
            }


            return false;
        }
    }
}