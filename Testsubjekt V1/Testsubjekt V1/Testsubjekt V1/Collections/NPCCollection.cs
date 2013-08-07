using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TestsubjektV1
{
    class NPCCollection : Collection<NPC>
    {
        int level;

        Model model0;
        Model model1;
        Model model2;

        public NPCCollection(World world, ContentManager Content)
            : base(Constants.CAP_NPCS)
        {
            //TODO
            for (int i = 0; i < Constants.CAP_NPCS; i++)
                _content.Add(new NPC(world));

            #region load models
            model0 = Content.Load<Model>("Models/T2");
            model1 = Content.Load<Model>("Models/stone");
            model2 = Content.Load<Model>("Models/tree1");
            #endregion

            //model0 = new ModelObject("cube");
        }

        public void update(BulletCollection bullets, Camera camera, Player p, Mission m)
        {
            //TODO
            foreach (NPC n in _content)
            {
                bool k = n.update(bullets, camera, p);
                if (!k)
                {
                    p.getEXP(n.XP);
                    if (m != null && n.kind == m.target)
                    {
                        m.actCount--;
                    }
                }
            }
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
                        case 0: n.setup(k, new ModelObject(model0), p, d, 0.02f, l, 10 + 2 * l, 5, 100); break;
                        case 1: n.setup(k, new ModelObject(model1), p, d, 0.04f, l, 8 + 2 * l, 6, 80); break;
                        case 2: n.setup(k, new ModelObject(model2), p, d, 0.01f, l, 12 + 4 * l, 8, 100); break;
                    }
                    break;
                }
            }
        }

        public void draw(Camera camera)
        {
            //TODO
            foreach (NPC npc in _content)
                npc.draw(camera);
        }
    }
}
