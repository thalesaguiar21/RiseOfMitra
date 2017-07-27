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
            CurrLife = 23;
            TotalLife = 23;
            Culture = ECultures.DALRIONS;
            Def = 5;
            Position = new Coord(0, 0);
            Size = 1;
            Atk = 9;
            AtkRange = 5;
            MovePoints = 4;
        }

        public override void ReAdapt(ETerrain terrain) {
            switch (terrain) {
                case ETerrain.MOUNTAIN:
                    MovePoints -= 1;
                    break;
                case ETerrain.PLAIN:
                    break;
                case ETerrain.RIVER:
                    Atk += 1;
                    break;
                case ETerrain.FIELD:
                    Def += 1;
                    break;
                case ETerrain.MARSH:
                    Def += 1;
                    break;
                case ETerrain.FOREST:
                    MovePoints += 1;
                    break;
                case ETerrain.DESERT:
                    MovePoints -= 2;
                    break;
                default:
                    throw new InvalidOperationException("Could not readapat for " + terrain);
            }
        }

        public override void UnAdapt(ETerrain terrain) {
            switch (terrain) {
                case ETerrain.MOUNTAIN:
                    MovePoints += 1;
                    break;
                case ETerrain.PLAIN:
                    break;
                case ETerrain.RIVER:
                    Atk -= 1;
                    break;
                case ETerrain.FIELD:
                    Def -= 1;
                    break;
                case ETerrain.MARSH:
                    Def -= 1;
                    break;
                case ETerrain.FOREST:
                    MovePoints -= 1;
                    break;
                case ETerrain.DESERT:
                    MovePoints += 2;
                    break;
                default:
                    throw new InvalidOperationException("Could not unadapat for " + terrain);
            }
        }
    }
}
