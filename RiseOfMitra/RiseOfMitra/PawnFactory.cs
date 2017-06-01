using System;
using Types;
using Cells;

namespace RiseOfMitra
{
    class PawnFactory
    {
        public ABasicPawn Create(ECultures nature, string[,] board) {
            ABasicPawn pawn = null;
            switch (nature) {
                case ECultures.DALRIONS:
                    pawn = new DalrionPawn();
                    pawn.SetBoard(board);
                    pawn.SetCurrLife(1);
                    pawn.SetTotalLife(10);
                    pawn.SetAtk(20);
                    pawn.SetAtkRange(5);
                    pawn.SetCulture(ECultures.DALRIONS);
                    pawn.SetDef(3);
                    pawn.SetMovePoints(20);
                    pawn.SetPos(new Coord(0, 0));
                    pawn.SetSize(1);

                    if (!ValidatePawn(pawn))
                        pawn = null;
                    break;

                case ECultures.RAHKARS:
                    pawn = new RahkarPawn();
                    pawn.SetBoard(board);
                    pawn.SetCurrLife(1);
                    pawn.SetTotalLife(12);
                    pawn.SetAtk(20);
                    pawn.SetAtkRange(5);
                    pawn.SetCulture(ECultures.RAHKARS);
                    pawn.SetDef(2);
                    pawn.SetMovePoints(20);
                    pawn.SetPos(new Coord(0, 0));
                    pawn.SetSize(1);
                    if (!ValidatePawn(pawn))
                        pawn = null;
                    break;
                default:
                    Console.WriteLine(nature + " isn't a valid culture!");
                    break;
            }
            return pawn;
        }


        private bool ValidatePawn(ABasicPawn pawn) {
            bool isValid = true;
            if (pawn.NativeOf() == ECultures.DALRIONS) {
                isValid &= pawn.GetCurrLife() == 1;
                isValid &= pawn.GetTotalLife() == 10;
                isValid &= pawn.GetAtk() == 20;
                isValid &= pawn.GetAtkRange() == 5;
                isValid &= pawn.GetDef() == 3;
                isValid &= pawn.GetMovePoints() == 20;
                isValid &= (pawn.GetPos().X == 0 && pawn.GetPos().Y == 0);
                isValid &= pawn.GetSize() == 1;
                isValid &= (pawn.GetBoard() != null);
            } else if (pawn.NativeOf() == ECultures.RAHKARS) {
                isValid &= pawn.GetAtk() == 20;
                isValid &= pawn.GetAtkRange() == 5;
                isValid &= pawn.GetCurrLife() == 1;
                isValid &= pawn.GetDef() == 2;
                isValid &= pawn.GetMovePoints() == 20;
                isValid &= (pawn.GetPos().X == 0 && pawn.GetPos().Y == 0);
                isValid &= pawn.GetSize() == 1;
                isValid &= (pawn.GetBoard() != null);
            }
            return isValid;
        }
    }
}
