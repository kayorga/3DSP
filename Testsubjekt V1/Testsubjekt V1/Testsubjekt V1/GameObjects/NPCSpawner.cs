using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestsubjektV1;
using Microsoft.Xna.Framework;

namespace Testsubjekt_V1
{
    class NPCSpawner
    {
        private int x;
        private int z;
        private bool active;
        private byte kind;
        private bool rate;
        private int cooldown;
        private int maxCooldown;

        public NPCSpawner()
        {
            x = 0;
            z = 0;
            active = false;
            kind = Constants.NPC_NONE;
            rate = Constants.SPAWN_INFINITE;
            cooldown = 0;
            maxCooldown = 150;
        }

        public void setup(byte kind, bool rate)
        {
            this.kind = kind;
            this.rate = rate;
        }

        public void setPos(int i, int j)
        {
            x = i;
            z = j;
            active = true;
        }

        public void update(NPCCollection npcs, Player p, Mission m)
        {
            if (!active) return;
            if (cooldown == 0)
            {
                Vector3 pos = new Vector3(Constants.MAP_SIZE - 2 * x - 1, 0, Constants.MAP_SIZE - 2 * z - 1);
                Vector3 dir = p.Position - pos;
                //Console.WriteLine("pre norm dist: " + dir.Length() + "x: " + dir.X + " y: " + dir.Y + " z: " + dir.Z);
                dir.Normalize();
                //Console.WriteLine("post norm dist: " + dir.Length() + "x: " + dir.X + " y: " + dir.Y + " z: " + dir.Z);
                npcs.generate(kind, pos, dir, p.lv);
                if (rate == Constants.SPAWN_ONCE)
                    active = false;
                else
                    cooldown = maxCooldown;



            }
            else cooldown = Math.Max(cooldown - 1, 0);
        }
    }
}
