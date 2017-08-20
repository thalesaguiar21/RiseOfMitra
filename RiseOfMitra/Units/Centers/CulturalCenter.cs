
using System;
using Utils;
using Utils.Space;
using Utils.Types;
using Units.Pawns;
using Boards;

namespace Units.Centers
{
    public class CulturalCenter : ABuilding
    {
        int UnitPerTurn;
        Coord SpawnPoint;
        int SpawnRange;

        public CulturalCenter(ECultures native)
        {
            if (native == ECultures.DALRIONS) BOARD_CHAR = BoardConsts.DALRION_CENTER;
            else if (native == ECultures.RAHKARS) BOARD_CHAR = BoardConsts.RAHKAR_CENTER;
            Culture = native;
            CurrLife = 0;
            TotalLife = 0;
            Def = 0;
            LifePerSec = 0;
            Position = new Coord(0, 0);
            Size = 5;
            UnitPerTurn = 10;
            SpawnPoint = new Coord(0, 0);
            SpawnRange = 4;
        }

        public CulturalCenter Copy(Board board)
        {
            CulturalCenter tmpCenter = new CulturalCenter(Culture) {
                TotalLife = TotalLife,
                CurrLife = CurrLife,
                Def = Def,
                LifePerSec = LifePerSec,
                Position = Position,
                Size = Size
            };
            tmpCenter.SetUnitPerTurn(UnitPerTurn);
            tmpCenter.SetSpawnPoint(board, new Coord(SpawnPoint.X, SpawnPoint.Y));
            tmpCenter.SetSpawnRange(SpawnRange);

            return tmpCenter;
        }

        public APawn GeneratePawn(Board boards)
        {
            var factory = new PawnFactory();
            var pawn = factory.Create(Culture);
            var pos = PlacementPosition(boards);
            if (pos == null) {
                UserUtils.PrintError("Can not generate more pawns!");
                return null;
            } else {
                pawn.Position = pos;
                return pawn;
            }

        }

        private Coord PlacementPosition(Board boards)
        {
            Coord spawnPoint = null;
            for (int i = 0; i < BoardConsts.MAX_LIN; i++) {
                for (int j = 0; j < BoardConsts.MAX_COL; j++) {
                    if ((boards.CellAt(i, j) == BoardConsts.EMPTY) && (Coord.Distance(SpawnPoint, new Coord(i, j)) <= SpawnRange)) {
                        spawnPoint = new Coord(i, j);
                        break;
                    }
                }
            }
            return spawnPoint;
        }

        public int GetUnitsPerTurn() { return UnitPerTurn; }
        public Coord GetSpawnPoint() { return SpawnPoint; }
        public int GetSpawnRange() { return SpawnRange; }

        public void SetUnitPerTurn(int value)
        {
            if (value >= 0)
                UnitPerTurn = value;
        }

        public void SetSpawnPoint(Board boards, Coord cell)
        {
            if (cell != null) {
                SpawnPoint = cell;
            }
        }

        public void SetSpawnRange(int range)
        {
            if (range > 0)
                SpawnRange = range;
        }
    }
}
