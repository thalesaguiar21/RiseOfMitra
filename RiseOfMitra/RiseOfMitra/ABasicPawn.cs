using RoMUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra
{
    abstract class ABasicPawn : Unit, IAdaptable
    {
        private int MovePoints;
        private int Atk;
        private const int MAX_MOVE = 5;
        private const int MAX_ATK = 10;


        public int GetMovePoints() { return MovePoints; }
        public int GetAtk() { return Atk; }

        public void SetMovePoints(int movePoints)
        {
            if (movePoints < 1 || movePoints > MAX_MOVE)
                Console.WriteLine(movePoints + " isn't a valid movement point!");
            else
            {
                MovePoints = movePoints;
            }
        }

        public void SetAtk(int atk)
        {
            if (atk < 0 || atk > MAX_ATK)
                Console.WriteLine(atk + " isn't a valid atack point!");
            else
            {
                Atk = atk;
            }
        }

        protected bool ExistPath(Coord init, Coord target)
        {
            if (init.IsSame(target))
                return true;
            else if (Board[target.X, target.Y] != BoardStrings.EMPTY)
                return false;
            else if (Coord.Distance(init, target) > MovePoints)
                return false;
            else if (init.X - 1 > 0 && Board[init.X - 1, init.Y] == BoardStrings.EMPTY)
                ExistPath(new Coord(init.X - 1, init.Y), target);
            else if (init.X + 1 < GameConsts.BOARD_LIN && Board[init.X + 1, init.Y] == BoardStrings.EMPTY)
                ExistPath(new Coord(init.X + 1, init.Y), target);
            else if (init.Y - 1 > 0 && Board[init.X, init.Y - 1] == BoardStrings.EMPTY)
                ExistPath(new Coord(init.X, init.Y - 1), target);
            else if (init.Y + 1 < GameConsts.BOARD_COL && Board[init.X, init.Y + 1] == BoardStrings.EMPTY)
                ExistPath(new Coord(init.X, init.Y + 1), target);
            return false;
        }

        public abstract void Move(Coord target);

        public abstract void Adapt(ETerrain terrain);
    }
}
