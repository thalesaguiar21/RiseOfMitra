using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoMUtils
{
    class Coord
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

        public static Coord ToCoord(string str)
        {
            str = str.Trim();
            int maxLength = ("" + GameConsts.BOARD_COL + "," + GameConsts.BOARD_LIN).Length;
            int minLenght = 3;
            if (str.Length <= maxLength || str.Length >= minLenght)
            {
                string[] comps = str.Split(',');
                if (comps.Length != 2)
                {
                    return null;
                }
                else
                {
                    try
                    {
                        return new Coord(int.Parse(comps[0]), int.Parse(comps[1]));
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Can't parse " + str + " into Coord!");
                    }
                }
            }
            return null;
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
            else if (pos.X > GameConsts.BOARD_LIN || pos.Y > GameConsts.BOARD_COL)
                return false;
            return true;
        }

        public override string ToString()
        {
            return "(" + (X + 1) + ", " + (Y + 1) + ")";
        }
    }
}
