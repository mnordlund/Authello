using Authello.ConsoleUI;
using System;
using System.Drawing;

namespace Authello.Players
{

    class NegamaxAI : IPlayer
    {
        class TreeNode
        {
            public bool IsLeaf;
            public Tile[,] board;
            public TreeNode[] children;
            public TreeNode bestChild;
            public (int x, int y) move;
        }

        public Tile Player { get; set; }

        public string PlayerName => "Negamax";

        public string PlayerDescription => @"
An implementation of the negamax algorithm. Based on the discription given in: https://en.wikipedia.org/wiki/Negamax.
";

        public int Depth { get; set; } = 10;

        private TreeNode Root { get; set; }

        public Point MakeMove(Tile[,] board)
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

            var result = Negamax(Root, Depth,Double.MinValue, Double.MaxValue, Player);
            UI.AddToLog($"Negamax result: {result}");

            return GetBestMove(Root);
        }

        private void CreateChildren(TreeNode node, Tile player)
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

        private double Negamax(TreeNode node, int depth, double alpha, double beta, Tile player)
        {
            if(depth == 0 || node.IsLeaf)
            {
                var multiplier = player == Player ? 1 : -1;
                return multiplier * GetHeuristicValue(node, player);
            }

            if(node.children == null)
            {
                CreateChildren(node, player);
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
            return value;
        }

        private double GetHeuristicValue(TreeNode node, Tile player)
        {
            // TODO Use weights in heuristics.
            var score = 0;
            var otherPlayerScore = 0;

            var otherPlayer = player.OtherPlayer();
            foreach(var tile in node.board)
            {
                if (tile == player) score++;
                if (tile == otherPlayer) otherPlayerScore++;
            }

            return (double)score/(double)otherPlayerScore;
        }

        private Point GetBestMove(TreeNode node)
        {
            return new Point(node.bestChild.move.x, node.bestChild.move.y);
        }
    }
}
