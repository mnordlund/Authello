using Authello.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authello.ConsoleUI
{
    class UI
    {
        private static BoardView boardView;
        public static BoardView CreateBoardView(Board board)
        {
            boardView = new BoardView(board);
            boardView.CreateUI();
            return boardView;

        }

        public static void AddToLog(string message)
        {
            if (boardView == null) return;

            boardView.AddToLog(message);
        }
        public static (IPlayer Black, IPlayer White) ChoosePlayers()
        {
            var menu = new PlayerMenu();
            return (menu.Show(Tile.Black), menu.Show(Tile.White));
        }
    }
}
