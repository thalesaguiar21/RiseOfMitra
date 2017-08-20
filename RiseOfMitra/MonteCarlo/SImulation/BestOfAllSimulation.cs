using RiseOfMitra.Players.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra.MonteCarlo.Simulation
{
    class BestOfAllSimulation : ISimulation
    {
        List<Node> ValidStates;

        private double Mean(List<Node> states) {
            double mean = 0;
            if (states != null && states.Count > 0) {
                foreach (Node state in states) {
                    mean += state.Value;
                }
                mean /= states.Count;
            }
            return mean;
        }

        private double StandartDeviation(List<Node> states, double mean) {
            double value = 0;
            if (states != null && states.Count > 0) {
                double variation = 0;
                foreach (Node state in states) {
                    variation += Math.Pow(state.Value - mean, 2.0);
                }

                variation /= states.Count;
                value = Math.Sqrt(variation);
            }
            return value;
        }

        public List<Node> Execute() {

            var filteredNodes = new List<Node>();
            if (ValidStates != null) {
                double mean = Mean(ValidStates);
                double stDev = StandartDeviation(ValidStates, mean);
                double max = 0.0;

                for (int i = 0; i < ValidStates.Count; i++) {
                    if (ValidStates[i].Value > max)
                        max = ValidStates[i].Value;
                }

                foreach (Node state in ValidStates) {
                    double cValue = state.Value;
                    if (cValue >= max - stDev) {
                        filteredNodes.Add(state);
                    }
                }
            }

            return filteredNodes;
        }

        public void SetUp(List<Node> validCmds) {
            ValidStates = validCmds;
        }
    }
}
