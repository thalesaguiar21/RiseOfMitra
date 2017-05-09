using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoMUtils;

namespace RiseOfMitra
{
    class RahkarPawn : ABasicPawn
    {
        public RahkarPawn()
        {
            BOARD_CHAR = BoardStrings.CHAR_RAHKAR_PAWN;
            SetCurrLife(12);
            SetAtk(4);
            SetCulture(ECultures.RAHKARS);
            SetDef(2);
            SetMovePoints(3);
            SetPos(new Coord(0, 0));
            SetSize(1);
        }

        public override string ToString()
        {
            return BOARD_CHAR;
        }

        public override void adapt(ETerrain terrain)
        {
            switch (terrain)
            {
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
    }
}
