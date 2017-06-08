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

        public override void Adapt(ETerrain prevTerrain, ETerrain curTerrain) {
            string msg = "Adapting for ";
            switch (prevTerrain) {
                case ETerrain.MOUNTAIN:
                    SetMovePoints(GetMovePoints() + 1);
                    break;
                case ETerrain.PLAIN:
                    SetAtk(GetAtk() - 1);
                    break;
                case ETerrain.RIVER:
                    SetAtk(GetAtk() - 1);
                    break;
                case ETerrain.FIELD:
                    SetDef(GetDef() - 1);
                    break;
                case ETerrain.MARSH:
                    SetDef(GetDef() + 1);
                    break;
                case ETerrain.FOREST:
                    SetMovePoints(GetMovePoints() - 1);
                    break;
                case ETerrain.DESERT:
                    SetMovePoints(GetMovePoints() - 2);
                    break;
                default:
                    break;
            }

            switch (curTerrain) {
                case ETerrain.MOUNTAIN:
                    SetMovePoints(GetMovePoints() - 1);
                    msg += "Mountain";
                    break;
                case ETerrain.PLAIN:
                    SetAtk(GetAtk() + 1);
                    msg += "Plain";
                    break;
                case ETerrain.RIVER:
                    SetAtk(GetAtk() + 1);
                    msg += "River";
                    break;
                case ETerrain.FIELD:
                    SetDef(GetDef() + 1);
                    msg += "Field";
                    break;
                case ETerrain.MARSH:
                    SetDef(GetDef() - 1);
                    msg += "Marsh";
                    break;
                case ETerrain.FOREST:
                    SetMovePoints(GetMovePoints() + 1);
                    msg += "Forest";
                    break;
                case ETerrain.DESERT:
                    SetMovePoints(GetMovePoints() + 2);
                    msg += "Desert";
                    break;
                default:
                    break;
            }
            Console.WriteLine(msg);
        }

        public override bool Move(Coord cursor) {
            bool validTarget = false;
            Dijkstra didi = new Dijkstra(Board, GetPos(), GetMovePoints());
            List<Coord> moveRange = didi.GetValidPaths(Commands.MOVE);
            if (moveRange.Count > 0) {
                do {
                    Coord target = RoMBoard.SelectPosition(Board, cursor, GetPos(), Commands.MOVE, moveRange);
                    validTarget = moveRange.Contains(target);

                    if (validTarget) {
                        Board[GetPos().X, GetPos().Y] = BoardConsts.EMPTY;
                        Board[target.X, target.Y] = BoardConsts.DALRION_PAWN;
                        Adapt(Terrains[GetPos().X, GetPos().Y], Terrains[target.X, target.Y]);
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
