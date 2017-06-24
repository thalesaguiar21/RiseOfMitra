using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boards;
using Players;
using Players.Commands;

namespace RiseOfMitra.MonteCarlo
{
    /// <summary>
    /// This class represents a Game Tree node, which is reachable by Given a a State(Board) applying
    /// the given Command.
    /// </summary>
    public class Node
    {
        public int visitCount;
        public double value;
        public Board boards;
        public ACommand cmd;
        List<Node> Childs;


        public Node(double value, Board boards, ACommand cmd) {
            visitCount = 0;
            this.boards = boards;
            this.cmd = cmd;
            this.value = value;
        }

        public static bool ValidateNode(Node node) {
            bool valid = true;
            if (node == null) {
                valid = false;
            } else if (node.visitCount < 0) {
                valid = false;
            } else if (node.boards == null) {
                valid = false;
            } else if (node.cmd == null) {
                valid = false;
            }
            return valid;
        }

        public bool Equals(Node otherNode) {
            if(otherNode is Node) {
                return boards.Equals(otherNode.boards) && cmd.Equals(otherNode.cmd);
            } else {
                return false;
            }
        }
    }
}
