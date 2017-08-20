using System;
using Boards;
using Utils;
using Utils.Types;
using Utils.Space;

namespace Units.Centers
{
    public class CulturalCenterFactory
    {
        public ABuilding Create(ECultures native, Board boards)
        {
            CulturalCenter center = null;
            switch (native) {
                case ECultures.DALRIONS:
                    center = new CulturalCenter(ECultures.DALRIONS);
                    center.TotalLife = 100;
                    center.CurrLife = 100;
                    center.Def = 3;
                    center.LifePerSec = 2;
                    int dSize = center.Size / 2;
                    center.Position = new Coord(1 + dSize, 1 + dSize);
                    center.SetSpawnPoint(boards, new Coord(1, center.Position.Y + dSize + 2));
                    break;
                case ECultures.RAHKARS:
                    center = new CulturalCenter(ECultures.RAHKARS);
                    center.TotalLife = 65;
                    center.CurrLife = 65;
                    center.Def = 4;
                    center.LifePerSec = 1;
                    int rSize = center.Size / 2;
                    center.Position = new Coord(BoardConsts.MAX_LIN - rSize - 2, BoardConsts.MAX_COL - rSize - 2);
                    center.SetSpawnPoint(boards, new Coord(BoardConsts.MAX_LIN - 1, center.Position.Y - rSize - 2));
                    break;
                default:
                    UserUtils.PrintError("Invalid culture. Can't create cultural center!");
                    Console.ReadLine();
                    break;
            }
            if (!Validate(center))
                center = null;
            return center;
        }

        private bool Validate(CulturalCenter center)
        {
            bool valid = true;
            if (center.Culture == ECultures.DALRIONS) {
                valid &= center.CurrLife == 100;
                valid &= center.CurrLife == center.TotalLife;
                valid &= center.Def == 3;
                valid &= center.LifePerSec == 2;
            } else {
                valid &= center.CurrLife == 65;
                valid &= center.CurrLife == center.TotalLife;
                valid &= center.Def == 4;
                valid &= center.LifePerSec == 1;
            }
            return valid;
        }
    }
}
