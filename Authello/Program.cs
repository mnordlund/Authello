using Authello.ConsoleUI;
using System.Diagnostics;

namespace Authello
{
    class Program
    {
        static void Main(string[] args)
        {
            Board b = new Board();

            var (Black, White) = UI.ChoosePlayers();
            var currentPlayer = Black;

            var boardView = UI.CreateBoardView(b);

            var sw = new Stopwatch();

            while (!b.GameOver)
            {
                UI.AddToLog($"{currentPlayer.Player}s turn.");
                sw.Restart();
                var move = currentPlayer.MakeMove(b.GetBoardArray());
                sw.Stop();

                UI.AddToLog($"{currentPlayer.Player} made a move in {sw.ElapsedMilliseconds}ms");

                if(!b.MakeMove(move.X, move.Y, currentPlayer.Player))
                {
                    boardView.AddToLog($"{currentPlayer.Player} made an invalid move ({move}).");
                }

                // Switch player
                currentPlayer = currentPlayer == Black ? White : Black;

                boardView.UpdateUI();
            }
            boardView.GameOver();
        }
    }
}
