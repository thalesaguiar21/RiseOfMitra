using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoMUtils
{
    class BoardStrings
    {
        public const string CHAR_DALRION_PAWN = "$";
        public const string CHAR_RAHKAR_PAWN = "#";
        public const string CHAR_DALRION_CENTER = "@";
        public const string CHAR_RAHKAR_CENTER = "%";

        private static string[] NON_VALID_UNITS = { ".", "X"};
        private static string[] dalrionChar = { "$", "@" };
        private static string[] rahkarChar = { "#", "%" };

        public static bool IsValid(string boardChar)
        {
            if (NON_VALID_UNITS.Contains(boardChar))
                return false;
            return true;
        }

        public static ECultures ToCulture(string msg)
        {
            if (dalrionChar.Contains(msg)) return ECultures.DALRIONS;
            else if (rahkarChar.Contains(msg)) return ECultures.RAHKARS;
            return ECultures.DEFAULT;
        }
    }
}
