using System;
using System.Drawing;
using System.Globalization;
using System.Linq;

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

        public Player Player { get; set; }

        public string PlayerDescription => "Uses a weighted score of each position of the board and makes the highest weighted available move.";

        public string PlayerName => "Weighted AI";

        public (int X, int Y) MakeMove(Board board)
        {
            var moves = Util.ListAllMoves(board, Player);

            if (moves.Length == 0) return (-1, -1);

            var bestMove = moves.Aggregate((itemA, itemB) => weights[itemA.X, itemA.Y] > weights[itemB.X, itemB.Y] ? itemA : itemB);

            return bestMove;
        }
    }
}
