using System;
using System.Text;
using Utils.Types;
using Utils.Space;
using Boards;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Units
{
    /// <summary>
    /// This class represents every pawn in the game. All pawns have a moviment point
    /// and must be adaptable to its current terrain.
    /// </summary>
    public abstract class APawn : Unit, IAdaptable
    {
        private int MovePoints;
        protected ETerrain[] PositiveTerrains;
        private const int MAX_MOVE = 25;
        protected const int MAX_RANGE = 10;

        public override string GetStatus() {
            StringBuilder msg = new StringBuilder(base.GetStatus());
            msg.Append("Mov: " + MovePoints + "\n");
            return msg.ToString();
        }

        public abstract APawn Copy(Board boards);

        public abstract void UnAdapt(ETerrain terrain);

        public abstract void ReAdapt(ETerrain terrain);

        public void Adapt(ETerrain prev, ETerrain actual) {
            UnAdapt(prev);
            ReAdapt(actual);
        }


        /// <summary>
        /// This method returns all the terrains which gives a positive bonus to the actual
        /// instance.
        /// </summary>
        /// <returns></returns>
        public ETerrain[] GetPositiveTerrains() { return PositiveTerrains; }

        public int GetMovePoints() { return MovePoints; }

        public void SetMovePoints(int movePoints) {
            if (movePoints < 1 || movePoints > MAX_MOVE)
                Console.WriteLine("Move points must be between {0} and {1}", 1, MAX_MOVE);
            else {
                MovePoints = movePoints;
            }
        }
    }
}
