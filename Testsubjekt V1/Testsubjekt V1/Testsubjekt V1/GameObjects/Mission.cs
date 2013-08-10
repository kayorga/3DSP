using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    abstract class Mission
    {
        public int target;
        internal int actCount;
        internal int tarCount;
        public byte level;

        public abstract string getLabel();

        public abstract bool complete();
        
        public abstract bool isType1();

        public abstract bool update(byte kind);

        public abstract void setup(byte level, byte kind, byte count);
    }
}
