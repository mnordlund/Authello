using Authello.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authello.ConsoleUI
{
    class UI
    {
        public static BoardView CreateBoardView(Board board)
        {
            var bv = new BoardView(board);
            bv.CreateUI();
            return bv;

        }
        public static (IPlayer Black, IPlayer White) ChoosePlayers()
        {
            var menu = new PlayerMenu();
            return (menu.Show(Tile.Black), menu.Show(Tile.White));
        }
    }
}
