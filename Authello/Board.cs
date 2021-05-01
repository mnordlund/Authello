using Authello.Players;
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
        public bool GameOver => IsGameOver();

        public Point LastMove { get; private set; }
        public int WhiteScore { get; private set; }
        public int BlackScore { get; private set; }

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

        private bool IsGameOver()
        {
            return Util.ListAllMoves(board, Tile.White).Length == 0 && Util.ListAllMoves(board, Tile.Black).Length == 0;
        }
        private void UpdateScore()
        {
            var blackScore = 0;
            var whiteScore = 0;

            foreach(var tile in board)
            {
                if (tile == Tile.Black) blackScore++;
                if (tile == Tile.White) whiteScore++;
            }

            BlackScore = blackScore;
            WhiteScore = whiteScore;
            
        }

        public bool MakeMove(int x, int y, Tile player)
        {
            // Sanity checks
            if (x < 0 || x > BoardSize || y < 0 || y > BoardSize) return false;
            if (board[x, y] != Tile.None) return false;
            if (player == Tile.None) return false;

            if(Util.IsValidMove(board, x, y, player))
            {
                board = Util.MakeMove(board, x, y, player);
                LastMove = new Point(x, y);
                UpdateScore();
                return true;
            }
            return false;
        }

        public Tile[,] GetBoardArray()
        {
            return (Tile[,])board.Clone();
        }
    }
}
