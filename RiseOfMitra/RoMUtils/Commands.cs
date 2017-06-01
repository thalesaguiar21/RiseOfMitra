using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consts
{
    public class Commands
    {
        public const string ATTACK = "ATTACK";
        public const string MOVE = "MOVE";
        public const string INSPECT = "INSPECT";
        public const string CONQUER = "CONQUER";
        public const string EXIT = "EXIT";

        public const string GET_PAWN = "GET PAWN";
        public const string GET_POS = "GET POS";

        public static List<string> GetCommands() {
            return new List<string>() { ATTACK, MOVE, INSPECT, CONQUER, EXIT };
        }
    }
}
