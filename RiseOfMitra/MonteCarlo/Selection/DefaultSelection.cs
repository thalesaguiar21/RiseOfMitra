using System;
using System.Collections.Generic;
using System.Linq;
using RiseOfMitra.Players.Commands;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra.MonteCarlo.Selection
{
    class DefaultSelection : ISelection
    {
        private double Threshold;

        public DefaultSelection(double threshold = 0.15) {
            Threshold = threshold;
            if(Threshold < 0) {
                Threshold = 0.0;
            }
        }

        public Node Execute(SelectionParameters args) {
            Node chosen = null;
            List<Node> bestNodes = new List<Node>();
            if (args.validStates != null && args.validStates.Count > 0) {
                double highestValue = args.validStates[0].Value;
                bestNodes.Add(args.validStates[0]);
                for (int i = 1; i < args.validStates.Count; i++) {
                    double currValue = args.validStates[i].Value;
                    if (currValue > highestValue) {
                        highestValue = currValue;
                    }
                }
                for (int i = 1; i < args.validStates.Count; i++) {
                    double currValue = args.validStates[i].Value;
                    if (currValue > highestValue * (1 - Threshold)) {
                        bestNodes.Add(args.validStates[i]);
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
