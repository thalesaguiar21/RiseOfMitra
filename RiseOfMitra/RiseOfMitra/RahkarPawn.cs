using Consts;
using Types;
using Cells;
using System;
using ShortestPath;
using System.Collections.Generic;

namespace Game
{
    class RahkarPawn : ABasicPawn
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
                    break;
                case ETerrain.DESERT:
                    SetAtk(GetAtk() - 1);
                    break;
                default:
                    break;
            }
        }
    }
}
