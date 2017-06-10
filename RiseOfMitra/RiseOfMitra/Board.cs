using System.Linq;
using System.Collections.Generic;
using System;
using Types;
using Cells;
using Consts;
using System.IO;

namespace Game
{
    class Board
    {
        private string[,] MainBoard;
        private ETerrain[,] Terrains;
        private Dictionary<ECultures, ConsoleColor> CultColors;
        private Dictionary<string, bool> Cmds;
        private string[] Legend;

        public Board(Player[] players) {
            Terrains = ReadTerrainFile();
            MainBoard = ClearBoard();
            CreateUnits(players);
            CultColors = new Dictionary<ECultures, ConsoleColor>();
            CultColors.Add(ECultures.DALRIONS, ConsoleColor.Blue);
            CultColors.Add(ECultures.RAHKARS, ConsoleColor.Yellow);

            Cmds = new Dictionary<string, bool>();
            Cmds.Add(Commands.ATTACK, true);
            Cmds.Add(Commands.MOVE, true);
            Cmds.Add(Commands.CONQUER, false);
            Cmds.Add(Commands.INSPECT, true);
            Cmds.Add(Commands.EXIT, true);

            Legend = new string[] {
                "Dalrion pawn: "    + BoardConsts.DALRION_PAWN,
                "Dalrion center: "  + BoardConsts.DALRION_CENTER,
                "Rahkar pawn: "     + BoardConsts.RAHKAR_PAWN,
                "Rahkar center: "   + BoardConsts.RAHKAR_CENTER,
                "Empty cell: "      + BoardConsts.EMPTY,
                "Cell with fog: "   + BoardConsts.FOG,
                "Blocked cell: "    + BoardConsts.BLOCKED,
            };
        }

        private string[,] ClearBoard() {
            string[,] auxBoard = new string[BoardConsts.MAX_LIN, BoardConsts.MAX_COL];
            for (int i = 0; i < BoardConsts.MAX_LIN; i++) {
                for (int j = 0; j < BoardConsts.MAX_COL; j++) {
                    auxBoard[i, j] = BoardConsts.EMPTY;
                }
            }
            return auxBoard;
        }

        private void CreateUnits(Player[] players) {
            if(players != null && players.Length == 2) {
                PawnFactory pawnFac = new PawnFactory();
                for (int i = 0; i < BoardConsts.INITIAL_PAWNS; i++) {
                    APawn dPawn = pawnFac.Create(ECultures.DALRIONS, this);
                    dPawn.SetPos(new Coord(1 + i, 7));
                    players[0].AddPawn(dPawn);

                    APawn rPawn = pawnFac.Create(ECultures.RAHKARS, this);
                    rPawn.SetPos(new Coord(BoardConsts.MAX_LIN - 2 - i, BoardConsts.MAX_COL - 8));
                    players[1].AddPawn(rPawn);
                }

                CulturalCenterFactory centFac = new CulturalCenterFactory();
                ABuilding dCenter = centFac.Create(ECultures.DALRIONS, this);
                ABuilding rCenter = centFac.Create(ECultures.RAHKARS, this);

                players[0].SetCulturalCenter((CulturalCenter)dCenter);
                players[1].SetCulturalCenter((CulturalCenter)rCenter);
            } else {
                throw new ArgumentException("Invalid player array!");
            }
        }

        private ETerrain[,] ReadTerrainFile() {
            string TerrainFilePath = @"C:\Users\thalesaguiar\Documents\Dev\C#\RiseOfMitra\terrains.txt";
            ETerrain[,] auxTerrains = new ETerrain[BoardConsts.MAX_LIN, BoardConsts.MAX_COL];
            int lin = 0;
            int col = 0;
            string line;
            Array terrains = Enum.GetValues(typeof(ETerrain));
            FileStream fStream = new FileStream(TerrainFilePath, FileMode.Open, FileAccess.Read);
            StreamReader file = new StreamReader(fStream);

            while ((line = file.ReadLine()) != null) {
                string[] cells = line.Split(' ');
                foreach (string cell in cells) {
                    cell.Trim();
                    int type = int.Parse(cell);
                    auxTerrains[lin, col] = (ETerrain)terrains.GetValue(type);
                    col++;
                }
                lin++;
                col = 0;
            }
            file.Close();

            return auxTerrains;
        }

        public Coord SelectPosition(Coord cursor) {
            bool selected = false;
            Coord selection = null;
            do {
                Console.Clear();
                PrintBoard(cursor);

                var move = Console.ReadKey(false).Key;
                switch (move) {
                    case ConsoleKey.Enter:
                        selected = true;
                        selection = new Coord(cursor.X, cursor.Y);
                        break;
                    case ConsoleKey.LeftArrow:
                        if (cursor.Y > 1)
                            cursor.Y--;
                        break;
                    case ConsoleKey.UpArrow:
                        if (cursor.X > 1)
                            cursor.X--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (cursor.Y < BoardConsts.MAX_COL - 2)
                            cursor.Y++;
                        break;
                    case ConsoleKey.DownArrow:
                        if (cursor.X < BoardConsts.MAX_LIN - 2)
                            cursor.X++;
                        break;
                    case ConsoleKey.Escape:
                        selected = true;
                        break;
                    default:
                        break;
                }
            } while (!selected);

            return selection;
        }

        public Coord SelectPosition(Coord cursor, Coord prevSelec, string cmd, List<Coord> avaiableCells) {
            bool selected = false;
            Coord selection = null;
            do {
                Console.Clear();
                PrintBoard(cmd, cursor, prevSelec, avaiableCells);
                if (cmd == Commands.MOVE)
                    Console.Write("TERRAIN IS " + Terrains[cursor.X, cursor.Y].Convert());
                var move = Console.ReadKey(false).Key;
                switch (move) {
                    case ConsoleKey.Enter:
                        selected = true;
                        selection = new Coord(cursor.X, cursor.Y);
                        break;
                    case ConsoleKey.LeftArrow:
                        if (cursor.Y > 1)
                            cursor.Y--;
                        break;
                    case ConsoleKey.UpArrow:
                        if (cursor.X > 1)
                            cursor.X--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (cursor.Y < BoardConsts.MAX_COL - 2)
                            cursor.Y++;
                        break;
                    case ConsoleKey.DownArrow:
                        if (cursor.X < BoardConsts.MAX_LIN - 2)
                            cursor.X++;
                        break;
                    case ConsoleKey.Escape:
                        selected = true;
                        break;
                    default:
                        break;
                }
            } while (!selected);

            return selection;
        }

        public void PrintBoard(string cmd, Coord cursor, Coord selection, List<Coord> avaiableCells) {
            BoardConsts consts = new BoardConsts();
            Console.WriteLine();
            for (int i = 0; i < BoardConsts.MAX_LIN; i++) {
                for (int j = 0; j < BoardConsts.MAX_COL; j++) {
                    if (selection != null && avaiableCells.Contains(new Coord(i, j))) {
                        if (Commands.MOVE == cmd)
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                        else if (Commands.ATTACK == cmd)
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                    }
                    if (cursor != null && cursor.Equals(new Coord(i, j))) {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                    }
                    if (consts.IsDalrion(MainBoard[i, j]))
                        ColoredPrint(MainBoard[i, j] + " ", CultColors[ECultures.DALRIONS]);
                    else if (consts.IsRahkar(MainBoard[i, j]))
                        ColoredPrint(MainBoard[i, j] + " ", CultColors[ECultures.RAHKARS]);
                    else Console.Write(MainBoard[i, j] + " ");
                    Console.ResetColor();
                }
                Console.Write("\t");

                if (i == 0) Console.Write("Commands: ");
                else if (i - 1 < Cmds.Count) {
                    string cCmd = Cmds.Keys.ToArray()[i - 1];
                    ColoredPrint("- " + cCmd, (Cmds[cCmd]) ? (ConsoleColor.Green) : (ConsoleColor.Red));
                } else if (i - 1 == Cmds.Count) {
                    Console.Write("Legenda:");
                } else if (i - 2 - Cmds.Count < Legend.Length) {
                    ColoredPrint("- " + Legend[i - 2 - Cmds.Count], ConsoleColor.DarkMagenta);
                }
                Console.Write("\n");
            }
        }

        public void PrintBoard() {
            BoardConsts consts = new BoardConsts();
            Console.WriteLine();
            for (int i = 0; i < BoardConsts.MAX_LIN; i++) {
                for (int j = 0; j < BoardConsts.MAX_COL; j++) {
                    if (consts.IsDalrion(MainBoard[i, j]))
                        ColoredPrint(MainBoard[i, j] + " ", CultColors[ECultures.DALRIONS]);
                    else if (consts.IsRahkar(MainBoard[i, j]))
                        ColoredPrint(MainBoard[i, j] + " ", CultColors[ECultures.RAHKARS]);
                    else Console.Write(MainBoard[i, j] + " ");
                    Console.ResetColor();
                }
                Console.Write("\t");

                if (i == 0) Console.Write("Commands: ");
                else if (i - 1 < Cmds.Count) {
                    string cCmd = Cmds.Keys.ToArray()[i - 1];
                    ColoredPrint("- " + cCmd, (Cmds[cCmd]) ? (ConsoleColor.Green) : (ConsoleColor.Red));
                } else if (i - 1 == Cmds.Count) {
                    Console.Write("Legenda:");
                } else if (i - 2 - Cmds.Count < Legend.Length) {
                    ColoredPrint("- " + Legend[i - 2 - Cmds.Count], ConsoleColor.DarkMagenta);
                }
                Console.Write("\n");
            }
        }

        public void PrintBoard(Coord cursor) {
            BoardConsts consts = new BoardConsts();
            Console.WriteLine();
            for (int i = 0; i < BoardConsts.MAX_LIN; i++) {
                for (int j = 0; j < BoardConsts.MAX_COL; j++) {
                    if (cursor != null && cursor.Equals(new Coord(i, j)))
                        Console.BackgroundColor = ConsoleColor.Cyan;
                    if (consts.IsDalrion(MainBoard[i, j]))
                        ColoredPrint(MainBoard[i, j] + " ", CultColors[ECultures.DALRIONS]);
                    else if (consts.IsRahkar(MainBoard[i, j]))
                        ColoredPrint(MainBoard[i, j] + " ", CultColors[ECultures.RAHKARS]);
                    else Console.Write(MainBoard[i, j] + " ");
                    Console.ResetColor();
                }
                Console.Write("\t");

                if (i == 0) Console.Write("Commands: ");
                else if (i - 1 < Cmds.Count) {
                    string cCmd = Cmds.Keys.ToArray()[i - 1];
                    ColoredPrint("- " + cCmd, (Cmds[cCmd]) ? (ConsoleColor.Green) : (ConsoleColor.Red));
                } else if (i - 1 == Cmds.Count) {
                    Console.Write("Legenda:");
                } else if (i - 2 - Cmds.Count < Legend.Length) {
                    ColoredPrint("- " + Legend[i - 2 - Cmds.Count], ConsoleColor.DarkMagenta);
                }
                Console.Write("\n");
            }
        }

        private void ColoredPrint(string msg, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ResetColor();
        }


        public string[,] GetBoard() { return MainBoard; }
        public ETerrain[,] GetTerrains() { return Terrains; }

        public string CellAt(Coord pos) {
            if ( Coord.IsValid(pos) ) {
                return MainBoard[pos.X, pos.Y];
            } else {
                return null;
            }
        }

        public string CellAt(int i, int j) {
            return CellAt(new Coord(i, j));
        }

        public Object TerrainAt(Coord pos) {
            if (Coord.IsValid(pos)) {
                return Terrains[pos.X, pos.Y];
            } else {
                return null;
            }
        }

        public Object TerrainAt(int i, int j) {
            return TerrainAt(new Coord(i, j));
        }

        public void SetCellAt(Coord pos, string value) {
            if (Coord.IsValid(pos))
                MainBoard[pos.X, pos.Y] = value;
        }

        public void SetTerrainAt(Coord pos, ETerrain value) {
            if (Coord.IsValid(pos))
                Terrains[pos.X, pos.Y] = value;
        }
    }
}
