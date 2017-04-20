using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoMUtils
{
    struct Coord
    {
        public int X;
        public int Y;

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static int Distance(Coord a, Coord b)
        {
            int squareTerm1 = (a.X - b.X) * (a.X - b.X);
            int squareTerm2 = (a.Y - b.Y) * (a.Y - b.Y);
            return (int) Math.Floor(Math.Sqrt(squareTerm1 + squareTerm2));
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}
