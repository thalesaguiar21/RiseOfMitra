﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.Types;
using Utils.Space;
using Juno;
using Boards;
using Players;
using Players.Commands;
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
        private bool ValidCmd;
        private Board Boards;

        public Game() {
            Play = true;
            ValidCmd = false;
            Boards = new Board();
            InitPlayers();
            CreateUnits();
            PlaceUnits();
        }

        public Game(Game game) {
            Boards = new Board(game.Boards);
            Play = game.Play;
            ValidCmd = game.ValidCmd;
            Gamers = new Player[2] { game.Gamers[0].Copy(Boards),
                                          game.Gamers[1].Copy(Boards) };
            CurPlayer = game.CurPlayer.Copy(Boards);
        }

        private void CreateUnits() {
            if (Gamers != null && Gamers.Length == 2) {
                PawnFactory pawnFac = new PawnFactory();
                for (int i = 0; i < BoardConsts.INITIAL_PAWNS; i++) {
                    APawn dPawn = pawnFac.Create(ECultures.DALRIONS, Boards);
                    dPawn.SetPos(new Coord(1 + i, 7));
                    Gamers[0].AddPawn(dPawn);

                    APawn rPawn = pawnFac.Create(ECultures.RAHKARS, Boards);
                    rPawn.SetPos(new Coord(BoardConsts.MAX_LIN - 2 - i, BoardConsts.MAX_COL - 8));
                    Gamers[1].AddPawn(rPawn);
                }

                CulturalCenterFactory centFac = new CulturalCenterFactory();
                ABuilding dCenter = centFac.Create(ECultures.DALRIONS, Boards);
                ABuilding rCenter = centFac.Create(ECultures.RAHKARS, Boards);

                Gamers[0].SetCulturalCenter((CulturalCenter)dCenter);
                Gamers[1].SetCulturalCenter((CulturalCenter)rCenter);
            } else {
                throw new ArgumentException("Invalid player array!");
            }
        }

        private void InitPlayers() {
            Gamers = new Player[2];

            Gamers[0] = new HumanPlayer(ECultures.DALRIONS);
            CurPlayer = Gamers[0];

            Gamers[1] = new MonteCarloTreeSearch(ECultures.RAHKARS, this);
            Gamers[1].SetCursor(new Coord(BoardConsts.MAX_LIN - 2, BoardConsts.MAX_COL - 2));
        }

        private void PlaceUnits() {
            foreach (Player it in Gamers) {
                foreach (Unit unit in it.GetUnits()) {
                    unit.Place();
                }
            }
        }

        public void Start() {
            Gaia gaia = new Gaia();
            do {                
                gaia.DoGaiaWill(Gamers[0], Gamers[1]);
                Boards.PrintBoard();
                //ShowValidMoves();
                Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
                ACommand cmd = CurPlayer.PrepareAction(Boards, GetOponent());
                ValidCmd = ChangeState(cmd);

                Console.Write("Press enter to continue...");
                Console.ReadLine();
                if (ValidCmd)
                    SetNextPlayer();

                foreach (Player player in Gamers) {
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

        public bool ChangeState(ACommand command) {
            if (command != null) {
                command.SetUp(Boards, CurPlayer, GetOponent());
                return command.Execute();                
            } else
                return false;
        }
        
        private void SetNextPlayer() {
            CurPlayer.SetTurn();
            CurPlayer.ExecuteTurnEvents(Boards.GetBoard());

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

        public Board GetState() {
            return Boards;
        }

        public bool GameOver() {
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
            List<ACommand> validMvs = new List<ABasicPawn>().Cast<ACommand>().ToList();
            foreach (APawn pawn in CurPlayer.GetPawns()) {
                Dijkstra didi = new Dijkstra(Boards.GetBoard(), pawn.GetPos(), pawn.GetMovePoints());
                List<Coord> moveRange = didi.GetValidPaths(Command.MOVE);
                foreach (Coord cell in moveRange) {
                    MoveCommand mv = new MoveCommand();
                    mv.SetUp(CurPlayer, pawn.GetPos(), cell, Boards);
                    if (mv.IsValid()) validMvs.Add(mv);
                }
            }

            return validMvs;
        }

        private List<ACommand> GetValidAttacks() {
            List<ACommand> validAtks = new List<ABasicPawn>().Cast<ACommand>().ToList();

            foreach(ABasicPawn pawn in CurPlayer.GetPawns()) {
                foreach (ABasicPawn enemy in GetOponent().GetPawns()) {
                    AttackCommand atk = new AttackCommand();
                    atk.SetUp(pawn.GetPos(), enemy.GetPos(), CurPlayer, GetOponent(), Boards);
                    if (atk.IsValid()) validAtks.Add(atk);
                }
            }

            return validAtks;
        }

        private void ShowValidMoves() {
            List<ACommand> cmds = GetValidCommands();
            Console.WriteLine("Valid moves count: " + cmds.Count);
            foreach (ACommand command in cmds) {
                Console.Write(command);
                Console.WriteLine();
            }

        }
    
        public static void Main() {
            try {
                Game rom = new Game();
                rom.Start();
            } catch (FormatException) {
                Console.WriteLine("Invalid Terrain file format!");
            } catch (IOException) {
                Console.WriteLine("Could not find Terrain file!");
            } catch (ArgumentException ae) {
                Console.WriteLine(ae.Message);
            } finally {
                Console.ReadLine();
            }
        }
    }
}
