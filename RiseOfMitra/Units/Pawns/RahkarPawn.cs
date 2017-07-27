using Utils.Types;
using Utils.Space;
using System;

namespace Units.Pawns
{
    class RahkarPawn : ABasicPawn
    {
        public RahkarPawn() {
            PositiveTerrains = new ETerrain[] { ETerrain.MOUNTAIN, ETerrain.RIVER, ETerrain.MARSH };
            BOARD_CHAR = BoardConsts.RAHKAR_PAWN;
            SetCurrLife(14);
            SetTotalLife(14);
            SetAtk(8);
            SetAtkRange(7);
            SetCulture(ECultures.RAHKARS);
            SetDef(7);
            SetMovePoints(5);
            SetPos(new Coord(0, 0));
            SetSize(1);
        }

        public override void ReAdapt(ETerrain terrain) {
            switch (terrain) {
                case ETerrain.MOUNTAIN:
                    SetMovePoints(GetMovePoints() - 1);
                    break;
                case ETerrain.PLAIN:
                    SetDef(GetDef() + 1);
                    break;
                case ETerrain.RIVER:
                    SetMovePoints(GetMovePoints() - 1);
                    break;
                case ETerrain.FIELD:
                    SetDef(GetDef() + 1);
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
                    throw new InvalidOperationException("Could not readapt for " + terrain);
            }
        }

        public override void UnAdapt(ETerrain terrain) {
            switch (terrain) {
                case ETerrain.MOUNTAIN:
                    SetMovePoints(GetMovePoints() + 1);
                    break;
                case ETerrain.PLAIN:
                    SetDef(GetDef() - 1);
                    break;
                case ETerrain.RIVER:
                    SetMovePoints(GetMovePoints() + 1);
                    break;
                case ETerrain.FIELD:
                    SetDef(GetDef() - 1);
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
                    throw new InvalidOperationException("Could not unadapt for " + terrain);
            }
        }

    }

}
