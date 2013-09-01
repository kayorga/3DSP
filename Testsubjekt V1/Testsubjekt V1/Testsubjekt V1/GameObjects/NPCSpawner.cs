using System;
using Microsoft.Xna.Framework;

namespace TestsubjektV1
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
            maxCooldown = 300;
        }

        public void setup(byte kind, bool rate)
        {
            this.kind = kind;
            this.rate = rate;
            cooldown = 0;
        }

        public void setPos(int i, int j)
        {
            x = i;
            z = j;
            active = true;
        }

        public void update(NPCCollection npcs, Player p, Mission m, bool isMainMission = false)
        {
            if (!active) return;
            if (cooldown == 0)
            {
                Vector3 pos = new Vector3(Constants.MAP_SIZE - 2 * x - 1, 0, Constants.MAP_SIZE - 2 * z - 1);
                Vector3 dir = p.Position - pos;

                dir.Normalize();

                if (isMainMission && kind != Constants.NPC_BOSS)
                    npcs.generate(kind, pos, dir, m.level - 10);
                else npcs.generate(kind, pos, dir, m.level);

                if (rate == Constants.SPAWN_ONCE)
                    active = false;
                else
                    cooldown = new Random().Next((int)(maxCooldown * 0.8f), (int)(maxCooldown * 1.2f));
            }
            else cooldown = Math.Max(cooldown - 1, 0);
        }
    }
}
