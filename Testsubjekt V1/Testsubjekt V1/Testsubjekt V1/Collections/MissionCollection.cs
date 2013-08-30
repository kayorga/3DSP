﻿using System;
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
        private World world;
        private NPCCollection npcs;

        public MissionCollection(World w, NPCCollection n)
            : base(4)
        {
            world = w;
            npcs = n;

            level = 1;
            Random ran = new Random();
            for (int i = 0; i < 4; ++i)
                _content.Add(new Type1Mission(n));

            activeMission = this[0];
            mainMission = this[3];
        }

        //generate new missions to replace completed/inactive ones
        public void generate(byte lv)
        {
            level = lv;

            Random ran = new Random();

            //replace all inactive regular missions by new ones
            for (int i = 0; i < 3; ++i)
            {
                Mission m = this[i];
                if (!m.active)
                {
                    byte mislv = (byte)((ran.Next(31) - 10) * level * 0.01f);
                    mislv = (byte)Math.Min(Math.Max(mislv + level, 1), 50);
                    byte kind = (byte)ran.Next(4);
                    byte count = (byte)Math.Min((int)Constants.CAP_MISSION_NPCS, 2 + ran.Next((int)1.5 * mislv));

                    byte area = (byte)(ran.Next(world.Maps.Count-1) + 1);

                    m.setup(mislv, kind, count, (byte) (i + 1), area, npcs.Labels, world.Labels);
                }
            }

            //generate new boss mission
            if (!this[3].active)
            {
                byte kind = Constants.NPC_BOSS;
                byte zone = (byte)(ran.Next(3) + 1);
                byte area = (byte)(ran.Next(world.Maps.Count-1) + 1);
                this[3].setup((byte)(this[3].level+12), kind, 1, zone, area, npcs.Labels, world.Labels);
            }
        }


        /// <summary>
        /// blocks Missions with a too high level or unblocks them
        /// </summary>
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

        /// <summary>
        /// deactivates all contents so that new Missions can be generated
        /// </summary>
        public override void clear()
        {
            for (int i = 0; i < 4; i++)
                this[i].active = false;
        }
    }
}
