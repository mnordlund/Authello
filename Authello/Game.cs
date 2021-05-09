using Authello.ConsoleUI;
using Authello.Players;

namespace Authello
{

    public enum Player : byte
    {
        Black = 0,
        White = 1
    }

    class Game
    {
        public bool GameOver => IsGameOver();

        public (int X, int Y) LastMove { get; private set; }
        public int BlackScore => Util.GetScore(board, Player.Black);
        public int WhiteScore => Util.GetScore(board, Player.White);

        public Board board { get; private set; }

        public Player CurrentPlayer { get; private set; }
        public Game()
        {
            Initialize();
        }

        public void Initialize()
        {
            board = new Board(0x1008000000, 0x810000000); ;
        }

        private bool IsGameOver()
        {
            return Util.ListAllMoves(board, Player.Black).Length == 0 && Util.ListAllMoves(board, Player.White).Length == 0;
        }

        public Player MakeMove(int x, int y)
        {
            // Player pass
            if(x == -1 && y == -1)
            {
                if(Util.ListAllMoves(board, CurrentPlayer).Length == 0)
                {
                    UI.AddToLog($"{CurrentPlayer} passes");
                    CurrentPlayer = CurrentPlayer.OtherPlayer();
                    return CurrentPlayer.OtherPlayer();
                }
            }
            // Sanity checks
            if (x < 0 || x > 8 || y < 0 || y > 8)
            {
                UI.AddToLog($"{CurrentPlayer} made out of board move ({x}x{y})");
                return CurrentPlayer;
            }

            if (Util.IsValidMove(board, x, y, CurrentPlayer))
            {
                board = Util.MakeMove(board, x, y, CurrentPlayer);
                LastMove = (x, y);
                CurrentPlayer = CurrentPlayer.OtherPlayer();
            }
            else
            {
                UI.AddToLog($"{CurrentPlayer} made invalid move ({x}x{y})");
            }
            return CurrentPlayer;
        }
    }
}
