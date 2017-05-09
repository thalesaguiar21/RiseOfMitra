using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoMUtils;

namespace RiseOfMitra
{
    class PawnFactory
    {
        public static ABasicPawn GetPawn(ECultures nature)
        {
            ABasicPawn pawn;
            switch (nature)
            {
                case ECultures.DALRIONS:
                    pawn = new DalrionPawn();
                    if (!validatePawn(pawn)) break;
                    return pawn;
                case ECultures.RAHKARS:
                    pawn = new RahkarPawn();
                    if (!validatePawn(pawn)) break;
                    return pawn;
                default:
                    Console.WriteLine(nature + " isn't a valid pawn!");
                    break;
            }
            return null;
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
            }
            else if (pawn.NativeOf() == ECultures.RAHKARS)
            {
                isValid &= pawn.GetAtk() == 4;
                isValid &= pawn.GetCurrLife() == 12;
                isValid &= pawn.GetDef() == 2;
                isValid &= pawn.GetMovePoints() == 3;
                isValid &= (pawn.GetPos().X == 0 && pawn.GetPos().Y == 0);
                isValid &= pawn.GetSize() == 1;
            }
            return isValid;
        }
    }
}
