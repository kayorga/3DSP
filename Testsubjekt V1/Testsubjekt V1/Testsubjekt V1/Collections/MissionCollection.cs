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
        private int level;

        public MissionCollection()
            : base(4)
        {
            //TODO
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
