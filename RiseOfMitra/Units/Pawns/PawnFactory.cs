using System;
using Boards;
using Utils;
using Utils.Types;
using Utils.Space;

namespace Units.Pawns
{
    public class PawnFactory
    {
        bool Validate;

        public PawnFactory(bool validate = true) {
            Validate = validate;
        }

        public APawn Create(ECultures nature) {
            // CurrLife, TotalLife, Atk, AtkRange, Def, MovePoints
            Object[] values = new Object[] { 1, 10, 20, 5, 3, 20};
            APawn pawn = null;

            switch (nature) {
                case ECultures.DALRIONS:
                    pawn = new DalrionPawn();
                    if (Validate)
                        values = new Object[] { 10, 10, 5, 2, 1, 3 };
                    pawn.SetCulture(ECultures.DALRIONS);
                    break;

                case ECultures.RAHKARS:
                    pawn = new RahkarPawn();
                    if (Validate)
                        values = new Object[] { 15, 15, 3, 2, 3, 3 };
                    pawn.SetCulture(ECultures.RAHKARS);
                    break;
                default:
                    UserUtils.PrintError(nature + " isn't a valid culture!");
                    Console.ReadLine();
                    break;
            }
            pawn.SetPos(new Coord(0, 0));
            pawn.SetCurrLife((int)values[0]);
            pawn.SetTotalLife((int)values[1]);
            pawn.SetAtk((int)values[2]);
            pawn.SetAtkRange((int)values[3]);
            pawn.SetDef((int)values[4]);
            pawn.SetMovePoints((int)values[5]);
            if (Validate && !ValidatePawn(pawn))
                pawn = null;
            
            return pawn;
        }

        private bool ValidatePawn(APawn pawn) {
            bool isValid = pawn != null;
            if (pawn.NativeOf() == ECultures.DALRIONS) {

                isValid &= pawn.GetCurrLife() == 10;
                isValid &= pawn.GetTotalLife() == 10;
                isValid &= pawn.GetAtk() == 5;
                isValid &= pawn.GetAtkRange() == 2;
                isValid &= pawn.GetDef() == 1;
                isValid &= pawn.GetMovePoints() == 3;
                isValid &= (pawn.GetPos().X == 0 && pawn.GetPos().Y == 0);
                isValid &= pawn.GetSize() == 1;
            } else if (pawn.NativeOf() == ECultures.RAHKARS) {

                isValid &= pawn.GetCurrLife() == 15;
                isValid &= pawn.GetTotalLife() == 15;
                isValid &= pawn.GetAtk() == 3;
                isValid &= pawn.GetAtkRange() == 2;
                isValid &= pawn.GetDef() == 3;
                isValid &= pawn.GetMovePoints() == 3;
                isValid &= (pawn.GetPos().X == 0 && pawn.GetPos().Y == 0);
                isValid &= pawn.GetSize() == 1;
            }
            return isValid;
        }
    }
}
