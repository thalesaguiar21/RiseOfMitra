using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra.MonteCarlo
{
    public class ExpandAll : IExpansion
    {
        public void Expand(Node root, Node subTree) {
            if (root != null) {
                if (!root.Childs.Contains(subTree)) {
                    root.Childs.Add(subTree);
                    subTree.VisitCount++;
                } else {
                    int i = root.Childs.IndexOf(subTree);
                    root.Childs[i].VisitCount++;
                }
            }
        }
    }
}
