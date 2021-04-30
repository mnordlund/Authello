
using System.Collections.Generic;

namespace Authello.Players
{
    static class Util
    {
        public static bool CompareBoards(Tile[,] boardA, Tile[,] boardB)
        {
            if (boardA.GetLength(0) != boardB.GetLength(0) || boardA.GetLength(1) != boardB.GetLength(1)) return false;

            for(var x = 0; x < boardA.GetLength(0); x++)
            {
                for(var y =0; y < boardB.GetLength(1); y++)
                {
                    if (boardA[x, y] != boardB[x, y]) return false;
                }
            }
            return true;
        }

        public static Tile GetWinner(Tile[,] board)
        {
            var white = 0;
            var black = 0;

            for (var x = 0; x < board.GetLength(0); x++)
            {
                for (var y = 0; y < board.GetLength(1); y++)
                {
                    switch (board[x, y])
                    {
                        case Tile.Black:
                            black++;
                            break;
                        case Tile.White:
                            white++;
                            break;
                    }
                }
            }
            if (white == black) return Tile.None;
            if (white < black) return Tile.Black;
            return Tile.White;
        }

        public static Tile[,] MakeMove(Tile[,] board, int x, int y, Tile player)
        {
            var retBoard = (Tile[,])board.Clone();
            var otherPlayer = player == Tile.Black ? Tile.White : Tile.Black;

            bool[,] searchMatrix = { {true,true,true},
                                     {true,false,true},
                                     {true,true,true} };

            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    if (searchMatrix[dx + 1, dy + 1])
                    {
                        searchMatrix[dx + 1, dy + 1] = checkDirection(retBoard, x, y, dx, dy, player, otherPlayer) != 0;
                    }
                }
            }

            retBoard[x, y] = player;

            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    if (searchMatrix[dx + 1, dy + 1])
                    {
                        swapDirection(retBoard, x, y, dx, dy, player, otherPlayer);
                    }
                }
            }

            return retBoard;
        }

        public static Tile OtherPlayer(this Tile player)
        {
            return player == Tile.Black ? Tile.White : Tile.Black;
        }

        public static (int X, int Y)[] ListAllMoves(Tile[,] board, Tile player)
        {
            var movesList = new List<(int X, int Y)>();

            for (var x = 0; x < board.GetLength(0); x++)
            {
                for (var y = 0; y < board.GetLength(1); y++)
                {
                    if (IsValidMove(board, x, y, player)) movesList.Add((x, y));
                }
            }

            //if (movesList.Count == 0) return null;

            return movesList.ToArray();
        }
        public static bool IsValidMove(Tile[,] board, int x, int y, Tile player)
        {
            // Sanity checks
            if (x < 0 || x > board.GetLength(0) || y < 0 || y > board.GetLength(1)) return false;
            if (board[x, y] != Tile.None) return false;
            if (player == Tile.None) return false;

            var otherPlayer = player == Tile.White ? Tile.Black : Tile.White;

            // TODO These could be done in parallel.
            // TODO Use a matrix to make this code nicer.
            return
                checkDirection(board, x, y, 0, -1, player, otherPlayer) > 0 ||
                checkDirection(board, x, y, 0, 1, player, otherPlayer) > 0 ||
                checkDirection(board, x, y, -1, 0, player, otherPlayer) > 0 ||
                checkDirection(board, x, y, 1, 0, player, otherPlayer) > 0 ||
                checkDirection(board, x, y, -1, -1, player, otherPlayer) > 0 ||
                checkDirection(board, x, y, 1, -1, player, otherPlayer) > 0 ||
                checkDirection(board, x, y, -1, 1, player, otherPlayer) > 0 ||
                checkDirection(board, x, y, 1, 1, player, otherPlayer) > 0;
        }
        public static int GetMoveScore(Tile[,] board, int x, int y, Tile player)
        {
            // Sanity checks
            if (x < 0 || x > board.GetLength(0) || y < 0 || y > board.GetLength(1)) return 0;
            if (board[x, y] != Tile.None) return 0;
            if (player == Tile.None) return 0;

            var otherPlayer = player == Tile.White ? Tile.Black : Tile.White;

            // TODO These could be done in parallel
            // TODO Use matrix to make this code nicer
            // N
            var score = checkDirection(board, x, y, 0, -1, player, otherPlayer);
            // S
            score += checkDirection(board, x, y, 0, 1, player, otherPlayer);
            // W
            score += checkDirection(board, x, y, -1, 0, player, otherPlayer);
            // E
            score += checkDirection(board, x, y, 1, 0, player, otherPlayer);
            // NW
            score += checkDirection(board, x, y, -1, -1, player, otherPlayer);
            // NE
            score += checkDirection(board, x, y, 1, -1, player, otherPlayer);
            // SW
            score += checkDirection(board, x, y, -1, 1, player, otherPlayer);
            // SE
            score += checkDirection(board, x, y, 1, 1, player, otherPlayer);

            return score;
        }

        private static int checkDirection(Tile[,] board, int x, int y, int dx, int dy, Tile player, Tile otherPlayer)
        {
            int posx = x + dx;
            int posy = y + dy;
            if (posx < 0 || posx >= board.GetLength(0) || posy < 0 || posy >= board.GetLength(1)) return 0;
            if (board[posx, posy] != otherPlayer) return 0;
            int score = 1;

            while (board[posx, posy] == otherPlayer)
            {
                posx += dx;
                posy += dy;
                score++;

                if (posx < 0 || posx >= board.GetLength(0) || posy < 0 || posy >= board.GetLength(1)) return 0;
            }

            if (board[posx, posy] == player)
            {
                return score;
            }
            return 0;
        }

        private static void swapDirection(Tile[,] board, int x, int y, int dx, int dy, Tile player, Tile otherPlayer)
        {
            int posx = x + dx;
            int posy = y + dy;

            while (board[posx, posy] == otherPlayer)
            {

                board[posx, posy] = player;

                posx += dx;
                posy += dy;

                if (posx < 0 || posx >= board.GetLength(0) || posy < 0 || posy >= board.GetLength(1)) return;
            }
        }
    }
}
