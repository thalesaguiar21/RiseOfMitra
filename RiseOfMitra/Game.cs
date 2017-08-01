using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Utils.Types;
using Utils.Space;
using Juno;
using Boards;
using RiseOfMitra.Players;
using RiseOfMitra.Players.Commands;
using Units;
using Units.Pawns;
using Units.Centers;
using RiseOfMitra.MonteCarlo;

namespace RiseOfMitra
{
    public class Game
    {
        private Player[] Gamers;
        private Player CurPlayer;
        private bool Play;
        private Board Boards;
        public static int wins = 0;

        public Game() {
            Play = true;
            Boards = new Board();
            InitPlayers();
            CreateUnits();
            PlaceUnits();
        }

        public Game(Game game) {
            Boards = new Board(game.Boards);
            Play = game.Play;
            Gamers = new Player[2] { game.Gamers[0].Copy(Boards),
                                          game.Gamers[1].Copy(Boards) };
            CurPlayer = game.CurPlayer.Copy(Boards);
        }

        private void CreateUnits() {
            if (Gamers != null && Gamers.Length == 2) {
                PawnFactory pawnFac = new PawnFactory();
                for (int i = 0; i < BoardConsts.INITIAL_PAWNS; i++) {
                    APawn dPawn = pawnFac.Create(ECultures.DALRIONS);
                    dPawn.Position = new Coord(1 + i, 7);
                    Gamers[0].AddPawn(dPawn);

                    APawn rPawn = pawnFac.Create(ECultures.RAHKARS);
                    rPawn.Position = new Coord(BoardConsts.MAX_LIN - 2 - i, BoardConsts.MAX_COL - 8);
                    Gamers[1].AddPawn(rPawn);
                }

                CulturalCenterFactory centFac = new CulturalCenterFactory();
                ABuilding dCenter = centFac.Create(ECultures.DALRIONS, Boards);
                ABuilding rCenter = centFac.Create(ECultures.RAHKARS, Boards);

                Gamers[0].SetCultCenter((CulturalCenter)dCenter);
                Gamers[1].SetCultCenter((CulturalCenter)rCenter);
            } else {
                throw new ArgumentException("Invalid player array!");
            }
        }

        private void InitPlayers() {

            Gamers = new Player[2];
            Gamers[0] = new RandomPlayer(ECultures.DALRIONS, this);
            Gamers[1] = new MonteCarloTreeSearch(ECultures.RAHKARS, this);

            CurPlayer = Gamers[0];
            Gamers[1].SetCursor(new Coord(BoardConsts.MAX_LIN - 2, BoardConsts.MAX_COL - 2));
        }

        private void PlaceUnits() {
            foreach (Player it in Gamers) {
                foreach (Unit unit in it.GetUnits()) {
                    unit.Place(Boards);
                }
            }
        }

        public void Menu() {
            UI menu = new UI();
            menu.PrintMenu();
        }

        public void Start() {
            Menu();
            Gaia gaia = new Gaia();
            int turn = 1;
            do {
                Boards.PrintBoard();
                Node state = CurPlayer.PrepareAction(Boards, GetOponent());
                ChangeState(state);
                //gaia.DoGaiaWill(Gamers[0], Gamers[1], Boards, turn);
                turn++;
                Console.Clear();
            } while (Play);

            if (CurPlayer.GetCultCenter() == null || CurPlayer.GetCultCenter().CurrLife <= 0) {
                string winner = "";
                if (CurPlayer.GetCulture() == ECultures.DALRIONS) {
                    winner = "Rahkars";
                    wins++;
                } else {
                    winner = "Dalrions";
                }
                string winnerMsg = String.Format("Congratulations! Now, {0} own the monopoly of Argyros!", winner);
                Console.WriteLine(winnerMsg);
            }
        }

        public void ChangeState(Node state, bool isSimulation = false) {
            if (state == null)
                SetNextPlayer();
            else if(Node.ValidateNode(state)) {
                state.Cmd.SetUp(Boards, CurPlayer, GetOponent());
                bool validCmd = state.Cmd.IsValid();
                if (validCmd) {
                    if (state.Value == 0)
                        state.Value = state.Cmd.Value();
                    if(state.Cmd.Execute(isSimulation))
                        SetNextPlayer();
                    
                    foreach (Player player in Gamers) {
                        if (player.GetCultCenter() == null || player.GetCultCenter().CurrLife <= 0)
                            Play = false;
                    }
                } else {
                    UserUtils.PrintError("Invalid command!");
                }
            } else {
                UserUtils.PrintError("Invalid node!");
            }
        }
        
        public void SetNextPlayer() {
            CurPlayer.IncreaseTurn();
            CurPlayer.ExecuteTurnEvents(Boards);

            if (CurPlayer == Gamers[0]) {
                CurPlayer = Gamers[1];
            } else {
                CurPlayer = Gamers[0];   
            }
        }

        public Player GetOponent() {
            if (CurPlayer == Gamers[0])
                return Gamers[1];
            else
                return Gamers[0];
        }

        public Board GetBoards() {
            return Boards;
        }

        public bool IsOver() {
            return !Play;
        }

        public Player GetCurPlayer() {
            return CurPlayer;
        }

        public List<ACommand> GetValidCommands() {
            List<ACommand> validCmds = new List<ACommand>();
            validCmds.AddRange(GetValidAttacks());
            validCmds.AddRange(GetValidMoviments());

            return validCmds;
        }

        private List<ACommand> GetValidMoviments() {
            List<ACommand> validMvs = new List<ACommand>();
            foreach (APawn pawn in CurPlayer.GetPawns()) {
                Dijkstra didi = new Dijkstra(Boards.GetBoard(), pawn.Position, pawn.MovePoints);
                List<Coord> moveRange = didi.GetValidPaths(Command.MOVE);
                foreach (Coord cell in moveRange) {
                    MoveCommand mv = new MoveCommand();
                    mv.SetUp(Boards, CurPlayer, GetOponent());
                    mv.SetUp(CurPlayer, pawn.Position, cell, Boards);
                    if (mv.IsValid()) validMvs.Add(mv);
                }
            }

            return validMvs;
        }

        private List<ACommand> GetValidAttacks() {
            List<ACommand> validAtks = new List<ACommand>();
            foreach(ABasicPawn pawn in CurPlayer.GetPawns()) {
                foreach (ABasicPawn enemy in GetOponent().GetPawns()) {
                    if(Coord.Distance(pawn.Position, enemy.Position) < pawn.MovePoints) {
                        AttackCommand atk = new AttackCommand();
                        atk.SetUp(Boards, CurPlayer, GetOponent());
                        atk.SetUp(pawn.Position, enemy.Position, CurPlayer, GetOponent(), Boards);
                        if (atk.IsValid()) validAtks.Add(atk);
                    }
                }
            }

            return validAtks;
        }

        public void SetCurPlayer(Player player) {
            CurPlayer = player;
        }

        public static void Main() {
            try {
                 Game rom = new Game();
                 rom.Start();
            } catch (FormatException) {
                Console.WriteLine("Invalid Terrain or Board file format!");
            } catch (IOException) {
                Console.WriteLine("Could not find Terrain or Board file!");
            } catch (ArgumentException ae) {
                Console.WriteLine(ae.Message);
            } finally {
                Console.ReadLine();
            }
        }
    }
}
