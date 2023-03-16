using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SenseCapital.Model
{
    public class Game
    {
        [BsonId]
        public int Id { get; set; }
        public  bool?[][] Field { get; set; }

        [JsonIgnore] 
        public string KeyOfFirstPlayer { get; set; }

        [JsonIgnore]
        public string KeyOfSecondPlayer { get; set; }
        public bool IsFinished { get; set; }
     
    }
}