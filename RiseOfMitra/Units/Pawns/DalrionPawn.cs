using System;
using System.Collections.Generic;

using Utils.Types;
using Utils.Space;

namespace Units.Pawns
{
    class DalrionPawn : ABasicPawn
    {
        public DalrionPawn() {
            PositiveTerrains = new ETerrain[] { ETerrain.MOUNTAIN, ETerrain.MARSH, ETerrain.DESERT };
            Boards = null;
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

        public override void Adapt(ETerrain prevTerrain, ETerrain curTerrain) {
            switch (prevTerrain) {
                case ETerrain.MOUNTAIN:
                    SetMovePoints(GetMovePoints() + 1);
                    break;
                case ETerrain.PLAIN:
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
                    SetMovePoints(GetMovePoints() + 2);
                    break;
                default:
                    break;
            }

            switch (curTerrain) {
                case ETerrain.MOUNTAIN:
                    SetMovePoints(GetMovePoints() - 1);
                    break;
                case ETerrain.PLAIN:
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
    }
}
