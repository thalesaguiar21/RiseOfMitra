using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Players.Commands;

namespace RiseOfMitra.MonteCarlo
{
    class UCBSelection : ISelectionStrategy
    {
        private Dictionary<ACommand, Node> validMoves;
        private int maxPlayouts;

        public UCBSelection(Dictionary<ACommand, Node> validMoves, int maxPlayouts) {
            this.validMoves = validMoves;
            this.maxPlayouts = maxPlayouts;
        }

        public ACommand Execute() {
            ACommand cmd = null;
            if (validMoves.Count > 0) {
                double maxValue = 0;
                foreach (ACommand tmpCmd in validMoves.Keys.ToList()) {
                    double meanPayout = validMoves[tmpCmd].Value / validMoves[tmpCmd].VisitCount;
                    double curValue = meanPayout +
                        Math.Sqrt((2 * Math.Log(validMoves[tmpCmd].VisitCount)) / maxPlayouts);
                    if (curValue >= maxValue) {
                        cmd = tmpCmd;
                        maxValue = curValue;
                    }
                }
            }
            return cmd;
        }
    }
}
