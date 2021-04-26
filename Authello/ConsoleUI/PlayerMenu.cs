using Authello.Players;
using System;

namespace Authello.ConsoleUI
{
    class PlayerMenu
    {
        // State
        private int selectedIndex;
        private int firstItemRow;
        private IPlayer[] playerList;

        // Colors
        private ConsoleColor menuBg = ConsoleColor.Black;
        private ConsoleColor menuFg = ConsoleColor.White;
        private ConsoleColor selectedBg = ConsoleColor.White;
        private ConsoleColor selectedFg = ConsoleColor.Black;

        private void WriteItem(int index)
        {
            if(selectedIndex == index)
            {
                Console.BackgroundColor = selectedBg;
                Console.ForegroundColor = selectedFg;
            }
            else
            {
                Console.BackgroundColor = menuBg;
                Console.ForegroundColor = menuFg;
            }

            Console.SetCursorPosition(0, index + firstItemRow);

            Console.Write($"{index} {playerList[index].PlayerName.PadRight(20).Substring(0, 20)}");
        }

        private void UpdateSelectedIndex(int newSelectedIndex)
        {
            var oldSelectedIndex = selectedIndex;
            selectedIndex = newSelectedIndex;
            WriteItem(oldSelectedIndex);
            WriteItem(selectedIndex);
        }
        public IPlayer Show(Tile playerTile)
        {
            Console.CursorVisible = false;
            Console.ResetColor();
            Console.Clear();

            Console.WriteLine($"Choose {playerTile} player:");
            firstItemRow = Console.CursorTop;
            playerList = PlayerFactory.ListAllIPlayers();
            var index = 0;
            selectedIndex = 0;

            foreach (var player in playerList)
            {
                WriteItem(index);
                index++;
            }

            var retval = SelectLoop(playerList);

            retval.Player = playerTile;
            return retval;
        }

        private IPlayer SelectLoop(IPlayer[] playerList)
        {
            for (; ; )
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    return playerList[selectedIndex];
                }

                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (selectedIndex == 0) continue;

                    UpdateSelectedIndex(selectedIndex - 1);

                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if (selectedIndex == playerList.Length - 1) continue;
                    UpdateSelectedIndex(selectedIndex + 1);
                }
                else
                {
                    var numValue = (int)Char.GetNumericValue(key.KeyChar);
                    if (numValue >= 0 && numValue < playerList.Length)
                    {
                        UpdateSelectedIndex(numValue);
                    }
                }
            }
        }
    }
}
