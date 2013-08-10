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

        public MissionCollection()
            : base(4)
        {
            //TODO
            level = 10;
            Random ran = new Random();
            for (int i = 0; i < 3; ++i)
            {
                
                byte kind = (byte)ran.Next(3);
                byte count = (byte)Math.Min((int)Constants.CAP_MISSION_NPCS, 2 + ran.Next((int)1.5 * level));
                _content.Add(new Type1Mission(level, kind, count, new byte[1]));
            }

            _content.Add(new Type1Mission(8, 3, 1, new byte[1]));

            activeMission = this[0];
            mainMission = this[3];
        }

        public void generate(byte lv)
        {
            level = (byte)new Random().Next(100);//(byte)Math.Min(lv, (byte)200);

            Random ran = new Random();
            for (int i = 0; i < 3; ++i)
            {
                Mission m = this[i];
                //if (m.complete())
                //{
                    byte kind = (byte)ran.Next(3);
                    byte count = (byte)Math.Min((int)Constants.CAP_MISSION_NPCS, 2 + ran.Next((int)1.5 * level));
                    m.setup(level, kind, count);
                //}
            }

            if (this[3].complete())
            {
                this[3] = new Type1Mission((byte)(level + 8), 3, 1, new byte[1]);
            }
        }

        public void update(int level)
        {
            //TODO
        }

        public override void clear()
        {
            //TODO
        }

        public void generate()
        {
            //TODO
        }
    }
}
