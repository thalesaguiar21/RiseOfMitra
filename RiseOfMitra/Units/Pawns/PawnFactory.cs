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
            APawn pawn = null;

            switch (nature) {
                case ECultures.DALRIONS:
                    return new DalrionPawn();

                case ECultures.RAHKARS:
                    return new RahkarPawn();

                default:
                    UserUtils.PrintError(nature + " isn't a valid culture!");
                    Console.ReadLine();
                    break;
            }
            if (Validate)
                pawn = null;
            
            return pawn;
        }
    }
}
