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
        private ISelectionStrategy Selection;
        private ISimulationStrategy SimulationStrat;
        private const int MAX_PLAYOUTS = 100;
        // Define the simulation max maximum time
        private const int MAX_SIMULATION_TIME = 4;
        // Defines the simulation depth
        private const int MAX_DEPTH = 10; 

        public MonteCarloTreeSearch(ECultures cult, Game game) {
            GameTree = new Node(0, game.GetBoards(), null);
            CurGame = game;
            Selection = null;
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

        // The AI Algorithm
        public override Node PrepareAction(Board boards, Player oponent) {
            if (GameTree.Childs == null || GameTree.Childs.Count <= 0) {
                GameTree.Childs = RunSimulation();
            }
            Selection = new OMCSelection(GameTree.Childs);
            GameTree = Selection.Execute();
            return GameTree;
        }

        private List<Node> RunSimulation() {
            // Childs of the current game state
            List<Node> nextMoves = new List<Node>();
            Node GameTreeCp = new Node(GameTree.Value, GameTree.Boards, GameTree.Cmd);
            
            for (int i = 0; i < MAX_PLAYOUTS; i++) {

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

                    // If the current player has no pawns left
                    if (mctsCmds == null || mctsCmds.Count == 0) {
                        MCTSGame.SetNextPlayer();
                        mctsCmds = MCTSGame.GetValidCommands();
                    }

                    int chosen = rand.Next(mctsCmds.Count);
                    Node nextState = new Node(mctsCmds[chosen].Value(), MCTSGame.GetBoards(), mctsCmds[chosen]);
                    path.Add(nextState);

                    // If the node is a directly descendent, it checks if its already added 
                    // to the next moves.
                    if (depth == 0) {
                        bool visited = false;
                        foreach (Node node in nextMoves) {
                            if (node.Equals(nextState)) {
                                visited = true;
                                break;
                            }
                        }
                        if (!visited)
                            nextMoves.Add(nextState);
                    }

                    // Checks if the state is in the game tree, if not then
                    // expands the game tree by adding the new state
                    bool expanded = false;
                    for (int k = 0; k < curr.Childs.Count; k++) {
                        if (curr.Childs[k].Equals(nextState)) {
                            expanded = true;
                            nextState = curr.Childs[k];
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

                // Backpropagates the value of nodes in the simulation path until
                for (int j = path.Count - 1; j > 0; j--) {
                    path[j - 1].Value += path[j].Value;
                    path[j - 1].Value /= j;
                }
            }             
                
            return nextMoves;
        }

        public void SetGame(Game game) {
            if (game != null)
                MCTSGame = game;
        }
    }
}
