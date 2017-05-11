using System.Linq;
using System.Collections.Generic;
using System;

namespace RoMUtils
{
    class BoardStrings
    {
        public const string CHAR_DALRION_PAWN = "$";
        public const string CHAR_RAHKAR_PAWN = "#";
        public const string CHAR_DALRION_CENTER = "@";
        public const string CHAR_RAHKAR_CENTER = "%";
        public const string EMPTY = ".";
        public const string FOG = "-";
        public const string BLOCKED = "X";

        private static string[] NON_VALID_UNITS = { ".", "X", "-"};
        private static string[] DalrionChar = { "$", "@" };
        private static string[] RahkarChar = { "#", "%" };
        private static Dictionary<ECultures, ConsoleColor> CultColors = new Dictionary<ECultures, ConsoleColor>
        {
            { ECultures.DALRIONS, ConsoleColor.Blue },
            { ECultures.RAHKARS, ConsoleColor.Yellow }
        };
        private static Dictionary<string, bool> Cmds = new Dictionary<string, bool>
        {
            { Commands.ATTACK, true },
            { Commands.MOVE, true },
            { Commands.CONQUER, false },
            { Commands.EXIT, true }
        };
        private static string[] Legend = new string[]
        {
            "Dalrion pawn: " + CHAR_DALRION_PAWN,
            "Dalrion center: " + CHAR_DALRION_CENTER,
            "Rahkar pawn: " + CHAR_RAHKAR_PAWN,
            "Rahkar center: " + CHAR_RAHKAR_CENTER,
            "Empty cell: " + EMPTY,
            "Cell with fog: " + FOG,
            "Blocked cell: " + BLOCKED,
        };

        public static bool IsValid(string boardChar)
        {
            if (NON_VALID_UNITS.Contains(boardChar))
                return false;
            return true;
        }

        public static ECultures ToCulture(string msg)
        {
            if (DalrionChar.Contains(msg)) return ECultures.DALRIONS;
            else if (RahkarChar.Contains(msg)) return ECultures.RAHKARS;
            return ECultures.DEFAULT;
        }

        public static void PrintBoard(string[,] board, string cmd, Coord cursorPos, Coord selection)
        {
            Console.WriteLine();
            for (int i = 0; i < GameConsts.BOARD_LIN; i++)
            {
                for (int j = 0; j < GameConsts.BOARD_COL; j++)
                {
                    ECultures cult = BoardStrings.ToCulture(board[i, j]);
                    if (selection != null && Coord.Distance(selection, new Coord(i, j)) < 5 && cmd == Commands.MOVE)
                        Console.BackgroundColor = ConsoleColor.Red;
                    if (cursorPos != null && cursorPos.IsSame(new Coord(i, j)))
                        Console.BackgroundColor = ConsoleColor.Cyan;
                    if (cult == ECultures.DALRIONS)
                        ColoredPrint(board[i, j] + " ", CultColors[ECultures.DALRIONS]);
                    else if (cult == ECultures.RAHKARS)
                        ColoredPrint(board[i, j] + " ", CultColors[ECultures.RAHKARS]);
                    else Console.Write(board[i, j] + " ");
                    Console.ResetColor();
                }
                Console.Write("\t");

                if (i == 0) Console.Write("Commands: ");
                else if (i - 1 < Cmds.Count)
                {
                    string cCmd = Cmds.Keys.ToArray()[i - 1];
                    ColoredPrint("- " + cCmd, (Cmds[cCmd]) ? (ConsoleColor.Green) : (ConsoleColor.Red));
                }
                else if (i - 1 == Cmds.Count)
                {
                    Console.Write("Legenda:");
                }
                else if (i - 2 - Cmds.Count < Legend.Length)
                {
                    ColoredPrint("- " + Legend[i - 2 - Cmds.Count], ConsoleColor.DarkMagenta);
                }
                Console.Write("\n");
            }
        }

        private static void ColoredPrint(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ResetColor();
        }
    }
}
