using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class NPCCollection : Collection<NPC>
    {
        int level;
        public NPCCollection()
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

        public void generate()
        {
            //TODO
        }

        public void draw(Camera camera)
        {
            //TODO
        }
    }
}
