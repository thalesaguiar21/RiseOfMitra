
using Types;
using Consts;
using Cells;
using System;

namespace RiseOfMitra
{
    class CulturalCenter : ABuilding
    {
        private int turn;
        private int unitPerTurn;

        public CulturalCenter(ECultures native) {
            if (native == ECultures.DALRIONS) BOARD_CHAR = BoardConsts.DALRION_CENTER;
            else if (native == ECultures.RAHKARS) BOARD_CHAR = BoardConsts.RAHKAR_CENTER;
            SetCulture(native);
            SetCurrLife(0);
            SetDef(0);
            SetLifePerSec(0);
            SetPos(new Coord(0, 0));
            SetSize(5);
            turn = 0;
            unitPerTurn = 3;
        }

        public void GeneratePawn(ECultures pawnCulture) {
            switch (pawnCulture) {
                case ECultures.DALRIONS:
                    Console.Write("Creating DALRION");
                    break;
                case ECultures.RAHKARS:
                    Console.Write("Creating RAHKAR");
                    break;
                default:
                    Console.Write("ERROR!");
                    break;
            }
            Console.ReadLine();
        }

        public void SetTurn() {
            turn = (turn + 1) % unitPerTurn;
            if (turn == 0)
                GeneratePawn(NativeOf());
        }
    }
}
