using System;
using System.Collections.Generic;
using Types;
using Cells;
using Consts;
using ShortestPath;

namespace RiseOfMitra
{
    class Game
    {
        private Player[] players;
        private int currPlayer;
        private bool play;
        private bool validCmd;
        private string[,] Board;
        private List<Unit> Units;

        public Game()
        {
            InitPlayers();
            currPlayer = 0;
            play = true;
            validCmd = false;
            Board = new string[BoardConsts.BOARD_LIN, BoardConsts.BOARD_COL];
            Units = new List<Unit>();

            // Adding units
            ClearBoard();
            CreateUnits();
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
            for (int i = 0; i < BoardConsts.BOARD_LIN; i++)
            {
                for (int j = 0; j < BoardConsts.BOARD_COL; j++)
                {
                    Board[i, j] = ".";
                }
            }
        }

        private void CreateUnits()
        {
            List<DalrionPawn> dPawns = new List<DalrionPawn>();
            List<DalrionPawn> rPawns = new List<DalrionPawn>();
            for (int i = 0; i < BoardConsts.INITIAL_PAWNS; i++)
            {
                ABasicPawn dPawn = PawnFactory.CreatePawn(ECultures.DALRIONS, Board);
                dPawn.SetPos(new Coord(1 + i, 7));
                Units.Add(dPawn);
                players[0].AddPawn(dPawn);

                ABasicPawn rPawn = PawnFactory.CreatePawn(ECultures.RAHKARS, Board);
                rPawn.SetPos(new Coord(BoardConsts.BOARD_LIN - 2 - i, BoardConsts.BOARD_COL - 8));
                Units.Add(rPawn);
                players[1].AddPawn(rPawn);
            }

            ABuilding dCenter = CulturalCenterFactory.CreateCultCenter(ECultures.DALRIONS, Board);
            ABuilding rCenter = CulturalCenterFactory.CreateCultCenter(ECultures.RAHKARS, Board);
            dCenter.SetPos(new Coord(1, 1));
            int buildSize = rCenter.GetSize() + 1;
            rCenter.SetPos(new Coord(BoardConsts.BOARD_LIN - buildSize, BoardConsts.BOARD_COL - buildSize));

            // Creating Cultural centers
            Units.Add(dCenter);
            Units.Add(rCenter);

            players[0].SetCulturalCenter((CulturalCenter) dCenter);
            players[1].SetCulturalCenter((CulturalCenter) rCenter);
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
                RoMBoard.PrintBoard(Board, null, null, null, 0);
                GetUserCmd();
                Console.Write("Press enter to finish...");
                Console.ReadLine();
                if(validCmd)
                    SetNextPlayer();
                Console.Clear();
            } while (play);

        }

        private Coord SelectPosition(Coord pos, Coord prevSelec, string cmd, int distance)
        {
            bool selected = false;
            Coord selection = null;
            do
            {
                Console.Clear();
                if(cmd == Commands.GET_POS || cmd == Commands.MOVE)
                    RoMBoard.PrintBoard(Board, Commands.MOVE, pos, prevSelec, distance);
                else
                    RoMBoard.PrintBoard(Board, null, pos, prevSelec, distance);
                var move = Console.ReadKey(false).Key;
                switch (move)
                {
                    case ConsoleKey.Enter:
                        selected = true;
                        selection = new Coord(pos.X, pos.Y);
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
                        if (pos.Y < BoardConsts.BOARD_COL - 2)
                            pos.Y++;
                        break;
                    case ConsoleKey.DownArrow:
                        if (pos.X < BoardConsts.BOARD_LIN - 2)
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

        private Coord SelectPosition(Coord pos, Coord prevSelec, string cmd, List<Coord> avaiableCells)
        {
            bool selected = false;
            Coord selection = null;
            do
            {
                Console.Clear();
                RoMBoard.PrintBoard(Board, Commands.MOVE, pos, prevSelec, avaiableCells);
                var move = Console.ReadKey(false).Key;
                switch (move)
                {
                    case ConsoleKey.Enter:
                        selected = true;
                        selection = new Coord(pos.X, pos.Y);
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
                        if (pos.Y < BoardConsts.BOARD_COL - 2)
                            pos.Y++;
                        break;
                    case ConsoleKey.DownArrow:
                        if (pos.X < BoardConsts.BOARD_LIN - 2)
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

        private void GetUserCmd()
        {
            string msg = String.Format("PLAYER {0} TURN.\nType in a command: ", currPlayer + 1);
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
                case Commands.INSPECT:
                    Inspect();
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
            //Console.Write("Select an ally pawn: ");
            //Coord allyPos = SelectPosition(players[nextPlayer].GetCursor(), null, Commands.ATTACK, 0);
            //Console.ReadLine();
            //Console.Write("Select an enemy pawn: ");
            //Coord enemyPos = SelectPosition(players[nextPlayer].GetCursor(), null, Commands.ATTACK, 0);
            //Console.WriteLine("Attacking enemy at " + enemyPos + " with ally at " + allyPos);

            Console.WriteLine("Attaking pawn...");
            Console.ReadLine();
        }

        private void Move()
        {
            Console.Write("Select an ally pawn...");
            Console.ReadLine();
            Coord allyPawn = null;
            bool validSelection = false;
            
            // Select a valid ally pawn
            do
            {
                string allyChar;
                if (players[currPlayer].GetCulture() == ECultures.DALRIONS)
                    allyChar = BoardStrings.DALRION_PAWN;
                else
                    allyChar = BoardStrings.RAHKAR_PAWN;

                allyPawn = SelectPosition(players[currPlayer].GetCursor(), null, Commands.MOVE, 0);
                // Verifica se a célula selecionada possui um peão aliado
                validSelection = Board[allyPawn.X, allyPawn.Y].Equals(allyChar);

                if (!validSelection)
                {
                    Console.Write("Invalid unit!");
                    Console.ReadLine();
                }
            } while (!validSelection);

            Dijkstra didi = new Dijkstra(Board, allyPawn, players[currPlayer].PawnAt(allyPawn).GetMovePoints());
            List<Coord> validCells = didi.GetValidPaths();
            Coord target;

            validSelection = false;
            do
            {
                target = SelectPosition(players[currPlayer].GetCursor(), allyPawn, Commands.MOVE, validCells);
                // Verifica se é possível se mover para a célula selecionada
                validSelection = validCells.Contains(target);

                if (!validSelection)
                {
                    Console.Write("Invalid unit!");
                }
            } while (!validSelection);

            players[currPlayer].PawnAt(allyPawn).Move(target);
        }

        private void Inspect()
        {
            Console.Write("Select an unit...");
            Console.ReadLine();
            Coord selPos = null;
            bool isUnit = false;
            validCmd = false;

            do
            {
                selPos = SelectPosition(players[currPlayer].GetCursor(), null, Commands.INSPECT, 0);
                isUnit = BoardStrings.IsValid(Board[selPos.X, selPos.Y]);
                string msg = "";

                foreach (Player it in players)
                {
                    if (it.GetCenter().InUnit(selPos))
                    {
                        msg = it.GetCenter().GetStatus();
                        break;
                    }
                    else if (it.PawnAt(selPos) != null)
                    {
                        msg = it.PawnAt(selPos).GetStatus();
                        break;
                    }
                    else
                    {
                        msg = "Invalid unit!";
                        isUnit = false;
                    }              
                }
                Console.Write(msg);
                Console.ReadLine();
            } while (!isUnit);
        }

        private void Conquer()
        {
            Console.WriteLine("PH");
        }

        private void SetNextPlayer()
        {
            currPlayer = (currPlayer + 1) % 2;
        }

        static void Main(string[] args)
        {
            Game rom = new Game();
            rom.Start();
        }
    }
}
