using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class Type1Mission : Mission
    {
        private byte[] kinds;

        public Type1Mission(byte lv, byte tKind, byte tCount, byte[] k)
        {
            //TODO
            kinds = k;
            setup(lv, tKind, tCount);
        }

        public override bool isType1()
        {
            return true;
        }

        public override string getLabel()
        {
            string l = "Level " + level + "\n\nExterminate " + ((tarCount == 1) ? "the Nemesis" : tarCount + " Type" + target + " Enemies");
            return l;
        }

        public override bool update(byte kind)
        {
            //TODO
            if (kind == target)
            {
                actCount+=1;
            }
            return true;
        }

        public override bool complete()
        {
            if (actCount >= tarCount)
                return true;
            return false;
        }

        public override void setup(byte lv, byte kind, byte count)
        {
            level = lv;
            target = kind;
            tarCount = count;
            actCount = 0;
        }
    }
}
