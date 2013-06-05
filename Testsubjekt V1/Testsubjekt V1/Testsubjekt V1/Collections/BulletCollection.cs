using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class BulletCollection : Collection<Bullet>
    {
        public BulletCollection(NPCCollection npcs, Player player, World world)
            : base(Constants.CAP_BULLETS)
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
