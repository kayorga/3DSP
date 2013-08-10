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
        public byte countKilledEnemies;
        public int countXPGained;

        public TimeSpan timeSpent;

        public abstract string getLabel();

        public abstract string getShortLabel();

        public abstract bool complete();
        
        public abstract bool isType1();

        public abstract bool update(byte kind, int exp);

        public abstract void setup(byte level, byte kind, byte count);
    }
}
