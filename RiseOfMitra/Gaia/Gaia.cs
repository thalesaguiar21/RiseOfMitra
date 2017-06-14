using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types;
using Game;
using Cells;

namespace Gaia
{
    class Gaia
    {
        PawnsPerTerrain DalrionPpts, RahkarPpts;

        const double MAX_CENTER_RISK = 100.0;
        const int NUM_OF_PARAMETERS = 4;
        const int NUM_OF_CULTS = 2;
        
        /* 
         *          PPTs    MeanAllyDistance    MeanEnemyDistance   CultCenterRisk
         * Dalrion  DPpts       8.4                     3.4                60.3 
         * Rahkar   RPpts       5.7                     3.4                30.73
         * 
         */
        Object[,] DomainData;

        public Gaia() {
            DomainData = new Object[NUM_OF_CULTS, NUM_OF_PARAMETERS];
            DalrionPpts = new PawnsPerTerrain(ECultures.DALRIONS);
            RahkarPpts = new PawnsPerTerrain(ECultures.RAHKARS);

            DomainData[0, 0] = DalrionPpts;
            DomainData[1, 0] = RahkarPpts;

            for (int i = 0; i < NUM_OF_CULTS; i++) {
                for (int j = 1; j < NUM_OF_PARAMETERS; j++) {
                    DomainData[i, j] = 0;
                }
            }
        }

        public void CalculateMeanAllyDistance() {
            List<APawn> pawns = new List<APawn>();



            
        }
    }
}
