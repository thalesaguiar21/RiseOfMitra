using System;
using System.Text;
using Utils.Types;
using Utils.Space;
using Boards;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Units
{
    public abstract class APawn : Unit, IAdaptable
    {
        private int MovePoints;
        private int Atk;
        private int AtkRange;
        protected ETerrain[] PositiveTerrains;
        private const int MAX_MOVE = 25;
        private const int MAX_RANGE = 5;
        private const int MAX_ATK = 25;

        public override string GetStatus() {
            StringBuilder msg = new StringBuilder(base.GetStatus());
            msg.Append("Mov: " + MovePoints + "\n");
            msg.Append("Atk: " + Atk + "\n");
            msg.Append("Atk range: " + AtkRange + "\n");
            return msg.ToString();
        }

        public abstract bool Move(Coord cursor);
        public abstract APawn Copy(Board boards);
        public abstract void Adapt(ETerrain prevTerrain, ETerrain curTerrain);

        public int GetMovePoints() { return MovePoints; }
        public int GetAtk() { return Atk; }
        public int GetAtkRange() { return AtkRange; }
        public ETerrain[] GetPositiveTerrains() { return PositiveTerrains; }

        public void SetAtkRange(int atkRange) {
            if (atkRange > 0 && atkRange <= MAX_RANGE)
                AtkRange = atkRange;
        }

        public void SetMovePoints(int movePoints) {
            if (movePoints < 1 || movePoints > MAX_MOVE)
                Console.WriteLine(movePoints + " isn't a valid movement point!");
            else {
                MovePoints = movePoints;
            }
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
