using System.Linq;
using System.Collections.Generic;
using System;
using System.IO;
using Utils;
using Utils.Types;
using Utils.Space;
using Units;

namespace Boards
{
    public class Board
    {
        private string[,] MainBoard;
        private ETerrain[,] Terrains;
        private Dictionary<ECultures, ConsoleColor> CultColors;
        private Dictionary<string, bool> Cmds;
        private string[] Legend;
        private string[] Status { get; set; }


        public Board(Board board) {
            MainBoard = new string[BoardConsts.MAX_LIN, BoardConsts.MAX_COL];
            Terrains = new ETerrain[BoardConsts.MAX_LIN, BoardConsts.MAX_COL];

            for (int i = 0; i < board.GetBoard().GetLongLength(0); i++) {
                for (int j = 0; j < board.GetBoard().GetLongLength(1); j++) {
                    MainBoard[i, j] = board.MainBoard[i, j];
                    Terrains[i, j] = board.Terrains[i, j];
                }
            }
            SetCellAt(new Coord(30, 3), "AQUI");
            CultColors = board.CultColors;
            Cmds = board.Cmds;
            Legend = board.Legend;
        }

        public Board() {
            Terrains = ReadTerrainFile();
            MainBoard = ReadMainBoard();
            CultColors = InitColors();
            Cmds = InitCommands();
            Legend = InitLegend();
            Status = null;
        }

        private Dictionary<ECultures, ConsoleColor> InitColors() {
            Dictionary<ECultures, ConsoleColor> tmpColors = new Dictionary<ECultures, ConsoleColor> {
                { ECultures.DALRIONS, ConsoleColor.Blue },
                { ECultures.RAHKARS, ConsoleColor.Yellow }
            };
            return tmpColors;
        }

        private Dictionary<string, bool> InitCommands() {
            Dictionary<string, bool> tmpDic = new Dictionary<string, bool> {
                { Command.ATTACK, true },
                { Command.MOVE, true },
                { Command.CONQUER, false },
                { Command.INSPECT, true },
                { Command.HELP, true },
                { Command.EXIT, true }
            };
            return tmpDic;
        }

        private string[] InitLegend() {
            string[] tmpLegend = new string[] {
                "Dalrion pawn: "    + BoardConsts.DALRION_PAWN,
                "Dalrion center: "  + BoardConsts.DALRION_CENTER,
                "Rahkar pawn: "     + BoardConsts.RAHKAR_PAWN,
                "Rahkar center: "   + BoardConsts.RAHKAR_CENTER,
                "Empty cell: "      + BoardConsts.EMPTY,
                "Cell with fog: "   + BoardConsts.FOG,
                "Blocked cell: "    + BoardConsts.BLOCKED,
            };
            return tmpLegend;
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

        private ETerrain[,] ReadTerrainFile() {
            string TerrainFilePath = @"C:\Users\thalesaguiar\Documents\Dev\C#\ROM\terrains.txt";
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

        private string[,] ReadMainBoard() {
            string TerrainFilePath = @"C:\Users\thalesaguiar\Documents\Dev\C#\ROM\board.txt";
            string[,] auxBoard = new string[BoardConsts.MAX_LIN, BoardConsts.MAX_COL];
            int lin = 0;
            int col = 0;
            string line;
            FileStream fStream = new FileStream(TerrainFilePath, FileMode.Open, FileAccess.Read);
            StreamReader file = new StreamReader(fStream);

            while ((line = file.ReadLine()) != null) {
                string[] cells = line.Split(' ');
                foreach (string cell in cells) {
                    cell.Trim();
                    auxBoard[lin, col] = cell;
                    col++;
                }
                lin++;
                col = 0;
            }
            file.Close();
            return auxBoard;
        }

        public Coord SelectUnit(IEnumerable<Unit> units) {
            if(units == null || units.Count() == 0) {
                UserUtils.PrintError("There are no pawns!");
            } else {
                
                Coord unitPosition = units.ElementAt(0).GetPos();
                int currUnit = 0;
                bool selected = false;
                int index = 0;
                do {
                    Console.Clear();
                    PrintBoard(unitPosition);

                    var move = Console.ReadKey(false).Key;
                    switch (move) {
                        case ConsoleKey.Enter:
                            selected = true;
                            break;
                        case ConsoleKey.LeftArrow:
                            // Select the previous pawn on the list
                            currUnit = (currUnit - 1) % units.Count();                            
                            break;
                        case ConsoleKey.RightArrow:
                            // Select the next pawn on the list
                            currUnit = (currUnit + 1) % units.Count();
                            break;
                        case ConsoleKey.Escape:
                            selected = false;
                            break;
                        default:
                            break;
                    }

                    index = SelectionIndex(currUnit, units.Count());
                    unitPosition = units.ElementAt(index).GetPos();
                } while (!selected);

                return unitPosition;
            }
            return null;
        }

        private int SelectionIndex(int curr, int max) {
            int index = curr;
            if(curr < 0) {
                index = max - Math.Abs(curr);
            }

            return index;
        }

        public Coord SelectPosition(Coord cursor, Coord prevSelec, string cmd, List<Coord> avaiableCells) {
            bool selected = false;
            Coord selection = null;
            do {
                Console.Clear();
                PrintBoard(cmd, cursor, prevSelec, avaiableCells);
                if (cmd == Command.MOVE)
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
                        if (Command.MOVE == cmd)
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                        else if (Command.ATTACK == cmd)
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
                PrintSideInfos(i);
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
                PrintSideInfos(i);
            }
            Status = null;
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
                PrintSideInfos(i);
            }
        }

        private void PrintSideInfos(int i) {
            Console.Write("\t");

            if (i == 0) Console.Write("Commands: ");
            else if (i - 1 < Cmds.Count) {
                string cCmd = Cmds.Keys.ToArray()[i - 1];
                ColoredPrint("- " + cCmd, (Cmds[cCmd]) ? (ConsoleColor.DarkGreen) : (ConsoleColor.DarkRed));
            } else if (i - 1 == Cmds.Count) {
                Console.Write("Legenda:");
            } else if (i - 2 - Cmds.Count < Legend.Length) {
                ColoredPrint("- " + Legend[i - 2 - Cmds.Count], ConsoleColor.DarkMagenta);
            } else if (i - 2 - Cmds.Count == Legend.Length) {
                Console.Write("Info:");
            } else if (Status != null && i - 3 - Cmds.Count - Legend.Length < Status.Length - 1) {
                ColoredPrint("- " + Status[i - 3 - Cmds.Count - Legend.Length], ConsoleColor.Cyan);
            }
            Console.Write("\n");

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

        public bool Equals(Board otherOboard) {
            if(otherOboard.MainBoard.GetLength(0) != BoardConsts.MAX_LIN
                || otherOboard.MainBoard.GetLength(1) != BoardConsts.MAX_COL) {
                return false;
            } else {
                for (int i = 0; i < BoardConsts.MAX_LIN; i++) {
                    for (int j = 0; j < BoardConsts.MAX_COL; j++) {
                        if (MainBoard[i, j] != otherOboard.MainBoard[i, j])
                            return false;
                    }
                }
                return true;
            }
        }
    }
}
