using System;
using System.Collections.Generic;
using System.Linq;
using RiseOfMitra.Players.Commands;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra.MonteCarlo
{
    class DefaultSelection : ISelection
    {
        private List<Node> ValidStates;
        private double Threshold;

        public DefaultSelection(double threshold = 0.15) {
            Threshold = threshold;
            if(Threshold < 0) {
                Threshold = 0.0;
            }
        }

        public Node Execute(List<Node> validStates) {
            Node chosen = null;
            List<Node> bestNodes = new List<Node>();
            if (validStates != null && validStates.Count > 0) {
                ValidStates = validStates;
                double highestValue = ValidStates[0].Value;
                for (int i = 1; i < ValidStates.Count; i++) {
                    double currValue = ValidStates[i].Cmd.Value();
                    if (currValue > highestValue) {
                        highestValue = currValue;
                    }
                }
                for (int i = 1; i < ValidStates.Count; i++) {
                    double currValue = ValidStates[i].Cmd.Value();
                    if (currValue > highestValue * (1 - Threshold)) {
                        bestNodes.Add(ValidStates[i]);
                    }
                }
                Random rnd = new Random();
                int selection = rnd.Next(bestNodes.Count);
                chosen = bestNodes[selection];
            }
            return chosen;
        }
    }
}
