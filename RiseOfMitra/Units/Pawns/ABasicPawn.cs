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
    /// <summary>
    /// This class represents every game pawn that can perform an attack.
    /// </summary>
    public abstract class ABasicPawn : APawn
    {
        int atk;
        int atkRange;
        const int MAX_ATK = 25;
        protected const int MAX_RANGE = 10;

        public override APawn Copy(Board boards)
        {
            ABasicPawn pawn;
            if (Culture == ECultures.DALRIONS)
                pawn = new DalrionPawn();
            else
                pawn = new RahkarPawn();
            pawn.CurrLife = CurrLife;
            pawn.TotalLife = TotalLife;
            pawn.Atk = Atk;
            pawn.Atk = Atk;
            pawn.Culture = Culture;
            pawn.Def = Def;
            pawn.Position = Position;
            pawn.Size = Size;

            return pawn;
        }

        public int Atk
        {
            get { return atk; }
            set {
                if ((value >= 0) && (value <= MAX_ATK))
                    atk = value;
            }
        }

        public int AtkRange
        {
            get { return atkRange; }
            set {
                if ((value > 0) && (value <= MAX_RANGE))
                    atkRange = value;
            }
        }

        public override string GetStatus()
        {
            var msg = new StringBuilder(base.GetStatus());
            msg.Append("Atk: " + Atk + "\n");
            msg.Append("Atk range: " + AtkRange + "\n");
            return msg.ToString();
        }
    }
}
