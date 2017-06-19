using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boards;
using Utils.Types;
using Utils.Space;

namespace Units.Pawns
{
    public class ABasicPawn : APawn
    {
        public override void Adapt(ETerrain prevTerrain, ETerrain curTerrain) {
            throw new NotImplementedException();
        }

        public override bool Move(Coord cursor) {
            throw new NotImplementedException();
        }

        public override APawn Copy(Board boards) {
            ABasicPawn pawn;
            if (NativeOf() == ECultures.DALRIONS)
                pawn = new DalrionPawn();
            else
                pawn = new RahkarPawn();

            Boards = boards;
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
