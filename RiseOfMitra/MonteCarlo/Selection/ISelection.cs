using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiseOfMitra.Players.Commands;

namespace RiseOfMitra.MonteCarlo.Selection
{
    public interface ISelection
    {
        Node Execute(SelectionParameters parameters);
    }
}
