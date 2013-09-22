using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestsubjektV1
{
    class BulletCollection : Collection<Bullet>
    {
        


        public BulletCollection(ContentManager Content, GraphicsDevice graphicsDevice)
            : base(Constants.CAP_BULLETS)
        {
            for (int i = 0; i < Constants.CAP_BULLETS; ++i)
                _content.Add(new Bullet(Content, graphicsDevice));
        }

        internal Bullet Bullet
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void update(GameTime gameTime, Camera camera, World world, NPCCollection npcs, Player p, Mission m)
        {
            for (int i = 0; i < Constants.CAP_BULLETS; i++)
                _content[i].update(gameTime, camera, world, npcs, p, m);
        }

        public void draw(Camera camera)
        {
            foreach (Bullet b in _content)
                b.draw(camera);
        }

        public override void clear()
        {
            foreach (Bullet b in _content)
                b.active = false;
        }

        public void generate(bool fromP, Vector3 pos, Vector3 dir, float spd, float mdist, int str, byte element, byte type = Constants.TYP_NIL)
        {
            switch (type)
            {
                case Constants.TYP_NIL: setupNext(fromP, pos, dir, spd, mdist, str, element, type); break;
                case Constants.TYP_BLA: setupNext(fromP, pos, dir, 0.4f * spd, mdist, 2 * str, element, type); break;
                case Constants.TYP_WAV: 
                    setupNext(fromP, pos, dir, 1.4f * spd, mdist, (int) (0.7f * str), element, type);
                    setupNext(fromP, pos, dir, 1.4f * spd, mdist, (int) (0.7f * str), element, type, true); break;
                case Constants.TYP_TRI: 
                    setupNext(fromP, pos, dir, spd, mdist, (int) (0.4f * str), element, type);
                    setupNext(fromP, pos, Vector3.Transform(dir, Matrix.CreateFromAxisAngle(new Vector3(0,1,0), (float) Math.PI / 36)), spd, mdist, (int) (0.4f * str), element, type);
                    setupNext(fromP, pos, Vector3.Transform(dir, Matrix.CreateFromAxisAngle(new Vector3(0,1,0), (float) Math.PI * 71 / 36)), spd, mdist, (int) (0.4f * str), element, type); break;
            }
        }

        private void setupNext(bool fromP, Vector3 pos, Vector3 dir, float spd, float mdist, int str, byte element, byte type, bool mirror = false)
        {
            for (int i = 0; i < Constants.CAP_BULLETS; i++)
            {
                Bullet b = _content[i];
                if (!b.active)
                {
                    b.setup(fromP, pos, dir, spd, mdist, str, element, type, mirror);
                    break;
                }
            }
        }
    }
}
