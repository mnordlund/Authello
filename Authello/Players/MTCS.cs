using Authello.ConsoleUI;
using Authello.Players.MTCSHelpers;
using System;
using System.Diagnostics;
using System.Drawing;

namespace Authello.Players
{
    class MTCS : IPlayer
    {
        public Tile Player { get; set; }

        public string PlayerName => "MCTS";

        public string PlayerDescription =>
@"An implementation of Monte Carlo Tree Search (MCTS).
Based on the description at: https://en.wikipedia.org/wiki/Monte_Carlo_tree_search";

        // Configuration
        public int ThinkTimeLimit { get; set; } = 1000;

        // State
        private TreeNode root;
        public Point MakeMove(Tile[,] board)
        {
            // TODO keep tree from last move
            //root = null;

            // Generate nodes
            if (root == null)
            {
                root = new TreeNode(board, Player.OtherPlayer(), -1, -1);
                UI.AddToLog("Created new tree.");
            }
            else
            {
                root = root.GetChildWithBoard(board);
                if(root == null)
                {
                    root = new TreeNode(board, Player.OtherPlayer(), -1, -1);
                    UI.AddToLog("Created new tree.");
                }
                else
                {
                    UI.AddToLog($"Found tree with {root.NodeCount} nodes.");
                }
            }

            // Visit nodes
            var sw = new Stopwatch();
            var visitCounter = 0;
            sw.Restart();
            while(sw.ElapsedMilliseconds < ThinkTimeLimit)
            {
                if (root.IsComplete) break;

                root.Visit();
                visitCounter++;
            }
            sw.Stop();

            UI.AddToLog($"Made {visitCounter} visits in {sw.ElapsedMilliseconds / 1000.0:N2} seconds.");
            UI.AddToLog($"TreeSize: {root.NodeCount} nodes");

            // Select move
            var bestMove = root.BestMove;
            root = root.BestChild;
            return new Point(bestMove.X, bestMove.Y);

        }


    }
}
