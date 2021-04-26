using Authello.Players;
using System;
using Authello.ConsoleUI;
using System.Threading;

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

            while (!b.GameOver)
            {
                var move = currentPlayer.MakeMove(b.GetBoardArray());

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
