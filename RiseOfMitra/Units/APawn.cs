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
        protected ETerrain[] PositiveTerrains;
        private const int MAX_MOVE = 10;
        protected const int MAX_RANGE = 8;

        public override string GetStatus() {
            StringBuilder msg = new StringBuilder(base.GetStatus());
            msg.Append("Mov: " + MovePoints + "\n");
            return msg.ToString();
        }

        public abstract bool Move(Coord cursor);
        public abstract APawn Copy(Board boards);
        public abstract void Adapt(ETerrain prevTerrain, ETerrain curTerrain);

        public int GetMovePoints() { return MovePoints; }
        public ETerrain[] GetPositiveTerrains() { return PositiveTerrains; }


        public void SetMovePoints(int movePoints) {
            if (movePoints < 1 || movePoints > MAX_MOVE)
                Console.WriteLine(movePoints + " isn't a valid movement point!");
            else {
                MovePoints = movePoints;
            }
        }
    }
}
