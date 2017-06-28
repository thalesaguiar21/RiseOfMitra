using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boards;
using RiseOfMitra.MonteCarlo;
using RiseOfMitra.Players.Commands;
using Utils.Space;
using Units.Pawns;
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
                int selected = rand.Next(validCmds.Count);
                return new Node(0.0, boards, validCmds[selected]);
            } else {
                return null;
            }

        }
    }
}
