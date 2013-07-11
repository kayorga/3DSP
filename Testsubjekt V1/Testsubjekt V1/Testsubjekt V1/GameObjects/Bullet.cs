using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestsubjektV1
{
    class Bullet
    {
        public bool active;
        public bool fromPlayer;
        Vector3 position;
        Vector3 direction;
        float speed;
        float distance;
        float maxDist;
        ModelObject bulletOb;
        World world;
        int strength;

        public Bullet(ContentManager Content)
        {
            active = false;
            fromPlayer = false;
            position = Vector3.Zero;
            direction = Vector3.Zero;
            speed = 0;
            distance = 0;
            maxDist = 0;
            strength = 1;

            bulletOb = new ModelObject(Content.Load<Model>("cube_rounded"));
        }

        public void setup(bool fromP, Vector3 pos, Vector3 dir, float spd, float mdist, int str)
        {
            active = true;
            fromPlayer = fromP;
            position = pos;
            direction = dir;
            speed = spd*.5f;
            distance = 0;
            maxDist = mdist;
            strength = str;
        }

        public void update(World world)
        {
            if (!active) return;
            position += speed * direction;
            distance += speed;

            bulletOb.Position = position;

            if (collision(world) || distance > maxDist)
            {
                active = false;
                return;
            }

            return;
            //false wenn getroffen oder distanz überschritten
        }

        private bool collision(World world) 
        {
            int xtile = (int)Math.Round(-1*(position.X) + (Constants.MAP_SIZE - 1)) / 2;
            int ztile = (int)Math.Round(-1*(position.Z) + (Constants.MAP_SIZE - 1)) / 2;
            if (world.MoveData[xtile][ztile] == 1)
                return true;
            return false;
        }

        public void draw(Camera camera)
        {
            if (!active) return;
            bulletOb.Draw(camera);
        }
    }
}
