using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra.MonteCarlo
{
    class OMCSelection : ISelection
    {
        private List<Node> ValidStates;

        private List<Meta.Numerics.Complex> Urgency(List<Node> states) {
            List<Meta.Numerics.Complex> stateUrgency = new List<Meta.Numerics.Complex>();
            double bestValue = 0.0;
            double mean = 0;
            foreach (Node state in states) {
                mean += state.Value;
                if (state.Value > bestValue)
                    bestValue = state.Value;
            }
            mean /= states.Count;

            foreach (Node state in states) {
                double iVariation = Math.Pow(state.Value - mean, 2.0) / states.Count;
                double value = 0.0;
                if (iVariation > 0.0)
                    value = (bestValue - state.Value) / (Math.Sqrt(2) * Math.Sqrt(iVariation));
                Meta.Numerics.Complex erfc = 1.0 - Meta.Numerics.Functions.AdvancedComplexMath.Erf(value);
                stateUrgency.Add(erfc);
            }
            return stateUrgency;
        }

        private List<Meta.Numerics.Complex> Fairness(List<Meta.Numerics.Complex> urgency, List<Node> states) {
            Meta.Numerics.Complex siblingsSum = 0;
            for (int i = 0; i < urgency.Count; i++) {
                siblingsSum += states[i].VisitCount * urgency[i];
            }

            List<Meta.Numerics.Complex> fairnessValues = new List<Meta.Numerics.Complex>();
            for (int i = 0; i < urgency.Count; i++) {
                fairnessValues.Add(states[i].VisitCount * urgency[i] / siblingsSum);
            }
            return fairnessValues;
        }

        public Node Execute(List<Node> validStates) {
            if(validStates != null) {
                ValidStates = validStates;
                List<Meta.Numerics.Complex> urgencies = Urgency(ValidStates);
                List<Meta.Numerics.Complex> fairnessValues = Fairness(urgencies, ValidStates);
                double max = 0;
                Node chosen = null;
                for (int i = 0; i < fairnessValues.Count; i++) {
                    if(fairnessValues[i].Re > max) {
                        max = fairnessValues[i].Re;
                        chosen = ValidStates[i];
                    }
                }
                return chosen;
            } else {
                return null;
            }
        }
    }
}
