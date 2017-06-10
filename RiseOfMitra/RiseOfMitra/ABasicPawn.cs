using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cells;
using Types;

namespace Game
{
    class ABasicPawn : APawn
    {
        public virtual void Adapt(ETerrain prevTerrain, ETerrain curTerrain);

        public override bool Move(Coord cursor) {
            throw new NotImplementedException();
        }
    }
}
