using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoMUtils;

namespace RiseOfMitra
{
    class CulturalCenter : ABuilding
    {
        private int unitsPerSec;
        
        public CulturalCenter(ECultures native)
        {
            if (native == ECultures.DALRIONS) BOARD_CHAR = BoardStrings.CHAR_DALRION_CENTER;
            else if (native == ECultures.RAHKARS) BOARD_CHAR = BoardStrings.CHAR_RAHKAR_CENTER;
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
