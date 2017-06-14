using System;
using Types;
using Cells;
using Units;

namespace Game
{
    class PawnFactory
    {
        bool Validate;
        public PawnFactory(bool validate = true) {
            Validate = validate;
        }
        public APawn Create(ECultures nature, Board boards) {
            // CurrLife, TotalLife, Atk, AtkRange, Def, MovePoints
            Object[] values = new Object[] { 1, 10, 20, 5, 3, 20};
            APawn pawn = null;

            switch (nature) {
                case ECultures.DALRIONS:
                    pawn = new DalrionPawn();
                    if (Validate)
                        values = new Object[] { 10, 10, 4, 2, 2, 5 };
                    pawn.SetCulture(ECultures.DALRIONS);
                    break;

                case ECultures.RAHKARS:
                    pawn = new RahkarPawn();
                    if (Validate)
                        values = new Object[] { 15, 15, 2, 2, 3, 3 };
                    pawn.SetCulture(ECultures.RAHKARS);
                    break;
                default:
                    pawn = new DalrionPawn();
                    Console.WriteLine(nature + " isn't a valid culture!");
                    break;
            }
            pawn.SetBoards(boards);
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
                isValid &= pawn.GetAtk() == 4;
                isValid &= pawn.GetAtkRange() == 2;
                isValid &= pawn.GetDef() == 2;
                isValid &= pawn.GetMovePoints() == 5;
                isValid &= (pawn.GetPos().X == 0 && pawn.GetPos().Y == 0);
                isValid &= pawn.GetSize() == 1;
                isValid &= (pawn.GetBoard() != null);
            } else if (pawn.NativeOf() == ECultures.RAHKARS) {

                isValid &= pawn.GetCurrLife() == 15;
                isValid &= pawn.GetTotalLife() == 15;
                isValid &= pawn.GetAtk() == 2;
                isValid &= pawn.GetAtkRange() == 2;
                isValid &= pawn.GetDef() == 3;
                isValid &= pawn.GetMovePoints() == 3;
                isValid &= (pawn.GetPos().X == 0 && pawn.GetPos().Y == 0);
                isValid &= pawn.GetSize() == 1;
                isValid &= (pawn.GetBoard() != null);
            }
            return isValid;
        }
    }
}
