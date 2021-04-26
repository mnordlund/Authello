using System;
using System.Drawing;
using System.Threading;

namespace Authello.ConsoleUI
{
    class BoardView
    {
        private static readonly bool DEBUG = false;
        public Board Board { get; set; }
        public bool ShowLog { get; set; } = true;

        // Positions
        private Point blackScorePos;
        private Point whiteScorePos;
        private Point boardPos;
        private Point overlayPos;
        private Point logPos;
        private int logHeight = 5;
        private int logWidth = 50;
        private Point endPos;

        // Colors
        private ConsoleColor blackScoreBg = ConsoleColor.Black;
        private ConsoleColor blackScoreFg = ConsoleColor.White;
        private ConsoleColor whiteScoreBg = ConsoleColor.White;
        private ConsoleColor whiteScoreFg = ConsoleColor.Black;
        private ConsoleColor boardBg = ConsoleColor.DarkGreen;
        private ConsoleColor whitePlayerColor = ConsoleColor.White;
        private ConsoleColor blackPlayerColor = ConsoleColor.Black;

        private ConsoleColor logBg = ConsoleColor.Black;
        private ConsoleColor[] logFg = { ConsoleColor.DarkGray, ConsoleColor.Gray, ConsoleColor.White };

        // State
        private Tile[,] currentBoard;

        private string[] log;

        public BoardView(Board board)
        {
            Board = board;
            CreateUI();
        }

        public void CreateUI()
        {
            log = new string[logHeight];

            Console.ResetColor();
            Console.Clear();

            // Black Score
            Console.BackgroundColor = blackScoreBg;
            Console.ForegroundColor = blackScoreFg;
            Console.Write($"  Black: ");
            blackScorePos = new Point(Console.CursorLeft, Console.CursorTop);
            Console.Write($"  {Board.BlackScore.ToString("00.##")}  ");

            // White Score
            Console.BackgroundColor = whiteScoreBg;
            Console.ForegroundColor = whiteScoreFg;
            Console.Write($"  White: ");
            whiteScorePos = new Point(Console.CursorLeft, Console.CursorTop);
            Console.WriteLine($"  {Board.WhiteScore.ToString("00.##")}  ");

            // Board
            Console.ResetColor();
            Console.WriteLine("  A B C D E F G H");
            boardPos = new Point(Console.CursorLeft + 2, Console.CursorTop);
            overlayPos = new Point(1, boardPos.Y + 2);

            currentBoard = Board.GetBoardArray();

            for (var y = 0; y < Board.BoardSize; y++)
            {
                Console.ResetColor();
                Console.Write($"{y + 1} ");
                Console.BackgroundColor = boardBg;

                for (var x = 0; x < Board.BoardSize; x++)
                {
                    switch (currentBoard[x, y])
                    {
                        case Tile.None:
                            Console.Write("  ");
                            break;
                        case Tile.White:
                            Console.ForegroundColor = whitePlayerColor;
                            Console.Write("O ");
                            break;
                        case Tile.Black:
                            Console.ForegroundColor = blackPlayerColor;
                            Console.Write("O ");
                            break;
                    }
                }
                Console.WriteLine();
            }

            logPos = new Point(Console.CursorLeft, Console.CursorTop);
            endPos = new Point(Console.CursorLeft, Console.CursorTop + logHeight);
            Console.ResetColor();
            Console.SetCursorPosition(endPos.X, endPos.Y);
        }

        public void UpdateUI()
        {
            // Black Score
            Console.BackgroundColor = blackScoreBg;
            Console.ForegroundColor = blackScoreFg;
            Console.SetCursorPosition(blackScorePos.X, blackScorePos.Y);
            Console.Write($"  {Board.BlackScore.ToString("00.##")}  ");

            // White Score
            Console.BackgroundColor = whiteScoreBg;
            Console.ForegroundColor = whiteScoreFg;
            Console.SetCursorPosition(whiteScorePos.X, whiteScorePos.Y);
            Console.WriteLine($"  {Board.WhiteScore.ToString("00.##")}  ");


            // Board
            Console.BackgroundColor = boardBg;

            var move = Board.LastMove;
            var newBoard = Board.GetBoardArray();

            UpdateTile(move.X, move.Y, newBoard);
            Thread.Sleep(200);

            bool[,] searchMatrix = { {true,true,true},
                                     {true,false,true},
                                     {true,true,true} };

            for (var r = 1; r <= Board.BoardSize; r++)
            {
                Thread.Sleep(200);

                for (var dx = -1; dx <= 1; dx++)
                {
                    for (var dy = -1; dy <= 1; dy++)
                    {
                        if (searchMatrix[dx + 1, dy + 1])
                        {
                            searchMatrix[dx + 1, dy + 1] = UpdateTile(move.X + (dx * r), move.Y + (dy * r), newBoard);
                        }
                    }
                }

                if(DEBUG) PrintMatrix<bool>(boardPos.X + 38, boardPos.Y, searchMatrix);

                if (!EvaluateMatrix(searchMatrix)) break;
            }

            if(DEBUG) DrawBoard(boardPos.X + 18, boardPos.Y, newBoard);
            Console.ResetColor();
            Console.SetCursorPosition(endPos.X, endPos.Y);
        }
        public void AddToLog(string message)
        {
            Console.BackgroundColor = logBg;
            for(var i = 0; i < log.Length -1; i++)
            {
                log[i] = log[i + 1];
                Console.SetCursorPosition(logPos.X, logPos.Y + i);
                Console.ForegroundColor = logFg[i < logFg.Length ? i : logFg.Length - 1];
                if (log[i] != null)
                {
                    Console.Write(log[i].PadRight(logWidth).Substring(0, logWidth));
                }
            }

            log[log.Length - 1] = message;
            Console.SetCursorPosition(logPos.X, logPos.Y + log.Length - 1);
            Console.ForegroundColor = logFg[log.Length - 1 < logFg.Length ? log.Length - 1 : logFg.Length - 1];
            Console.Write(log[log.Length - 1].PadRight(logWidth).Substring(0, logWidth));
        }

        public void GameOver()
        {
            Console.SetCursorPosition(overlayPos.X, overlayPos.Y);

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGray;

            Console.Write("+----------------+");
            Console.CursorTop++;
            Console.CursorLeft = overlayPos.X;
            Console.Write("|   Game  Over   |");
            Console.CursorTop++;
            Console.CursorLeft = overlayPos.X;
            var winner = "Draw ";
            if(Board.BlackScore > Board.WhiteScore)
            {
                winner = "Black";
            }
            else if (Board.BlackScore < Board.WhiteScore)
            {
                winner = "White";
            }
            Console.Write($"| Winner:  {winner} |");
            Console.CursorTop++;
            Console.CursorLeft = overlayPos.X;
            Console.Write("+----------------+");


            Console.SetCursorPosition(endPos.X, endPos.Y);
            Console.ResetColor();
        }

        private void DrawBoard(int posx, int posy, Tile[,] board)
        {
            Console.BackgroundColor = boardBg;

            for(var y = 0; y < board.GetLength(1); y++)
            {
                Console.SetCursorPosition(posx, posy + y);
                for (var x = 0; x < board.GetLength(0); x++)
                {
                    switch(board[x,y])
                    {
                        case Tile.Black:
                            Console.ForegroundColor = blackPlayerColor;
                            break;
                        case Tile.White:
                            Console.ForegroundColor = whitePlayerColor;
                            break;
                        case Tile.None:
                            Console.ForegroundColor = boardBg;
                            break;
                    }

                    Console.Write("O ");
                }
            }
        }

        private bool EvaluateMatrix(bool[,] matrix)
        {
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    if (matrix[x, y]) return true;
                }
            }
            return false;
        }

        private void PrintMatrix<T>(int posx, int posy, T[,] matrix)
        {
            for (var y = 0; y < matrix.GetLength(1); y++)
            {
                Console.SetCursorPosition(posx, posy + y);
                for (var x = 0; x < matrix.GetLength(0); x++)
                {
                    Console.Write($"{matrix[x, y]}, ");
                }
            }
        }

        private bool UpdateTile(int x, int y, Tile[,] newBoard)
        {
            if (x < 0 || y < 0 || x >= Board.BoardSize || y >= Board.BoardSize) return false;

            if (newBoard[x, y] != currentBoard[x, y])
            {
                // Draw
                Console.SetCursorPosition(boardPos.X + (x * 2), boardPos.Y + y);
                switch (newBoard[x, y])
                {
                    case Tile.Black:
                        Console.ForegroundColor = blackPlayerColor;
                        Console.Write("O ");
                        break;
                    case Tile.White:
                        Console.ForegroundColor = whitePlayerColor;
                        Console.Write("O ");
                        break;
                    case Tile.None:
                        Console.Write("  ");
                        break;
                }

                currentBoard[x, y] = newBoard[x, y];
                return true;
            }
            return false;
        }
    }
}
