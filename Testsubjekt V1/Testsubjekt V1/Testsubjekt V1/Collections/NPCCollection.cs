using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestsubjektV1
{
    class NPCCollection : Collection<NPC>
    {
        int level;

        ModelObject model0;
        ModelObject model1;
        ModelObject model2;

        public NPCCollection(World world)
            : base(Constants.CAP_NPCS)
        {
            //TODO
            for (int i = 0; i < Constants.CAP_NPCS; i++)
                _content.Add(new NPC(world));
        }

        public void update(BulletCollection bullets, Camera camera, Player p, Mission m)
        {
            //TODO
            //foreach (NPC n in _content)
            //{
            //    bool k = n.update(bullets, camera);
            //    if (!k)
            //    {
            //        p.getEXP(n.XP);
            //        if (n.kind == m.target)
            //        {
            //            m.actCount--;
            //        }
            //    }
            //}
        }

        public override void clear()
        {
            //TODO
        }

        public void generate(int k, Vector3 p, Vector3 d, int l)
        {
            //TODO
            for (int i = 0; i < Constants.CAP_NPCS; i++)
            {
                NPC n = _content[i];
                if (!n.active)
                {
                    switch (k)
                    {
                        case 0: n.setup(k, model0, p, d, 0.2f, l, 10 + 2 * l, 5, 100); break;
                        case 1: n.setup(k, model1, p, d, 0.4f, l, 8 + 2 * l, 6, 80); break;
                        case 2: n.setup(k, model2, p, d, 0.1f, l, 12 + 4 * l, 8, 100); break;
                    }
                }
            }
        }

        public void draw(Camera camera)
        {
            //TODO
        }
    }
}
