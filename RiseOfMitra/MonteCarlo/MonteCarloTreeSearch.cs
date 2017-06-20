using System;
using System.Collections.Generic;
using System.Linq;
using Players;
using Utils.Types;
using Utils.Space;
using Players.Commands;
using Boards;
using RiseOfMitra;
using Units.Pawns;
using System.Diagnostics;

namespace RiseOfMitra.MonteCarlo
{
    public class MonteCarloTreeSearch : Player
    {
        List<Node> GameTree;
        Game curGame;
        Game MCTSGame;
        private const int MAX_SIMULATION_TIME = 60; // Define um tempo máximo de execução de simulações
        private const int MAX_SIMULATION_MOVE = 10; // Define o quão profundo será a simulação

        public MonteCarloTreeSearch(ECultures cult, Game game) {
            Culture = cult;
            Cursor = new Coord(1, 1);
            Pawns = new List<APawn>();
            Center = null;
            curGame = game;
            GameTree = new List<Node>();
        }

        private MonteCarloTreeSearch(ECultures cult) {
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

        // The algorithm
        public override ACommand PrepareAction(Board boards, Player oponent) {

            // Selection
            // No início dessa função, significa que o jogador humano já executou alguma ação
            // Dessa forma, deve-se adicionar um novo nó à Game Tree
            GameTree.Add(new Node(0, 0, new Board(curGame.GetState()), oponent));
            // Cria um novo comando que será realizado pela Inteligência artificial
            ACommand rndCmd = null;
            // Cria um cronômetro para especificar o tempo limite para as simulações
            Stopwatch cron = new Stopwatch();
            cron.Start();
            Console.Write("Musashi is thinking");
            while (cron.Elapsed < TimeSpan.FromSeconds(MAX_SIMULATION_TIME)) {
                Console.Write(".");
                // Executa simulações até que um tempo especificado seja esgotado
                RunSimulation();
                System.Threading.Thread.Sleep(3000);
            }
            if (curGame != null) {
                Random rnd = new Random();
                MCTSGame = new Game(curGame);
                List<ACommand> allCMds = MCTSGame.GetValidCommands();
                if (allCMds != null) {
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

        private ACommand RunSimulation() {
            ACommand cmd = null;
            // Cria uma cópia do jogo atual para evitar que as simulações interfiram no jogo real
            MCTSGame = new Game(curGame);
            // Cria uma cópia dos estados alcançados até agora pela Game Tree
            List<Node> gameTreeCp = new List<Node>(GameTree);
            // Cria uma subárvore onde serão adicionados os nós expandidos
            List<Node> gameTreeExapansion = new List<Node>();
            gameTreeExapansion.Add(gameTreeCp.Last());
            // Inicializa as simulações
            int counter = 0;
            while(counter < MAX_SIMULATION_MOVE) {
                // Identifica e cria todos os comandos possíveis a partir do estado atual
                List<ACommand> validCmds = MCTSGame.GetValidCommands();
                // Seleciona um comando válido aleatoriamente
                ACommand rndCmd = null;
                Random rnd = new Random();
                if (validCmds != null) {
                    int move = rnd.Next(validCmds.Count);
                    rndCmd = validCmds[move];
                }
                bool expand = true;
                // Executa o comando na cópia do jogo
                MCTSGame.ChangeState(rndCmd);
                // Adiciona o novo estado a cópia da Game Tree
                gameTreeExapansion.Add(new Node(0, 0, new Board(MCTSGame.GetState()), MCTSGame.GetCurPlayer()));
                // Verifica se o novo estado é um estado final
                if (MCTSGame.GameOver()) break;
                counter++;
            }
            return cmd;
        }

        private void Expand() {

        }

        public void SetGame(Game game) {
            if (game != null)
                MCTSGame = game;
        }
    }
}
