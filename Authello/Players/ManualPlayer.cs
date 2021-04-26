using System;
using System.Drawing;

namespace Authello.Players
{
    class ManualPlayer : IPlayer
    {
        public Tile Player { get; set; }

        public string PlayerDescription =>"Allows you to play against the computer or a friend.";

        public string PlayerName => "Manual Player";

        public Point MakeMove(Tile[,] board)
        {
            int x, y;
            for (; ; )
            {
                Console.Write("Make a move:                ");
                Console.CursorLeft = 13;

                var move = Console.ReadLine();

                if(move.ToLower().Equals("pass"))
                {
                    x = -1; y = -1;
                    break;
                }

                var components = move.ToUpper().ToCharArray();

                x = components[0] - 'A';
                y = (int)Char.GetNumericValue(components[1]) - 1;

                if (Util.GetMoveScore(board, x, y, Player) != 0) break;

                Console.WriteLine($"Invalid move {x}, {y}");
            }

            return new Point(x, y);
        }
    }
}
