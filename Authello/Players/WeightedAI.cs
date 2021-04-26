using System;
using System.Drawing;
using System.Globalization;

namespace Authello.Players
{
    class WeightedAI : IPlayer
    {
        public double[,] weights =
        {
            { 10.0,-10.0, 7.0, 2.0, 2.0,7.0,-10.0, 10.0, },
            {-10.0,-10.0, 5.0,-2.0,-2.0,5.0,-10.0,-10.0, },
            {  7.0,  5.0, 5.0, 1.0, 1.0,5.0,  5.0,  7.0, },
            {  2.0, -2.0, 1.0, 0.0, 0.0,1.0, -2.0,  2.0, },
            {  2.0, -2.0, 1.0, 0.0, 0.0,1.0, -2.0,  2.0, },
            {  7.0,  5.0, 5.0, 1.0, 1.0,5.0,  5.0,  7.0, },
            {-10.0,-10.0, 5.0,-2.0,-2.0,5.0,-10.0,-10.0, },
            { 10.0,-10.0, 7.0, 2.0, 2.0,7.0,-10.0, 10.0, },
        };

        public Tile Player { get; set; }

        public string PlayerDescription => "Uses a weighted score of each position of the board and makes the highest weighted available move.";

        public string PlayerName => "Weighted AI";

        public Point MakeMove(Tile[,] board)
        {
            var bestMoveX = -1;
            var bestMoveY = -1;
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (!Util.IsValidMove(board, x, y, Player)) continue;

                    if ((bestMoveX < 0 && bestMoveY < 0) || weights[x,y] > weights[bestMoveX, bestMoveY])
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
