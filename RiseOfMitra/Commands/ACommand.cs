using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cells;

namespace Command
{
    abstract class ACommand
    {
        protected const string INVALID_POS = "The selected position is not valid! ";
        protected const string OCCUPIED_CELL = "The targeted cell is occupied! ";
        protected const string PLAYER = "Current player can not be null! ";
        protected const string NO_PAWN = "No pawn found at one of given positions! ";
        protected const string OUT_OF_RANGE = "The target is not in range! ";
        protected const string BLOCK = "The enemy blocked the attack, no damage dealt! ";
        protected const string NO_OPONENT = "The oponent can not be null! ";
        protected const string NO_BOARDS = "Current boards can not be null! ";
        protected const string NO_ENEMIES = "This pawn has no enemies in range! ";

        protected Board Boards;
        protected string ErrorMsg;
        protected Coord Target;

        public abstract bool Execute();
        public abstract bool IsValid();
        protected abstract bool Validate();

        public override string ToString() {
            return String.Format("Target: {0}\n", Target);
        }
    }
}
