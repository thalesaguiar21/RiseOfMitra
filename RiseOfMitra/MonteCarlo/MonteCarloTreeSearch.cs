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
        List<Node> GameTree;
        List<ACommand> ExpandedCommands;
        Game curGame;
        Game MCTSGame;
        private const int MAX_SIMULATION_TIME = 3; // Define um tempo máximo de execução de simulações
        private const int MAX_SIMULATION_MOVE = 10; // Define o quão profundo será a simulação

        public MonteCarloTreeSearch(ECultures cult, Game game) {
            Culture = cult;
            Cursor = new Coord(1, 1);
            Pawns = new List<APawn>();
            Center = null;
            curGame = game;
            GameTree = new List<Node>();
            ExpandedCommands = new List<ACommand>();
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

        // The AI Algorithm
        public override ACommand PrepareAction(Board boards, Player oponent) {
            // No início dessa função, significa que o jogador humano já executou alguma ação
            // Dessa forma, deve-se adicionar um novo nó à Game Tree
            GameTree.Add(new Node(0, 0, new Board(curGame.GetState()), oponent));
            // Cria um novo comando que será realizado pela Inteligência artificial
            ACommand rndCmd = null;
            // Identifica e seleciona todos os comandos válidos
            List<ACommand> cmds = curGame.GetValidCommands();
            // Cria um cronômetro para especificar o tempo limite para as simulações
            Stopwatch cron = new Stopwatch();
            cron.Start();
            Console.Write("Musashi is thinking...");
            while (cron.Elapsed < TimeSpan.FromSeconds(MAX_SIMULATION_TIME)) {
                // Executa simulações até que um tempo especificado seja esgotado
                RunSimulation(cmds);
            }
            // Após as simulações, alguns comandos aleatórios foram selecionados.
            // Dentre eles, usa-se alguma métrica de seleção
            Random rnd = new Random();
            int move = rnd.Next(ExpandedCommands.Count);

            rndCmd = ExpandedCommands[move];
            
            ExpandedCommands.Clear();

            boards.SetStatus(("Musashi will do:\n" + rndCmd.ToString()).Split('\n'));
            Console.Clear();
            boards.PrintBoard();
            return rndCmd;
        }

        private void RunSimulation(List<ACommand> validMoves) {
            bool Expand = true;
            // Cria uma cópia do jogo atual para evitar que as simulações interfiram no jogo real
            MCTSGame = new Game(curGame);
            // Seleciona um comando válido aleatoriamente
            ACommand rndCmd = null;
            Random rnd = new Random();
            if (validMoves != null) {
                int move = rnd.Next(validMoves.Count);
                rndCmd = validMoves[move];
            }
            // Adiciona um possível comando para a Game Tree a partir do estado
            if (Expand && !ExpandedCommands.Contains(rndCmd))
                ExpandedCommands.Add(rndCmd);
            // Cria uma cópia dos estados alcançados até agora pela Game Tree
            List<Node> gameTreeCp = new List<Node>(GameTree);
            // Inicializa as simulações
            int counter = 0;
            while(counter < MAX_SIMULATION_MOVE) {
                // Identifica e cria todos os comandos possíveis a partir do estado atual
                List<ACommand> validCmds = MCTSGame.GetValidCommands();
                // Seleciona um comando válido aleatoriamente
                ACommand posRndCmd = null;
                if (validCmds != null) {
                    int move = rnd.Next(validCmds.Count);
                    posRndCmd = validCmds[move];
                }
                // Executa o comando na cópia do jogo
                MCTSGame.ChangeState(posRndCmd);
                // Verifica se o novo estado é um estado final
                if (MCTSGame.GameOver())
                    break;
                counter++;
            }
        }

        private void Expand() {

        }

        public void SetGame(Game game) {
            if (game != null)
                MCTSGame = game;
        }
    }
}
