using System;
using System.Collections.Generic;
using Types;
using Cells;
using Consts;
using System.IO;
using System.Linq;
using ShortestPath;

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
            PlaceUnits();
        }

        public Game(Game game) {
            this.Boards = new Board(game.Boards);
            this.Play = game.Play;
            this.ValidCmd = game.ValidCmd;
            this.Players = new Player[2] { game.Players[0].Copy(Boards),
                                           game.Players[1].Copy(Boards)};
            this.CurPlayer = game.CurPlayer.Copy(Boards);
        }

        private void InitPlayers() {
            Players = new Player[2];
            Players[0] = new HumanPlayer(ECultures.DALRIONS);
            Players[1] = new HumanPlayer(ECultures.RAHKARS);

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
                Console.WriteLine("Number of valid moviments: " + GetValidCommands().Count);
                Boards.PrintBoard();
                ACommand cmd = CurPlayer.PrepareAction(Boards, GetOponent());
                ValidCmd = cmd.Execute();
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

        public List<ACommand> GetValidCommands() {
            List<ACommand> validCmds = new List<ACommand>();
            validCmds.AddRange(GetValidAttacks());
            validCmds.AddRange(GetValidMoviments());

            return validCmds;
        }

        private List<ACommand> GetValidMoviments() {
            List<ACommand> validMvs = new List<ABasicPawn>().Cast<ACommand>().ToList();
            MoveCommand mv = new MoveCommand();
            foreach (APawn pawn in CurPlayer.GetPawns()) {
                Dijkstra didi = new Dijkstra(Boards.GetBoard(), pawn.GetPos(), pawn.GetMovePoints());
                List<Coord> moveRange = didi.GetValidPaths(Commands.MOVE);
                foreach (Coord cell in moveRange) {
                    mv.SetUp(CurPlayer, pawn.GetPos(), cell, Boards);
                    if (mv.IsValid()) validMvs.Add(mv);
                }
            }

            return validMvs;
        }

        private List<ACommand> GetValidAttacks() {
            List<ACommand> validAtks = new List<ABasicPawn>().Cast<ACommand>().ToList();
            AttackCommand atk = new AttackCommand();

            foreach(ABasicPawn pawn in CurPlayer.GetPawns()) {
                foreach (ABasicPawn enemy in GetOponent().GetPawns()) {
                    atk.SetUp(pawn.GetPos(), enemy.GetPos(), CurPlayer, GetOponent(), Boards);
                    if (atk.IsValid()) validAtks.Add(atk);
                }
            }

            return validAtks;
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
