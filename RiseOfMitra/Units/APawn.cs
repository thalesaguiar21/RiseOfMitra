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
        private int movePoints;
        protected ETerrain[] positiveTerrains;
        private const int MAX_MOVE = 25;

        public int MovePoints {
            get { return movePoints; }
            set
            {
                if ((value > 0) && (value <= MAX_MOVE))
                    movePoints = value;
            }
        }

        /// <summary>
        /// Gets or sets all terrains that give positive bonuses for this pawn.
        /// </summary>
        public ETerrain[] PositiveTerrains {
            get { return positiveTerrains; }
            set { positiveTerrains = value; }
        }

        public override string GetStatus() {
            StringBuilder msg = new StringBuilder(base.GetStatus());
            msg.Append("Mov: " + MovePoints + "\n");
            return msg.ToString();
        }

        /// <summary>
        /// A copy method for pawns.
        /// </summary>
        /// <param name="boards">The board where it must be placed.</param>
        /// <returns>A copy of the current Pawn instance.</returns>
        public abstract APawn Copy(Board boards);

        public abstract void UnAdapt(ETerrain terrain);

        public abstract void ReAdapt(ETerrain terrain);

        public void Adapt(ETerrain prev, ETerrain actual) {
            UnAdapt(prev);
            ReAdapt(actual);
        }
    }
}
