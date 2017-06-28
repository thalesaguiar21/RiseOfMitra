
using System;
using Utils.Space;
using Utils.Types;
using Units.Pawns;
using Boards;

namespace Units.Centers
{
    public class CulturalCenter : ABuilding
    {
        private int UnitPerTurn;
        private Coord SpawnPoint;
        private int SpawnRange;

        public CulturalCenter(ECultures native) {
            if (native == ECultures.DALRIONS) BOARD_CHAR = BoardConsts.DALRION_CENTER;
            else if (native == ECultures.RAHKARS) BOARD_CHAR = BoardConsts.RAHKAR_CENTER;
            SetCulture(native);
            SetCurrLife(0);
            SetTotalLife(0);
            SetDef(0);
            SetLifePerSec(0);
            SetPos(new Coord(0, 0));
            SetSize(5);
            UnitPerTurn = 10;
            SpawnPoint = new Coord(0, 0);
            SpawnRange = 4;
        }

        public CulturalCenter Copy(Board board) {
            CulturalCenter tmpCenter = new CulturalCenter(NativeOf());
            tmpCenter.SetCurrLife(GetCurrLife());
            tmpCenter.SetTotalLife(GetTotalLife());
            tmpCenter.SetDef(GetDef());
            tmpCenter.SetLifePerSec(GetLifePerSec());
            tmpCenter.SetPos(GetPos());
            tmpCenter.SetSize(GetSize());
            tmpCenter.SetUnitPerTurn(UnitPerTurn);
            tmpCenter.SetSpawnPoint(board, new Coord(SpawnPoint.X, SpawnPoint.Y));
            tmpCenter.SetSpawnRange(SpawnRange);

            return tmpCenter;
        }

        public APawn GeneratePawn(Board boards) {
            PawnFactory factory = new PawnFactory();
            APawn pawn = factory.Create(NativeOf());
            Coord pos = PlacementPosition(boards);
            if (pos == null) {
                Console.Write("Can not generate more pawns!");
                return null;
            } else {
                pawn.SetPos(pos);
                return pawn;
            }
            
        }

        private Coord PlacementPosition(Board boards) {
            Coord spawnPoint = null;
            for(int i = 0; i < BoardConsts.MAX_LIN; i++) {
                for(int j=0; j < BoardConsts.MAX_COL; j++) {
                    if (boards.CellAt(i, j) == BoardConsts.EMPTY && Coord.Distance(SpawnPoint, new Coord(i, j)) <= SpawnRange) {
                        spawnPoint = new Coord(i, j);
                    }
                }
            }
            return spawnPoint;
        }

        public int GetUnitsPerTurn() { return UnitPerTurn; }
        public Coord GetSpawnPoint() { return SpawnPoint; }
        public int GetSpawnRange() { return SpawnRange; }

        public void SetUnitPerTurn(int value) {
            if (value >= 0)
                UnitPerTurn = value;
        }

        public void SetSpawnPoint(Board boards, Coord cell) {
            if(cell != null && boards.CellAt(cell) == BoardConsts.EMPTY) {
                SpawnPoint = cell;
            }
        }

        public void SetSpawnRange(int range) {
            if (range > 0)
                SpawnRange = range;
        }
    }
}
