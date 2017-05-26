using System;

namespace RiseOfMitra
{
    class ABuilding : Unit
    {
        private int LifePerSec;
        private const int MAX_LIFEPERSEC = 3;

        public int GetLifePerSec() { return LifePerSec; }

        public void SetLifePerSec(int lifePerSec)
        {
            if (lifePerSec < 0 || lifePerSec > MAX_LIFEPERSEC)
                Console.WriteLine(lifePerSec + " isn't a valid regen rate!");
            else
            {
                LifePerSec = lifePerSec;
            }
        }

        protected void Regen()
        {
            SetCurrLife(GetCurrLife() + LifePerSec);
        }
    }
}
