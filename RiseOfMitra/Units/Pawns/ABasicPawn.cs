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
        private int Atk;
        private int AtkRange;
        private const int MAX_ATK = 25;

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

        public override string GetStatus() {
            StringBuilder msg = new StringBuilder(base.GetStatus());
            msg.Append("Atk: " + Atk + "\n");
            msg.Append("Atk range: " + AtkRange + "\n");
            return msg.ToString();
        }


        public int GetAtk() { return Atk; }
        public int GetAtkRange() { return AtkRange; }

        public void SetAtkRange(int atkRange) {
            if (atkRange > 0 && atkRange <= MAX_RANGE)
                AtkRange = atkRange;
        }

        public void SetAtk(int atk) {
            if (atk < 0 || atk > MAX_ATK)
                Console.WriteLine(atk + " isn't a valid atack point!");
            else {
                Atk = atk;
            }
        }
    }
}
