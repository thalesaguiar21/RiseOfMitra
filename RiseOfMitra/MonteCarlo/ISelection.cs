using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiseOfMitra.Players.Commands;

namespace RiseOfMitra.MonteCarlo
{
    public interface ISelection
    {
        Node Execute(List<Node> validNodes);
    }
}
