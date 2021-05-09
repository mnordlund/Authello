namespace Authello
{
    public class Board
    {
        public ulong BlackTileMap;
        public ulong WhiteTileMap;

        public Board()
        {
            BlackTileMap = 0;
            WhiteTileMap = 0;
        }

        public Board(Board board)
        {
            board.BlackTileMap = BlackTileMap;
            board.WhiteTileMap = WhiteTileMap;
        }

        public Board(ulong blackTileMap, ulong whiteTileMap)
        {
            BlackTileMap = blackTileMap;
            WhiteTileMap = whiteTileMap;
        }
    };
}
