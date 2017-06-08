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
        private Player curPlayer;
        private bool play;
        private bool validCmd;
        private string[,] Board;

        public Game() {
            InitPlayers();
            play = true;
            validCmd = false;
            Board = new string[BoardConsts.BOARD_LIN, BoardConsts.BOARD_COL];

            // Adding units
            ClearBoard();
            CreateUnits();
            PlaceUnits();
        }

        private void InitPlayers() {
            players = new Player[2];
            players[0] = new Player(ECultures.DALRIONS);
            players[1] = new Player(ECultures.RAHKARS);

            players[1].SetCursor(new Coord(BoardConsts.BOARD_LIN - 2, BoardConsts.BOARD_COL - 2));
            curPlayer = players[0];
        }

        private void ClearBoard() {
            for (int i = 0; i < BoardConsts.BOARD_LIN; i++) {
                for (int j = 0; j < BoardConsts.BOARD_COL; j++) {
                    Board[i, j] = BoardConsts.EMPTY;
                }
            }
        }

        private void CreateUnits() {
            PawnFactory pawnFac = new PawnFactory();
            for (int i = 0; i < BoardConsts.INITIAL_PAWNS; i++) {
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

            players[0].SetCulturalCenter((CulturalCenter)dCenter);
            players[1].SetCulturalCenter((CulturalCenter)rCenter);
        }

        private void PlaceUnits() {
            foreach (Player it in players) {
                foreach (Unit unit in it.GetUnits()) {
                    unit.Place();
                }
            }
        }

        public void Start() {
            do {
                RoMBoard.PrintBoard(Board, null);
                GetUserCmd();
                Console.Write("Press enter to continue...");
                Console.ReadLine();
                if (validCmd)
                    SetNextPlayer();

                foreach (Player player in players) {
                    if (player.GetCenter() == null || player.GetCenter().GetCurrLife() <= 0)
                        play = false;
                }
                Console.Clear();
            } while (play);
            string winner = "";
            if (curPlayer.GetCenter() == null || curPlayer.GetCenter().GetCurrLife() <= 0) {
                if (curPlayer.GetCulture() == ECultures.DALRIONS)
                    winner = "RAHKARS";
                else
                    winner = "DALRIONS";
            }
            Console.WriteLine(winner + " ARE THE WINNERs!");
            Console.ReadLine();
        }

        private void GetUserCmd() {
            string msg = String.Format("{0} TURN.\nType in a command: ", curPlayer.GetCulture());
            Console.Write(msg);
            string userCmd = Console.ReadLine().Trim().ToUpper();
            validCmd = true;

            switch (userCmd) {
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

        private void Attack() {

            Coord enemyPos = null;
            enemyPos = curPlayer.PerformAttack(Board, GetOponent().GetUnits());
            if(enemyPos != null) {
                Unit enemy = GetOponent().GetUnitAt(enemyPos);
                if (enemy.GetCurrLife() <= 0) {
                    GetOponent().GetUnitAt(enemyPos).Erase();
                    GetOponent().RemoveUnitAt(enemyPos);
                }
            }
            validCmd = enemyPos != null;
        }

        private void Move() {
            validCmd = curPlayer.PeformMove(Board);
        }

        private void Inspect() {
            Console.Write("Select an unit...");
            Console.ReadLine();
            Coord selPos = null;
            bool isUnit = false;
            validCmd = false;
            BoardConsts consts = new BoardConsts();

            do {
                selPos = RoMBoard.SelectPosition(Board, curPlayer.GetCursor());
                Unit unit = null;

                foreach (Player it in players) {
                    unit = it.GetUnitAt(selPos);
                    if (unit != null)
                        break;
                }

                isUnit = consts.IsValid(Board[selPos.X, selPos.Y]) && unit != null;
                string msg = "";

                if (isUnit)
                    msg = unit.GetStatus();
                else
                    msg = "Invalid unit!";

                Console.Write(msg);
                Console.ReadLine();
            } while (!isUnit);
        }

        private void Conquer() {
            Console.WriteLine("PH");
        }

        private void SetNextPlayer() {
            curPlayer.SetTurn();
            curPlayer.ExecuteTurnEvents(Board);

            if (curPlayer == players[0]) {
                curPlayer = players[1];
            } else {
                curPlayer = players[0];   
            }
        }

        private Player GetOponent() {
            if (curPlayer == players[0])
                return players[1];
            else
                return players[0];
        }

        static void Main(string[] args) {
            Game rom = new Game();
            rom.Start();
        }
    }
}
