using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiseOfMitra.Players.Commands;

namespace RiseOfMitra.MonteCarlo
{
    class UCTSelection : ISelection
    {
        private List<Node> ValidStates;
        private double Beta;
        private Node Root;

        public UCTSelection (double beta) {
            if (beta > 0)
                Beta = beta;    
        }

        public void SetRoot(Node root) {
            if (root != null)
                Root = root;
        }

        public Node Execute(List<Node> validStates) {
            Node chosen = null;
            if (validStates != null && validStates.Count > 0 && Root != null) {
                ValidStates = validStates;
                double maxValue = 0;
                foreach (Node state in ValidStates) {
                    double meanPayout = state.Value / state.VisitCount;
                    double curValue = meanPayout + Math.Sqrt((2 * Math.Log(state.VisitCount)) / Root.VisitCount);
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
