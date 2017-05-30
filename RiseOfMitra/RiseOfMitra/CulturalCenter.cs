
using Types;
using Consts;
using Cells;

namespace RiseOfMitra
{
    class CulturalCenter : ABuilding
    {
        private int unitsPerSec;
        
        public CulturalCenter(ECultures native)
        {
            if (native == ECultures.DALRIONS) BOARD_CHAR = BoardConsts.DALRION_CENTER;
            else if (native == ECultures.RAHKARS) BOARD_CHAR = BoardConsts.RAHKAR_CENTER;
            SetCulture(ECultures.DEFAULT);
            SetCurrLife(0);
            SetDef(0);
            SetLifePerSec(0);
            SetPos(new Coord(0, 0));
            SetSize(5);
            unitsPerSec = 0;
        }

        public void GeneratePawn(ECultures pawnCulture)
        {
            switch (pawnCulture)
            {
                case ECultures.DEFAULT:
                    break;
                case ECultures.DALRIONS:
                    break;
                case ECultures.RAHKARS:
                    break;
                default:
                    break;
            }
        }
    }
}
