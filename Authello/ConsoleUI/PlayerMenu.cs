using Authello.Players;
using System;
using System.Drawing;

namespace Authello.ConsoleUI
{
    class PlayerMenu
    {
        // State
        private int selectedIndex;
        private int firstItemRow;
        private IPlayer[] iplayerList;

        // Colors
        private ConsoleColor menuBg = ConsoleColor.Black;
        private ConsoleColor menuFg = ConsoleColor.White;
        private ConsoleColor selectedBg = ConsoleColor.White;
        private ConsoleColor selectedFg = ConsoleColor.Black;
        private ConsoleColor descriptionBg = ConsoleColor.DarkGray;
        private ConsoleColor descriptionFg = ConsoleColor.White;

        // Positons and sizes
        private Point descriptionPoint;
        private int DescriptionWidth = 60;
        private int DescriptionHeight = 10;
        private int itemWidth = 20;

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

            Console.Write($"{index} {iplayerList[index].PlayerName.setLength(itemWidth)}");
        }

        private void UpdateSelectedIndex(int newSelectedIndex)
        {
            var oldSelectedIndex = selectedIndex;
            selectedIndex = newSelectedIndex;
            WriteItem(oldSelectedIndex);
            WriteItem(selectedIndex);

            UpdateDescription();
        }
        public IPlayer Show(Player player)
        {
            Console.CursorVisible = false;
            Console.ResetColor();
            Console.Clear();

            Console.WriteLine($"Choose {player} player:");
            firstItemRow = Console.CursorTop;
            descriptionPoint = new Point(itemWidth + 5, Console.CursorTop);
            iplayerList = PlayerFactory.ListAllIPlayers();
            var index = 0;
            selectedIndex = 0;

            foreach (var iplayer in iplayerList)
            {
                WriteItem(index);
                index++;
            }

            UpdateDescription();

            var retval = SelectLoop(iplayerList);

            retval.Player = player;
            return retval;
        }

        private void UpdateDescription()
        {
            Console.BackgroundColor = descriptionBg;
            Console.ForegroundColor = descriptionFg;

            var desc = iplayerList[selectedIndex].PlayerDescription.transformToRows(DescriptionWidth);
            for(int h = 0; h < DescriptionHeight; h++)
            {
                Console.SetCursorPosition(descriptionPoint.X, descriptionPoint.Y + h);

                if (h < desc.Length)
                {
                    Console.Write(desc[h].setLength(DescriptionWidth));
                }
                else
                {
                    Console.Write(" ".setLength(DescriptionWidth));
                }

            }
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
