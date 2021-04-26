using System.Drawing;

namespace Authello.Players
{
    class NaiveAI : IPlayer
    {
        public Tile Player { get; set; }
        public string PlayerDescription => @"A Naive implementation that only tries to maximize the score given by each move.";

        public string PlayerName => "Naive AI";

        public Point MakeMove(Tile[,] board)
        {
            int bestMoveX = 0;
            int bestMoveY = 0;
            int bestMoveScore = 0;

            for(int x = 0; x < board.GetLength(0); x++)
            {
                for(int y = 0; y < board.GetLength(1); y++)
                {
                    var currentScore = Util.GetMoveScore(board, x, y, Player);
                    if(currentScore > bestMoveScore)
                    {
                        bestMoveX = x;
                        bestMoveY = y;
                    }
                }
            }

            return new Point(bestMoveX, bestMoveY);
        }
    }
}
