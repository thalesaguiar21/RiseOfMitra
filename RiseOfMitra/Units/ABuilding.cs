using System;
using System.Text;
using Utils;

namespace Units
{
    public abstract class ABuilding : Unit
    {
        int lifePerSec;
        const int MAX_LIFEPERSEC = 3;

        public override string GetStatus()
        {
            StringBuilder msg = new StringBuilder(base.GetStatus());
            msg.Append("Life/sec: " + LifePerSec + "\n");
            return msg.ToString();
        }

        public int LifePerSec
        {

            get { return lifePerSec; }
            set {
                if ((value >= 0) && (value <= MAX_LIFEPERSEC))
                    lifePerSec = value;
            }
        }

        public void Regen()
        {
            if (CurrLife < TotalLife)
                CurrLife = CurrLife + LifePerSec;
        }
    }
}
