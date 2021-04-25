using System;
using System.Drawing;

namespace Authello
{

    [Flags]
    public enum Tile : byte
    {
        None,
        Black,
        White
    }

    class Board
    {
        public bool GameOver => getScore(Tile.None) == 0;

        public Point LastMove { get; private set; }

        public int BoardSize => 8;
        private Tile[,] board;
        public Board()
        {
            board = new Tile[BoardSize, BoardSize];
            Initialize();
        }

        public void Initialize()
        {
            for (var x = 0; x < BoardSize; x++)
            {
                for (var y = 0; y < BoardSize; y++)
                {
                    board[x, y] = Tile.None;
                }
            }

            board[3, 3] = Tile.White;
            board[3, 4] = Tile.Black;
            board[4, 3] = Tile.Black;
            board[4, 4] = Tile.White;
        }

        public int getScore(Tile player)
        {
            int score = 0;
            for (var x = 0; x < BoardSize; x++)
            {
                for (var y = 0; y < BoardSize; y++)
                {
                    if (board[x, y] == player) score++;
                }
            }

            return score;
        }

        public bool MakeMove(int x, int y, Tile player)
        {
            // Sanity checks
            if (x < 0 || x > BoardSize || y < 0 || y > BoardSize) return false;
            if (board[x, y] != Tile.None) return false;
            if (player == Tile.None) return false;

            var otherPlayer = player == Tile.White ? Tile.Black : Tile.White;

            // N
            var score = checkDirection(x, y, 0, -1, player, otherPlayer);
            // S
            score += checkDirection(x, y, 0, 1, player, otherPlayer);
            // W
            score += checkDirection(x, y, -1, 0, player, otherPlayer);
            // E
            score += checkDirection(x, y, 1, 0, player, otherPlayer);
            // NW
            score += checkDirection(x, y, -1, -1, player, otherPlayer);
            // NE
            score += checkDirection(x, y, 1, -1, player, otherPlayer);
            // SW
            score += checkDirection(x, y, -1, 1, player, otherPlayer);
            // SE
            score += checkDirection(x, y, 1, 1, player, otherPlayer);

            if (score == 0) return false;

            board[x, y] = player;
            LastMove = new Point(x, y);

            return true;
        }

        private int checkDirection(int x, int y, int dx, int dy, Tile player, Tile otherPlayer)
        {
            int posx = x + dx;
            int posy = y + dy;
            if (posx < 0 || posx >= BoardSize || posy < 0 || posy >= BoardSize) return 0;
            if (board[posx, posy] != otherPlayer) return 0;
            int score = 1;

            while (board[posx, posy] == otherPlayer)
            {
                posx += dx;
                posy += dy;
                score++;

                if (posx < 0 || posx >= BoardSize || posy < 0 || posy >= BoardSize) return 0;
            }

            if (board[posx, posy] == player)
            {
                posx = x + dx;
                posy = y + dy;
                while (board[posx, posy] == otherPlayer)
                {
                    board[posx, posy] = player;
                    posx += dx;
                    posy += dy;
                }
                return score;
            }
            return 0;
        }

        public Tile[,] GetBoardArray()
        {
            return (Tile[,])board.Clone();
        }
    }
}
