using System.Drawing;

namespace Authello.Players
{
    interface IPlayer
    {
        public Tile Player { get; set; }
        public string PlayerDescription { get; }
        public Point MakeMove(Tile[,] board);

        
    }
}
