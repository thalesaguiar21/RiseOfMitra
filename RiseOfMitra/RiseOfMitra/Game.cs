using System;
using System.Collections.Generic;
using System.Linq;
using RoMUtils;

namespace RiseOfMitra
{
    class Game
    {
        private Player[] players;
        private int nextPlayer;
        private bool play;
        private bool validCmd;
        private string[,] Board;
        private List<Unit> Units;

        public Game()
        {
            InitPlayers();
            nextPlayer = 0;
            play = true;
            validCmd = false;
            Board = new string[GameConsts.BOARD_LIN, GameConsts.BOARD_COL];
            Units = new List<Unit>();

            // Adding units
            ClearBoard();
            CreatePawns();
            ABuilding dCenter = CulturalCenterFactory.CreateCultCenter(ECultures.DALRIONS, Board);
            ABuilding rCenter = CulturalCenterFactory.CreateCultCenter(ECultures.RAHKARS, Board);
            dCenter.SetPos(new Coord(1, 1));
            int buildSize = rCenter.GetSize() + 1;
            rCenter.SetPos(new Coord(GameConsts.BOARD_LIN - buildSize, GameConsts.BOARD_COL - buildSize));

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
                ABasicPawn dPawn = PawnFactory.CreatePawn(ECultures.DALRIONS, Board);
                dPawn.SetPos(new Coord(1 + i, 7));
                Units.Add(dPawn);

                ABasicPawn rPawn = PawnFactory.CreatePawn(ECultures.RAHKARS, Board);
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
                BoardStrings.PrintBoard(Board, null, null, null);
                GetUserCmd();
                Console.Write("Press enter to finish...");
                Console.ReadLine();
                if(validCmd)
                    SetNextPlayer();
                Console.Clear();
            } while (play);

        }

        private Coord SelectedPosition(Coord pos, Coord prevSelec, string cmd)
        {
            bool selected = false;
            Coord selection = null;
            do
            {
                Console.Clear();
                if(cmd == Commands.GET_POS || cmd == Commands.MOVE)
                    BoardStrings.PrintBoard(Board, Commands.MOVE, pos, prevSelec);
                else
                    BoardStrings.PrintBoard(Board, null, pos, prevSelec);
                var move = Console.ReadKey(false).Key;
                switch (move)
                {
                    case ConsoleKey.Enter:
                        selected = true;
                        if (cmd == Commands.MOVE)
                        {
                            if (BoardStrings.IsValid(Board[pos.X, pos.Y]))
                            {
                                players[nextPlayer].SetCursor(pos);
                                selection = new Coord(pos.X, pos.Y);
                            }
                            else
                                selection = null;
                        }
                        else if (cmd == Commands.GET_POS)
                        {
                            players[nextPlayer].SetCursor(pos);
                            selection = new Coord(pos.X, pos.Y);
                        }
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
                        selected = true;
                        break;
                    default:
                        break;
                }
            } while (!selected);

            return selection;
        }

        private void GetUserCmd()
        {
            string msg = String.Format("{0}s turn. Type in a command: ", nextPlayer + 1);
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
            Coord allyPos = SelectedPosition(players[nextPlayer].GetCursor(), null, Commands.ATTACK);
            Console.ReadLine();
            Console.Write("Select an enemy pawn: ");
            Coord enemyPos = SelectedPosition(players[nextPlayer].GetCursor(), null, Commands.ATTACK);
            Console.WriteLine("Attacking enemy at " + enemyPos + " with ally at " + allyPos);
        }

        private void Move()
        {
            Console.Write("Select an ally pawn...");
            Console.ReadLine();
            Coord allyPawn = null;
            bool validSelection = false;
            do
            {
                allyPawn = SelectedPosition(players[nextPlayer].GetCursor(), null, Commands.MOVE);
                if (allyPawn == null)
                {
                    validSelection = false;
                    Console.WriteLine("That's not a valid unit, please select an ally Pawn!");
                    Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    BoardStrings.PrintBoard(Board, Commands.MOVE, players[nextPlayer].GetCursor(), allyPawn);
                    validSelection = ConfirmSelection();
                }
            } while (!validSelection);

            Coord target = null;
            bool validTarget = false;
            do
            {
                target = SelectedPosition(players[nextPlayer].GetCursor(), allyPawn, Commands.GET_POS);
                if (target == null)
                {
                    validTarget = false;
                    Console.WriteLine("That's not a valid selection, please select another cell!");
                    Console.ReadLine();
                }
                else if (Board[target.X, target.Y] != BoardStrings.EMPTY)
                {
                    validTarget = false;
                    Console.WriteLine("Plase select an empty cell!");
                    Console.ReadLine();
                }
                else
                {
                    validTarget = ConfirmSelection();
                }
            } while (!validTarget);
            Console.WriteLine("Moving unit at " + allyPawn + " to " + target);
            Board[allyPawn.X, allyPawn.Y] = BoardStrings.EMPTY;
            string pawnChar = null;
            switch (players[nextPlayer].GetCulture())
            {
                case ECultures.DEFAULT:
                    break;
                case ECultures.DALRIONS:
                    pawnChar = BoardStrings.CHAR_DALRION_PAWN;
                    break;
                case ECultures.RAHKARS:
                    pawnChar = BoardStrings.CHAR_RAHKAR_PAWN;
                    break;
                default:
                    break;
            }
            Board[target.X, target.Y] = pawnChar;
            Console.ReadLine();
        }

        public bool ConfirmSelection()
        {
            bool confirmed = false;
            Console.WriteLine("Press S to confirm or C to select another pawn...");
            ConsoleKey pressedKey;
            do
            {
                pressedKey = Console.ReadKey(false).Key;
                if (pressedKey == ConsoleKey.S)
                    confirmed = true;
                else if (pressedKey == ConsoleKey.C)
                    confirmed = false;
            } while (pressedKey != ConsoleKey.S && pressedKey != ConsoleKey.C);
            
            return confirmed;
        }

        private void Conquer()
        {
            Console.WriteLine("PH");
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
