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
            players[0] = new Player();
            players[1] = new Player();
            players[0].SetCulture(ECultures.DALRIONS);
            players[1].SetCulture(ECultures.RAHKARS);

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
            dCenter.SetPos(new Coord(1, 1));
            int buildSize = rCenter.GetSize() + 1;
            rCenter.SetPos(new Coord(BoardConsts.BOARD_LIN - buildSize, BoardConsts.BOARD_COL - buildSize));

            players[0].SetCulturalCenter((CulturalCenter)dCenter);
            players[1].SetCulturalCenter((CulturalCenter)rCenter);
        }

        private void PlaceUnits() {
            foreach (Player it in players) {
                foreach (Unit unit in it.GetPawns()) {
                    PlaceUnit(unit, unit.GetPos());
                }
                PlaceUnit(it.GetCenter(), it.GetCenter().GetPos());
            }
        }

        private void PlaceUnit(Unit unit, Coord init) {
            for (int i = 0; i < unit.GetSize(); i++) {
                for (int k = 0; k < unit.GetSize(); k++) {
                    int cX = init.X + i;
                    int cY = init.Y + k;
                    Board[cX, cY] = unit.ToString();
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

        public bool ConfirmSelection() {
            bool confirmed = false;
            Console.WriteLine("Press (C) to confirm or (R) to select another pawn...");
            ConsoleKey pressedKey;
            do {
                pressedKey = Console.ReadKey(false).Key;
                if (pressedKey == ConsoleKey.S)
                    confirmed = true;
                else if (pressedKey == ConsoleKey.C)
                    confirmed = false;
            } while (pressedKey != ConsoleKey.S && pressedKey != ConsoleKey.C);

            return confirmed;
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

        private ABasicPawn GetAllyPawn() {
            ABasicPawn pawn = null;
            bool isAlly = false;

            while (!isAlly) {
                Coord pos = RoMBoard.SelectPosition(Board, curPlayer.GetCursor());
                pawn = curPlayer.GetPawnAt(pos);
                BoardConsts consts = new BoardConsts();

                if (consts.ToCulture(Board[pos.X, pos.Y]) == curPlayer.GetCulture() && pawn != null)
                    isAlly = true;
                else {
                    Console.Write("Invalid unit! Press (C) to exit or (R) to reselect: ");

                    bool valid;
                    do {
                        valid = true;
                        var move = Console.ReadKey(false).Key;
                        switch (move) {
                            case ConsoleKey.C:
                                pawn = null;
                                isAlly = true;
                                break;
                            case ConsoleKey.R:
                                isAlly = false;
                                break;
                            default:
                                valid = false;
                                break;
                        }
                    } while (!valid);
                }
            }
            return pawn;
        }

        private void Attack() {
            ABasicPawn allyPawn = GetAllyPawn();

            if (allyPawn != null) {
                Dijkstra didi = new Dijkstra(Board, allyPawn.GetPos(), allyPawn.GetAtkRange());
                List<Coord> attackRange = didi.GetValidPaths(Commands.ATTACK);
                Coord target = null;
                List<Unit> enemies = new List<Unit>();

                foreach (Unit unit in GetOponent().GetUnits()) {
                    foreach (Coord cell in attackRange) {
                        if (unit.InUnit(cell)) {
                            enemies.Add(unit);
                        }
                    }
                }

                if (enemies.Count > 0) {
                    target = RoMBoard.SelectPosition(Board, curPlayer.GetCursor(), allyPawn.GetPos(), Commands.ATTACK, attackRange);
                    Unit enemy = null;

                    for (int i = 0; i < enemies.Count; i++) {
                        if (enemies[i].InUnit(target)) {
                            enemy = enemies[i];
                            break;
                        }
                    }

                    if (enemy != null) {
                        int res = allyPawn.GetAtk() - enemy.GetDef();
                        if (res > 0) {
                            enemy.SetCurrLife(enemy.GetCurrLife() - res);
                            if (enemy.GetCurrLife() > 0) {
                                Console.WriteLine("You dealt {0} damage", res);
                            } else {
                                Console.WriteLine("Killed the enemy!");
                                Coord ePos = enemy.GetPos();
                                Board[ePos.X, ePos.Y] = BoardConsts.EMPTY;
                                GetOponent().RemoveUnitAt(ePos);
                            }
                        } else {
                            Console.WriteLine("The opponent has blocked!");
                        }
                    } else {
                        Console.Write("Invalid unit! Press (C) to exit or (R) to reselect: ");
                    }
                } else {
                    validCmd = false;
                    Console.Write("This pawn has no enemies in range!");
                    Console.ReadLine();
                }
            } else {
                validCmd = false;
            }
        }

        private void Move() {
            bool valid = false;
            do {
                valid = curPlayer.PeformMove(Board);
                Console.ReadLine();
            } while (!valid);
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
            if (curPlayer == players[0])
                curPlayer = players[1];
            else
                curPlayer = players[0];
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
