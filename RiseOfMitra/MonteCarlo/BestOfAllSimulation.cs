using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra.MonteCarlo
{
    class BestOfAllSimulation : ISimulationStrategy
    {
        List<Node> ValidStates;

        public BestOfAllSimulation(List<Node> validStates) {
            if(validStates != null) {
                ValidStates = validStates;
            }
        }

        private double Mean(List<Node> states) {
            double mean = 0;
            if (states != null && states.Count > 0) {
                // calcula a variância populacional
                foreach (Node node in states) {
                    mean += node.Value;
                }
                mean /= states.Count;
            }
            return mean;
        }

        private double StandartDeviation(List<Node> states, double mean) {
            double value = 0;
            if (states != null && states.Count > 0) {
                double variation = 0;
                foreach (Node node in states) {
                    variation += Math.Pow(node.Value - mean, 2.0);
                }

                variation /= states.Count;
                value = Math.Sqrt(variation);
            }
            return value;
        }

        public List<Node> Execute() {
            List<Node> bestOfAll = new List<Node>();
            double mean = Mean(ValidStates);
            double stDev = StandartDeviation(ValidStates, mean);

            foreach (Node state in ValidStates) {
                if(state.Value >= mean - stDev && state.Value <= mean + stDev) {
                    bestOfAll.Add(state);
                }
            }

            return bestOfAll;
        }
    }
}
