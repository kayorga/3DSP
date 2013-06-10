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

        //Screen return commands
        public const int CMD_NONE = 0;
        public const int CMD_LOAD1 = 1;
        public const int CMD_LOAD2 = 2;
        public const int CMD_LOAD3 = 3;
        public const int CMD_EXIT = -1;

        //Map sizes
        public const int MAP_SIZE = 15;
    }
}
