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

        public override bool update(byte kind)
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

        public override void setup(byte level, byte kind, byte count)
        {
            throw new NotImplementedException();
        }
    }
}
