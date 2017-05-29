using System;
using Types;
using Cells;

namespace RiseOfMitra
{
    class CulturalCenterFactory
    {
        public ABuilding Create(ECultures native, string[,] board)
        {
            CulturalCenter center = null;
            switch (native)
            {
                case ECultures.DEFAULT:
                    Console.WriteLine("Can't create default Cultural Centers!");
                    break;
                case ECultures.DALRIONS:
                    center = new CulturalCenter(ECultures.DALRIONS);
                    center.SetBoard(board);
                    center.SetCulture(ECultures.DALRIONS);
                    center.SetCurrLife(100);
                    center.SetTotalLife(100);
                    center.SetDef(3);
                    center.SetLifePerSec(2);
                    center.SetPos(new Coord(0, 0));
                    center.SetSize(5);
                    break;
                case ECultures.RAHKARS:
                    center = new CulturalCenter(ECultures.RAHKARS);
                    center.SetBoard(board);
                    center.SetCulture(ECultures.RAHKARS);
                    center.SetCurrLife(130);
                    center.SetTotalLife(130);
                    center.SetDef(2);
                    center.SetLifePerSec(1);
                    center.SetPos(new Coord(0, 0));
                    center.SetSize(5);
                    break;
                default:
                    Console.WriteLine("Invalid culture. Can't create cultural center!");
                    break;
            }
            if (center.GetBoard() == null)
                center = null;
            return center;
        }
    }
}
