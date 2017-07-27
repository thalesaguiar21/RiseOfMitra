using Utils.Types;
using Utils.Space;

namespace Units.Temples
{
    class Temple : ABuilding
    {
        public Temple(ECultures culture) {
            Culture = culture;
            CurrLife = 0;
            TotalLife = 0;
            Def = 0;
            Position = new Coord(0, 0);
            Size = 2;
            LifePerSec = 0;
        }
    }
}
