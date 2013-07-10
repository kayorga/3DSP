using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    static class Constants
    {
        public const int RES_X = 1024;
        public const int RES_Y = 768;
        public const int MAX_LEVEL = 20;
        public const int CAP_BULLETS = 20;
        public const int CAP_NPCS = 10;
        public const int CAP_MODS = 20;

        //Screen return commands
        public const int CMD_EXIT = -1;
        public const int CMD_NONE = 0;
        public const int CMD_LOAD1 = 1;
        public const int CMD_LOAD2 = 2;
        public const int CMD_LOAD3 = 3;
        public const int CMD_NEW = 4;
        public const int CMD_PAUSE = 5;
        public const int CMD_JOURNAL = 6;
        public const int CMD_MOD = 7;

        //Mod types
        public const int MOD_NIL = -1;
        public const int MOD_ELM = 0;
        public const int MOD_TYP = 1;
        public const int MOD_STR = 2;
        public const int MOD_SPD = 3;
        public const int MOD_RCG = 4;
        public const int MOD_ACP = 5;

        //Map sizes
        public const int MAP_SIZE = 15;
    }
}
