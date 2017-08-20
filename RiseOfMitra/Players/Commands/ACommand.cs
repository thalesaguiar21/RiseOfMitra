using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Boards;

using Utils.Space;
using Utils.Types;

namespace RiseOfMitra.Players.Commands
{
    public abstract class ACommand
    {
        protected const string INVALID_POS = "The selected position is not valid! ";
        protected const string OCCUPIED_CELL = "The targeted cell is occupied! ";
        protected const string NO_PAWN = "No pawn found at one of given positions! ";
        protected const string OUT_OF_RANGE = "The target is not in range! ";
        protected const string BLOCK = "The enemy blocked the attack, no damage dealt! ";
        protected const string NO_ENEMIES = "This pawn has no enemies in range! ";

        protected string ErrorMsg;
        protected Coord origin;
        protected Coord target;
        protected Player curPlayer;
        protected Player oponent;

        public Coord Origin
        {
            get { return origin; }
        }

        public Coord Target
        {
            get { return target; }

        }

        public Player CurPlayer
        {
            get { return curPlayer; }
            set { curPlayer = value; }
        }

        public Player Oponent
        {
            get { return oponent; }
            set { oponent = value; }
        }

        public abstract bool Execute(Board board, bool isSimulation = false);
        public abstract bool Equals(ACommand otherCmd);
        public abstract bool IsValid(Board board);
        public abstract string GetShort();

        public override string ToString()
        {
            return String.Format("Target: {0}\n", Target);
        }
    }
}
