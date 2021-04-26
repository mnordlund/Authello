
namespace Authello.Players
{
    class Util
    {
        // TODO Add function to list all valid moves given a board and a player.
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
                checkDirection(board, x, y, 0, 1, player, otherPlayer) > 0  ||
                checkDirection(board, x, y, -1, 0, player, otherPlayer) > 0 ||
                checkDirection(board, x, y, 1, 0, player, otherPlayer) > 0  ||
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
    }
}
