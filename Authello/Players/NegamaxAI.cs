using Authello.ConsoleUI;
using System;

namespace Authello.Players
{

    class NegamaxAI : IPlayer
    {
        class TreeNode
        {
            public bool IsLeaf;
            public Board board;
            public TreeNode[] children;
            public TreeNode bestChild;
            public (int x, int y) move;
        }

        public Player Player { get; set; }

        public string PlayerName => "Negamax";

        public string PlayerDescription => @"
An implementation of the negamax algorithm with alpha beta pruning. Based on the discription given in: https://en.wikipedia.org/wiki/Negamax.
";

        public int Depth { get; set; } = 5;

        private TreeNode Root { get; set; }

        public (int X, int Y) MakeMove(Board board)
        {
            if (Root == null)
            {
                Root = new TreeNode()
                {
                    board = board
                };
            }
            else
            {
                var candidates = Root.bestChild.children;

                Root = null;
                foreach(var candidate in candidates)
                {
                    if(Util.CompareBoards(candidate.board, board))
                    {
                        UI.AddToLog("Found partial tree");
                        Root = candidate;
                        break;
                    }
                }

                if(Root == null)
                {
                    Root = new TreeNode()
                    {
                        board = board
                    };
                }
            }

            var result = Negamax(Root, Depth, double.NegativeInfinity, double.PositiveInfinity, Player);
            UI.AddToLog($"Negamax result: {result}");

            return GetBestMove(Root);
        }

        private void CreateChildren(TreeNode node, Player player)
        {
            var moves = Util.ListAllMoves(node.board, player);
            if (moves.Length == 0)
            {
                if (Util.ListAllMoves(node.board, player.OtherPlayer()).Length != 0)
                {
                    node.children = new TreeNode[1];
                    node.children[0] = new TreeNode()
                    {
                        board = node.board,
                        move = (-1, -1)
                    };
                }
                else
                {
                    node.IsLeaf = true;
                }
                return;
            }
            node.children = new TreeNode[moves.Length];

            for(var i = 0; i < moves.Length; i++)
            {
                node.children[i] = new TreeNode()
                {
                    board = Util.MakeMove(node.board, moves[i].X, moves[i].Y, player),
                    move = moves[i],
                };

            }
        }

        private double Negamax(TreeNode node, int depth, double alpha, double beta, Player player)
        {

            if (node.children == null)
            {
                CreateChildren(node, player);
            }

            if (depth == 0 || node.IsLeaf)
            {
                return GetHeuristicValue(node, player);
            }


            var value = Double.NegativeInfinity;

            foreach(var child in node.children)
            {
                var negamax = -Negamax(child, depth - 1, -beta, -alpha, player.OtherPlayer());
                if (value < negamax)
                {
                    value = negamax;
                    node.bestChild = child;
                }

                alpha = Math.Max(alpha, value);
                if (alpha >= beta)
                    break;
            }
            if (node.bestChild == null && node.children.Length != 0)
            {
                UI.AddToLog("Found no best child");
            }
            return value;
        }

        private double GetHeuristicValue(TreeNode node, Player player)
        {
            var score = Util.GetScore(node.board, player);
            var otherPlayerScore = Util.GetScore(node.board, player.OtherPlayer());

            return (double)score/(double)otherPlayerScore;
        }

        private (int X, int Y) GetBestMove(TreeNode node)
        {
            return node.bestChild.move;
        }
    }
}
