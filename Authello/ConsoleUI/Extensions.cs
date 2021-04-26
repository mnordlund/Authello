using System;
using System.Collections.Generic;
using System.Text;

namespace Authello.ConsoleUI
{
    static class Extensions
    {
        public static string setLength(this string str, int len)
        {
            return str.PadRight(len).Substring(0, len);
        }

        public static string[] transformToRows(this string str, int width)
        {
            var strings = new List<string>();

            var words = str.Split(' ');
            var currentLineLength = 0;
            strings.Add("");
            for(int i = 0; i < words.Length; i++)
            {
                if(currentLineLength + words[i].Length <= width)
                {
                    currentLineLength += words[i].Length + 1;
                    strings[strings.Count - 1] += words[i] + " ";
                }
                else
                {

                    if (words[i].Length > width)
                    {
                        currentLineLength = 0;
                        strings.Add(words[i].Substring(0, width));
                        strings.Add("");
                    }
                    else
                    {
                        currentLineLength = words[i].Length + 1;
                        strings.Add(words[i] + " ");
                    }
                }
            }
            return strings.ToArray();
        }
    }
}
