using Consts;
using Types;
using Cells;
using System;

namespace RiseOfMitra
{
    class RahkarPawn : ABasicPawn
    {
        public RahkarPawn()
        {
            Board = null;
            BOARD_CHAR = BoardStrings.RAHKAR_PAWN;
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

        public override string ToString()
        {
            return BOARD_CHAR;
        }

        public override void Adapt(ETerrain terrain)
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

        public override void Move(Coord target)
        {
            Board[GetPos().X, GetPos().Y] = BoardStrings.EMPTY;
            Board[target.X, target.Y] = BoardStrings.RAHKAR_PAWN;
            SetPos(target);
        }
    }
}
