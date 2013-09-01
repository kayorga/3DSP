using System;

namespace TestsubjektV1
{
    class Type1Mission : Mission
    {
        public Type1Mission()
        {
            //TODO
            kinds = new byte[4];
            label = "";
            target = 0;
            tarCount = 0;
            actCount = 0;
            level = 0;
            countKilledEnemies = 0;
            dmgOut = 0;
            dmgIn = 0;
            zone = 1;
            area = 0;
            startLv = 0;
            active = false;
            blocked = false;
        }

        public Type1Mission(byte lv, byte tKind, byte tCount, byte z, byte a, string[] nl, string[] zl, bool state, byte[] kinds)
        {
            //TODO
            if (state)
                setup(lv, tKind, tCount, z, a, kinds, nl, zl);
            else
                active = state;
            countKilledEnemies = 0;
        }

        public override bool isType1()
        {
            return true;
        }

        public override string getLabel()
        {
            if (blocked)
                return "Blocked!\n\nDo other Missions first!";
            else return label;
        }

        public override void reset(int l)
        {
            startLv = (byte)l;
            countXPGained = 0;
        }

        public override string getShortLabel(NPCCollection npcs)
        {
            string l = actCount + " / " + tarCount + " " + npcs.Labels[target] + " down";
            return l;
        }

        public override bool update(byte kind, int exp)
        {
            if (kind == target)
            {
                actCount = (byte)Math.Min(actCount + 1, tarCount);
            }
            countKilledEnemies++;
            countXPGained += exp;
            return true;
        }

        public override bool complete()
        {
            if (actCount >= tarCount)
            {
                active = false;
                return true;
            }
            return false;
        }

        public override void setup(byte lv, byte kind, byte count, byte z, byte a, byte[] kinds, string[] nl, string[] zl)
        {
            level = lv;
            target = kind;
            tarCount = count;
            actCount = 0;
            this.kinds = kinds;
            zone = z;
            area = a;
            makeLabel(nl, zl);

            kinds = new byte[4];
            kinds[0] = kind;
            Random ran = new Random();
            for (int i = 1; i < 4; i++)
                kinds[i] = (byte)ran.Next(3);

            if (kind == Constants.NPC_BOSS)
                area = 1;

            active = true;
        }

        private string makeLabel(string[] nl, string[] zl)
        {
            label = "Level " + level;
            label += "\n\nTarget: " + ((tarCount == 1) ? nl[target] : tarCount + " " + nl[target]);
            label += "\nZone: " + zl[zone-1] + " Area " + area;
            label += "\nEnemies: \n";

            for (int i = 0; i < kinds.Length; i++)
                label += "   " + nl[kinds[i]];

            return label;
        }

        public override void reward(Player player, ModCollection mods)
        {
            int exp = (int)(40 * (float)level / (float)player.level);
                exp = Math.Min(exp, 75);
                exp = Math.Max(exp, 1);

                if (player.lv != 50)
                {
                    player.getEXP(exp);
                    countXPGained += exp;
                }

                

            if (startLv < player.level)
            {
                mods.generate(Math.Min(player.level, level));
                rewardLabel = mods.lastMod;
            }
            else rewardLabel = "-";
        }

        public override void reset()
        {
            actCount = 0;
            countKilledEnemies = 0;
            dmgOut = 0;
            dmgIn = 0;
        }
    }
}
