﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra.MonteCarlo
{
    class GameTree
    {
        Node Root;
        Node CurState;

        public GameTree(Node root) {
            if(root != null) {
                Root = root;
                CurState = Root;
            }
        }
        
    }
}