namespace SenseCapital.Model
{
    public class Game
    {
        public int Id { get; set; }
        public  bool?[][] Field { get; set; } 

        public int KeyOfFirstPlayer { get; set; }
        public int KeyOfSecondPlayer { get; set; }
     
    }
}