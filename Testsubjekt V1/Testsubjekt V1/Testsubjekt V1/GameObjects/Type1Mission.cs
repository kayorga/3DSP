using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class Type1Mission : Mission
    {
        private int[] kinds;

        public Type1Mission()
        {
            //TODO
        }

        public override bool isType1()
        {
            return true;
        }

        public override bool update()
        {
            //TODO
            return true;
        }
    }
}
