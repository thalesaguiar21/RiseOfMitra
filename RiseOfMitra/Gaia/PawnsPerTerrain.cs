using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types;

namespace Gaia
{
    public class PawnsPerTerrain {
        public ECultures Cult;
        public Dictionary<ETerrain, int> PPTs { get; }

        public PawnsPerTerrain(ECultures cult) {
            Array terrains = Enum.GetValues(typeof(ETerrain));
            PPTs = new Dictionary<ETerrain, int>();

            for (int i = 0; i < terrains.Length; i++) {
                PPTs.Add((ETerrain)terrains.GetValue(i), 0);
            }
        }

        public void SetNumOfPawnsAt(ETerrain terrain, int numOfPawns) {
            if(numOfPawns > 0) {
                PPTs.Add(terrain, numOfPawns);
            }
        }
    }
}
