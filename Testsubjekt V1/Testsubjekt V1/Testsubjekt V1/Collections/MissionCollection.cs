using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class MissionCollection : Collection<Mission>
    {
        public Mission activeMission;
        public Mission mainMission;
        private byte level;
        public bool isMissionActive;
        private World world;
        private NPCCollection npcs;

        public MissionCollection(World w, NPCCollection n)
            : base(4)
        {
            world = w;
            npcs = n;

            level = 1;
            Random ran = new Random();
            for (int i = 0; i < 3; ++i)
                _content.Add(new Type1Mission());

            _content.Add(new Type1Mission());

            activeMission = this[0];
            mainMission = this[3];
        }

        public void generate(byte lv)
        {
            level = lv;//(byte)new Random().Next(100);

            Random ran = new Random();
            for (int i = 0; i < 3; ++i)
            {
                Mission m = this[i];
                if (!m.active)
                {
                    byte kind = (byte)ran.Next(3);
                    byte count = (byte)Math.Min((int)Constants.CAP_MISSION_NPCS, 2 + ran.Next((int)1.5 * level));

                    byte area = (byte)(ran.Next(world.Maps.Count-1) + 1);
                    
                    m.setup(level, kind, count, (byte) (i + 1), area, npcs.Labels, world.Labels);
                }
            }

            if (!this[3].active)
            {
                byte kind = 3;
                byte zone = (byte)(ran.Next(3) + 1);
                byte area = (byte)(ran.Next(world.Maps.Count-1) + 1);
                this[3].setup((byte)(this[3].level+12), kind, 1, zone, area, npcs.Labels, world.Labels);
            }
        }

        public void update()
        {
            byte[] lvs = new byte[] { this[0].level, this[1].level, this[2].level };
            byte minLv = lvs.Min<byte>();

            for (int i = 0; i < 3; i++)
            {
                if (this[i].level > minLv + 5 || this[i].level > this[3].level)
                    this[i].blocked = true;
                else this[i].blocked = false;
            }
        }

        public override void clear()
        {
            //TODO
            for (int i = 0; i < 4; i++)
                this[i].active = false;
        }
    }
}
