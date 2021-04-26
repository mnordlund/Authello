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

            var players = UI.ChoosePlayers();
            var currentPlayer = players.Black;

            var boardView = UI.CreateBoardView(b);

            while (!b.GameOver)
            {
                var move = currentPlayer.MakeMove(b.GetBoardArray());

                if(!b.MakeMove(move.X, move.Y, currentPlayer.Player))
                {
                    Console.WriteLine($"Player {currentPlayer} made an invalid move ({move}).");
                }

                // Switch player
                currentPlayer = currentPlayer == players.Black ? players.White : players.Black;

                boardView.UpdateUI();
            }
            boardView.GameOver();
        }
    }
}
