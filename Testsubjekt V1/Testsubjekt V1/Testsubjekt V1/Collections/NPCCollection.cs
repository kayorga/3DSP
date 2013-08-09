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

        Player player;
        bool[][] moveData;
        public bool[][] npcMoveData { get { return moveData; } }
        World world;

        public NPCCollection(World w, ContentManager Content, Player pl)
            : base(Constants.CAP_NPCS)
        {
            //TODO
            world = w;
            for (int i = 0; i < Constants.CAP_NPCS; i++)
                _content.Add(new NPC(world));

            player = pl;
            moveData = new bool[world.size * 3][];
            for (int i = 0; i < world.size * 3; ++i)
            {
                moveData[i] = new bool[world.size * 3];
            }

            #region load models
            model0 = Content.Load<Model>("Models/T2");
            model1 = Content.Load<Model>("Models/stone");
            model2 = Content.Load<Model>("Models/tree1");
            #endregion

            //model0 = new ModelObject("cube");
        }

        private void clearMoveData()
        {
            foreach (bool[] b in moveData)
            {
                Array.Clear(b, 0, b.Length);
            }
        }

        public void update(BulletCollection bullets, Camera camera, Player p, Mission m)
        {
            //TODO

            clearMoveData();

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
                else
                {
                    float nx = n.position.X;
                    float nz = n.position.Z;
                    int X = (int)Math.Round((-1 * n.position.X + world.size - 1) * 3.0f / 2.0f);
                    int Z = (int)Math.Round((-1 * n.position.Z + world.size - 1) * 3.0f / 2.0f);
                    //Console.WriteLine("target: " + n.target.X + "," + n.target.Z + " nx: " + nx + " nz: " + nz +" X: " + X + " Z: " + Z);
                    //for (int i = -1; i < 2; ++i)
                    {
                    //    for (int j = -1; j < 2; ++j )
                            moveData[X][Z] = true;
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
                        case 0: n.setup(k, new ModelObject(model0), p, d, 0.025f, l, 10 + 2 * l, 5, 100, player, this); break;
                        case 1: n.setup(k, new ModelObject(model1), p, d, 0.04f, l, 8 + 2 * l, 6, 80, player, this); break;
                        case 2: n.setup(k, new ModelObject(model2), p, d, 0.01f, l, 12 + 4 * l, 8, 100, player, this); break;
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
