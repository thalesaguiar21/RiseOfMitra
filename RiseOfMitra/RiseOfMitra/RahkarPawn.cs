using Consts;
using Types;
using Cells;
using System;
using ShortestPath;
using System.Collections.Generic;

namespace Game
{
    class RahkarPawn : APawn
    {
        public RahkarPawn() {
            Boards = null;
            BOARD_CHAR = BoardConsts.RAHKAR_PAWN;
            SetCurrLife(0);
            SetTotalLife(0);
            SetAtk(0);
            SetAtkRange(0);
            SetCulture(ECultures.RAHKARS);
            SetDef(0);
            SetMovePoints(1);
            SetPos(new Coord(0, 0));
            SetSize(1);
        }

        public override string ToString() {
            return BOARD_CHAR;
        }

        public override void Adapt(ETerrain prevTerrain, ETerrain curTerrain) {
            switch (prevTerrain) {
                case ETerrain.MOUNTAIN:
                    SetMovePoints(GetMovePoints() + 1);
                    break;
                case ETerrain.PLAIN:
                    SetDef(GetDef());
                    break;
                case ETerrain.RIVER:
                    SetMovePoints(GetMovePoints() + 1);
                    break;
                case ETerrain.FIELD:
                    SetDef(GetDef() - 1);
                    break;
                case ETerrain.MARSH:
                    SetAtk(GetAtk() - 2);
                    break;
                case ETerrain.FOREST:
                    break;
                case ETerrain.DESERT:
                    SetAtk(GetAtk() + 1);
                    break;
                default:
                    break;
            }

            switch (curTerrain) {
                case ETerrain.MOUNTAIN:
                    SetMovePoints(GetMovePoints() + 1);
                    break;
                case ETerrain.PLAIN:
                    SetDef(GetDef() - 1);
                    break;
                case ETerrain.RIVER:
                    SetMovePoints(GetMovePoints() - 1);
                    break;
                case ETerrain.FIELD:
                    SetDef(GetDef() + 1);
                    break;
                case ETerrain.MARSH:
                    SetAtk(GetAtk() + 2);
                    break;
                case ETerrain.FOREST:
                    SetAtk(GetAtk() + 1);
                    break;
                case ETerrain.DESERT:
                    SetAtk(GetAtk() - 1);
                    break;
                default:
                    break;
            }
        }

        public override bool Move(Coord cursor) {
            bool validTarget = false;
            Dijkstra didi = new Dijkstra(Boards.GetBoard(), GetPos(), GetMovePoints());
            List<Coord> moveRange = didi.GetValidPaths(Commands.MOVE);
            if(moveRange.Count > 0) {
                do {
                    Coord target = Boards.SelectPosition(cursor, GetPos(), Commands.MOVE, moveRange);
                    validTarget = moveRange.Contains(target);

                    if (validTarget) {
                        Boards.SetCellAt(GetPos(), BoardConsts.EMPTY);
                        Boards.SetCellAt(target, BoardConsts.RAHKAR_PAWN);
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
