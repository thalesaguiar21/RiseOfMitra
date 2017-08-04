using RiseOfMitra.Players.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra.MonteCarlo.Simulation
{
    public interface ISimulation
    {
        void SetUp(List<ACommand> validCmds);
        List<ACommand> Execute();
    }
}
