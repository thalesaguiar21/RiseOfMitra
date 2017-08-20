using System.Collections.Generic;

namespace RiseOfMitra.MonteCarlo.Simulation
{
    public interface ISimulation
    {
        void SetUp(List<Node> validCmds);
        List<Node> Execute();
    }
}
