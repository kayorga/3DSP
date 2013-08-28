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

        Model[] models;

        Player player;
        byte[][] moveData;
        public byte[][] npcMoveData { get { return moveData; } }
        World world;
        AStar pathFinder;
        public AStar PathFinder { get { return pathFinder; } }

        static string[] labels = { "Ts", "Rocks", "Trees", "Cube"};
        public string[] Labels { get { return labels; } }

        private Mission mission;

        public NPCCollection(World w, ContentManager Content, Player pl)
            : base(Constants.CAP_NPCS)
        {
            //TODO
            world = w;
            for (int i = 0; i < Constants.CAP_NPCS; i++)
                _content.Add(new NPC(world));

            player = pl;
            moveData = new byte[world.size * 2][];
            for (int i = 0; i < world.size * 2; ++i)
            {
                moveData[i] = new byte[world.size * 2];
            }

            models = new Model[4];
            #region load models
            models[0] = Content.Load<Model>("Models/T");
            models[1] = Content.Load<Model>("Models/stone");
            models[2] = Content.Load<Model>("Models/tree1");
            models[3] = Content.Load<Model>("cube_rounded");
            #endregion

            //model0 = new ModelObject("cube");
            pathFinder = new AStar(world, pl, new Point(1,1), this);
        }

        private void clearMoveData()
        {
            foreach (byte[] b in moveData)
            {
                Array.Clear(b, 0, b.Length);
            }
        }

        public void update(BulletCollection bullets, Camera camera, Player p, Mission m)
        {
            //TODO

            clearMoveData();

            for (int i = 0; i < Constants.CAP_NPCS; ++i)
            {
                NPC n = _content[i];

                if (!n.active)
                    continue;

                bool k = n.update(bullets, camera, p, m);
                if (!k)
                {
                    //p.getEXP(n.XP);
                    //if (m != null && n.kind == m.target)
                    //{
                    //    m.actCount--;
                    //}
                }
                else
                {
                    float nx = n.position.X;
                    float nz = n.position.Z;
                        float tx = n.target.X;
                        float tz = n.target.Z;
                        int TX = (int)Math.Round((-1 * tx + world.size - 1));
                        int TZ = (int)Math.Round((-1 * tz + world.size - 1));
                        moveData[TX][TZ] = 255;

                    int X = (int)Math.Round((-1 * nx + world.size - 1));
                    int Z = (int)Math.Round((-1 * nz + world.size - 1));
                    //for (int l = -1; i < 2; ++i)
                    //{
                        //for (int j = -1; j < 2; ++j )
                            moveData[X][Z] = (byte) (i + 1);
                    //}
                }
            }
        }

        public override void clear()
        {
            foreach (NPC n in _content)
                n.active = false;
        }

        public void generate(byte k, Vector3 p, Vector3 d, int l)
        {
            //TODO
            //k = (byte)(new Random().Next(3));
            for (int i = 0; i < Constants.CAP_NPCS; i++)
            {
                NPC n = _content[i];
                if (!n.active)
                {
                    n.setup(k, new ModelObject(models[k]), p, d, l, player, this);
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
