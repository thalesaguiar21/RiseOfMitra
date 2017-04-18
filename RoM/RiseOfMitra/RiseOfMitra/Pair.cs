using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoMUtils
{
    struct Pair
    {
        public int x;
        public int y;

        public static int Distance(Pair a, Pair b)
        {
            return (int)Math.Floor(Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2)));
        }
    }
}
