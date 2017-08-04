using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra.MonteCarlo.Expansion
{
    public interface IExpansion
    {
        void Expand(Node root, Node subTree);
    }
}
