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

        public string PlayerDescription => "Weighted AI";

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

        public void mutateWeights()
        {
            var rnd = new Random();
            for (int x = 0; x < weights.GetLength(0); x++)
            {
                for (int y = 0; y < weights.GetLength(1); y++)
                {
                    weights[x, y] += (rnd.NextDouble() - 0.5) * 0.005;
                }
            }
        }

        public void PrintWeights()
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            for (int y = 0; y < weights.GetLength(1); y++)
            {
                Console.Write("{");
                for (int x = 0; x < weights.GetLength(1); x++)
                {
                    Console.Write($"{weights[x,y].ToString(culture)},");
                }
                Console.WriteLine("},");
            }
        }

        public void CloneWeights(double[,] newWeights)
        {
            for (int x = 0; x < weights.GetLength(0); x++)
            {
                for (int y = 0; y < weights.GetLength(1); y++)
                {
                    weights[x, y] = newWeights[x, y];
                }
            }
        }
    }
}
