using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    struct SaveData
    {
        public int playerLevel;
        public int playerXP;
        public Weapon playerWeapon;
        public List<Mod> mods;
        public byte[] missionLevels;
        public byte[] missionTKinds;
        public byte[] missionTCounts;
        public byte[] missionZones;
        public byte[] missionAreas;
        public bool[] missionStates;
        //public byte[][] missionKinds;

        //byte lv, byte kind, byte count, byte z, byte a, string[] nl, string[] zl
    }
}
