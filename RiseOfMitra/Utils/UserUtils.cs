using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    class UserUtils
    {
        private static void PrintMessage(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ResetColor();
        }

        public static void PrintSucess(string msg)
        {
            PrintMessage(msg, ConsoleColor.Green);
        }

        public static void PrintError(string msg)
        {
            PrintMessage(msg, ConsoleColor.Red);
        }
    }
}
