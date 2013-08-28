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
        //-1 undef
        //0 element
        //1 bullet type
        //2 strength
        //3 speed
        //4 recharge
        //5 ammo cap

        public int value;
        //values type 0:
        //0 plasma
        //1 wave

        public Mod(int t, int v = 0)
        {
            setup(t, v);
        }

        public void setup(int t, int v = 0)
        {
            type = t;
            value = v;
        }

        public string getLabel() {
            switch (type)
            {
                case Constants.MOD_NIL: return "";
                case Constants.MOD_ELM: return "Element " + getElementLabel();
                case Constants.MOD_TYP: return getTypeLabel();
                case Constants.MOD_STR: return "Strength Lv" + value;
                case Constants.MOD_SPD: return "Speed Lv" + value;
                case Constants.MOD_RCG: return "Recharge Lv" + value;
                case Constants.MOD_ACP: return "Ammo Lv" + value;
                default: return "UNDEF";
            }
        }

        private string getElementLabel()
        {
            switch (value)
            {
                case 0: return "Plasma ";
                case 1: return "Photon ";
                case 2: return "Phazon ";
                default: return "";
            }
        }

        private string getTypeLabel()
        {
            switch (value)
            {
                case 0: return "Charge";
                case 1: return "Blast";
                case 2: return "Cannon";
                default: return "Beam";
            }
        }
    }
}
