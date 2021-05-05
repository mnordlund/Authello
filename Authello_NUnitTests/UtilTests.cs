using Authello;
using Authello.Players;
using NUnit.Framework;

namespace Authello_NUnitTests
{
    public class UtilTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EastFlippingMove()
        {
            /* Initial board
             * -WWWWB--
             * --------
             * --------
             * --------
             * --------
             * --------
             * --------
             * 
             * Avalable move 0,0
             * Make move
             * 
             * Final board
             * BBBBBB--
             * --------
             * --------
             * etc.
             * 
             */
            // Setup
            Board board = new Board();
            board.BlackTileMap = 0x4;
            board.WhiteTileMap = 0x78;

            (int X, int y)[] availableMoves = Util.ListAllMoves(board, Player.Black);

            bool isValidMove = Util.IsValidMove(board, 0, 0, Player.Black);

            Board newBoard = Util.MakeMove(board, 0, 0, Player.Black);

            // Available moves
            Assert.AreEqual(1, availableMoves.Length);
            Assert.AreEqual((0, 0), availableMoves[0]);

            // The move is valid
            Assert.IsTrue(isValidMove);

            // Move should not affect the original board.
            Assert.AreNotSame(board, newBoard);
            Assert.AreEqual(0x4, board.BlackTileMap);
            Assert.AreEqual(0x78, board.WhiteTileMap);

            // The move has resulted in the correct board.
            Assert.AreEqual(0x0, newBoard.WhiteTileMap);
            Assert.AreEqual(0xFC, newBoard.BlackTileMap);
        }

        [Test]
        public void WestFlippingMove()
        {
            /* Initial board
             * --BWWWW-
             * --------
             * --------
             * --------
             * --------
             * --------
             * --------
             * 
             * Avalable move 7,0
             * Make move
             * 
             * Final board
             * --WWWWWW
             * --------
             * --------
             * etc.
             * 
             */
            // Setup
            Board board = new Board();
            board.BlackTileMap = 0x20;
            board.WhiteTileMap = 0x1E;

            (int X, int y)[] availableMoves = Util.ListAllMoves(board, Player.Black);

            bool isValidMove = Util.IsValidMove(board, 7, 0, Player.Black);

            Board newBoard = Util.MakeMove(board, 7, 0, Player.Black);

            // Available moves
            Assert.AreEqual(1, availableMoves.Length);
            Assert.AreEqual((7, 0), availableMoves[0]);

            // The move is valid
            Assert.IsTrue(isValidMove);

            // Move should not affect the original board.
            Assert.AreNotSame(board, newBoard);
            Assert.AreEqual(0x20, board.BlackTileMap);
            Assert.AreEqual(0x1E, board.WhiteTileMap);

            // The move has resulted in the correct board.
            Assert.AreEqual(0x0, newBoard.WhiteTileMap);
            Assert.AreEqual(0x3F, newBoard.BlackTileMap);
        }

        [Test]
        public void NorthFlippingMove()
        {
            /* Initial board
             * --------
             * --------
             * B-------
             * W-------
             * W-------
             * W-------
             * --------
             * 
             * Avalable move 0,7
             * Make move
             * 
             * Final board
             * --------
             * --------
             * --------
             * B-------
             * B-------
             * B-------
             * B-------
             * B-------
             * 
             */
            // Setup
            Board board = new Board();
            board.BlackTileMap = 0x80000000;
            board.WhiteTileMap = 0x80808000000000;

            (int X, int y)[] availableMoves = Util.ListAllMoves(board, Player.Black);

            bool isValidMove = Util.IsValidMove(board, 0, 7, Player.Black);

            Board newBoard = Util.MakeMove(board, 0, 7, Player.Black);

            // Available moves
            Assert.AreEqual(1, availableMoves.Length);
            Assert.AreEqual((0, 7), availableMoves[0]);

            // The move is valid
            Assert.IsTrue(isValidMove);

            // Move should not affect the original board.
            Assert.AreNotSame(board, newBoard);
            Assert.AreEqual(0x80000000, board.BlackTileMap);
            Assert.AreEqual(0x80808000000000, board.WhiteTileMap);

            // The move has resulted in the correct board.
            Assert.AreEqual(0x0, newBoard.WhiteTileMap);
            Assert.AreEqual(0x8080808080000000, newBoard.BlackTileMap);
        }

        [Test]
        public void SouthFlippingMove()
        {
            /* Initial board
             * --------
             * -------W
             * -------W
             * -------W
             * -------B
             * --------
             * --------
             * 
             * Avalable move 7,0
             * Make move
             * 
             * Final board
             * -------B
             * -------B
             * -------B
             * -------B
             * -------B
             * -------B
             * --------
             * --------
             * 
             */
            // Setup
            Board board = new Board();
            board.BlackTileMap = 0x100000000;
            board.WhiteTileMap = 0x1010100;

            (int X, int y)[] availableMoves = Util.ListAllMoves(board, Player.Black);

            bool isValidMove = Util.IsValidMove(board, 7, 0, Player.Black);

            Board newBoard = Util.MakeMove(board, 7, 0, Player.Black);

            // Available moves
            Assert.AreEqual(1, availableMoves.Length);
            Assert.AreEqual((7, 0), availableMoves[0]);

            // The move is valid
            Assert.IsTrue(isValidMove);

            // Move should not affect the original board.
            Assert.AreNotSame(board, newBoard);
            Assert.AreEqual(0x100000000, board.BlackTileMap);
            Assert.AreEqual(0x1010100, board.WhiteTileMap);

            // The move has resulted in the correct board.
            Assert.AreEqual(0x0, newBoard.WhiteTileMap);
            Assert.AreEqual(0x101010101, newBoard.BlackTileMap);
        }

        [Test]
        public void NWFlippingMove()
        {
            /* Initial board
             * B-------
             * -W------
             * --W-----
             * ---W----
             * --------
             * --------
             * --------
             * 
             * Avalable move 4,4
             * Make move
             * 
             * Final board
             * B-------
             * -B------
             * --B-----
             * ---B----
             * ----B---
             * --------
             * --------
             * --------
             * 
             */
            // Setup
            Board board = new Board();
            board.BlackTileMap = 0x80;
            board.WhiteTileMap = 0x10204000;

            (int X, int y)[] availableMoves = Util.ListAllMoves(board, Player.Black);

            bool isValidMove = Util.IsValidMove(board, 4, 4, Player.Black);

            Board newBoard = Util.MakeMove(board, 4, 4, Player.Black);

            // Available moves
            Assert.AreEqual(1, availableMoves.Length);
            Assert.AreEqual((4, 4), availableMoves[0]);

            // The move is valid
            Assert.IsTrue(isValidMove);

            // Move should not affect the original board.
            Assert.AreNotSame(board, newBoard);
            Assert.AreEqual(0x80, board.BlackTileMap);
            Assert.AreEqual(0x10204000, board.WhiteTileMap);

            // The move has resulted in the correct board.
            Assert.AreEqual(0x0, newBoard.WhiteTileMap);
            Assert.AreEqual(0x810204080, newBoard.BlackTileMap);
        }

        [Test]
        public void SEFlippingMove()
        {
            /* Initial board
             * --------
             * -W------
             * --W-----
             * ---W----
             * ----B---
             * --------
             * --------
             * 
             * Avalable move 0,0
             * Make move
             * 
             * Final board
             * B-------
             * -B------
             * --B-----
             * ---B----
             * ----B---
             * --------
             * --------
             * --------
             * 
             */
            // Setup
            Board board = new Board();
            board.BlackTileMap = 0x800000000;
            board.WhiteTileMap = 0x10204000;

            (int X, int y)[] availableMoves = Util.ListAllMoves(board, Player.Black);

            bool isValidMove = Util.IsValidMove(board, 0, 0, Player.Black);

            Board newBoard = Util.MakeMove(board, 0, 0, Player.Black);

            // Available moves
            Assert.AreEqual(1, availableMoves.Length);
            Assert.AreEqual((0, 0), availableMoves[0]);

            // The move is valid
            Assert.IsTrue(isValidMove);

            // Move should not affect the original board.
            Assert.AreNotSame(board, newBoard);
            Assert.AreEqual(0x800000000, board.BlackTileMap);
            Assert.AreEqual(0x10204000, board.WhiteTileMap);

            // The move has resulted in the correct board.
            Assert.AreEqual(0x0, newBoard.WhiteTileMap);
            Assert.AreEqual(0x810204080, newBoard.BlackTileMap);
        }

        [Test]
        public void BlackStarFlippingMove()
        {
            /* Initial board
             * -B--B--B
             * --W-W-W-
             * ---WWW--
             * BWWW-WWB
             * ---WWW--
             * --W-W-W-
             * -B--B--B
             * 
             * Avalable move 4, 3
             * Make move
             * 
             * Final board
             * -B--B--B
             * --B-B-B-
             * ---BBB--
             * BBBBBBBB
             * ---BBB--
             * --B---B-
             * -B-----B
             * 
             */
            // Setup
            Board board = new Board();
            board.BlackTileMap = 0x49000081000049;
            board.WhiteTileMap = 0x2A1C761C2A00;

            int scoreBlack = Util.GetScore(board, Player.Black);
            int scoreWhite = Util.GetScore(board, Player.White);

            (int X, int y)[] availableMoves = Util.ListAllMoves(board, Player.Black);

            bool isValidMove = Util.IsValidMove(board, 4, 3, Player.Black);

            int moveScore = Util.GetMoveScore(board, 4, 3, Player.Black);

            Board newBoard = Util.MakeMove(board, 4, 3, Player.Black);

            int newScoreBlack = Util.GetScore(newBoard, Player.Black);
            int newScoreWhite = Util.GetScore(newBoard, Player.White);

            // Available moves
            Assert.AreEqual(1, availableMoves.Length);
            Assert.AreEqual((4, 3), availableMoves[0]);

            // The move is valid
            Assert.IsTrue(isValidMove);

            // Move should not affect the original board.
            Assert.AreNotSame(board, newBoard);
            Assert.AreEqual(0x49000081000049, board.BlackTileMap);
            Assert.AreEqual(0x2A1C761C2A00, board.WhiteTileMap);

            // The move has resulted in the correct board.
            Assert.AreEqual(0x0, newBoard.WhiteTileMap);
            Assert.AreEqual(0x492A1CFF1C2A49, newBoard.BlackTileMap);

            // Scores
            Assert.AreEqual(8, scoreBlack);
            Assert.AreEqual(17, scoreWhite);
            Assert.AreEqual(18, moveScore);
            Assert.AreEqual(26, newScoreBlack);
            Assert.AreEqual(0, newScoreWhite);
        }


        [Test]
        public void WhiteStarFlippingMove()
        {
            /* Initial board
             * -W--W--W
             * --B-B-B-
             * ---BBB--
             * WBBB-BBW
             * ---BBB--
             * --B-B-B-
             * -W--W--W
             * 
             * AvalaWlB movB 4, 3
             * MakB movB
             * 
             * Final Woard
             * -W--W--W
             * --W-W-W-
             * ---WWW--
             * WWWWWWWW
             * ---WWW--
             * --W---W-
             * -W-----W
             * 
             */
            // Setup
            Board board = new Board();
            board.BlackTileMap = 0x2A1C761C2A00;
            board.WhiteTileMap = 0x49000081000049;

            int scoreBlack = Util.GetScore(board, Player.Black);
            int scoreWhite = Util.GetScore(board, Player.White);

            (int X, int y)[] availableMoves = Util.ListAllMoves(board, Player.White);

            bool isValidMove = Util.IsValidMove(board, 4, 3, Player.White);

            int moveScore = Util.GetMoveScore(board, 4, 3, Player.White);

            Board newBoard = Util.MakeMove(board, 4, 3, Player.White);

            int newScoreBlack = Util.GetScore(newBoard, Player.Black);
            int newScoreWhite = Util.GetScore(newBoard, Player.White);

            // Available moves
            Assert.AreEqual(1, availableMoves.Length);
            Assert.AreEqual((4, 3), availableMoves[0]);

            // The move is valid
            Assert.IsTrue(isValidMove);

            // Move should not affect the original board.
            Assert.AreNotSame(board, newBoard);
            Assert.AreEqual(0x2A1C761C2A00, board.BlackTileMap);
            Assert.AreEqual(0x49000081000049, board.WhiteTileMap);

            // The move has resulted in the correct board.
            Assert.AreEqual(0x492A1CFF1C2A49, newBoard.WhiteTileMap);
            Assert.AreEqual(0x0, newBoard.BlackTileMap);

            // Scores
            Assert.AreEqual(17, scoreBlack);
            Assert.AreEqual(8, scoreWhite);
            Assert.AreEqual(18, moveScore);
            Assert.AreEqual(0, newScoreBlack);
            Assert.AreEqual(26, newScoreWhite);
        }
    }
}