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
        public int winCount;
        public Board state;
        public Player curPlayer;


        public Node(int visitCount, int winCount, Board state, Player curPlayer) {
            this.visitCount = visitCount;
            this.winCount = winCount;
            this.state = state;
            this.curPlayer = curPlayer;
        }

        private static bool ValidateNode(Node node) {
            bool valid = true;
            if (node == null)
                valid = false;
            else if (node.visitCount < 0)
                valid = false;
            else if (node.winCount < 0)
                valid = false;
            else if (node.state == null)
                valid = false;
            else if (node.curPlayer == null)
                valid = false;
            return valid;
        }
    }
}
