using System;
using System.Collections.Generic;
using System.Linq;
using Players.Commands;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra.MonteCarlo
{
    class DefaultSelection : ISelectionStrategy
    {
        private List<ACommand> validCmds;

        public DefaultSelection(List<ACommand> validCmds) {
            this.validCmds = validCmds;
        }

        public ACommand Execute() {
            ACommand chosen = null;
            double threshold = 0.15;
            List<ACommand> bestCommands = new List<ACommand>();
            if (validCmds.Count > 0) {
                double highestValue = validCmds[0].Value();
                for (int i = 1; i < validCmds.Count; i++) {
                    double currValue = validCmds[i].Value();
                    if (currValue > highestValue) {
                        highestValue = currValue;
                    }
                }
                for (int i = 1; i < validCmds.Count; i++) {
                    double currValue = validCmds[i].Value();
                    if (currValue > highestValue * (1 - threshold)) {
                        bestCommands.Add(validCmds[i]);
                    }
                }
                Random rnd = new Random();
                int selection = rnd.Next(bestCommands.Count);
                chosen = bestCommands[selection];
            }
            return chosen;
        }
    }
}
