
using System.Collections.Generic;

namespace Authello.Players
{
    public static class Util
    {
        public static readonly byte[] BitsInByte = { 0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 4, 5, 5, 6, 5, 6, 6, 7, 5, 6, 6, 7, 6, 7, 7, 8, };
        public static readonly byte[] BitPos = { 0x1, 0x2, 0x4, 0x8, 0x10, 0x20, 0x40, 0x80 };
        public static readonly ulong[,] TilePos = {
{0x1, 0x100, 0x10000, 0x1000000, 0x100000000, 0x10000000000, 0x1000000000000, 0x100000000000000, },
{0x2, 0x200, 0x20000, 0x2000000, 0x200000000, 0x20000000000, 0x2000000000000, 0x200000000000000, },
{0x4, 0x400, 0x40000, 0x4000000, 0x400000000, 0x40000000000, 0x4000000000000, 0x400000000000000, },
{0x8, 0x800, 0x80000, 0x8000000, 0x800000000, 0x80000000000, 0x8000000000000, 0x800000000000000, },
{0x10, 0x1000, 0x100000, 0x10000000, 0x1000000000, 0x100000000000, 0x10000000000000, 0x1000000000000000, },
{0x20, 0x2000, 0x200000, 0x20000000, 0x2000000000, 0x200000000000, 0x20000000000000, 0x2000000000000000, },
{0x40, 0x4000, 0x400000, 0x40000000, 0x4000000000, 0x400000000000, 0x40000000000000, 0x4000000000000000, },
{0x80, 0x8000, 0x800000, 0x80000000, 0x8000000000, 0x800000000000, 0x80000000000000, 0x8000000000000000, },
};

        public static bool CompareBoards(Board boardA, Board boardB)
        {
            return boardA.BlackTileMap == boardB.BlackTileMap && boardA.WhiteTileMap == boardB.WhiteTileMap;
        }

        public static int GetScore(Board board, Player player)
        {
            var score = 0;
            var boards = board.UnpackBoards();
            for (int i = 0; i < 8; i++)
            {
                score += BitsInByte[boards[(int)player][i]];
            }
            return score;
        }

        public static Player OtherPlayer(this Player player)
        {
            return (Player)(((int)player + 1) % 2);
        }

        /// <summary>
        /// Makes a move on a board, returns a new Board with the appropriate tiles flipped.
        /// Assumes the move is valid, use IsValidMove to ensure move is valid before calling this function.
        /// </summary>
        public static Board MakeMove(Board board, int x, int y, Player player)
        {
            // Get flip matrix
            ulong flipMatrix = CalculateFlipMatrix(board, x, y, player);

            ulong playerTileMap;
            ulong otherPlayerTileMap;

            if(player == Player.Black)
            {
                playerTileMap = board.BlackTileMap;
                otherPlayerTileMap = board.WhiteTileMap;
            }
            else
            {
                playerTileMap = board.WhiteTileMap;
                otherPlayerTileMap = board.BlackTileMap;

            }

            playerTileMap |= TilePos[x, y];
            playerTileMap |= flipMatrix;
            otherPlayerTileMap &= ~flipMatrix;

            if (player == Player.Black)
            {
                return new Board(playerTileMap, otherPlayerTileMap);
            }
            else
            {
                return new Board(otherPlayerTileMap, playerTileMap);
            }
        }

        private static ulong CalculateFlipMatrix(Board board, int x, int y, Player player, bool breakOnFirstAngle = false)
        {
            ulong playerBoard;
            ulong otherPlayerBoard;

            if(player == Player.Black)
            {
                playerBoard = board.BlackTileMap;
                otherPlayerBoard = board.WhiteTileMap;
            }
            else
            {
                playerBoard = board.WhiteTileMap;
                otherPlayerBoard = board.BlackTileMap;
            }

            bool[,] searchMatrix = { { true, true,  true },
                                     { true, false, true },
                                     { true, true,  true } };

            // Initialized to 0
            ulong[,] flipMatrix = new ulong[3, 3];

            for(int i = 0; i< 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    flipMatrix[i, j] = 0;
                }
            }

            for (int r = 1; r < 8; r++)
            {
                // Stop if we have no searches left
                if (!(searchMatrix[0, 0] || searchMatrix[1, 0] || searchMatrix[2, 0] ||
                    searchMatrix[0, 1] || searchMatrix[1, 1] || searchMatrix[1, 2] ||
                    searchMatrix[0, 2] || searchMatrix[2, 1] || searchMatrix[2, 2]))
                {
                    break;
                }

                for (var dy = -1; dy < 2; dy++)
                {
                    for (var dx = -1; dx < 2; dx++)
                    {
                        if (searchMatrix[dx + 1, dy + 1])
                        {
                            // Are we outside the board?
                            if((x + (dx * r)) < 0 || (x + (dx * r)) >= 8 ||
                                    (y + (dy * r)) < 0 || (y + (dy * r)) >= 8)
                            {
                                flipMatrix[dx + 1, dy + 1] = 0;
                                searchMatrix[dx + 1, dy + 1] = false;
                            }
                            // Are we still on an other player tile?
                            else if ((otherPlayerBoard & TilePos[x + (dx * r), y + (dy * r)]) != 0)
                            {
                                flipMatrix[dx + 1, dy + 1] |= TilePos[x + (dx * r), y + (dy * r)];
                            }
                            // If not then are we on a player tile and more than one tile away from origo?
                            else if ((playerBoard & TilePos[x + (dx * r), y + (dy * r)]) != 0 && r > 1)
                            {
                                searchMatrix[dx + 1, dy + 1] = false;
                                if(breakOnFirstAngle) return flipMatrix[dx + 1, dy + 1];
                            }
                            // Otherwise stop the search
                            else
                            {
                                flipMatrix[dx + 1, dy + 1] = 0;
                                searchMatrix[dx + 1, dy + 1] = false;
                            }
                        }
                    }
                }
            }

            return flipMatrix[0, 0] | flipMatrix[0, 1] | flipMatrix[0, 2] |
                flipMatrix[1, 0] | flipMatrix[1, 1] | flipMatrix[1, 2] |
                flipMatrix[2, 0] | flipMatrix[2, 1] | flipMatrix[2, 2];
        }

        /// <summary>
        /// Lists all valid moves on a given board for a given player.
        /// Returns the moves as an (X, Y) array.
        /// </summary>
        public static (int X, int Y)[] ListAllMoves(Board board, Player player)
        {
            var validMoves = new List<(int X, int Y)>();
            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++)
                {
                    if (IsValidMove(board, x, y, player))
                    {
                        validMoves.Add((x, y));
                    }
                }
            }
            return validMoves.ToArray();
        }
        public static bool IsValidMove(Board board, int x, int y, Player player)
        {
            if (x < 0 || x >= 8 || y < 0 || y >= 8) return false;
            if ((board.BlackTileMap & TilePos[x, y]) != 0 || (board.WhiteTileMap & TilePos[x, y]) != 0) return false;

            // TODO this could be faster if we break on first hit.
            return CalculateFlipMatrix(board, x, y, player, true) != 0;
        }
        public static int GetMoveScore(Board board, int x, int y, Player player)
        {
            var startScore = GetScore(board, player);
            var endScore = GetScore(MakeMove(board, x, y, player), player);

            return endScore - startScore;
        }

        public static int GetWinner(Board board)
        {
            var blackScore = GetScore(board, Player.Black);
            var whiteScore = GetScore(board, Player.White);

            if (blackScore > whiteScore) return (int)Player.Black;
            if (blackScore < whiteScore) return (int)Player.White;

            // Draw
            return 3;
        }

        public static byte[][] UnpackBoards(this Board board)
        {
            var boardArray = new byte[2][];
            boardArray[0] = System.BitConverter.GetBytes(board.BlackTileMap);
            boardArray[1] = System.BitConverter.GetBytes(board.WhiteTileMap);

            return boardArray;
        }

        public static Board packBoard(this byte[][] boardArray)
        {
            Board board = new Board();
            board.BlackTileMap = System.BitConverter.ToUInt64(boardArray[0]);
            board.WhiteTileMap = System.BitConverter.ToUInt64(boardArray[1]);
            return board;
        }
    }
}
