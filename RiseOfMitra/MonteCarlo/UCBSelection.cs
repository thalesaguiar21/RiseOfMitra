using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiseOfMitra.Players.Commands;

namespace RiseOfMitra.MonteCarlo
{
    class UCBSelection : ISelectionStrategy
    {
        private List<Node> validMoves;
        private int maxPlayouts;

        public UCBSelection(List<Node> validMoves, int maxPlayouts) {
            this.validMoves = validMoves;
            this.maxPlayouts = maxPlayouts;
        }

        public Node Execute() {
            Node chosen = null;
            if (validMoves.Count > 0) {
                double maxValue = 0;
                foreach (Node tmpNode in validMoves) {
                    double meanPayout = tmpNode.Cmd.Value() / tmpNode.VisitCount;
                    double curValue = meanPayout +
                        Math.Sqrt((2 * Math.Log(tmpNode.VisitCount)) / maxPlayouts);
                    if (curValue >= maxValue) {
                        chosen = tmpNode;
                        maxValue = curValue;
                    }
                }
            }
            return chosen;
        }
    }
}
