using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boards;
using RiseOfMitra.Players.Commands;

namespace RiseOfMitra.MonteCarlo
{
    /// <summary>
    /// This class represents a Game Tree node, which is reachable by Given a a State(Board) applying
    /// the given Command.
    /// </summary>
    public class Node
    {
        public int VisitCount;
        public double Value;
        public Board Boards;
        public ACommand Cmd;
        public List<Node> Childs;


        public Node(double value, Board boards, ACommand cmd) {
            VisitCount = 0;
            Boards = boards;
            Cmd = cmd;
            Value = value;
            Childs = new List<Node>();
        }

        public static bool ValidateNode(Node node) {
            bool valid = true;
            if (node == null) {
                valid = false;
            } else if (node.VisitCount < 0) {
                valid = false;
            } else if (node.Boards == null) {
                valid = false;
            } else if (node.Cmd == null) {
                valid = false;
            }
            return valid;
        }

        public bool Equals(Node otherNode) {
            if(otherNode is Node) {
                return Boards.Equals(otherNode.Boards) && Cmd.Equals(otherNode.Cmd);
            } else {
                return false;
            }
        }
    }
}
