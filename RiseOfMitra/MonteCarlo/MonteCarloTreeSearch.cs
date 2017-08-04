using System;
using System.Collections.Generic;
using System.Linq;
using RiseOfMitra.Players;
using Utils;
using Utils.Types;
using Utils.Space;
using RiseOfMitra.Players.Commands;
using Boards;
using Units;
using System.Diagnostics;

namespace RiseOfMitra.MonteCarlo
{
    public class MonteCarloTreeSearch : Player
    {
        public Node GameTree;
        private Game CurGame;
        private Game MCTSGame;
        private ISelection Selection;
        private ISimulation Simulation;
        private IExpansion Expansion;
        private const int MAX_PLAYOUTS = 100;
        // Define the simulation max maximum time
        private const int MAX_TIME = 4;
        // Defines the simulation depth
        private const int MAX_SIMULATION_DEPTH = 5; 

        public MonteCarloTreeSearch(ECultures cult, ISelection selection, ISimulation simulation, Game game) {
            GameTree = new Node(0, game.GetBoards(), null);
            CurGame = game;
            Selection = selection;
            Simulation = simulation;
            Expansion = new ExpandAll();
            Culture = cult;
            Cursor = new Coord(1, 1);
            Pawns = new List<APawn>();
            CultCenter = null;

        }

        private MonteCarloTreeSearch(ECultures cult) {
            GameTree = new Node(0, null, null);
            Culture = cult;
            Cursor = new Coord(1, 1);
            Pawns = new List<APawn>();
            CultCenter = null;
        }

        public override Player Copy(Board board) {
            Player mcts = new MonteCarloTreeSearch(GetCulture());
            Coord tmpCursor = new Coord(GetCursor().X, GetCursor().Y);
            for (int i = 0; i < GetPawns().Count; i++) {
                APawn tmpPawn = GetPawns()[i].Copy(board);
                mcts.AddPawn(tmpPawn);
            }
            mcts.SetCultCenter(CultCenter.Copy(board));
            mcts.SetCursor(tmpCursor);
            return mcts;
        }
        
        public override Node PrepareAction(Node currState, Player oponent) {

            Node nextPlay = null;
            // Verify if the last state belongs to the mcts game tree
            // If the curr state is null, that means the previous player had no moves
            // and the game state did not changed.
            if(currState != null) {
                if (!GameTree.Childs.Contains(currState)) {
                    GameTree = currState;
                } else {
                    foreach (Node node in GameTree.Childs) {
                        if (node.Equals(currState)) {
                            GameTree = node;
                            break;
                        }
                    }
                }
            }

            if (Selection != null && Simulation != null) {
                List<Node> selectionPath = new List<Node>();
                Stopwatch cron = new Stopwatch();
                TimeSpan max = new TimeSpan(0, 0, MAX_TIME);
                SelectionParameters args;
                int simulationResult = 0;
                Node auxRoot = GameTree;
                selectionPath.Add(auxRoot);
                // Traverse the game tree searching until it reaches a node that has no childs
                if(auxRoot != null) {
                    while ((auxRoot.Childs != null) && (auxRoot.Childs.Count > 0)) {
                        
                        args.root = auxRoot;
                        args.validStates = auxRoot.Childs;

                        auxRoot = Selection.Execute(args);
                        auxRoot.VisitCount++;
                        selectionPath.Add(auxRoot);
                    }
                }
                // Starts to simulate games from the last node visited in the previous loop
                cron.Start();
                while (cron.Elapsed < max) {
                    if (auxRoot != null) {
                        simulationResult = RunSimulation(auxRoot);
                        Backpropagation(selectionPath, simulationResult);
                        selectionPath.Clear();
                    }
                    auxRoot = GameTree;
                }

                Random rnd = new Random();
                nextPlay = GameTree.Childs.ElementAt(rnd.Next(auxRoot.Childs.Count));
                GameTree = nextPlay;
            }
            return nextPlay;
        }


        /// <summary>
        /// Plays k simulated games using the given Simulation Strategy.
        /// </summary>
        /// <param name="state">The game tree root</param>
        /// <returns>
        /// 1 case the AI wins, -1 case it loses and 0 if the simulation dos not reach
        /// a leaf node that finish the game.
        /// </returns>
        private int RunSimulation(Node state) {

            MCTSGame = new Game(CurGame);
            int result = 0;
            int playouts = 0;
            Random rnd = new Random();
            List<ACommand> simulatedCmds;
            List<ACommand> validCmds;
            
            while (playouts < MAX_SIMULATION_DEPTH) {
                
                validCmds = MCTSGame.GetValidCommands();
                // If the current player has no pawns left
                if(validCmds.Count == 0) {
                    MCTSGame.SetNextPlayer();
                    validCmds = MCTSGame.GetValidCommands();
                }

                Simulation.SetUp(validCmds);
                simulatedCmds = Simulation.Execute();

                int index = rnd.Next(simulatedCmds.Count);
                Player playerCp = MCTSGame.GetCurPlayer().Copy(MCTSGame.GetBoards());
                Node nextState = new Node(simulatedCmds[index].Value(), MCTSGame.GetBoards(), simulatedCmds[index]);

                Expansion.Expand(state, nextState);

                MCTSGame.ChangeState(nextState, true);
                nextState.Cmd.SetCurPlayer(playerCp);
                state = nextState;
                playouts++;

                if (MCTSGame.IsOver()) {
                    break;
                }
            }
            return result;
        }

        private void Backpropagation(List<Node> path, int result) {

            if (path != null && path.Count > 0) {
                if (result == 0 || result == 1 || result == -1) {
                    foreach (Node state in path) {
                        state.WinRate += result;
                        state.VisitCount++;
                    }
                } else {
                    UserUtils.PrintError(result + " is not a valid simulation result!");
                    Console.ReadLine();
                }
            }
        }

        public void SetGame(Game game) {
            if (game != null)
                MCTSGame = game;
        }
    }
}
