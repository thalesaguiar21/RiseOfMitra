using System;
using Types;

namespace RiseOfMitra
{
    class PawnFactory
    {
        public static ABasicPawn CreatePawn(ECultures nature, string[,] board)
        {
            ABasicPawn pawn = null;
            switch (nature)
            {
                case ECultures.DALRIONS:
                    pawn = new DalrionPawn();
                    pawn.SetBoard(board);
                    if (!validatePawn(pawn))
                        pawn = null;
                    break;
                case ECultures.RAHKARS:
                    pawn = new RahkarPawn();
                    pawn.SetBoard(board);
                    if (!validatePawn(pawn))
                        pawn = null;
                    break;
                default:
                    Console.WriteLine(nature + " isn't a valid culture!");
                    break;
            }
            return pawn;
        }


        private static bool validatePawn(ABasicPawn pawn)
        {
            bool isValid = true;
            if(pawn.NativeOf() == ECultures.DALRIONS)
            {
                isValid &= pawn.GetAtk() == 3;
                isValid &= pawn.GetCurrLife() == 10;
                isValid &= pawn.GetDef() == 3;
                isValid &= pawn.GetMovePoints() == 3;
                isValid &= (pawn.GetPos().X == 0 && pawn.GetPos().Y == 0);
                isValid &= pawn.GetSize() == 1;
                isValid &= (pawn.GetBoard() != null);
            }
            else if (pawn.NativeOf() == ECultures.RAHKARS)
            {
                isValid &= pawn.GetAtk() == 4;
                isValid &= pawn.GetCurrLife() == 12;
                isValid &= pawn.GetDef() == 2;
                isValid &= pawn.GetMovePoints() == 3;
                isValid &= (pawn.GetPos().X == 0 && pawn.GetPos().Y == 0);
                isValid &= pawn.GetSize() == 1;
                isValid &= (pawn.GetBoard() != null);
            }
            return isValid;
        }
    }
}
