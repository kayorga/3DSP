using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    abstract class Mission
    {
        public int target;
        public int actCount;
        private int count;
        
        public abstract bool isType1();

        public abstract bool update();
    }
}
