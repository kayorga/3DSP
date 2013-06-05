using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class NPCCollection : Collection<NPC>
    {
        int level;
        public NPCCollection(BulletCollection bullets, Mission activeMission, Player player, World world)
            : base(Constants.CAP_NPCS)
        {
            //TODO
        }

        public void update()
        {
            //TODO
        }

        public override void clear()
        {
            //TODO
        }

        public override void generate()
        {
            //TODO
        }
    }
}
