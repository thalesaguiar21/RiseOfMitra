using System;
using Types;
using Cells;
using System.Text;
using System.Collections.Generic;
using ShortestPath;
using Consts;

namespace RiseOfMitra
{
    abstract class ABasicPawn : Unit, IAdaptable
    {
        private int MovePoints;
        private int Atk;
        private int AtkRange;
        private const int MAX_MOVE = 20;
        private const int MAX_RANGE = 5;
        private const int MAX_ATK = 20;

        public new string GetStatus() {
            StringBuilder msg = new StringBuilder(base.GetStatus());
            msg.Append("Mov: " + MovePoints + "\n");
            msg.Append("Atk: " + Atk + "\n");
            msg.Append("Atk range: " + AtkRange + "\n");
            return msg.ToString();
        }

        public int GetMovePoints() { return MovePoints; }
        public int GetAtk() { return Atk; }
        public int GetAtkRange() { return AtkRange; }

        public void SetAtkRange(int atkRange) {
            if (atkRange > 0 && atkRange <= MAX_RANGE)
                AtkRange = atkRange;
        }

        public void SetMovePoints(int movePoints) {
            if (movePoints < 1 || movePoints > MAX_MOVE)
                Console.WriteLine(movePoints + " isn't a valid movement point!");
            else {
                MovePoints = movePoints;
            }
        }

        public void SetAtk(int atk) {
            if (atk < 0 || atk > MAX_ATK)
                Console.WriteLine(atk + " isn't a valid atack point!");
            else {
                Atk = atk;
            }
        }

        public virtual Coord Attack(Coord cursor, List<Unit> enemies) {
            bool validTarget = false;
            Coord target = null;
            Dijkstra didi = new Dijkstra(Board, GetPos(), GetAtkRange());
            List<Coord> attackRange = didi.GetValidPaths(Commands.ATTACK);
            List<Coord> enemiesInRange = new List<Coord>();

            foreach (Coord cell in attackRange) {
                foreach (Unit unit in enemies) {
                    if (unit.InUnit(cell)) {
                        enemiesInRange.Add(unit.GetPos());
                        break;
                    }
                }
            }

            if (enemiesInRange.Count > 0) {
                do {
                    target = RoMBoard.SelectPosition(Board, cursor, GetPos(), Commands.ATTACK, attackRange);

                    validTarget = enemiesInRange.Contains(target);

                    if (validTarget) {
                        foreach (Unit unit in enemies) {
                            if (unit.InUnit(target)) {
                                int res = GetAtk() - unit.GetDef();
                                unit.SetCurrLife(unit.GetCurrLife() - res);
                                break;
                            }
                        }
                    } else {
                        Console.Write("Invalid target! ");
                        Console.ReadLine();
                    }
                } while (!validTarget);
            } else {
                Console.Write("No enemies in range! ");
            }
            return target;
        }

        public abstract bool Move(Coord cursor);

        public abstract void Adapt(ETerrain terrain);
    }
}
