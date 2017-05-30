using System.Linq;
using System.Collections.Generic;
using System;
using Types;
using Cells;
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
            { Commands.ATTACK,  true },
            { Commands.MOVE,    true },
            { Commands.CONQUER, false },
            { Commands.INSPECT, true },
            { Commands.EXIT,    true }
        };

        private static string[] Legend = new string[]
        {
            "Dalrion pawn: "    + BoardConsts.DALRION_PAWN,
            "Dalrion center: "  + BoardConsts.DALRION_CENTER,
            "Rahkar pawn: "     + BoardConsts.RAHKAR_PAWN,
            "Rahkar center: "   + BoardConsts.RAHKAR_CENTER,
            "Empty cell: "      + BoardConsts.EMPTY,
            "Cell with fog: "   + BoardConsts.FOG,
            "Blocked cell: "    + BoardConsts.BLOCKED,
        };

        public static void PrintBoard(string[,] board, string cmd, Coord cursor, Coord selection, List<Coord> avaiableCells)
        {
            Console.WriteLine();
            for (int i = 0; i < BoardConsts.BOARD_LIN; i++)
            {
                for (int j = 0; j < BoardConsts.BOARD_COL; j++)
                {
                    ECultures cult = BoardConsts.ToCulture(board[i, j]);
                    if (selection != null && avaiableCells.Contains(new Coord(i, j)))
                    {
                        if (Commands.MOVE == cmd)
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                        else if (Commands.ATTACK == cmd)
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                    }
                    if (cursor != null && cursor.Equals(new Coord(i, j)))
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

        public static void PrintBoard(string[,] board, Coord cursorPos)
        {
            Console.WriteLine();
            for (int i = 0; i < BoardConsts.BOARD_LIN; i++)
            {
                for (int j = 0; j < BoardConsts.BOARD_COL; j++)
                {
                    ECultures cult = BoardConsts.ToCulture(board[i, j]);
                    if (cursorPos != null && cursorPos.Equals(new Coord(i, j)))
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
