using System;
using System.Collections.Generic;
using Types;
using Cells;
using Consts;
using System.IO;

namespace Game
{
    class Game
    {
        private Player[] Players;
        private Player CurPlayer;
        private bool Play;
        private bool ValidCmd;
        private Board Boards;

        public Game() {
            InitPlayers();
            Play = true;
            ValidCmd = false;
            Boards = new Board(Players);
            // Adding units
            PlaceUnits();
        }

        private void InitPlayers() {
            Players = new Player[2];
            Players[0] = new Player(ECultures.DALRIONS);
            Players[1] = new Player(ECultures.RAHKARS);

            Players[1].SetCursor(new Coord(BoardConsts.MAX_LIN - 2, BoardConsts.MAX_COL - 2));
            CurPlayer = Players[0];
        }

        private void PlaceUnits() {
            foreach (Player it in Players) {
                foreach (Unit unit in it.GetUnits()) {
                    unit.Place();
                }
            }
        }

        public void Start() {
            do {
                Boards.PrintBoard();
                GetUserCmd();
                Console.Write("Press enter to continue...");
                Console.ReadLine();
                if (ValidCmd)
                    SetNextPlayer();

                foreach (Player player in Players) {
                    if (player.GetCenter() == null || player.GetCenter().GetCurrLife() <= 0)
                        Play = false;
                }
                Console.Clear();
            } while (Play);
            string winner = "";
            if (CurPlayer.GetCenter() == null || CurPlayer.GetCenter().GetCurrLife() <= 0) {
                if (CurPlayer.GetCulture() == ECultures.DALRIONS)
                    winner = "RAHKARS";
                else
                    winner = "DALRIONS";
            }
            Console.WriteLine(winner + " ARE THE WINNERs!");
            Console.ReadLine();
        }

        private void GetUserCmd() {
            string msg = String.Format("{0} TURN.\nType in a command: ", CurPlayer.GetCulture());
            Console.Write(msg);
            string userCmd = Console.ReadLine().Trim().ToUpper();
            ValidCmd = true;

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
                    Play = false;
                    break;
                default:
                    ValidCmd = false;
                    Console.WriteLine(userCmd + " isn't a valid command!");
                    break;
            }
        }

        private void Attack() {

            Coord enemyPos = null;
            enemyPos = CurPlayer.PerformAttack(Boards, GetOponent().GetUnits());
            if(enemyPos != null) {
                Unit enemy = GetOponent().GetUnitAt(enemyPos);
                if (enemy.GetCurrLife() <= 0) {
                    GetOponent().GetUnitAt(enemyPos).Erase();
                    GetOponent().RemoveUnitAt(enemyPos);
                }
            }
            ValidCmd = enemyPos != null;
        }

        private void Move() {
            ValidCmd = CurPlayer.PeformMove(Boards);
        }

        private void Inspect() {
            Console.Write("Select an unit...");
            Console.ReadLine();
            Coord selPos = null;
            bool isUnit = false;
            ValidCmd = false;
            BoardConsts consts = new BoardConsts();

            do {
                selPos = Boards.SelectPosition(CurPlayer.GetCursor());
                Unit unit = null;

                foreach (Player it in Players) {
                    unit = it.GetUnitAt(selPos);
                    if (unit != null)
                        break;
                }

                isUnit = consts.IsValid(Boards.CellAt(selPos)) && unit != null;
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
            CurPlayer.SetTurn();
            CurPlayer.ExecuteTurnEvents(Boards.GetBoard());

            if (CurPlayer == Players[0]) {
                CurPlayer = Players[1];
            } else {
                CurPlayer = Players[0];   
            }
        }

        private Player GetOponent() {
            if (CurPlayer == Players[0])
                return Players[1];
            else
                return Players[0];
        }

        static void Main(string[] args) {

            try {
                Game rom = new Game();
                rom.Start();
            } catch (FormatException) {
                Console.WriteLine("Invalid Terrain file format!");
            } catch (IOException) {
                Console.WriteLine("Could not find Terrain file!");
            } catch (ArgumentException ae) {
                Console.WriteLine(ae.Message);
            }  finally {
                Console.ReadLine();
            }
            
        }
    }
}
