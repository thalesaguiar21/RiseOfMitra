using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boards;
using RiseOfMitra.MonteCarlo;
using RiseOfMitra.Players.Commands;
using Utils.Space;
using Units;
using Utils.Types;

namespace RiseOfMitra.Players
{
    class RandomPlayer : Player
    {
        private Game CurGame;

        public RandomPlayer(ECultures cult, Game curGame) {
            CurGame = curGame;
            Culture = cult;
            Pawns = new List<APawn>();
            CultCenter = null;
            Cursor = new Coord(1, 1);
        }

        public override Player Copy(Board board) {
            Player random = new RandomPlayer(Culture, CurGame);
            Coord tmpCursor = new Coord(0, 0);
            for (int i = 0; i < GetPawns().Count; i++) {
                APawn tmpPawn = GetPawns()[i].Copy(board);
                random.AddPawn(tmpPawn);
            }
            random.SetCultCenter(CultCenter);
            random.SetCursor(tmpCursor);
            return random;
        }

        public override Node PrepareAction(Node currState, Player oponent) {

            Random rand = new Random();
            List<ACommand> validCmds = CurGame.GetValidCommands();

            if(currState != null && validCmds != null && validCmds.Count > 0) {
                double highest = 0;
                foreach (ACommand cmd in validCmds) {
                    if (cmd.Value() > highest)
                        highest = cmd.Value();
                }

                List<ACommand> someCmds = new List<ACommand>();
                for (int i = 0; i < validCmds.Count; i++) {
                    if (validCmds[i].Value() >= 0.5 * highest)
                        someCmds.Add(validCmds[i]);
                }

                int selected = rand.Next(someCmds.Count);
                return new Node(currState.Boards, someCmds[selected]);
            } else {
                return null;
            }
        }
    }
}
