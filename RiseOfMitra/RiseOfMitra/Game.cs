using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoMUtils;

namespace RiseOfMitra
{
    class Game
    {
        private const string EMPTY = ".";
        private const string FOG = "@";
        private const string BLOCK = "X";

        private Player[] players;
        private int nextPlayer;
        private bool play;
        private bool validCmd;
        private string[,] Board;
        private List<Unit> Units;
        private Dictionary<string, bool> MyCommands;
        private Dictionary<ECultures, ConsoleColor> BoardColors;

        public Game()
        {
            InitPlayers();
            nextPlayer = 0;
            play = true;
            validCmd = false;
            Board = new string[GameConsts.BOARD_LIN, GameConsts.BOARD_COL];
            Units = new List<Unit>();
            MyCommands = new Dictionary<string, bool>();
            BoardColors = new Dictionary<ECultures, ConsoleColor>();

            // Adding units
            InitCommands();
            ClearBoard();
            CreatePawns();
            ABuilding dCenter = CulturalCenterFactory.GetCulturalCenter(ECultures.DALRIONS);
            ABuilding rCenter = CulturalCenterFactory.GetCulturalCenter(ECultures.RAHKARS);
            dCenter.SetPos(new Coord(1, 1));
            int buildSize = rCenter.GetSize() + 1;
            rCenter.SetPos(new Coord(GameConsts.BOARD_LIN - buildSize, GameConsts.BOARD_COL - buildSize));

            // Defining colors
            BoardColors.Add(ECultures.DALRIONS, ConsoleColor.Blue);
            BoardColors.Add(ECultures.RAHKARS, ConsoleColor.Yellow);

            // Creating Cultural centers
            Units.Add(dCenter);
            Units.Add(rCenter);
            PlaceUnits();
        }

        private void InitPlayers()
        {
            players = new Player[2];
            players[0] = new Player();
            players[1] = new Player();
            players[0].SetCulture(ECultures.DALRIONS);
            players[1].SetCulture(ECultures.RAHKARS);
        }

        private void InitCommands()
        {
            MyCommands.Add(Commands.ATTACK, true);
            MyCommands.Add(Commands.MOVE, true);
            MyCommands.Add(Commands.CONQUER, false);
            MyCommands.Add(Commands.EXIT, true);
        }

        private void ClearBoard()
        {
            for (int i = 0; i < GameConsts.BOARD_LIN; i++)
            {
                for (int j = 0; j < GameConsts.BOARD_COL; j++)
                {
                    Board[i, j] = ".";
                }
            }
        }

        private void CreatePawns()
        {
            for (int i = 0; i < GameConsts.INITIAL_PAWNS; i++)
            {
                ABasicPawn dPawn = PawnFactory.GetPawn(ECultures.DALRIONS);
                dPawn.SetPos(new Coord(1 + i, 7));
                Units.Add(dPawn);

                ABasicPawn rPawn = PawnFactory.GetPawn(ECultures.RAHKARS);
                rPawn.SetPos(new Coord(GameConsts.BOARD_LIN - 2 - i, GameConsts.BOARD_COL - 8));
                Units.Add(rPawn);
            }
        }

        private void PlaceUnits()
        {
            foreach (Unit unit in Units)
            {
                PlaceUnit(unit, unit.GetPos());
            }
        }

        private void PlaceUnit(Unit unit, Coord init)
        {
            for (int i = 0; i < unit.GetSize(); i++)
            {
                for (int k = 0; k < unit.GetSize(); k++)
                {
                    int cX = init.X + i;
                    int cY = init.Y + k;
                    Board[cX, cY] = unit.ToString();
                }
            }
        }

        public void Start()
        {
            do
            {
                PrintBoard(null, null);
                GetUserCmd();
                Console.Write("Press enter to finish...");
                Console.ReadLine();
                if(validCmd)
                    SetNextPlayer();
                Console.Clear();
            } while (play);

        }

        private Coord SelectedPosition(Coord pos, string cmd)
        {
            bool selected = true;
            do
            {
                Console.Clear();
                PrintBoard(pos, null);
                var move = Console.ReadKey(false).Key;
                switch (move)
                {
                    case ConsoleKey.Enter:
                        if (BoardStrings.IsValid(Board[pos.X, pos.Y]))
                        {
                            if(cmd == Commands.MOVE)
                            {
                                Console.Clear();
                                PrintBoard(pos, Commands.MOVE);
                            }
                            players[nextPlayer].SetCursor(pos);
                            return new Coord(pos.X, pos.Y);
                        }
                        else
                            Console.WriteLine("That's not a valid unit!");
                        Console.ReadLine();
                        break;
                    case ConsoleKey.LeftArrow:
                        if (pos.Y > 1)
                            pos.Y--;
                        break;
                    case ConsoleKey.UpArrow:
                        if (pos.X > 1)
                            pos.X--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (pos.Y < GameConsts.BOARD_COL - 2)
                            pos.Y++;
                        break;
                    case ConsoleKey.DownArrow:
                        if (pos.X < GameConsts.BOARD_LIN - 2)
                            pos.X++;
                        break;
                    case ConsoleKey.Escape:
                        selected = false;
                        break;
                    default:
                        break;
                }
            } while (selected);

            return null;
        }

        private void PrintBoard(Coord cursorPos, string userCmd)
        {
            Console.WriteLine();
            for (int i = 0; i < GameConsts.BOARD_LIN; i++)
            {
                for (int j = 0; j < GameConsts.BOARD_COL; j++)
                {
                    ECultures cult = BoardStrings.ToCulture(Board[i, j]);
                    if (cursorPos != null
                        && cursorPos.IsSame(new Coord(i, j)))
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.Write(Board[i, j] + " ");
                        Console.ResetColor();
                    }
                    else if (cult == ECultures.DALRIONS)
                        ColoredPrint(Board[i, j] + " ", BoardColors[ECultures.DALRIONS]);
                    else if(cult == ECultures.RAHKARS)
                        ColoredPrint(Board[i, j] + " ", BoardColors[ECultures.RAHKARS]);
                    else if (cursorPos != null &&
                            Coord.Distance(cursorPos, new Coord(i, j)) < 5 && userCmd == Commands.MOVE)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write(Board[i, j] + " ");
                        Console.ResetColor();
                    }
                    else Console.Write(Board[i, j] + " ");
                }
                Console.Write("\t");

                if (i == 0) Console.Write("Commands: ");
                else if (i - 1 < MyCommands.Count)
                {
                    string cmd = MyCommands.Keys.ToArray()[i - 1];
                    ColoredPrint("- " + cmd, (MyCommands[cmd]) ? (ConsoleColor.Green) : (ConsoleColor.Red));
                }
                Console.Write("\n");
            }
        }

        private void GetUserCmd()
        {
            string msg = String.Format("Player {0} turn. Type in a command: ", nextPlayer + 1);
            Console.Write(msg);
            string userCmd = Console.ReadLine().Trim().ToUpper();
            validCmd = true;

            switch (userCmd)
            {
                case Commands.ATTACK:
                    Attack();
                    break;
                case Commands.MOVE:
                    Move();
                    break;
                case Commands.CONQUER:
                    Conquer();
                    break;
                case Commands.EXIT:
                    play = false;
                    break;
                default:
                    validCmd = false;
                    Console.WriteLine(userCmd + " isn't a valid command!");
                    break;
            }
        }

        private void Attack()
        {
            Console.Write("Select an ally pawn: ");
            Coord allyPos = SelectedPosition(players[nextPlayer].GetCursor(), Commands.ATTACK);
            Console.ReadLine();
            Console.Write("Select an enemy pawn: ");
            Coord enemyPos = SelectedPosition(players[nextPlayer].GetCursor(), Commands.ATTACK);
            Console.WriteLine("Attacking enemy at " + enemyPos + " with ally at " + allyPos);
        }

        private void Move()
        {
            validCmd = false;
            Coord allyPos = SelectedPosition(players[nextPlayer].GetCursor(), Commands.MOVE);
            Console.ReadLine();
            Coord target = SelectedPosition(players[nextPlayer].GetCursor(), Commands.MOVE);
        }

        private ABasicPawn PawnAt(Coord pos)
        {
            foreach (Unit unit in Units)
            {
                if(unit.GetType().Name == typeof(DalrionPawn).Name 
                    || unit.GetType().Name == typeof(RahkarPawn).Name)
                {
                    Coord uCoord = unit.GetPos();
                    if (uCoord.X == pos.X && uCoord.Y == pos.Y)
                        return (ABasicPawn) unit;
                }
            }
            return null;
        }

        private void Conquer()
        {
            Console.WriteLine("PH");
        }

        private void ColoredPrint(string cmd, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(cmd);
            Console.ResetColor();
        }

        private void SetNextPlayer()
        {
            nextPlayer = (nextPlayer + 1) % 2;
        }

        static void Main(string[] args)
        {
            Game rom = new Game();
            rom.Start();
        }
    }
}
