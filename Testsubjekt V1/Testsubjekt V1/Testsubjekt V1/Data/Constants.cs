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
        public const int CMD_INTRO = 11;
        public const int CMD_DEX = 12;
        public const int CMD_CREDITS = 13;
        public const int CMD_TITLE = 14;
        public const int CMD_GAMEOVER = 15;
        public const int CMD_CHARINFO = 16;
        public const int CMD_HELP = 17;

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
        public const byte MOD_NIL = 255;
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

        public const byte PLAYER_NPC_INVINCIBILITY = 45;
        public const byte PLAYER_BULLET_INVINCIBILITY = 15;
        public const byte PLAYER_HIT_DELAY = 45;

        //Map sizes
        public const byte MAP_SIZE = 25;
    }
}
