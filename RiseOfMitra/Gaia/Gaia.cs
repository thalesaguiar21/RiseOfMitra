using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types;
using Game;
using Cells;
using Players;
using Units;


/* 
 *          PPTs    MeanAllyDistance    MeanEnemyDistance   CultCenterRisk
 * Dalrion  DPpts       8.4                     3.4                60.3 
 * Rahkar   RPpts       5.7                     3.4                30.73
 * 
 */

namespace Juno
{
    public class Gaia
    {
        private const double CRITICAL_DIST = Math.E * 0.6;
        private const double MAX_CENTER_RISK = 100.0;
        private const int NUM_OF_CULTS = 2;
        private const int ALLY_DIST = 0;
        private const int ENMY_DIST = 1;
        private const int CENTER_RISK = 2;

        PawnsPerTerrain DalrionPpts, RahkarPpts;
        List<ABasicPawn> NearEnemies;
        List<double> DalrionStatistcs;
        List<double> RahkarStatistcs;

        public Gaia() {
            DalrionPpts = new PawnsPerTerrain(ECultures.DALRIONS);
            RahkarPpts = new PawnsPerTerrain(ECultures.RAHKARS);
            NearEnemies = new List<ABasicPawn>();
            DalrionStatistcs = new List<double>() { 0, 0, 0 };
            RahkarStatistcs = new List<double>() { 0, 0, 0 };
        }

        public void DoGaiaWill(Player playerOne, Player playerTwo) {
            InspectField(playerOne, playerTwo);

            Console.WriteLine(DalrionPpts);
            Console.WriteLine(String.Format("ALY_DIST: {0} | ENMY_DIST: {1} | CENTER_RISK: {2}", 
                DalrionStatistcs[ALLY_DIST], DalrionStatistcs[ENMY_DIST], DalrionStatistcs[CENTER_RISK]));

            Console.WriteLine(RahkarPpts);
            Console.WriteLine(String.Format("ALY_DIST: {0} | ENMY_DIST: {1} | CENTER_RISK: {2}",
                RahkarStatistcs[ALLY_DIST], RahkarStatistcs[ENMY_DIST], RahkarStatistcs[CENTER_RISK]));
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
                double totalDist = 0;
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
                    double totalDist = 0;
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
            if(pawns != null && pawns.Count > 0) {
                if (pawns[0].NativeOf() == ECultures.DALRIONS)
                    DalrionPpts.ResetNumbers();
                else
                    RahkarPpts.ResetNumbers();

                foreach (APawn pawn in pawns) {
                    if (pawn.NativeOf() == ECultures.DALRIONS)
                        DalrionPpts.IncreasePawnsAt(pawn.GetTerrain());
                    else
                        RahkarPpts.IncreasePawnsAt(pawn.GetTerrain());
                }
            }
        }

        private void CalculateCulturalCenterRisk(List<ABasicPawn> allies, List<ABasicPawn> enemies, CulturalCenter center) {
            if(allies != null && enemies != null && center != null) {
                if(enemies.Count > 0) {
                    double risk = 0;
                    risk += RiskPerEnemyProximity(enemies, center);
                    risk += RiskPerAllyCloseToEnemy(allies);
                    if(center.GetCurrLife() > 0) risk += Math.Exp(1.0 / center.GetCurrLife());
                    SetCenterRisk(allies[0].NativeOf(), risk);
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
                result = Math.Exp(1.0 / nearAllies);
            return result;
        }

        private double RiskPerEnemyProximity(List<ABasicPawn> enemies, CulturalCenter center) {
            NearEnemies.Clear();
            double result = 0;
            if(enemies != null && center != null) {
                if(enemies.Count > 0) {
                    foreach (ABasicPawn enemy in enemies) {
                        double dist = Math.Exp(1.0 / Coord.Distance(enemy.GetPos(), center.GetPos()));
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
                    DalrionStatistcs[CENTER_RISK] = risk;
                else
                    RahkarStatistcs[CENTER_RISK] = risk;
            }
        }

        private void SetMeanAllyDistance(ECultures cult, double dist) {
            if (dist >= 0) {
                if (cult == ECultures.DALRIONS)
                    DalrionStatistcs[ALLY_DIST] = dist;
                else
                    RahkarStatistcs[ALLY_DIST] = dist;
            }
        }

        private void SetMeanEnemyDist(ECultures cult, double dist) {
            if (dist >= 0) {
                if (cult == ECultures.DALRIONS)
                    DalrionStatistcs[ENMY_DIST] = dist;
                else
                    RahkarStatistcs[ENMY_DIST] = dist;
            }
        }
    }
}
