using RoMUtils;
using System;
using Consts;
using Types;
using Cells;

namespace RiseOfMitra
{
    class DalrionPawn : ABasicPawn
    {
        public DalrionPawn()
        {
            Board = null;
            BOARD_CHAR = BoardStrings.DALRION_PAWN;
            SetCurrLife(10);
            SetAtk(3);
            SetCulture(ECultures.DALRIONS);
            SetDef(3);
            SetMovePoints(3);
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

        public override void Move(Coord target)
        {
            Board[GetPos().X, GetPos().Y] = BoardStrings.EMPTY;
            Board[target.X, target.Y] = BoardStrings.DALRION_PAWN;
            SetPos(target);
        }
    }
}
