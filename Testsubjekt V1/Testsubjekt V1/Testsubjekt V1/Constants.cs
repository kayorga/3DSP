using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    static class Constants
    {
        public const bool DEBUG = true;

        public const int RES_X = 1024;
        public const int RES_Y = 768;
        public const byte MAX_LEVEL = 20;
        public const byte CAP_BULLETS = 50;
        public const byte CAP_NPCS = 8; // must be less than 254!
        public const byte CAP_MODS = 20;
        public const byte CAP_MISSION_NPCS = 15;

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
        public const int CMD_MISSIONCOMPLETE = 8;
        public const int CMD_BACK = 9;
        public const int CMD_MISSIONINFO = 10;

        //NPC and Bullet elements
        public const byte ELM_NIL = 0;
        public const byte ELM_PLA = 1;
        public const byte ELM_HEA = 2;
        public const byte ELM_ICE = 3;

        //Bullet types
        public const byte TYP_NIL = 0;
        public const byte TYP_TRI = 1;
        public const byte TYP_BLA = 2;
        public const byte TYP_WAV = 3;

        //Mod types
        public const byte MOD_NIL = 0;
        public const byte MOD_ELM = 1;
        public const byte MOD_TYP = 2;
        public const byte MOD_STR = 3;
        public const byte MOD_SPD = 4;
        public const byte MOD_RCG = 5;
        public const byte MOD_ACP = 6;

        //NPC Spawn rates
        public const bool SPAWN_INFINITE = true;
        public const bool SPAWN_ONCE = false;

        //NPC kinds
        public const byte NPC_NONE = 0;
        public const byte NPC_PLAS = 1;
        public const byte NPC_HEAT = 2;
        public const byte NPC_ICE = 3;
        public const byte NPC_BOSS = 4;

        public const byte PLAYER_INVINCIBILITY = 45;

        //Map sizes
        public const byte MAP_SIZE = 25;
    }
}
