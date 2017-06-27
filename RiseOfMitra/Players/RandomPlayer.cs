using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boards;
using RiseOfMitra.MonteCarlo;

namespace RiseOfMitra.Players
{
    class RandomPlayer : Player
    {
        public override Player Copy(Board board) {
            throw new NotImplementedException();
        }

        public override Node PrepareAction(Board boards, Player oponent) {
            throw new NotImplementedException();
        }
    }
}
