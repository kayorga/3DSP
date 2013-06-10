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
            
        }

        public void update()
        {
            for (int i = 0; i < Constants.CAP_BULLETS; i++)
            {
                Bullet b = _content[i];
                if (b != null)

                    if (!b.update())
                        b = null;
            }  
        }

        public override void clear()
        {
            for (int i = 0; i < Constants.CAP_BULLETS; i++)
            {
                _content[i] = null;
            }
        }

        public override void generate()
        {
            for (int i = 0; i < Constants.CAP_BULLETS; i++)
            {
                Bullet b = _content[i];
                if (b == null)
                    b = new Bullet();
            }  
        }
    }
}
