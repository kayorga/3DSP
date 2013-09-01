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
        Model[] models;

        Player player;
        byte[][] moveData;
        public byte[][] npcMoveData { get { return moveData; } }
        World world;
        AStar pathFinder;
        public AStar PathFinder { get { return pathFinder; } }

        static string[] labels = { "NIL", "PLA", "HEA", "ICE", "Boss"};
        public string[] Labels { get { return labels; } }

        private BillboardEngine billboardEngine;

        private GraphicsDevice graphicsDevice;

        private Queue<DmgNumber> queue;

        public NPCCollection(World w, ContentManager Content, Player pl, GraphicsDevice graphicsDevice, AudioManager audio)
            : base(Constants.CAP_NPCS)
        {
            //TODO
            world = w;
            
            Texture2D hitTexture = Content.Load<Texture2D>("bam");

            billboardEngine = new BillboardEngine(5, graphicsDevice);
            billboardEngine.Effect.Texture = hitTexture;

            this.graphicsDevice = graphicsDevice;
            for (int i = 0; i < Constants.CAP_NPCS; i++)
                _content.Add(new NPC(Content, graphicsDevice, world, billboardEngine, audio));

            player = pl;
            moveData = new byte[world.size * 2][];
            for (int i = 0; i < world.size * 2; ++i)
            {
                moveData[i] = new byte[world.size * 2];
            }

            models = new Model[5];
            #region load models
            models[0] = Content.Load<Model>("Models/enemy4");
            models[1] = Content.Load<Model>("Models/enemy3");
            models[2] = Content.Load<Model>("Models/enemy1");
            models[3] = Content.Load<Model>("Models/enemy2");
            models[4] = Content.Load<Model>("Models/boss");
            #endregion

            queue = new Queue<DmgNumber>();

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

        public void update(GameTime gameTime, BulletCollection bullets, Camera camera, Player p, Mission m)
        {
            //TODO

            clearMoveData();

            //Initialize BillboardEngine
            billboardEngine.Begin(camera.ViewMatrix);
            billboardEngine.AddBillboard(new Vector3(0.0f, 1.0f, 0.0f), Color.Transparent, 0.01f);

            for (int i = 0; i < Constants.CAP_NPCS; ++i)
            {
                NPC n = _content[i];

                if (!n.active)
                    continue;

                bool k = n.update(gameTime, bullets, camera, p, m);
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
            queue.Clear();
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

        public void draw(Camera camera, SpriteBatch spriteBatch, SpriteFont font)
        {
            //TODO

            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            graphicsDevice.BlendState = BlendState.Additive;
            billboardEngine.Draw(graphicsDevice, camera);
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            
            foreach (NPC npc in _content)
                npc.draw(camera, queue);

            while (queue.Count > 0)
            {
                DmgNumber num = queue.Dequeue();
                Color color = (num.crit) ? Color.Gold : Color.White;

                spriteBatch.Begin();
                spriteBatch.DrawString(font, num.dmg.ToString(), num.position, color);
                spriteBatch.End();
                
            }

            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        }

        public void draw(Camera camera)
        {
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            graphicsDevice.BlendState = BlendState.Additive;
            billboardEngine.Draw(graphicsDevice, camera);
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (NPC npc in _content)
                npc.draw(camera);
        }

        public void sortNpcsUpward()
        {
            _content.Sort(NPC.compareUpward);
        }

        public void sortNpcsDownward()
        {
            _content.Sort(NPC.compareDownward);
        }
    }
}
