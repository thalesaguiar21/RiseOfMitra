﻿using System;
using Types;
using Cells;
using Consts;

namespace Game
{
    class CulturalCenterFactory
    {
        public ABuilding Create(ECultures native, Board boards) {
            CulturalCenter center = null;
            switch (native) {
                case ECultures.DALRIONS:
                    center = new CulturalCenter(ECultures.DALRIONS);
                    center.SetBoards(boards);
                    center.SetCurrLife(30);
                    center.SetTotalLife(30);
                    center.SetDef(2);
                    center.SetLifePerSec(2);
                    center.SetPos(new Coord(1, 1));
                    center.SetSpawnPoint(new Coord(center.GetPos().X, center.GetPos().Y + center.GetSize()));
                    break;
                case ECultures.RAHKARS:
                    center = new CulturalCenter(ECultures.RAHKARS);
                    center.SetBoards(boards);
                    center.SetCurrLife(35);
                    center.SetTotalLife(35);
                    center.SetDef(1);
                    center.SetLifePerSec(1);
                    int buildSize = center.GetSize() + 1;
                    center.SetPos(new Coord(BoardConsts.MAX_LIN - buildSize, BoardConsts.MAX_COL - buildSize));
                    center.SetSpawnPoint(new Coord(center.GetPos().X, center.GetPos().Y - 1));
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
