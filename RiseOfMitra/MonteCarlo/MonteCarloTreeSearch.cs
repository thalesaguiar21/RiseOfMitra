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

namespace RiseOfMitra.MonteCarlo
{
    public class MonteCarloTreeSearch : Player
    {
        public Node GameTree;
        private Game CurGame;
        private Game MCTSGame;
        private int strat;
        private ISelectionStrategy Selection;
        private ISimulationStrategy SimulationStrat;
        private const int MAX_PLAYOUTS = 50;
        private const int MAX_SIMULATION_TIME = 4; // Define um tempo máximo de execução de simulações
        private const int MAX_DEPTH = 4; // Define o quão profundo será a simulação

        public MonteCarloTreeSearch(ECultures cult, Game game, int strat = 1) {
            GameTree = new Node(0, game.GetBoards(), null);
            CurGame = game;
            Selection = null;
            Culture = cult;
            Cursor = new Coord(1, 1);
            Pawns = new List<APawn>();
            CultCenter = null;
            this.strat = strat;
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

        // The AI Algorithm
        public override Node PrepareAction(Board boards, Player oponent) {
            if (GameTree.Childs == null || GameTree.Childs.Count <= 0) {
                GameTree.Childs = RunSimulation();
            }
            Selection = new OMCSelection(GameTree.Childs, MAX_PLAYOUTS);
            GameTree = Selection.Execute();
            return GameTree;
        }

        private List<Node> RunSimulation() {
            // Childs of the current game state
            List<Node> nextMoves = new List<Node>();
            Node GameTreeCp = new Node(GameTree.Value, GameTree.Boards, GameTree.Cmd);

            Stopwatch cron = new Stopwatch();
            cron.Start();

            int playouts = 0;
            while(playouts < MAX_PLAYOUTS) {
                MCTSGame = new Game(CurGame);
                Node curr = GameTreeCp;
                List<Node> path = new List<Node>();

                Random rand = new Random();
                int depth = 0;
                while (depth < MAX_DEPTH) {
                    if (MCTSGame.IsOver()) {
                        break;
                    }

                    List<ACommand> mctsCmds = MCTSGame.GetValidCommands();
                    if(mctsCmds == null || mctsCmds.Count == 0) {
                        MCTSGame.SetNextPlayer();
                        mctsCmds = MCTSGame.GetValidCommands();
                    }
                    SimulationStrat = new BestOfAllSimulation(mctsCmds);

                    List<ACommand> bestCmds = SimulationStrat.Execute();
                    
                    int chosen = rand.Next(bestCmds.Count);
                    Node nextState = new Node(bestCmds[chosen].Value(), 
                                              MCTSGame.GetBoards(),
                                              bestCmds[chosen]);
                    // Saves the current path taken
                    path.Add(nextState);

                    // If the node is a directly descendent, it checks if its already added 
                    // to the next moves.
                    if(depth == 0) {
                        bool visited = false;
                        foreach (Node node in nextMoves) {
                            if (node.Equals(nextState)) {
                                visited = true;
                                break;
                            }
                        }
                        if(!visited)
                            nextMoves.Add(nextState);
                    }

                    // Checks if the state is in the game tree, if not then
                    // expands the game tree by adding the new state
                    bool expanded = false;
                    for (int i = 0; i < curr.Childs.Count; i++) {
                        if (curr.Childs[i].Equals(nextState)) {
                            expanded = true;
                            nextState = curr.Childs[i];
                            break;
                        }
                    }
                    if (!expanded) {
                        curr.Childs.Add(nextState);
                    }

                    // Increments the node visit count, create a copy of the current player
                    // and executes the new state in the game copy
                    nextState.VisitCount++;
                    Player playerCp = MCTSGame.GetCurPlayer().Copy(MCTSGame.GetBoards());
                    MCTSGame.ChangeState(nextState, true);

                    nextState.Cmd.SetCurPlayer(playerCp);
                    curr = nextState;
                    depth++;
                }
                playouts++;
                for (int i = path.Count - 1; i > 0; i--) {
                    path[i - 1].Value += path[i].Value;
                }
            }
            if (nextMoves == null) {
                UserUtils.PrintError("No moves left!");
            }
                
            return nextMoves;
        }

        public void SetGame(Game game) {
            if (game != null)
                MCTSGame = game;
        }
    }
}
