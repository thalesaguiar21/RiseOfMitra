using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    abstract class ACommand
    {
        public abstract bool Execute();
        protected abstract bool Validate();
    }
}
