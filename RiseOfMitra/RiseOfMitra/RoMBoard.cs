using System.Linq;
using System.Collections.Generic;
using System;
using Types;
using Cells;
using Consts;
using Consts;

namespace RiseOfMitra
{
    class RoMBoard
    {
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
            "Dalrion pawn: " + BoardStrings.CHAR_DALRION_PAWN,
            "Dalrion center: " + BoardStrings.CHAR_DALRION_CENTER,
            "Rahkar pawn: " + BoardStrings.CHAR_RAHKAR_PAWN,
            "Rahkar center: " + BoardStrings.CHAR_RAHKAR_CENTER,
            "Empty cell: " +BoardStrings. EMPTY,
            "Cell with fog: " + BoardStrings.FOG,
            "Blocked cell: " + BoardStrings.BLOCKED,
        };

        public static bool IsValid(string boardChar)
        {
            if (BoardStrings.IVALID_UNITS.Contains(boardChar))
                return false;
            return true;
        }

        public static ECultures ToCulture(string msg)
        {
            if (BoardStrings.DALRION_UNITS.Contains(msg)) return ECultures.DALRIONS;
            else if (BoardStrings.RAHKAR_UNITS.Contains(msg)) return ECultures.RAHKARS;
            return ECultures.DEFAULT;
        }

        private bool IsValidAdajcent(string[,] board, Coord neigh, Coord init, int range)
        {
            bool isValid = true;
            if (board[neigh.X, neigh.Y] != BoardStrings.EMPTY)
                isValid = false;
            if (Coord.Distance(init, neigh) > range)
                isValid = false;
            return isValid;
        }

        public List<Coord> GetNeighbors(string[,] board, Coord init, int range)
        {
            List<Coord> adj = new List<Coord>();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if(IsValidAdajcent(board, new Coord(i, j), init, range))
                    {
                        adj.Add(new Coord(i, j));
                    }
                }
            }
            return adj;
        }

        public static void PrintBoard(string[,] board, string cmd, Coord cursorPos, Coord selection, int distance)
        {
            Console.WriteLine();
            for (int i = 0; i < BoardConsts.BOARD_LIN; i++)
            {
                for (int j = 0; j < BoardConsts.BOARD_COL; j++)
                {
                    ECultures cult = ToCulture(board[i, j]);
                    if (selection != null && Coord.Distance(selection, new Coord(i, j)) < distance && cmd == Commands.MOVE)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
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
