using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Types
{
    public class Command
    {
        public const string ATTACK = "ATTACK";
        public const string MOVE = "MOVE";
        public const string INSPECT = "INSPECT";
        public const string CONQUER = "CONQUER";
        public const string HELP = "HELP";
        public const string EXIT = "EXIT";

        public static List<string> GetCommands()
        {
            return new List<string>() { ATTACK, MOVE, INSPECT, CONQUER, HELP, EXIT };
        }
    }
}
