using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SenseCapital.Model
{
    public class Game
    {
        [BsonIgnore]
        public string Id => BsonId.ToString();

        [BsonId]
        [JsonIgnore]
        public ObjectId BsonId { get; set; }
        public  bool?[][] Field { get; set; }

        [JsonIgnore]
        public string KeyOfFirstPlayer { get; set; } = string.Empty;

        [JsonIgnore]
        public string KeyOfSecondPlayer { get; set; } = string.Empty;
        public bool IsFinished { get; set; }
     
    }
}