using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boards;
using Players;

namespace RiseOfMitra.MonteCarlo
{
    class Node
    {
        public int visitCount;
        public double value;
        public int winCount;


        public Node(int visitCount, int winCount, double value) {
            this.visitCount = visitCount;
            this.winCount = winCount;
            this.value = value;
        }

        private static bool ValidateNode(Node node) {
            bool valid = true;
            if (node == null)
                valid = false;
            else if (node.visitCount < 0)
                valid = false;
            else if (node.winCount < 0)
                valid = false;
            return valid;
        }
    }
}
