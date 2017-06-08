using System;
using System.Text;

namespace RiseOfMitra
{
    class ABuilding : Unit
    {
        private int LifePerSec;
        private const int MAX_LIFEPERSEC = 3;

        public override string GetStatus() {
            StringBuilder msg = new StringBuilder(base.GetStatus());
            msg.Append("Life/sec: " + LifePerSec + "\n");
            return msg.ToString();
        }

        public void Regen() {
            SetCurrLife(GetCurrLife() + LifePerSec);
        }

        public int GetLifePerSec() { return LifePerSec; }

        public void SetLifePerSec(int lifePerSec) {
            if (lifePerSec < 0 || lifePerSec > MAX_LIFEPERSEC)
                Console.WriteLine(lifePerSec + " isn't a valid regen rate!");
            else {
                LifePerSec = lifePerSec;
            }
        }
    }
}
