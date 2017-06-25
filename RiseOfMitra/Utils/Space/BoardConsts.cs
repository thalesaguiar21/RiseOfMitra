using System.Linq;

namespace Utils.Space
{
    public class BoardConsts
    {
        // Limits
        public const int MAX_LIN = 25;
        public const int MAX_COL = 35;
        public const int INITIAL_PAWNS = 6;

        // Strings
        public const string DALRION_PAWN = "$";
        public const string RAHKAR_PAWN = "#";
        public const string DALRION_CENTER = "@";
        public const string RAHKAR_CENTER = "%";
        public const string EMPTY = ".";
        public const string FOG = "-";
        public const string BLOCKED = "X";

        private string[] DALRION_UNITS = { "$", "@" };
        private string[] RAHKAR_UNITS = { "#", "%" };


        public bool IsDalrion(string cell) {
            return DALRION_UNITS.Contains(cell);
        }

        public bool IsRahkar(string cell) {
            return RAHKAR_UNITS.Contains(cell);
        }

        public bool IsValid(string cell) {
            return IsRahkar(cell) || IsDalrion(cell);
        }
    }
}

