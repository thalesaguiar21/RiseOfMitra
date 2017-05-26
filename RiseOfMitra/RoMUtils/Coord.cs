using System;
using Consts;

namespace Cells
{
    public class Coord
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

        public bool IsSame(Coord b)
        {
            if (X == b.X && Y == b.Y)
                return true;
            return false;
        }

        public static bool IsValid(Coord pos)
        {
            if (pos == null)
                return false;
            else if (pos.X < 0 || pos.Y < 0)
                return false;
            else if (pos.X > BoardConsts.BOARD_LIN || pos.Y > BoardConsts.BOARD_COL)
                return false;
            return true;
        }

        public override string ToString()
        {
            return "(" + (X + 1) + ", " + (Y + 1) + ")";
        }
    }
}
