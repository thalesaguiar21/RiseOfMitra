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
        public int WinRate;


        public Node(Board boards, ACommand cmd) {
            Childs = new List<Node>();
            VisitCount = 0;
            Boards = boards;
            Cmd = cmd;
            if (cmd != null) {
                Value = cmd.Value();
            } else {
                Value = 0;
            }
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

        public override bool Equals(object obj) {
            if (obj is Node other) {
                return Boards.Equals(other.Boards) && Cmd.Equals(other.Cmd);
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
