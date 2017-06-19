using Utils.Types;
using Utils.Space;

namespace Units.Temples
{
    class Temple : ABuilding
    {
        public Temple(ECultures culture) {
            SetCulture(culture);
            SetCurrLife(0);
            SetTotalLife(0);
            SetDef(0);
            SetLifePerSec(0);
            SetPos(new Coord(0, 0));
            SetSize(2);
        }
    }
}
