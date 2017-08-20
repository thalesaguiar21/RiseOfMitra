using System;

namespace Utils.Space
{
    public class Coord : IEquatable<Coord>
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
            int xs = Math.Abs(a.X - b.X);
            int ys = Math.Abs(a.Y - b.Y);
            return xs + ys;
        }

        public static bool IsValid(Coord pos)
        {
            if (pos == null)
                return false;
            else if (pos.X <= 0 || pos.Y <= 0)
                return false;
            else if (pos.X >= BoardConsts.MAX_LIN - 1 || pos.Y >= BoardConsts.MAX_COL - 1)
                return false;
            return true;
        }

        public override string ToString()
        {
            return "(" + (X) + ", " + (Y) + ")";
        }

        public bool Equals(Coord other)
        {
            return other.X == X && other.Y == Y;
        }
    }
}
