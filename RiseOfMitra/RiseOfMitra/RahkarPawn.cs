using Consts;
using Types;
using Cells;
using System;
using ShortestPath;
using Consts;
using System.Collections.Generic;

namespace RiseOfMitra
{
    class RahkarPawn : ABasicPawn
    {
        public RahkarPawn() {
            Board = null;
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

        public override void Adapt(ETerrain terrain) {
            switch (terrain) {
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
            Dijkstra didi = new Dijkstra(Board, GetPos(), GetMovePoints());
            List<Coord> moveRange = didi.GetValidPaths(Commands.MOVE);
            if(moveRange.Count > 0) {
                do {
                    Coord target = RoMBoard.SelectPosition(Board, cursor, GetPos(), Commands.MOVE, moveRange);
                    validTarget = moveRange.Contains(target);

                    if (validTarget) {
                        Board[GetPos().X, GetPos().Y] = BoardConsts.EMPTY;
                        Board[target.X, target.Y] = BoardConsts.RAHKAR_PAWN;
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
