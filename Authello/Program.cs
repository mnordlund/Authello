using Authello.Players;
using System;
using System.Threading;

namespace Authello
{
    class Program
    {

        static void TrainWeigthedAIs()
        {
            var matches = 1000000;

            var p1 = new WeightedAI()
            {
                Player = Tile.Black
            };
            var p2 = new WeightedAI()
            {
                Player = Tile.White
            };

            var b = new Board();
            Console.Write("Training AI...");
            p2.mutateWeights();
            for (var matchCount = 0; matchCount < matches; matchCount++)
            {
                var currentPlayer = p1;
                while(!b.GameOver)
                {
                    var move = currentPlayer.MakeMove(b.GetBoardArray());
                    b.MakeMove(move.X, move.Y, currentPlayer.Player);
                    currentPlayer = currentPlayer == p1 ? p2 : p1;
                }
                if(b.getScore(p1.Player) > b.getScore(p2.Player))
                {
                    p2.CloneWeights(p1.weights);
                    p1.mutateWeights();
                }
                else
                {
                    p1.CloneWeights(p2.weights);
                    p2.mutateWeights();
                }
            }
            Console.WriteLine("Done!");

            // We've already flipped the weights
            if(b.getScore(p1.Player) > b.getScore(p2.Player))
            {
                p2.PrintWeights();
            }else
            {
                p1.PrintWeights();
            }

        }

        static void Main(string[] args)
        {
            //Program.TrainWeigthedAIs();
            //return; 

            Program p = new Program();

            IPlayer player1 = PlayerFactory.GetPlayer(Tile.Black);
            IPlayer player2 = PlayerFactory.GetPlayer(Tile.White);

            Board b = new Board();

            IPlayer currentPlayer = player1;
            var UI = new ConsoleUI(b);

            while (!b.GameOver)
            {

                Console.WriteLine($"Current player: {currentPlayer.Player}");

                Thread.Sleep(50);

                var move = currentPlayer.MakeMove(b.GetBoardArray());

                if(!b.MakeMove(move.X, move.Y, currentPlayer.Player))
                {
                    Console.WriteLine($"Player {currentPlayer} made an invalid move ({move}).");
                }

                // Switch player
                currentPlayer = currentPlayer == player1 ? player2 : player1;

                UI.UpdateUI();
            }
            Console.WriteLine($"Game Over");
            if(b.getScore(player1.Player) > b.getScore(player2.Player))
            {
                Console.WriteLine($"Winner: {player1.PlayerDescription}");
            }
            else
            {
                Console.WriteLine($"Winner: {player2.PlayerDescription}");

            }
        }
    }
}
