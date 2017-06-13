using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    abstract class ACommand
    {
        protected string INVALID_POS = "The selected position is not valid! ";
        protected string OCCUPIED_CELL = "The targeted cell is occupied! ";
        protected string PLAYER = "Current player can not be null! ";
        protected string NO_PAWN = "No pawn found at one of given positions! ";
        protected string OUT_OF_RANGE = "The target is not in range! ";
        protected string BLOCK = "The enemy blocked the attack, no damage dealt! ";
        protected string NO_OPONENT = "The oponent can not be null! ";
        protected string NO_BOARDS = "Current boards can not be null! ";
        protected string NO_ENEMIES = "This pawn has no enemies in range! ";


        public abstract bool Execute();
        protected abstract bool Validate();
    }
}
