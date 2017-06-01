using System;
using Types;
using Cells;
using System.Text;

namespace RiseOfMitra
{
    abstract class ABasicPawn : Unit, IAdaptable
    {
        private int MovePoints;
        private int Atk;
        private int AtkRange;
        private const int MAX_MOVE = 10;
        private const int MAX_RANGE = 5;
        private const int MAX_ATK = 10;

        public new string GetStatus() {
            StringBuilder msg = new StringBuilder(base.GetStatus());
            msg.Append("Mov: " + MovePoints + "\n");
            msg.Append("Atk: " + Atk + "\n");
            msg.Append("Atk range: " + AtkRange + "\n");
            return msg.ToString();
        }

        public int GetMovePoints() { return MovePoints; }
        public int GetAtk() { return Atk; }
        public int GetAtkRange() { return AtkRange; }

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

        public abstract void Move(Coord target);

        public abstract void Attack(Coord target);

        public abstract void Adapt(ETerrain terrain);
    }
}
