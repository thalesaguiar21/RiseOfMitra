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
            BOARD_CHAR = BoardConsts.DALRION_PAWN;
            SetCurrLife(23);
            SetTotalLife(23);
            SetAtk(9);
            SetAtkRange(5);
            SetCulture(ECultures.DALRIONS);
            SetDef(5);
            SetMovePoints(4);
            SetPos(new Coord(0, 0));
            SetSize(1);
        }

        public override void ReAdapt(ETerrain terrain) {
            switch (terrain) {
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
                    SetMovePoints(GetMovePoints() - 2);
                    break;
                default:
                    throw new InvalidOperationException("Could not readapat for " + terrain);
            }
        }

        public override void UnAdapt(ETerrain terrain) {
            switch (terrain) {
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
                    throw new InvalidOperationException("Could not unadapat for " + terrain);
            }
        }
    }
}
