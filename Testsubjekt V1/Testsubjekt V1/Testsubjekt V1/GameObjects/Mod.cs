using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class Mod
    {
        public int type;
        //types:
        //0 element
        //1 bullet type
        //2 strength
        //3 speed
        //4 cooldown
        //5 recharge
        //6 ammo cap

        public int value;
        //values type 0:
        //0 plasma
        //1 wave

        public Mod(int t, int v)
        {
            type = t;
            value = v;
        }

        public void upgrade(int level)
        {
            if (type == 0) return;

        }
    }
}
