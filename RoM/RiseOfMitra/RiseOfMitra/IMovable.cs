using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoMUtils;
namespace RiseOfMitra
{
    interface IMovable
    {
        Pair Move(Pair target);
        Pair Move(Pair[] targetPath);
    }
}
