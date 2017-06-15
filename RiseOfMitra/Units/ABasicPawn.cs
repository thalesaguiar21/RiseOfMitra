using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boards;
using Cells;
using Types;

namespace Units
{
    public class ABasicPawn : APawn
    {
        public override void Adapt(ETerrain prevTerrain, ETerrain curTerrain) {
            throw new NotImplementedException();
        }

        public override bool Move(Coord cursor) {
            throw new NotImplementedException();
        }

        public override APawn Copy() {
            DalrionPawn pawn = new DalrionPawn();
            Boards = new Board(GetBoards());
            pawn.SetCurrLife(GetCurrLife());
            pawn.SetTotalLife(GetTotalLife());
            pawn.SetAtk(GetAtk());
            pawn.SetAtkRange(GetAtkRange());
            pawn.SetCulture(NativeOf());
            pawn.SetDef(GetDef());
            pawn.SetMovePoints(GetMovePoints());
            pawn.SetPos(GetPos());
            pawn.SetSize(GetSize());

            return pawn;
        }
    }
}
