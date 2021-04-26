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
                        var pos = 0;
                        while(words[i].Length - pos > width)
                        {
                            strings.Add(words[i].Substring(pos, width));
                            pos += width;
                        }

                        if (pos != words[i].Length)
                        {
                            strings.Add(words[i].Substring(pos, words[i].Length - pos) + " ");
                            currentLineLength = strings[strings.Count - 1].Length + 1;
                        }
                        else 
                        {
                            currentLineLength = 0;
                            strings.Add("");
                        }
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
