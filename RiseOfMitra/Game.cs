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
using RiseOfMitra.MonteCarlo.Selection;
using RiseOfMitra.MonteCarlo.Simulation;

namespace RiseOfMitra
{

    public class Game
    {
        Player[] Gamers;
        Player CurPlayer;
        bool Play;
        Board Boards;
        public static int wins = 0;

        public Game()
        {
            Play = true;
            Boards = new Board();
            InitPlayers();
            CreateUnits();
            PlaceUnits();
        }

        public Game(Game game)
        {
            Boards = new Board(game.Boards);
            Play = game.Play;
            Gamers = new Player[2] { game.Gamers[0].Copy(Boards), game.Gamers[1].Copy(Boards) };
            CurPlayer = game.CurPlayer.Copy(Boards);
        }

        private void CreateUnits()
        {
            if (Gamers != null && Gamers.Length == 2) {
                var pawnFac = new PawnFactory();
                for (int i = 0; i < BoardConsts.INITIAL_PAWNS; i++) {
                    var dPawn = pawnFac.Create(ECultures.DALRIONS);
                    dPawn.Position = new Coord(1 + i, 7);
                    Gamers[0].AddPawn(dPawn);

                    var rPawn = pawnFac.Create(ECultures.RAHKARS);
                    rPawn.Position = new Coord(BoardConsts.MAX_LIN - 2 - i, BoardConsts.MAX_COL - 8);
                    Gamers[1].AddPawn(rPawn);
                }

                var centFac = new CulturalCenterFactory();
                var dCenter = centFac.Create(ECultures.DALRIONS, Boards);
                var rCenter = centFac.Create(ECultures.RAHKARS, Boards);

                Gamers[0].SetCultCenter((CulturalCenter)dCenter);
                Gamers[1].SetCultCenter((CulturalCenter)rCenter);
            } else {
                throw new ArgumentException("Invalid player array!");
            }
        }

        private void InitPlayers()
        {
            Gamers = new Player[2];
            Gamers[0] = new HumanPlayer(ECultures.DALRIONS);
            Gamers[1] = new HumanPlayer(ECultures.RAHKARS);

            CurPlayer = Gamers[0];
            Gamers[1].SetCursor(new Coord(BoardConsts.MAX_LIN - 2, BoardConsts.MAX_COL - 2));
        }

        private void PlaceUnits()
        {
            foreach (Player it in Gamers) {
                foreach (Unit unit in it.GetUnits()) {
                    unit.Place(Boards);
                }
            }
        }

        public void Menu()
        {
            var menu = new UI();
            menu.PrintMenu();
        }

        public void Start()
        {
            Menu();
            var gaia = new Gaia();
            int turn = 1;
            var firstCmd = new MoveCommand(new Coord(1, 1), new Coord(1, 1), CurPlayer, GetOponent());
            var current = new Node(Boards, firstCmd);
            do {
                Boards.Status = Gamers[0].GetCultCenter().GetStatus().Split('\n');
                Boards.PrintBoard();
                Node state = CurPlayer.PrepareAction(current, GetOponent());
                ChangeState(state);
                current = state;
                //gaia.DoGaiaWill(Gamers[0], Gamers[1], Boards, turn);
                turn++;
                Console.Clear();
            } while (Play);

            if (HasWinner()) {
                string winnerMsg = String.Format("Congratulations! Now, {0} own the monopoly of Argyros!", GetOponent().GetCulture());
                Console.WriteLine(winnerMsg);
            }
        }

        private bool HasWinner()
        {
            bool hasWinner = false;
            if (CurPlayer.GetCultCenter() == null) {
                hasWinner = true;
            } else if (CurPlayer.GetCultCenter().CurrLife <= 0) {
                hasWinner = true;
            }
            if (hasWinner && GetOponent().GetCulture() == ECultures.RAHKARS) {
                wins++;
            }
            return hasWinner;
        }

        public void ChangeState(Node state, bool isSimulation = false)
        {
            if (state == null)
                SetNextPlayer();
            else if (Node.ValidateNode(state)) {
                if (state.Command.Execute(Boards, isSimulation)) {
                    SetNextPlayer();
                }
                foreach (Player player in Gamers) {
                    if (player.GetCultCenter() == null || player.GetCultCenter().CurrLife <= 0)
                        Play = false;
                }
            } else {
                UserUtils.PrintError("Invalid node!");
            }
        }

        public void SetNextPlayer()
        {
            CurPlayer.IncreaseTurn();
            CurPlayer.ExecuteTurnEvents(Boards);
            if (CurPlayer.Equals(Gamers[0])) {
                CurPlayer = Gamers[1];
            } else {
                CurPlayer = Gamers[0];
            }
        }

        public Player GetOponent()
        {
            if (CurPlayer == Gamers[0])
                return Gamers[1];
            else
                return Gamers[0];
        }

        public Board GetBoards()
        {
            return Boards;
        }

        public bool IsOver()
        {
            return !Play;
        }

        public Player GetCurPlayer()
        {
            return CurPlayer;
        }

        public List<ACommand> GetValidCommands()
        {
            var validCmds = new List<ACommand>();
            validCmds.AddRange(GetValidAttacks());
            validCmds.AddRange(GetValidMoviments());

            return validCmds;
        }

        private List<MoveCommand> GetValidMoviments()
        {
            var validMvs = new List<MoveCommand>();
            foreach (APawn pawn in CurPlayer.GetPawns()) {
                var didi = new Dijkstra(Boards.GetBoard(), pawn.Position, pawn.MovePoints);
                var moveRange = didi.GetValidPaths(Command.MOVE);
                foreach (Coord cell in moveRange) {
                    var mv = new MoveCommand(pawn.Position, cell, CurPlayer, GetOponent());
                    if (mv.IsValid(Boards)) validMvs.Add(mv);
                }
            }

            return validMvs;
        }

        private List<AttackCommand> GetValidAttacks()
        {
            var validAtks = new List<AttackCommand>();
            foreach (ABasicPawn pawn in CurPlayer.GetPawns()) {
                foreach (ABasicPawn enemy in GetOponent().GetPawns()) {
                    if (Coord.Distance(pawn.Position, enemy.Position) < pawn.MovePoints) {
                        var atk = new AttackCommand(pawn.Position, enemy.Position, CurPlayer, GetOponent());
                        if (atk.IsValid(Boards)) validAtks.Add(atk);
                    }
                }
            }

            return validAtks;
        }

        public void SetCurPlayer(Player player)
        {
            CurPlayer = player;
        }

        public static void Main()
        {

            try {
                int t = 0;
                while (t < 20) {
                    Game rom = new Game();
                    rom.Start();
                }
            } catch (FormatException) {
                Console.WriteLine("Invalid Terrain or Board file format!");
            } catch (IOException) {
                Console.WriteLine("Could not find Terrain or Board file!");
            } finally {
                Console.ReadLine();
            }
        }
    }
}
