
using Types;
using Consts;
using Cells;
using System;

namespace RiseOfMitra
{
    class CulturalCenter : ABuilding
    {
        private int UnitPerTurn;
        private Coord SpawnPoint;
        private int SpawnRange;

        public CulturalCenter(ECultures native) {
            if (native == ECultures.DALRIONS) BOARD_CHAR = BoardConsts.DALRION_CENTER;
            else if (native == ECultures.RAHKARS) BOARD_CHAR = BoardConsts.RAHKAR_CENTER;
            SetCulture(native);
            SetCurrLife(0);
            SetDef(0);
            SetLifePerSec(0);
            SetPos(new Coord(0, 0));
            SetSize(5);
            UnitPerTurn = 2;
            SpawnPoint = new Coord(0, 0);
            SpawnRange = 4;
        }

        public ABasicPawn GeneratePawn() {
            PawnFactory factory = new PawnFactory();
            ABasicPawn pawn = factory.Create(NativeOf(), Board);
            Coord pos = GetPlacementPosition();
            if (pos == null) {
                Console.Write("Can not generate more pawns!");
                return null;
            } else {
                pawn.SetPos(pos);
                return pawn;
            }
            
        }

        private Coord GetPlacementPosition() {
            Coord spawnPoint = null;
            for(int i = 0; i < BoardConsts.BOARD_LIN; i++) {
                for(int j=0; j < BoardConsts.BOARD_COL; j++) {
                    if (Board[i, j] == BoardConsts.EMPTY && Coord.Distance(SpawnPoint, new Coord(i, j)) <= SpawnRange) {
                        spawnPoint = new Coord(i, j);
                    }
                }
            }
            return spawnPoint;
        }

        public int GetUnitsPerTurn() { return UnitPerTurn; }
        public Coord GetSpawnPoint() { return SpawnPoint; }

        public void SetunitPerTurn(int value) {
            if (value >= 0)
                UnitPerTurn = value;
        }

        public void SetSpawnPoint(Coord cell) {
            if(cell != null && Board[cell.X, cell.Y] == BoardConsts.EMPTY) {
                SpawnPoint = cell;
            }
        }
    }
}
