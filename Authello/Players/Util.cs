
using System.Collections.Generic;

namespace Authello.Players
{
    public static class Util
    {
        public static readonly byte[] BitsInByte ={ 0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 4, 5, 5, 6, 5, 6, 6, 7, 5, 6, 6, 7, 6, 7, 7, 8, };
        public static readonly byte[] BitPos = { 0x80, 0x40, 0x20, 0x10, 0x8, 0x4, 0x2, 0x1 };
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
            var boards = board.UnpackBoards();
            // Set move position
            boards[(int)player][y] |= BitPos[x];

            // Get directions to flip
            var flipMatrix = CalculateFlipMatrix(board, x, y, player);

            // Flip bits
            byte[,] searchMatrix = { { (byte)((BitPos[x] & flipMatrix[0, 0])) , (byte)(BitPos[x] & flipMatrix[0, 1]), (byte)((BitPos[x] & flipMatrix[0, 2])) },
                                     { (byte)((BitPos[x] & flipMatrix[1, 0])), 0x00     , (byte)(BitPos[x] & flipMatrix[1, 2]) },
                                     { (byte)((BitPos[x] & flipMatrix[2, 0])), (byte)(BitPos[x] & flipMatrix[2, 1]), (byte)((BitPos[x] & flipMatrix[2, 2])) } };

             var otherPlayer = (int)player.OtherPlayer();

            for (var ypos = 1; ypos < 8; ypos++)
            {

                for (var dy = -1; dy <= 1; dy++)
                {
                    byte[] directions = {
                    (byte)(searchMatrix[dy + 1, 0] << ypos), // Left
                    searchMatrix[dy + 1, 1], // Straight
                    (byte)(searchMatrix[dy + 1, 2] >> ypos) // Right
                    };

                    // No searches to be done in this direction
                    if (((byte)(directions[0] | directions[1] | directions[2])) == 0x00) continue;

                    var row = y + (dy * ypos);

                    for (var direction = 0; direction < 3; direction++)
                    {
                        if (row < 0 || row >= 8 || directions[direction] == 0x00)
                        {
                            // Stop flipping in this direction
                            searchMatrix[dy + 1, direction] = 0x00;
                            continue;
                        }

                        // Do we still have a match?
                        if ((boards[otherPlayer][row] & directions[direction]) != directions[direction])
                        {
                            // Stop flipping in this direction
                            searchMatrix[dy + 1, direction] = 0x00;
                        }
                        else
                        {
                            // Flip tile
                            boards[otherPlayer][row] &= (byte)~directions[direction];
                            boards[(int)player][row] |= directions[direction];
                        }
                    }
                }
            }

            return boards.packBoard();

        }

        private static byte[,] CalculateFlipMatrix(Board board, int x, int y, Player player, bool breakOnTrue = false)
        {
            var otherPlayer = (int)player.OtherPlayer();

            var boards = board.UnpackBoards();

            byte[,] searchMatrix = { { BitPos[x], BitPos[x], BitPos[x] },
                                     { BitPos[x], 0x00     , BitPos[x] },
                                     { BitPos[x], BitPos[x], BitPos[x] } };

            // Initialized to false
            byte[,] flipMatrix = new byte[3, 3];


            for (var ypos = 1; ypos < 8; ypos++)
            {

                for (var dy = -1; dy <= 1; dy++)
                {
                    byte[] directions = 
                        {
                        (byte)(searchMatrix[dy + 1, 0] << ypos),
                        searchMatrix[dy + 1, 1],
                        (byte)(searchMatrix[dy + 1, 2] >> ypos)
                        };

                    // No searches to be done in this direction
                    if (((byte)(directions[0] | directions[1] | directions[2])) == 0x00) continue;

                    var row = y + (dy * ypos);
                    for ( var direction = 0; direction < 3; direction++)
                    {
                        if (row < 0 || row >= 8 || directions[direction] == 0x00)
                        {
                            // Stop searching in this direction
                            searchMatrix[dy + 1, direction] = 0x00;
                            continue;
                        }

                        // Do we still have a match?
                        if ((boards[otherPlayer][row] & directions[direction]) != directions[direction])
                        {
                            // No check if player tile then we have a match.
                            if (ypos > 1 && (boards[(int)player][row] & directions[direction]) == directions[direction])
                            {
                                // We have a match thus the move is valid.
                                flipMatrix[dy + 1, direction] = 0xFF;

                                if (breakOnTrue) return flipMatrix;

                            }
                            // Stop searching in this direction
                            searchMatrix[dy + 1, direction] = 0x00;
                        }
                    }
                }
            }
            return flipMatrix;
        }

        /// <summary>
        /// Lists all valid moves on a given board for a given player.
        /// Returns the moves as an (X, Y) array.
        /// </summary>
        public static (int X, int Y)[] ListAllMoves(Board board, Player player)
        {
            var validMoves = new List<(int X, int Y)>();
            for(var y = 0; y < 8; y++)
            {
                for(var x = 0; x < 8; x++)
                {
                    if(IsValidMove(board, x, y, player))
                    {
                        validMoves.Add((x, y));
                    }
                }
            }
            return validMoves.ToArray();
        }
        public static bool IsValidMove(Board board, int x, int y, Player player)
        {
            var boards = board.UnpackBoards();
            if (x < 0 || x >= 8 || y < 0 || y >= 8) return false;
            if ((boards[0][y] & BitPos[x]) != 0 || (boards[1][y] & BitPos[x]) != 0) return false;

            var flipMatrix = CalculateFlipMatrix(board, x, y, player, true);

            return (flipMatrix[0, 0] | flipMatrix[0, 1] | flipMatrix[0, 2] |
                    flipMatrix[1, 0] | flipMatrix[1, 1] | flipMatrix[1, 2] |
                    flipMatrix[2, 0] | flipMatrix[2, 1] | flipMatrix[2, 2]  ) == 0xFF;
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
            Board board;
            board.BlackTileMap = System.BitConverter.ToUInt64(boardArray[0]);
            board.WhiteTileMap = System.BitConverter.ToUInt64(boardArray[1]);
            return board;
        }
    }
}
