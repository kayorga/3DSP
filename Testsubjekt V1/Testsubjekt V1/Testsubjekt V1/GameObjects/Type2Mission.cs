using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class Type2Mission : Mission
    {
        public Type2Mission()
        {
            //TODO
        }

        public override bool isType1()
        {
            return false;
        }

        public override bool update(byte kind, int exp)
        {
            //TODO
            return true;
        }

        public override bool complete()
        {
            throw new NotImplementedException();
        }

        public override string getLabel()
        {
            throw new NotImplementedException();
        }

        public override string getShortLabel()
        {
            throw new NotImplementedException();
        }

        public override void setup(byte level, byte kind, byte count, byte zone, byte area, string[] nl, string[] zl)
        {
            throw new NotImplementedException();
        }

        public override void reward(Player player, ModCollection modCollection)
        {
            throw new NotImplementedException();
        }

        public override void reset()
        {
            throw new NotImplementedException();
        }
    }
}
