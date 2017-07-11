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
            Center = null;
            Cursor = new Coord(1, 1);
        }

        public override Player Copy(Board board) {
            Player random = new HumanPlayer(GetCulture());
            Coord tmpCursor = new Coord(GetCursor().X, GetCursor().Y);
            for (int i = 0; i < GetPawns().Count; i++) {
                APawn tmpPawn = GetPawns()[i].Copy(board);
                random.AddPawn(tmpPawn);
            }
            random.SetCulturalCenter(Center.Copy(board));
            random.SetCursor(tmpCursor);
            return random;
        }

        public override Node PrepareAction(Board boards, Player oponent) {
            //Console.WriteLine("Random player's turn!");
            Random rand = new Random();
            List<ACommand> validCmds = CurGame.GetValidCommands();

            if(validCmds != null && validCmds.Count > 0) {
                double highest = 0;
                foreach (ACommand cmd in validCmds) {
                    if (cmd.Value() > highest)
                        highest = cmd.Value();
                }

                List<ACommand> someCmds = new List<ACommand>();
                for (int i = 0; i < validCmds.Count; i++) {
                    if (validCmds[i].Value() >= 0.7 * highest)
                        someCmds.Add(validCmds[i]);
                }

                int selected = rand.Next(someCmds.Count);
                return new Node(0.0, boards, someCmds[selected]);
            } else {
                return null;
            }

        }
    }
}
