using System;
using Boards;
using Utils.Types;
using Utils.Space;

namespace Units.Centers
{
    public class CulturalCenterFactory
    {
        public ABuilding Create(ECultures native, Board boards) {
            CulturalCenter center = null;
            switch (native) {
                case ECultures.DALRIONS:
                    center = new CulturalCenter(ECultures.DALRIONS);
                    center.SetCurrLife(30);
                    center.SetTotalLife(30);
                    center.SetDef(0);
                    center.SetLifePerSec(2);
                    int dSize = center.GetSize() / 2;
                    center.SetPos(new Coord(1 + dSize, 1 + dSize));
                    center.SetSpawnPoint(boards, new Coord(center.GetPos().X, center.GetPos().Y + center.GetSize()));
                    break;
                case ECultures.RAHKARS:
                    center = new CulturalCenter(ECultures.RAHKARS);
                    center.SetCurrLife(35);
                    center.SetTotalLife(35);
                    center.SetDef(0);
                    center.SetLifePerSec(1);
                    int rSize = center.GetSize() / 2;
                    center.SetPos(new Coord(BoardConsts.MAX_LIN - rSize - 2, BoardConsts.MAX_COL - rSize - 2));
                    center.SetSpawnPoint(boards, new Coord(center.GetPos().X, center.GetPos().Y - 1));
                    break;
                default:
                    Console.WriteLine("Invalid culture. Can't create cultural center!");
                    break;
            }
            if (!Validate(center))
                center = null;
            return center;
        }

        private bool Validate(CulturalCenter center) {
            bool valid = true;
            if(center.NativeOf() == ECultures.DALRIONS) {
                valid &= center.GetCurrLife() == 30;
                valid &= center.GetCurrLife() == center.GetTotalLife();
                valid &= center.GetDef() == 0;
                valid &= center.GetLifePerSec() == 2;

            } else {
                valid &= center.GetCurrLife() == 35;
                valid &= center.GetCurrLife() == center.GetTotalLife();
                valid &= center.GetDef() == 0;
                valid &= center.GetLifePerSec() == 1;
            }
            return valid;
        }
    }
}
