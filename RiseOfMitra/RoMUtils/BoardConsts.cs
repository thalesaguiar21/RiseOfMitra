using System.Linq;
using Types;

namespace Consts
{
    public class BoardConsts
    {
        // Limits
        public const int BOARD_LIN = 35;
        public const int BOARD_COL = 45;
        public const int INITIAL_PAWNS = 6;

        // Strings
        public const string DALRION_PAWN = "$";
        public const string RAHKAR_PAWN = "#";
        public const string DALRION_CENTER = "@";
        public const string RAHKAR_CENTER = "%";
        public const string EMPTY = ".";
        public const string FOG = "-";
        public const string BLOCKED = "X";

        private static string[] DALRION_UNITS = { "$", "@" };
        private static string[] RAHKAR_UNITS = { "#", "%" };


        public static bool IsDalrion(string cell)
        {
            return DALRION_UNITS.Contains(cell);
        }

        public static bool IsRahkar(string cell)
        {
            return RAHKAR_UNITS.Contains(cell);
        }

        public static bool IsValid(string cell)
        {
            return IsRahkar(cell) || IsDalrion(cell);
        }


        public static ECultures ToCulture(string msg)
        {
            if (IsDalrion(msg)) return ECultures.DALRIONS;
            else if (IsRahkar(msg)) return ECultures.RAHKARS;
            return ECultures.DEFAULT;
        }
    }
}

