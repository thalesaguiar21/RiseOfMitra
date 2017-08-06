using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiseOfMitra.Players.Commands;

namespace RiseOfMitra.MonteCarlo.Simulation
{
    public class UrgencySimulation : ISimulation
    {
        private List<ACommand> ValidMoves;
        
        public List<ACommand> Execute() {
            List<ACommand> simulatedCmds = new List<ACommand>();
            if(ValidMoves != null && ValidMoves.Count > 0) {
                double[] urgencies = new double[ValidMoves.Count];
                double[] cmdValue = new double[ValidMoves.Count];
                double total = 0;


                Random rnd = new Random();
                double threshold = rnd.Next(cmdValue.Length) / (double)cmdValue.Length;

                for (int i = 0; i < ValidMoves.Count; i++) {
                    cmdValue[i] = ValidMoves[i].Value();
                    total += cmdValue[i];
                }
                for (int i = 0; i < ValidMoves.Count; i++) {
                    urgencies[i] = cmdValue[i] / total;
                    if(urgencies[i] >= threshold) {
                        simulatedCmds.Add(ValidMoves[i]);
                    }
                }
            }
            return simulatedCmds;
        }

        public void SetUp(List<ACommand> validCmds) {
            ValidMoves = validCmds;
        }
    }
}
