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

        public Mod()
        {
            setup(0, 0);
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
                case Constants.ELM_NIL: return "";
                case Constants.ELM_PLA: return "Plasma ";
                case Constants.ELM_HEA: return "Heat ";
                case Constants.ELM_ICE: return "Ice ";
                default: return "";
            }
        }

        private string getTypeLabel()
        {
            switch (value)
            {
                case Constants.TYP_NIL: return "Beam";
                case Constants.TYP_BLA: return "Blast";
                case Constants.TYP_WAV: return "Wave";
                case Constants.TYP_TRI: return "Triplet";
                default: return "Beam";
            }
        }
    }
}
