using RoMUtils;
using System;
using Consts;
using Types;
using Cells;
using ShortestPath;
using System.Collections.Generic;

namespace RiseOfMitra
{
    class DalrionPawn : ABasicPawn
    {
        public DalrionPawn() {
            Board = null;
            BOARD_CHAR = BoardConsts.DALRION_PAWN;
            SetCurrLife(0);
            SetTotalLife(0);
            SetAtk(0);
            SetAtkRange(0);
            SetCulture(ECultures.DALRIONS);
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
                    SetMovePoints(GetMovePoints() - 1);
                    break;
                case ETerrain.PLAIN:
                    SetAtk(GetAtk() + 1);
                    break;
                case ETerrain.RIVER:
                    SetAtk(GetAtk() + 1);
                    break;
                case ETerrain.FIELD:
                    SetDef(GetDef() + 1);
                    break;
                case ETerrain.MARSH:
                    SetDef(GetDef() - 1);
                    break;
                case ETerrain.FOREST:
                    SetMovePoints(GetMovePoints() + 1);
                    break;
                case ETerrain.DESERT:
                    SetMovePoints(GetMovePoints() + 2);
                    break;
                default:
                    break;
            }
        }

        public override bool Move(Coord cursor) {
            bool validTarget = false;
            Dijkstra didi = new Dijkstra(Board, GetPos(), GetMovePoints());
            List<Coord> moveRange = didi.GetValidPaths(Commands.MOVE);
            if (moveRange.Count > 0) {
                do {
                    Coord target = RoMBoard.SelectPosition(Board, cursor, GetPos(), Commands.MOVE, moveRange);
                    validTarget = moveRange.Contains(target);

                    string msg = "";

                    if (validTarget) {
                        msg = "Moving to position";
                        Board[GetPos().X, GetPos().Y] = BoardConsts.EMPTY;
                        Board[target.X, target.Y] = BoardConsts.DALRION_PAWN;
                        SetPos(target);
                    } else {
                        msg = "Invalid target!";
                    }

                    Console.WriteLine(msg);
                } while (!validTarget);
            } else {
                Console.WriteLine("This pawn can't move!");
            }
            return validTarget;
        }

        public override bool Attack(Coord target) {
            throw new NotImplementedException();
        }
    }
}
