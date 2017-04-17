using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RoMUtils;

namespace RiseOfMitra
{
    abstract class AUnit
    {
        protected Pair Position;
        protected int MaxLifePoints;
        protected int LifePoints;

        public Pair GetPosition() { return this.Position; }
        public int GetMaxLifePoints() { return this.MaxLifePoints; }
        public int GetLifePoints() { return this.LifePoints; }

        public void SetPosition(Pair position)
        {
            bool validX = position.x >= 0 && position.x <= GameConsts.BOARD_SIZE;
            bool validY = position.y >= 0 && position.y <= GameConsts.BOARD_SIZE;

            if (validX && validY)
            {
                Position = position;
            }
            else
            {
                Console.WriteLine("Posição inválida!");
            }
        }

        public void SetMaxLifePoints(int maxLifePoints)
        {
            bool validMax = maxLifePoints >= 0 && maxLifePoints <= GameConsts.MAX_LIFE;

            if (validMax)
            {
                MaxLifePoints = maxLifePoints;
            }
            else
            {
                Console.WriteLine("Quantidade máxima de vida deve ser um número entre 0 e " + GameConsts.MAX_LIFE);
            }
        }

        public void SetLifePoints(int lifePoints)
        {
            bool validLife = lifePoints >= 0 && lifePoints <= MaxLifePoints;

            if (validLife)
            {
                LifePoints = lifePoints;
            }
            else
            {
                Console.WriteLine("Pontos de vida inválidos");
            }
        }
    }
}
