﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void generate(bool fromP, Vector3 pos, Vector3 dir, float spd, float mdist, int str)
        {
            for (int i = 0; i < Constants.CAP_BULLETS; i++)
            {
                Bullet b = _content[i];
                if (!b.active)
                {
                    b.setup(fromP, pos, dir, spd, mdist, str);
                    break;
                }
            }  
        }
    }
}
