using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra.MonteCarlo.Expansion
{
    public class ExpandAll : IExpansion
    {
        public void Expand(Node root, Node subTree) {
            if (root != null) {
                int i = root.Childs.IndexOf(subTree);
                if (i == - 1) {
                    subTree.VisitCount++;
                    root.Childs.Add(subTree);
                } else {
                    root.Childs[i].VisitCount++;
                }
            }
        }
    }
}
