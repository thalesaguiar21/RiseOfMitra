using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoMUtils
{
    static class GameConsts
    {
        public static int BOARD_SIZE = 35;
        public static int[] validDalrionsUnits = new int[3] { 1, 3, 5 };
        public static int[] validRahkarsUnits = new int[3] { 1, 3, 5 };
    }

    struct Pair
    {
        public int x;
        public int y;

        public static int Distance(Pair a, Pair b)
        {
            return (int) Math.Floor(Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2)));
        }
    }
}
