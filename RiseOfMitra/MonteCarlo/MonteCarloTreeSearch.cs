using System;
using System.Collections.Generic;
using System.Linq;

using RiseOfMitra.Players;
using RiseOfMitra.Players.Commands;
using RiseOfMitra.MonteCarlo.Expansion;
using RiseOfMitra.MonteCarlo.Selection;
using RiseOfMitra.MonteCarlo.Simulation;

using Utils;
using Utils.Types;
using Utils.Space;

using Boards;

using Units;

namespace RiseOfMitra.MonteCarlo
{
    public class MonteCarloTreeSearch : Player
    {
        public Node GameTree;
        Game CurGame;
        Game MCTSGame;
        ISelection Selection;
        ISimulation Simulation;
        IExpansion Expansion;
        const int MAX_PLAYOUTS = 10;
        // Define the simulation max maximum time
        const int MAX_TIME = 4;
        // Defines the simulation depth
        const int MAX_SIMULATION_DEPTH = 5;

        public MonteCarloTreeSearch(ECultures cult, ISelection selection, ISimulation simulation, Game game)
        {
            GameTree = new Node(game.GetBoards(), null);
            CurGame = game;
            Selection = selection;
            Simulation = simulation;
            Expansion = new ExpandAll();
            Culture = cult;
            Cursor = new Coord(1, 1);
            Pawns = new List<APawn>();
            CultCenter = null;
        }

        private MonteCarloTreeSearch(MonteCarloTreeSearch other)
        {
            GameTree = new Node(null, null);
            Culture = other.GetCulture();
            Cursor = new Coord(1, 1);
            Pawns = new List<APawn>();
            CultCenter = other.CultCenter;
        }

        public override Player Copy(Board board)
        {
            var mcts = new MonteCarloTreeSearch(this);
            var tmpCursor = new Coord(GetCursor().X, GetCursor().Y);
            for (int i = 0; i < GetPawns().Count; i++) {
                var tmpPawn = GetPawns()[i].Copy(board);
                mcts.AddPawn(tmpPawn);
            }
            mcts.SetCultCenter(CultCenter.Copy(board));
            mcts.SetCursor(tmpCursor);
            return mcts;
        }

        public override Node PrepareAction(Node prevState, Player oponent)
        {
            if (prevState != null) {
                int index = GameTree.Childs.IndexOf(prevState);
                if (index == -1) {
                    GameTree = prevState;
                } else {
                    GameTree = GameTree.Childs[index];
                }
            }
            return RunMCTS();
        }

        private Node RunMCTS()
        {
            var path = new Stack<Node>();
            var nextMove = new Node(null, null);
            Node currMove = GameTree;
            Node lastMove = currMove;
            int playouts = 0;
            int simResult = 0;
            var rnd = new Random();
            while (playouts < MAX_PLAYOUTS) {
                MCTSGame = new Game(CurGame);
                currMove = RunSelection(lastMove, currMove, path);
                Expansion.Expand(lastMove, currMove);
                simResult = RunSimulation(currMove);
                Backpropagation(path, simResult);
                currMove = GameTree;
                playouts++;
            }
            nextMove = GameTree.Childs.ElementAt(rnd.Next(GameTree.Childs.Count));
            return nextMove;
        }

        private Node RunSelection(Node lastMove, Node currMove, Stack<Node> path)
        {
            Node result = null;
            SelectionParameters parameters;
            bool select = true;

            while (select) {
                path.Push(currMove);
                var validCmds = MCTSGame.GetValidCommands();
                if (validCmds.Count == 0) {
                    MCTSGame.ChangeState(null, true);
                    validCmds = MCTSGame.GetValidCommands();
                }
                parameters.root = lastMove;
                parameters.validStates = new List<Node>();
                foreach (ACommand cmd in validCmds) {
                    parameters.validStates.Add(new Node(MCTSGame.GetBoards(), cmd));
                }
                lastMove = currMove;
                currMove = Selection.Execute(parameters);
                Player tmpPlayer = MCTSGame.GetCurPlayer().Copy(MCTSGame.GetBoards());
                currMove.Command.CurPlayer = tmpPlayer;
                select = ((lastMove.Equals(currMove)) || (lastMove.Childs.Contains(currMove)))
                    && !MCTSGame.IsOver();
                if (select)
                    MCTSGame.ChangeState(currMove, true);
                result = currMove;
            }
            return result;
        }


        /// <summary>
        /// Plays k simulated games using the given Simulation Strategy.
        /// </summary>
        /// <param name="state">The game tree root</param>
        /// <returns>
        /// 1 case the AI wins, -1 case it loses and 0 if the simulation dos not reach
        /// a leaf node that finish the game.
        /// </returns>
        private int RunSimulation(Node state)
        {
            int result = 0;
            int depth = 0;
            double simValue = 0;
            double mctsValue = 0;
            var tmpGame = new Game(MCTSGame);
            var currState = new Node(state.Boards, state.Command);
            var validStates = new List<Node>();
            var rnd = new Random();

            while (depth < MAX_SIMULATION_DEPTH && !tmpGame.IsOver()) {
                validStates = Node.FromRange(tmpGame.GetBoards(), tmpGame.GetValidCommands());
                if (validStates.Count == 0) {
                    tmpGame.ChangeState(null, true);
                    validStates = Node.FromRange(tmpGame.GetBoards(), tmpGame.GetValidCommands());
                }
                Simulation.SetUp(validStates);
                validStates = Simulation.Execute();

                var tmpState = validStates.ElementAt(rnd.Next(validStates.Count));
                tmpState.Command.CurPlayer = tmpGame.GetCurPlayer().Copy(tmpGame.GetBoards());
                var nextMove = new Node(tmpGame.GetBoards(), tmpState.Command);
                simValue += nextMove.Value;
                if (tmpGame.GetCurPlayer() is MonteCarloTreeSearch) {
                    mctsValue += nextMove.Value;
                }

                tmpGame.ChangeState(nextMove, true);
                depth++;
            }

            double res = simValue - mctsValue;
            if (res == 0) {
                result = 0;
            } else if (res < 0) {
                result = 1;
            } else {
                result = -1;
            }

            return result;
        }

        private void Backpropagation(Stack<Node> path, int result)
        {
            if (path != null && path.Count > 0) {
                if (result == 0 || result == 1 || result == -1) {
                    Node last = path.Pop();
                    while (path.Count > 0) {
                        Node curr = path.Pop();
                        curr.Value = last.Value;
                        last = curr;
                    }
                } else {
                    UserUtils.PrintError(result + " is not a valid simulation result!");
                    Console.ReadLine();
                }
            }
        }

        public void SetGame(Game game)
        {
            if (game != null)
                MCTSGame = game;
        }
    }
}
