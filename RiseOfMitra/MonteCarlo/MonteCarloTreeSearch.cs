using System;
using System.Collections.Generic;
using System.Linq;
using RiseOfMitra.Players;
using Utils.Types;
using Utils.Space;
using RiseOfMitra.Players.Commands;
using Boards;
using Units.Pawns;
using System.Diagnostics;

namespace RiseOfMitra.MonteCarlo
{
    public class MonteCarloTreeSearch : Player
    {
        public Node GameTree;
        private Game CurGame;
        private Game MCTSGame;
        private ISelectionStrategy Selection;
        private ISimulationStrategy SimulationStrat;
        private const int MAX_PLAYOUTS = 500;
        private const int MAX_SIMULATION_TIME = 3; // Define um tempo máximo de execução de simulações
        private const int MAX_DEPTH = 4; // Define o quão profundo será a simulação

        public MonteCarloTreeSearch(ECultures cult, Game game) {
            GameTree = new Node(0, game.GetState(), null);
            CurGame = game;
            Selection = null;
            Culture = cult;
            Cursor = new Coord(1, 1);
            Pawns = new List<APawn>();
            Center = null;
        }

        private MonteCarloTreeSearch(ECultures cult) {
            GameTree = new Node(0, null, null);
            Culture = cult;
            Cursor = new Coord(1, 1);
            Pawns = new List<APawn>();
            Center = null;
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

        // The AI Algorithm
        public override Node PrepareAction(Board boards, Player oponent) {

            Console.Write("Musashi is thinking...");
            if (GameTree.Childs == null || GameTree.Childs.Count <= 0) {
                GameTree.Childs = RunSimulation();
            }
            Console.WriteLine("Finisehd");

            Selection = new OMCSelection(GameTree.Childs);
            GameTree = Selection.Execute();
            Console.WriteLine(GameTree.Cmd.ToString());
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

                Random rand = new Random();
                int depth = 0;
                while (depth < MAX_DEPTH) {
                    List<ACommand> mctsCmds = MCTSGame.GetValidCommands();
                    SimulationStrat = new BestOfAllSimulation(mctsCmds);

                    List<ACommand> bestCmds = SimulationStrat.Execute();
                    int chosen = rand.Next(bestCmds.Count);
                    Node nextState = new Node(bestCmds[chosen].Value(), 
                                              new Board(MCTSGame.GetState()),
                                              bestCmds[chosen]);

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
                    Player playerCp = MCTSGame.GetCurPlayer().Copy(MCTSGame.GetState());
                    MCTSGame.ChangeState(nextState, true);

                    nextState.Cmd.SetCurPlayer(playerCp);
                    curr = nextState;
                    depth++;
                }
                playouts++;
            }
            return nextMoves;
        }

        public void SetGame(Game game) {
            if (game != null)
                MCTSGame = game;
        }
    }
}
