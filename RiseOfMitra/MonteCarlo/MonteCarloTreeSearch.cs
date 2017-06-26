using System;
using System.Collections.Generic;
using System.Linq;
using Players;
using Utils.Types;
using Utils.Space;
using Players.Commands;
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
        ISelectionStrategy Selection;
        private const int MAX_SIMULATION_TIME = 3; // Define um tempo máximo de execução de simulações
        private const int MAX_DEPTH = 3; // Define o quão profundo será a simulação

        public MonteCarloTreeSearch(ECultures cult, Game game) {
            GameTree = new Node(0, game.GetState(), null);
            CurGame = game;
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
            Random rnd = new Random();


            if(GameTree.Childs != null && GameTree.Childs.Count > 0) {
                Console.Write("Musashi is thinking...");
                int t = rnd.Next(GameTree.Childs.Count);
                Console.WriteLine("Finisehd");
                GameTree = GameTree.Childs[t];
                return GameTree;
            } else {
                Stopwatch cron = new Stopwatch();
                cron.Start();
                Console.Write("Musashi is thinking...");
                GameTree.Childs = RunSimulation();
                //Run selection
                Console.WriteLine("Finisehd");
                Console.ReadLine();
                int t = rnd.Next(GameTree.Childs.Count);
                GameTree = GameTree.Childs[t];
                return GameTree;
            }
        }

        private List<Node> RunSimulation() {
            // Childs of the current game state
            List<Node> nextMoves = new List<Node>();

            Stopwatch cron = new Stopwatch();
            cron.Start();
            int playouts = 0;
            while(playouts < 200) {
                MCTSGame = new Game(CurGame);
                Node GameTreeCp = new Node(GameTree.Value, GameTree.Boards, GameTree.Cmd);
                Node curr = GameTreeCp;

                Random rand = new Random();
                int depth = 0;
                while (depth < MAX_DEPTH) {
                    List<ACommand> mctsCmds = MCTSGame.GetValidCommands();
                    int cmd = rand.Next(mctsCmds.Count);
                    Node nextState = new Node(mctsCmds[cmd].Value(), 
                                              new Board(MCTSGame.GetState()), 
                                              mctsCmds[cmd]);
                    nextState.VisitCount++;

                    if(depth == 0) {
                        bool visited = false;
                        foreach (Node node in nextMoves) {
                            if (node.Equals(nextState)) {
                                visited = true;
                                node.VisitCount++;
                                break;
                            }
                        }
                        if(!visited)
                            nextMoves.Add(nextState);
                    }

                    if (!curr.Childs.Contains(nextState)) {
                        curr.Childs.Add(nextState);
                    } else {
                        int visited = curr.Childs.IndexOf(nextState);
                        curr.Childs.ElementAt(visited).VisitCount++;
                    }

                    MCTSGame.ChangeState(nextState);
                    curr = nextState;
                    depth++;
                }
                playouts++;
            }
            return nextMoves;
        }

        private void Expand(Node state) {

        }

        public void SetGame(Game game) {
            if (game != null)
                MCTSGame = game;
        }
    }
}
