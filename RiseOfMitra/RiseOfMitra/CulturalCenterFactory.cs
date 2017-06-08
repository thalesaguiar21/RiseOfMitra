using System;
using Types;
using Cells;
using Consts;

namespace RiseOfMitra
{
    class CulturalCenterFactory
    {
        public ABuilding Create(ECultures native, string[,] board) {
            CulturalCenter center = null;
            switch (native) {
                case ECultures.DALRIONS:
                    center = new CulturalCenter(ECultures.DALRIONS);
                    center.SetBoard(board);
                    center.SetCurrLife(1);
                    center.SetTotalLife(100);
                    center.SetDef(3);
                    center.SetLifePerSec(2);
                    center.SetPos(new Coord(1, 1));
                    center.SetSize(5);
                    center.SetSpawnPoint(new Coord(center.GetPos().X, center.GetPos().Y + center.GetSize()));
                    break;
                case ECultures.RAHKARS:
                    center = new CulturalCenter(ECultures.RAHKARS);
                    center.SetBoard(board);
                    center.SetCulture(ECultures.RAHKARS);
                    center.SetCurrLife(1);
                    center.SetTotalLife(130);
                    center.SetDef(2);
                    center.SetLifePerSec(1);
                    int buildSize = center.GetSize() + 1;
                    center.SetPos(new Coord(BoardConsts.BOARD_LIN - buildSize, BoardConsts.BOARD_COL - buildSize));
                    center.SetSpawnPoint(new Coord(center.GetPos().X, center.GetPos().Y - 1));
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
