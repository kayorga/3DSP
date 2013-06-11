using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class NPC : Character
    {
        int cooldown;
        int maxCooldn;

        public NPC()
            : base()
        {
            //TODO
        }

        public override bool update(BulletCollection bullets, Camera camera)
        {
            //TODO
            return true;
        }
    }
}
