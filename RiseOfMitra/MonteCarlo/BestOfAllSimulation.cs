using Players.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra.MonteCarlo
{
    class BestOfAllSimulation : ISimulationStrategy
    {
        List<ACommand> ValidCmds;

        public BestOfAllSimulation(List<ACommand> validCmds) {
            if(validCmds != null) {
                ValidCmds = validCmds;
            }
        }

        private double Mean(List<ACommand> commands) {
            double mean = 0;
            if (commands != null && commands.Count > 0) {
                foreach (ACommand cmd in commands) {
                    mean += cmd.Value();
                }
                mean /= commands.Count;
            }
            return mean;
        }

        private double StandartDeviation(List<ACommand> commands, double mean) {
            double value = 0;
            if (commands != null && commands.Count > 0) {
                double variation = 0;
                foreach (ACommand cmd in commands) {
                    variation += Math.Pow(cmd.Value() - mean, 2.0);
                }

                variation /= commands.Count;
                value = Math.Sqrt(variation);
            }
            return value;
        }

        public List<ACommand> Execute() {
            List<ACommand> bestOfAll = new List<ACommand>();
            double mean = Mean(ValidCmds);
            double stDev = StandartDeviation(ValidCmds, mean);
            double max = 0;

            for (int i = 0; i < ValidCmds.Count; i++) {
                if (ValidCmds[i].Value() > max)
                    max = ValidCmds[i].Value();
            }

            foreach (ACommand cmd in ValidCmds) {
                if(cmd.Value() >= max - stDev) {
                    bestOfAll.Add(cmd);
                }
            }

            return bestOfAll;
        }
    }
}
