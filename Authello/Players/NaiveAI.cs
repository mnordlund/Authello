using Authello.ConsoleUI;

namespace Authello.Players
{
    class NaiveAI : IPlayer
    {
        public Player Player { get; set; }
        public string PlayerDescription => @"A Naive implementation that only tries to maximize the score given by each move.";

        public string PlayerName => "Naive AI";

        public (int X, int Y) MakeMove(Board board)
        {
            (int X, int Y) bestMove = (-1, -1);
            int bestMoveScore = 0;


            var moves = Util.ListAllMoves(board, Player);

            foreach(var move in moves)
            {
                var moveScore = Util.GetMoveScore(board, move.X, move.Y, Player);
                if (moveScore > bestMoveScore)
                {
                    bestMove = move;
                    bestMoveScore = moveScore;
                }
            }

            UI.AddToLog($"Best move found {bestMove}");
            return bestMove;
        }
    }
}
