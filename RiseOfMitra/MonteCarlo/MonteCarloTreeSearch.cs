using System;
using System.Collections.Generic;
using Players;
using Utils.Types;
using Utils.Space;
using Players.Commands;
using Boards;
using RiseOfMitra;
using Units.Pawns;

namespace MonteCarlo
{
    public class MonteCarloTreeSearch : Player
    {
        SelectionStrategy selector;
        Game curGame;
        Game MCTSGame;
        List<Board> States;

        public MonteCarloTreeSearch(ECultures cult, Game game) {
            Culture = cult;
            Cursor = new Coord(1, 1);
            Pawns = new List<APawn>();
            Center = null;
            selector = null;
            curGame = game;
        }

        private MonteCarloTreeSearch(ECultures cult) {
            Culture = cult;
            Cursor = new Coord(1, 1);
            Pawns = new List<APawn>();
            Center = null;
            selector = null;
        }

        public override Player Copy(Board board) {
            Player mcts = new MonteCarloTreeSearch(GetCulture());
            Coord tmpCursor = new Coord(GetCursor().X, GetCursor().Y);
            for (int i = 0; i < GetPawns().Count; i++) {
                APawn tmpPawn = GetPawns()[i].Copy(board);
                mcts.AddPawn(tmpPawn);
            }
            mcts.SetCulturalCenter(Center.Copy(board));
            mcts.SetCursor(tmpCursor);
            return mcts;
        }

        public override ACommand PrepareAction(Board boards, Player oponent) {
            MCTSGame = new Game(curGame);
            ACommand rndCmd = null;
            if(curGame != null) {
                Random rnd = new Random();
                List<ACommand> allCMds = MCTSGame.GetValidCommands();
                if(allCMds != null) {
                    int move = rnd.Next(allCMds.Count);
                    rndCmd = allCMds[move];
                }
            }
            boards.SetStatus(("Musashi will do:\n" + rndCmd.ToString()).Split('\n'));
            Console.Clear();
            boards.PrintBoard();
            Console.Write("Musashi turn...");
            return rndCmd;
        }

        private void RunSimulation() {

        }

        private void Expand() {

        }

        public void SetGame(Game game) {
            if (game != null)
                MCTSGame = game;
        }
    }
}
