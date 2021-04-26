using System.Drawing;

namespace Authello.Players
{
    interface IPlayer
    {
        public Tile Player { get; set; }
        public string PlayerName { get; }
        public string PlayerDescription { get; }
        public Point MakeMove(Tile[,] board);

        
    }
}
