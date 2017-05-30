﻿using System;
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
        private Player curPlayer;
        private bool play;
        private bool validCmd;
        private string[,] Board;

        public Game()
        {
            InitPlayers();
            play = true;
            validCmd = false;
            Board = new string[BoardConsts.BOARD_LIN, BoardConsts.BOARD_COL];

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

            players[1].SetCursor(new Coord(BoardConsts.BOARD_LIN - 2, BoardConsts.BOARD_COL - 2));
            curPlayer = players[0];
        }
        
        private void ClearBoard()
        {
            for (int i = 0; i < BoardConsts.BOARD_LIN; i++)
            {
                for (int j = 0; j < BoardConsts.BOARD_COL; j++)
                {
                    Board[i, j] = BoardConsts.EMPTY;
                }
            }
        }

        private void CreateUnits()
        {
            PawnFactory pawnFac = new PawnFactory();
            for (int i = 0; i < BoardConsts.INITIAL_PAWNS; i++)
            {
                ABasicPawn dPawn = pawnFac.Create(ECultures.DALRIONS, Board);
                dPawn.SetPos(new Coord(1 + i, 7));
                players[0].AddPawn(dPawn);

                ABasicPawn rPawn = pawnFac.Create(ECultures.RAHKARS, Board);
                rPawn.SetPos(new Coord(BoardConsts.BOARD_LIN - 2 - i, BoardConsts.BOARD_COL - 8));
                players[1].AddPawn(rPawn);
            }

            CulturalCenterFactory centFac = new CulturalCenterFactory(); 
            ABuilding dCenter = centFac.Create(ECultures.DALRIONS, Board);
            ABuilding rCenter = centFac.Create(ECultures.RAHKARS, Board);
            dCenter.SetPos(new Coord(1, 1));
            int buildSize = rCenter.GetSize() + 1;
            rCenter.SetPos(new Coord(BoardConsts.BOARD_LIN - buildSize, BoardConsts.BOARD_COL - buildSize));
            
            players[0].SetCulturalCenter((CulturalCenter) dCenter);
            players[1].SetCulturalCenter((CulturalCenter) rCenter);
        }

        private void PlaceUnits()
        {
            foreach (Player it in players)
            {
                foreach (Unit unit in it.GetPawns())
                {
                    PlaceUnit(unit, unit.GetPos());
                }
                PlaceUnit(it.GetCenter(), it.GetCenter().GetPos());
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
                RoMBoard.PrintBoard(Board, null);
                GetUserCmd();
                Console.Write("Press enter to finish your turn...");
                Console.ReadLine();
                if(validCmd)
                    SetNextPlayer();

                foreach (Player player in players)
                {
                    if (player.GetCenter().GetCurrLife() <= 0)
                        play = false;
                }
                Console.Clear();
            } while (play);
            string msg = "";
            if(curPlayer.GetCenter().GetCurrLife() <= 0)
            {
                if (curPlayer.GetCulture() == ECultures.DALRIONS)
                    msg = "DALRIONS";
                else
                    msg = "RAHKARS";
            }
            Console.WriteLine(" ARE THE WINNERs!");
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

        private void GetUserCmd()
        {
            string msg = String.Format("{0} TURN.\nType in a command: ", curPlayer.GetCulture());
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
            bool isAlly = false;
            Coord allyPos = null;
            ABasicPawn allyPawn = null;

            do
            {
                allyPos = RoMBoard.SelectPosition(Board, curPlayer.GetCursor());
                allyPawn = curPlayer.GetPawnAt(allyPos);

                string allyChar;
                if (curPlayer.GetCulture() == ECultures.DALRIONS)
                    allyChar = BoardConsts.DALRION_PAWN;
                else
                    allyChar = BoardConsts.RAHKAR_PAWN;

                // Verifica se a célula selecionada possui um peão aliado
                if (Board[allyPos.X, allyPos.Y].Equals(allyChar) && allyPawn != null)
                    isAlly = true;
                else
                    isAlly = false;

                if (!isAlly)
                {
                    Console.Write("Invalid unit!");
                    Console.ReadLine();
                }
            } while (!isAlly);

            Dijkstra didi = new Dijkstra(Board, allyPawn.GetPos(), allyPawn.GetAtkRange());
            List<Coord> attackRange = didi.GetValidPaths(Commands.ATTACK);
            Coord target = null;
            List<Unit> enemyUnitsInRange = new List<Unit>();

            foreach (Unit unit in GetOponent().GetUnits())
            {
                foreach (Coord cell in attackRange)
                {
                    if (unit.InUnit(cell))
                    {
                        enemyUnitsInRange.Add(unit);
                    }
                }
            }

            if (enemyUnitsInRange.Count > 0)
            {
                bool inRange = false;
                do
                {
                    target = RoMBoard.SelectPosition(Board, allyPos, curPlayer.GetCursor(), Commands.ATTACK, attackRange);
                    Unit enemySelected = null;

                    foreach (Unit unit in enemyUnitsInRange)
                    {
                        if (unit.InUnit(target))
                        {
                            enemySelected = unit;
                            break;
                        }
                    }

                    inRange = attackRange.Contains(target) && enemySelected != null;
                    
                    if (inRange)
                    {
                        Unit enemy = GetOponent().GetPawnAt(target);
                        if (enemy == null)
                            enemy = GetOponent().GetCenter();

                        int res = allyPawn.GetAtk() - enemy.GetDef();
                        if (res > 0)
                        {
                            enemy.SetCurrLife(enemy.GetCurrLife() - res);
                            if(enemy.GetCurrLife() <= 0)
                            {
                                if (enemy is ABasicPawn)
                                {
                                    Board[target.X, target.Y] = BoardConsts.EMPTY;
                                    GetOponent().GetUnits().Remove(enemy);
                                }
                                Console.WriteLine("KILLED");
                            }
                            Console.WriteLine("You have dealt {0} damage", res);
                        }
                        else
                        {
                            Console.WriteLine("The opponent has blocked");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid target");
                        Console.ReadLine();
                    }
                } while (!inRange);
            }
            else
            {
                validCmd = false;
                Console.Write("This pawn has no enemies in range!");
                Console.ReadLine();
            }
        }

        private void Move()
        {
            Console.Write("Select an ally pawn...");
            Console.ReadLine();
            Coord allyPos = null;
            bool validSelection = false;
            
            // Select a valid ally pawn
            do
            {
                string allyChar;
                if (curPlayer.GetCulture() == ECultures.DALRIONS)
                    allyChar = BoardConsts.DALRION_PAWN;
                else
                    allyChar = BoardConsts.RAHKAR_PAWN;

                allyPos = RoMBoard.SelectPosition(Board, curPlayer.GetCursor());

                // Verifica se a célula selecionada possui um peão aliado
                if (Board[allyPos.X, allyPos.Y].Equals(allyChar) && curPlayer.GetPawnAt(allyPos) != null)
                    validSelection = true;
                else
                    validSelection = false;

                if (!validSelection)
                {
                    Console.Write("Invalid unit!");
                    Console.ReadLine();
                }
            } while (!validSelection);

            Dijkstra didi = new Dijkstra(Board, allyPos, curPlayer.GetPawnAt(allyPos).GetMovePoints());
            List<Coord> validCells = didi.GetValidPaths(Commands.MOVE);
            Coord target;

            validSelection = false;
            do
            {
                target = RoMBoard.SelectPosition(Board, curPlayer.GetCursor(), allyPos, Commands.MOVE, validCells);
                // Verifica se é possível se mover para a célula selecionada
                validSelection = validCells.Contains(target);

                if (!validSelection)
                {
                    Console.Write("Invalid unit!");
                }
            } while (!validSelection);

            curPlayer.GetPawnAt(allyPos).Move(target);
        }

        private void Inspect()
        {
            Console.Write("Select an unit...");
            Console.ReadLine();
            Coord selPos = null;
            bool isUnit = false;
            validCmd = false;
            BoardConsts consts = new BoardConsts();

            do
            {
                selPos = RoMBoard.SelectPosition(Board, curPlayer.GetCursor());
                isUnit = consts.IsValid(Board[selPos.X, selPos.Y]);
                string msg = "";

                foreach (Player it in players)
                {
                    if (it.GetCenter().InUnit(selPos))
                    {
                        msg = it.GetCenter().GetStatus();
                        isUnit = true;
                        break;
                    }
                    else if (it.GetPawnAt(selPos) != null)
                    {
                        msg = it.GetPawnAt(selPos).GetStatus();
                        isUnit = true;
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
            if (curPlayer == players[0])
                curPlayer = players[1];
            else
                curPlayer = players[0];
        }

        private Player GetOponent()
        {
            if (curPlayer == players[0])
                return players[1];
            else
                return players[0];
        }

        static void Main(string[] args)
        {
            Game rom = new Game();
            rom.Start();
        }
    }
}
