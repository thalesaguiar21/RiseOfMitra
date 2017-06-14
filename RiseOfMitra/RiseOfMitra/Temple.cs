using Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types;
using Units;

namespace Game
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
