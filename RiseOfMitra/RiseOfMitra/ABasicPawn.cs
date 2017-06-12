using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cells;
using Types;
using ShortestPath;
using Consts;

namespace Game
{
    abstract class ABasicPawn : APawn
    {
        public virtual Coord Attack(Coord cursor, List<Unit> enemies) {
            bool validTarget = false;
            Coord target = null;
            Dijkstra didi = new Dijkstra(Boards.GetBoard(), GetPos(), GetAtkRange());
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
                target = Boards.SelectPosition(cursor, GetPos(), Commands.ATTACK, attackRange);
                validTarget = enemiesInRange.Contains(target);
            } else {
                Console.Write("No enemies in range! ");
            }
            return target;
        }

        public override bool Move(Coord cursor) {
            bool validTarget = false;
            Dijkstra didi = new Dijkstra(Boards.GetBoard(), GetPos(), GetMovePoints());
            List<Coord> moveRange = didi.GetValidPaths(Commands.MOVE);
            if (moveRange.Count > 0) {
                do {
                    Coord target = Boards.SelectPosition(cursor, GetPos(), Commands.MOVE, moveRange);
                    validTarget = moveRange.Contains(target);

                    if (validTarget) {
                        Boards.SetCellAt(GetPos(), BoardConsts.EMPTY);
                        Boards.SetCellAt(target, ToString());
                        Adapt((ETerrain)Boards.TerrainAt(GetPos()), (ETerrain)Boards.TerrainAt(target));
                        SetPos(target);
                    } else {
                        Console.Write("Invalid target! ");
                        Console.ReadLine();
                    }
                } while (!validTarget);
            } else {
                Console.Write("This pawn can't move! ");
            }
            return validTarget;
        }
    }
}
