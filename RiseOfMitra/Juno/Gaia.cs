using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiseOfMitra.Players;
using Units;
using Units.Centers;
using Units.Pawns;
using Utils.Types;
using Utils.Space;
using Boards;
using Utils;



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

        public void DoGaiaWill(Player playerOne, Player playerTwo, Board boards, int turn) {
            Random rnd = new Random();
            int actionTurn = 10 + rnd.Next(6);
            if (turn % actionTurn == 0) {
                InspectField(playerOne, playerTwo, boards);
                Array terrains = Enum.GetValues(typeof(ETerrain));
                int terrain = rnd.Next(terrains.Length);
                int cultToCheck = rnd.Next(2);
                Player other = null;
                Player currPlayer = null;
                PawnsPerTerrain currPpt = null;
                List<double> currStatistics = null;

                if (cultToCheck == 0) {
                    if (playerOne.GetCulture() == ECultures.DALRIONS) {
                        currPlayer = playerOne;
                        other = playerTwo;
                        currPpt = DalrionPpts;
                        currStatistics = DalrionStatistcs;
                    } else {
                        currPlayer = playerTwo;
                        other = playerOne;
                        currPpt = RahkarPpts;
                        currStatistics = RahkarStatistcs;
                    }
                } else {
                    if (playerOne.GetCulture() == ECultures.RAHKARS) {
                        currPlayer = playerOne;
                        other = playerTwo;
                        currPpt = RahkarPpts;
                        currStatistics = RahkarStatistcs;
                    } else {
                        currPlayer = playerTwo;
                        other = playerOne;
                        currPpt = DalrionPpts;
                        currStatistics = DalrionStatistcs;
                    }
                }
                List<APawn> playerPawns = currPlayer.GetPawns();
                string msg = "Gaia will now ";
                if (currPpt.PPTs[(ETerrain)terrains.GetValue(terrain)] < 2) {
                    if (currStatistics[ENMY_DIST] < 1) {
                        if(currPpt.OccupiedTerrains() > 3) {
                            msg += "increase the defense of pawns at " + (ETerrain)terrains.GetValue(terrain);
                            foreach (APawn pawn in playerPawns) {
                                if((ETerrain)boards.TerrainAt(pawn.GetPos()) == (ETerrain)terrains.GetValue(terrain)) {
                                    pawn.SetDef(pawn.GetDef() + 3);
                                }
                            }
                        } else {
                            msg += "set the atk of pawns at " + (ETerrain)terrains.GetValue(terrain) + " to 1!";
                            foreach (APawn pawn in playerPawns) {
                                if ((ETerrain)boards.TerrainAt(pawn.GetPos()) == (ETerrain)terrains.GetValue(terrain)
                                    && pawn is ABasicPawn) {
                                    ABasicPawn bPawn = (ABasicPawn)pawn;
                                    bPawn.SetAtk(1);
                                }
                            }
                        }
                    } else if (currStatistics[ENMY_DIST] >= 1 && currStatistics[ENMY_DIST] < 3) {
                        msg += "increase the move points of pawns at " + (ETerrain)terrains.GetValue(terrain);
                        foreach (APawn pawn in playerPawns) {
                            if ((ETerrain)boards.TerrainAt(pawn.GetPos()) != (ETerrain)terrains.GetValue(terrain))
                                pawn.SetMovePoints(pawn.GetMovePoints() + 2);
                        }
                    } else {
                        if(currStatistics[ALLY_DIST] < 3) {
                            msg += "set the move range of pawns at " + (ETerrain)terrains.GetValue(terrain) + " to 1!";
                            foreach (APawn pawn in playerPawns) {
                                if ((ETerrain)boards.TerrainAt(pawn.GetPos()) == (ETerrain)terrains.GetValue(terrain))
                                    pawn.SetMovePoints(1);
                            }
                        } else if (currStatistics[ALLY_DIST] >= 3  && currStatistics[ALLY_DIST] < 7) {
                            msg += "increase the attack range of pawns at " + (ETerrain)terrains.GetValue(terrain);
                            foreach (APawn pawn in playerPawns) {
                                if ((ETerrain)boards.TerrainAt(pawn.GetPos()) == (ETerrain)terrains.GetValue(terrain)
                                    && pawn is ABasicPawn) {
                                    ABasicPawn bPawn = (ABasicPawn)pawn;
                                    bPawn.SetAtkRange(bPawn.GetAtkRange() + 2);
                                }
                            }
                        } else {
                            msg += "set the def of pawns at " + (ETerrain)terrains.GetValue(terrain) + " to 1!";
                            foreach (APawn pawn in playerPawns) {
                                if ((ETerrain)boards.TerrainAt(pawn.GetPos()) != (ETerrain)terrains.GetValue(terrain))
                                    pawn.SetDef(1);
                            }
                        }
                    }
                }

                UserUtils.PrintSucess(msg + " For " + currPlayer.GetCulture());
                Console.ReadLine();
            }
        }

        private void InspectField(Player playerOne, Player playerTwo, Board boards) {
            // Calculating player two statistics 
            SetPawnsPerTerrain(playerOne.GetPawns(), boards);
            CalculateMeanAllyDistance(playerOne.GetUnits());
            CalculateMeanEnemyDistance(playerOne.GetUnits(), playerTwo.GetUnits());
            CalculateCulturalCenterRisk(playerOne.GetAttackers(), playerTwo.GetAttackers(), playerOne.GetCenter());
            // Calculating player two statistics 
            SetPawnsPerTerrain(playerTwo.GetPawns(), boards);
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

        private void SetPawnsPerTerrain(List<APawn> pawns, Board boards) {
            if(pawns != null && pawns.Count > 0) {
                if (pawns[0].NativeOf() == ECultures.DALRIONS)
                    DalrionPpts.ResetNumbers();
                else
                    RahkarPpts.ResetNumbers();

                foreach (APawn pawn in pawns) {
                    if (pawn.NativeOf() == ECultures.DALRIONS)
                        DalrionPpts.IncreasePawnsAt((ETerrain)boards.TerrainAt(pawn.GetPos()));
                    else
                        RahkarPpts.IncreasePawnsAt((ETerrain)boards.TerrainAt(pawn.GetPos()));
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
