using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class Type1Mission : Mission
    {
        public Type1Mission()
        {
            //TODO
            kinds = null;
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
            active = false;
            blocked = false;
        }

        public Type1Mission(byte lv, byte tKind, byte tCount, byte z, byte a, string[] nl, string[] zl, bool state)
        {
            //TODO
            kinds = null;
            if (state)
                setup(lv, tKind, tCount, z, a, nl, zl);
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

        public override string getShortLabel()
        {
            string l = actCount + " / " + tarCount + " Type" + target + " Enemies down";
            return l;
        }

        public override bool update(byte kind, int exp)
        {
            //TODO
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

        public override void setup(byte lv, byte kind, byte count, byte z, byte a, string[] nl, string[] zl)
        {
            level = lv;
            target = kind;
            tarCount = count;
            actCount = 0;
            zone = z;
            area = a;
            makeLabel(nl, zl);

            kinds = new byte[4];
            kinds[0] = kind;
            Random ran = new Random();
            for (int i = 1; i < 4; i++)
                kinds[i] = (byte)ran.Next(3);

            active = true;
        }

        private string makeLabel(string[] nl, string[] zl)
        {
            label = "Level " + level;
            label += "\n\nTarget: " + ((tarCount == 1) ? nl[target] : tarCount + " " + nl[target]);
            label += "\nZone: " + zl[zone-1] + " Area " + area;

            return label;
        }

        public override void reward(Player player, ModCollection mods)
        {
            int exp = (int)(25 * (float)level / (float)player.level);
                exp = Math.Min(exp, 40);
                exp = Math.Max(exp, 1);
            player.getEXP(exp);
            mods.generate(Math.Min(player.level, level));
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
