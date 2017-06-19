using System;
using System.Collections.Generic;
using System.Linq;
using Utils.Types;

namespace Juno
{
    public class PawnsPerTerrain {
        public ECultures Cult;
        public Dictionary<ETerrain, int> PPTs { get; }

        public PawnsPerTerrain(ECultures cult) {
            Cult = cult;
            Array terrains = Enum.GetValues(typeof(ETerrain));
            PPTs = new Dictionary<ETerrain, int>();

            for (int i = 0; i < terrains.Length; i++) {
                PPTs.Add((ETerrain)terrains.GetValue(i), 0);
            }
        }

        public void IncreasePawnsAt(ETerrain terrain) {
            if (PPTs.ContainsKey(terrain))
                PPTs[terrain] += 1;
        }

        public void ResetNumbers() {
            foreach (ETerrain key in PPTs.Keys.ToList()) {
                PPTs[key] = 0;
            }
        }

        public void SetNumOfPawnsAt(ETerrain terrain, int numOfPawns) {
            if(numOfPawns > 0) {
                if (PPTs.ContainsKey(terrain))
                    PPTs[terrain] = numOfPawns;
                else
                    PPTs.Add(terrain, numOfPawns);
            }
        }

        public override string ToString() {
            string msg = String.Format("CULTURE: {0} --> (TERRAIN, PAWN) : ", Cult);
            foreach (ETerrain key in PPTs.Keys) {
                msg += String.Format("({0}, {1}) ;", key, PPTs[key]);
            }
            return msg;
        }
    }
}
