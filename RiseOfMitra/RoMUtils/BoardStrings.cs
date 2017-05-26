using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consts
{
    public class BoardStrings
    {
        public const string CHAR_DALRION_PAWN = "$";
        public const string CHAR_RAHKAR_PAWN = "#";
        public const string CHAR_DALRION_CENTER = "@";
        public const string CHAR_RAHKAR_CENTER = "%";
        public const string EMPTY = ".";
        public const string FOG = "-";
        public const string BLOCKED = "X";

        public static string[] IVALID_UNITS = { ".", "X", "-" };
        public static string[] DALRION_UNITS = { "$", "@" };
        public static string[] RAHKAR_UNITS = { "#", "%" };
    }
}
