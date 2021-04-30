using Authello.ConsoleUI;
using System;
using System.Linq;

namespace Authello.Players.MTCSHelpers
{
    class TreeNode
    {

        public TreeNode Parent { get; }
        private Tile[,] Board { get; }
        public Tile player { get; }

        public (int X, int Y) Move { get; }

        public double Wins { get; private set; }
        public double Visits { get; private set; }

        private TreeNode[] Children { get; set; }
        public bool IsLeaf { get; private set; } = false;
        public Tile Winner { get; private set; } = Tile.None;

        public bool IsComplete { get; private set; }
        public (int X, int Y) BestMove => GetBestMove();

        public double ExplorationWeight { get; set; } = 1.414213562373095;

        public TreeNode BestChild { get; private set; }

        public int NodeCount => _nodeCount();

        private int _nodeCount()
        {
            if (IsLeaf || Children == null) return 1;

            return Children.Sum((item) => item.NodeCount);
        }

        public TreeNode(Tile[,] board, Tile player, int x, int y, TreeNode parent = null)
        {
            this.player = player;
            Move = (x, y);
            Board = board;
            Parent = parent;
        }

        public void GenerateChildren()
        {
            var moves = Util.ListAllMoves(Board, player.OtherPlayer());

            if (moves.Length == 0)
            {
                if (Util.ListAllMoves(Board, player.OtherPlayer()).Length != 0)
                {
                    // The only valid move is a pass that let's the oponent do its move.
                    Children = new TreeNode[1];
                    Children[0] = new TreeNode(Board, player.OtherPlayer(), -1, -1, this);
                }
                else
                {
                    IsLeaf = true;
                    IsComplete = true;
                    Winner = Util.GetWinner(Board);
                }
            }

            Children = new TreeNode[moves.Length];

            for (var i = 0; i < moves.Length; i++)
            {
                Children[i] = new TreeNode(Util.MakeMove(Board, moves[i].X, moves[i].Y, player.OtherPlayer()), player.OtherPlayer(), moves[i].X, moves[i].Y, this);
            }
        }

        public Tile Visit()
        {
            if(Children == null && !IsLeaf)
            {
                GenerateChildren();
            }

            if (IsLeaf)
            {
                return Winner;
            }

            var nextChild = Children.FirstOrDefault(item => item.Visits == 0);

            if (nextChild == null)
            {
                // Choose child
                nextChild = Children.Aggregate((child1, child2) => (child1.IsComplete ? child2 : ((child1.Wins / child1.Visits) + ExplorationWeight * Math.Sqrt(Math.Log(Visits) / child1.Visits)) >
                                                                       ((child2.Wins / child2.Visits) + ExplorationWeight * Math.Sqrt(Math.Log(Visits) / child2.Visits)) ? child1 : child2));
            }

            var winner = nextChild.Visit();

            if(winner == player)
            {
                Wins++;
                //scores[nextNode].Wins++;
            }

            //if(Children[nextNode].IsComplete)
            if(nextChild.IsComplete)
            {
                IsComplete = Children.All(child => child.IsComplete);
            }

            //scores[nextNode].Visits++;
            Visits++;

            // Pruning
            if(Visits > 1000 && (Wins / Visits) < 0.3)
            {
                IsComplete = true;
            }

            return winner;
        }

        private (int X, int Y) GetBestMove()
        {
            if (IsLeaf) return (-1, -1);

            var bestMove = 0;
            var bestScore = -1.0;

            for(var i = 0; i < Children.Length; i++)
            {
                if (Children[i].Visits == 0) continue;

                if(((double)Children[i].Wins / (double)Children[i].Visits) > bestScore)
                {
                    bestMove = i;
                }
            }

            var probToWin = (double)Children[bestMove].Wins / (double)Children[bestMove].Visits;
            UI.AddToLog($"Best move has: {probToWin:N4} chance of win.");

            BestChild = Children[bestMove];
            return Children[bestMove].Move;
        }

        public TreeNode GetChildWithBoard(Tile[,] board)
        {
            if (Children == null) return null;

            foreach(var child in Children)
            {
                if (child == null) continue;
                if (Util.CompareBoards(board, child.Board)) return child;
            }
            return null;
        }
    }
}
