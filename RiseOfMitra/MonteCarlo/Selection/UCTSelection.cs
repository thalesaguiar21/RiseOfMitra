using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiseOfMitra.Players.Commands;

namespace RiseOfMitra.MonteCarlo.Selection
{
    class UCTSelection : ISelection
    {
        double Beta;

        public UCTSelection(double beta)
        {
            if (beta > 0)
                Beta = beta;
        }

        public Node Execute(SelectionParameters args)
        {
            Node chosen = null;
            if (args.validStates != null && args.validStates.Count > 0 && args.root != null) {
                double maxValue = 0;
                foreach (Node state in args.validStates) {
                    double meanPayout = state.Value / state.VisitCount;
                    double curValue = meanPayout + Math.Sqrt((Beta * Math.Log(state.VisitCount)) / args.root.VisitCount);
                    if (curValue >= maxValue) {
                        chosen = state;
                        maxValue = curValue;
                    }
                }
            }
            return chosen;
        }
    }
}
