using System;
using Types;
using Cells;

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

        public abstract void Move(Coord target);

        public abstract void Adapt(ETerrain terrain);
    }
}
