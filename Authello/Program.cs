using Authello.ConsoleUI;
using System.Diagnostics;

namespace Authello
{
    class Program
    {
        static void Main(string[] args)
        {
            Game othello = new Game();

            var (Black, White) = UI.ChoosePlayers();
            var currentPlayer = othello.CurrentPlayer == Player.Black ? Black : White;

            var boardView = UI.CreateBoardView(othello);

            var sw = new Stopwatch();

            while (!othello.GameOver)
            {
                UI.AddToLog($"{currentPlayer.Player}s turn.");
                sw.Restart();
                var move = currentPlayer.MakeMove(othello.board);
                sw.Stop();

                UI.AddToLog($"{currentPlayer.Player} made a move in {sw.ElapsedMilliseconds / 1000.0:N2}s");

                othello.MakeMove(move.X, move.Y);

                // Switch player
                currentPlayer = othello.CurrentPlayer == Player.Black ? Black : White;

                boardView.UpdateUI();
            }
            boardView.GameOver();
        }
    }
}
