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
            TotalLife = 14;
            CurrLife = 14;
            Culture = ECultures.RAHKARS;
            Def = 7;
            Position = new Coord(0, 0);
            Size = 1;
            Atk = 8;
            AtkRange = 7;
            MovePoints = 5;
        }

        public override void ReAdapt(ETerrain terrain) {
            switch (terrain) {
                case ETerrain.MOUNTAIN:
                    MovePoints -= 1;
                    break;
                case ETerrain.PLAIN:
                    Def += 1;
                    break;
                case ETerrain.RIVER:
                    MovePoints -= 1;
                    break;
                case ETerrain.FIELD:
                    Def += 1;
                    break;
                case ETerrain.MARSH:
                    Atk -= 2;
                    break;
                case ETerrain.FOREST:
                    break;
                case ETerrain.DESERT:
                    Atk += 1;
                    break;
                default:
                    throw new InvalidOperationException("Could not readapt for " + terrain);
            }
        }

        public override void UnAdapt(ETerrain terrain) {
            switch (terrain) {
                case ETerrain.MOUNTAIN:
                    MovePoints += 1;
                    break;
                case ETerrain.PLAIN:
                    Def -= 1;
                    break;
                case ETerrain.RIVER:
                    MovePoints += 1;
                    break;
                case ETerrain.FIELD:
                    Def -= 1;
                    break;
                case ETerrain.MARSH:
                    Atk += 2;
                    break;
                case ETerrain.FOREST:
                    break;
                case ETerrain.DESERT:
                    Atk -= 1;
                    break;
                default:
                    throw new InvalidOperationException("Could not unadapt for " + terrain);
            }
        }

    }

}
