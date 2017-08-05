﻿using System;
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
using RiseOfMitra.MonteCarlo.Expansion;
using RiseOfMitra.MonteCarlo.Selection;
using RiseOfMitra.MonteCarlo.Simulation;

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

        private MonteCarloTreeSearch(MonteCarloTreeSearch other) {
            GameTree = new Node(0, null, null);
            Culture = other.GetCulture();
            Cursor = new Coord(1, 1);
            Pawns = new List<APawn>();
            CultCenter = other.CultCenter;
        }

        public override Player Copy(Board board) {
            Player mcts = new MonteCarloTreeSearch(this);
            Coord tmpCursor = new Coord(GetCursor().X, GetCursor().Y);
            for (int i = 0; i < GetPawns().Count; i++) {
                APawn tmpPawn = GetPawns()[i].Copy(board);
                mcts.AddPawn(tmpPawn);
            }
            mcts.SetCultCenter(CultCenter.Copy(board));
            mcts.SetCursor(tmpCursor);
            return mcts;
        }
        
        public override Node PrepareAction(Node prevState, Player oponent) {

            Node nextPlay = null;
            // Verify if the last state belongs to the mcts game tree
            // If the curr state is null, that means the previous player had no moves
            // and the game state did not changed.
            if(prevState != null) {
                if (!GameTree.Childs.Contains(prevState)) {
                    GameTree = prevState;
                } else {
                    foreach (Node node in GameTree.Childs) {
                        if (node.Equals(prevState)) {
                            GameTree = node;
                            break;
                        }
                    }
                }
            }

            if (Selection != null && Simulation != null) {
                Node currState = GameTree;
                Node lastNode = currState;
                List<Node> selectionPath = new List<Node>();
                List<Node> validStates = new List<Node>();
                Stopwatch cron = new Stopwatch();
                TimeSpan max = new TimeSpan(0, 0, MAX_TIME);
                SelectionParameters args;
                int simulationResult = 0;
                selectionPath.Add(currState);
                // Traverse the game tree searching until it reaches a node that has no childs

                cron.Start();
                while (cron.Elapsed < max) {
                    MCTSGame = new Game(CurGame);
                    // Traverse the game tree using a given strategy
                    while (lastNode.Equals(currState) || lastNode.Childs.Contains(currState)) {
                        validStates.Clear();
                        List<ACommand> validCmds = MCTSGame.GetValidCommands();
                        if (validCmds.Count > 0) {
                            foreach (ACommand cmd in validCmds) {
                                validStates.Add(new Node(cmd.Value(), MCTSGame.GetBoards(), cmd));
                            }

                            args.root = currState;
                            args.validStates = validStates;

                            lastNode = currState;
                            currState = Selection.Execute(args);
                            selectionPath.Add(currState);
                        } else {
                            currState = null;
                        }              
                        MCTSGame.ChangeState(currState, true);
                        if (MCTSGame.IsOver()) {
                            break;
                        }
                    }
                    if (currState != null && !MCTSGame.IsOver()) {
                        // Here, the selected node is added to the game tree
                        Expansion.Expand(lastNode, currState);
                        // Starts to simulate games starting from the new state added
                        simulationResult = RunSimulation(currState);
                        // Backpropagates the simulation result through the node visited in the selection
                        Backpropagation(selectionPath, simulationResult);
                    }
                    currState = GameTree;
                    selectionPath.Clear();
                }

                Random rnd = new Random();
                if (!MCTSGame.IsOver()) {
                    nextPlay = GameTree.Childs.ElementAt(rnd.Next(currState.Childs.Count));
                    GameTree = nextPlay;
                }
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

            int result = 0;
            int playouts = 0;
            Random rnd = new Random();
            List<ACommand> simulatedCmds;
            List<ACommand> validCmds;
            Game tmpGame = new Game(MCTSGame);
            Node nextState = null;


            while (playouts < MAX_SIMULATION_DEPTH) {

                if (tmpGame.IsOver()) {
                    break;
                }
                validCmds = tmpGame.GetValidCommands();
                
                // If the current player has no pawns left
                if(validCmds.Count > 0) {
                    Simulation.SetUp(validCmds);
                    simulatedCmds = Simulation.Execute();

                    Player playerCp = tmpGame.GetCurPlayer().Copy(tmpGame.GetBoards());
                    int index = rnd.Next(simulatedCmds.Count);
                    nextState = new Node(simulatedCmds[index].Value(), tmpGame.GetBoards(), simulatedCmds[index]);

                    Expansion.Expand(state, nextState);

                    nextState.Cmd.SetCurPlayer(playerCp);
                    state = nextState;
                } else {
                    nextState = null;
                }
                tmpGame.ChangeState(nextState, true);
                playouts++;
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
