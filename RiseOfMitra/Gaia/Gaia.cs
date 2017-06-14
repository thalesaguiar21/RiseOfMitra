using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types;
using Game;
using Cells;
using Units;

namespace Gaia
{
    class Gaia
    {
        private const double MAX_CENTER_RISK = 100.0;
        private const int NUM_OF_PARAMETERS = 4;
        private const int NUM_OF_CULTS = 2;
        private const double CRITICAL_DIST = Math.E * 0.6;

        PawnsPerTerrain DalrionPpts, RahkarPpts;
        List<ABasicPawn> NearEnemies;
        
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
            NearEnemies = new List<ABasicPawn>();

            DomainData[0, 0] = DalrionPpts;
            DomainData[1, 0] = RahkarPpts;

            for (int i = 0; i < NUM_OF_CULTS; i++) {
                for (int j = 1; j < NUM_OF_PARAMETERS; j++) {
                    DomainData[i, j] = 0;
                }
            }
        }

        public void DoGaiaWill(Player playerOne, Player playerTwo) {
            InspectField(playerOne, playerTwo);

            Console.WriteLine(DomainData[0, 0]);
            Console.WriteLine(String.Format("ALY_DIST: {0} | ENMY_DIST: {1} | CENTER_RISK: {2}", DomainData[0, 1], DomainData[0, 2], DomainData[0, 3]));

            Console.WriteLine(DomainData[1, 0]);
            Console.WriteLine(String.Format("ALY_DIST: {0} | ENMY_DIST: {1} | CENTER_RISK: {2}", DomainData[1, 1], DomainData[1, 2], DomainData[1, 3]));
        }

        private void InspectField(Player playerOne, Player playerTwo) {
            // Calculating player two statistics 
            SetPawnsPerTerrain(playerOne.GetPawns());
            CalculateMeanAllyDistance(playerOne.GetUnits());
            CalculateMeanEnemyDistance(playerOne.GetUnits(), playerTwo.GetUnits());
            CalculateCulturalCenterRisk(playerOne.GetAttackers(), playerTwo.GetAttackers(), playerOne.GetCenter());
            // Calculating player two statistics 
            SetPawnsPerTerrain(playerTwo.GetPawns());
            CalculateMeanAllyDistance(playerTwo.GetUnits());
            CalculateMeanEnemyDistance(playerTwo.GetUnits(), playerOne.GetUnits());
            CalculateCulturalCenterRisk(playerTwo.GetAttackers(), playerOne.GetAttackers(), playerTwo.GetCenter());
        }

        private void CalculateMeanAllyDistance(List<Unit> allies) {
            if(allies != null) {
                int totalDist = 0;
                if (allies.Count > 2) {
                    for (int i = 0; i < allies.Count; i++) {
                        List<int> distances = new List<int>();
                        for (int j = 1; j < allies.Count; j++) {
                            if (i != j) {
                                distances.Add(Coord.Distance(allies[i].GetPos(), allies[j].GetPos()));
                            }
                        }
                        totalDist += distances.Min();
                    }
                    if (totalDist > 0)
                        SetMeanAllyDistance(allies[0].NativeOf(), totalDist / allies.Count);
                } else
                    SetMeanAllyDistance(allies[0].NativeOf(), 0);
            }            
        }

        private void CalculateMeanEnemyDistance(List<Unit> allies, List<Unit> enemies) {
            if(allies != null && enemies != null) {
                if(allies.Count > 0 && enemies.Count > 0) {
                    int totalDist = 0;
                    for (int i = 0; i < allies.Count; i++) {
                        int min = Coord.Distance(allies[i].GetPos(), enemies[0].GetPos());
                        for (int j = 1; j < enemies.Count; j++) {
                            int currDist = Coord.Distance(allies[i].GetPos(), enemies[j].GetPos());
                            if (currDist < min) min = currDist;
                        }
                        totalDist += min;
                    }
                    if (totalDist > 0)
                        SetMeanEnemyDist(allies[0].NativeOf(), totalDist / allies.Count);
                }
            }
        }

        private void SetPawnsPerTerrain(List<APawn> pawns) {
            foreach (APawn pawn in pawns) {
                if (pawn.NativeOf() == ECultures.DALRIONS)
                    DalrionPpts.IncreasePawnsAt(pawn.GetTerrain());
                else
                    RahkarPpts.IncreasePawnsAt(pawn.GetTerrain());
            }
        }

        private void CalculateCulturalCenterRisk(List<ABasicPawn> allies, List<ABasicPawn> enemies, CulturalCenter center) {
            if(allies != null && enemies != null && center != null) {
                if(enemies.Count > 0) {
                    double risk = 0;
                    risk += RiskPerEnemyProximity(enemies, center);
                    risk += RiskPerAllyCloseToEnemy(allies);
                    if(center.GetCurrLife() > 0) risk += Math.Exp(1 / center.GetCurrLife());
                } else if (allies.Count > 0){
                    SetCenterRisk(allies[0].NativeOf(), 0);
                } else {
                    // No enemies or allies on the board, both center are in no risk
                    SetCenterRisk(ECultures.DALRIONS, 0);
                    SetCenterRisk(ECultures.RAHKARS, 0);
                }
            }
        }

        private double RiskPerAllyCloseToEnemy(List<ABasicPawn> allies) {
            double result = 0;
            int nearAllies = 0;
            foreach (ABasicPawn ally in allies) {
                foreach (ABasicPawn enemy in NearEnemies) {
                    double dist = Coord.Distance(ally.GetPos(), enemy.GetPos());
                    if (dist <= ally.GetMovePoints()) {
                        nearAllies++;
                    }
                }
            }
            if (nearAllies > 0)
                result = Math.Exp(1 / nearAllies);
            return result;
        }

        private double RiskPerEnemyProximity(List<ABasicPawn> enemies, CulturalCenter center) {
            double result = 0;
            if(enemies != null && center != null) {
                if(enemies.Count > 0) {
                    foreach (ABasicPawn enemy in enemies) {
                        double dist = Math.Exp(1 / Coord.Distance(enemy.GetPos(), center.GetPos()));
                        result += dist;
                        if (dist <= CRITICAL_DIST)
                            NearEnemies.Add(enemy);
                    }
                    result = result / enemies.Count;
                } else {
                    result = 0;
                }
            }
            return result;
        }

        private void SetCenterRisk(ECultures cult, double risk) {
            if(risk >= 0) {
                if (cult == ECultures.DALRIONS)
                    DomainData[0, 3] = risk;
                else
                    DomainData[1, 3] = risk;
            }
        }

        private void SetMeanAllyDistance(ECultures cult, double dist) {
            if (dist >= 0) {
                if (cult == ECultures.DALRIONS)
                    DomainData[0, 1] = dist;
                else
                    DomainData[1, 1] = dist;
            }
        }

        private void SetMeanEnemyDist(ECultures cult, double dist) {
            if (dist >= 0) {
                if (cult == ECultures.DALRIONS)
                    DomainData[0, 2] = dist;
                else
                    DomainData[1, 2] = dist;
            }
        }
    }
}
