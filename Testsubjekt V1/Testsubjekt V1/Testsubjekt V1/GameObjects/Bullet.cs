using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

        public Bullet()
        {
            active = false;
            fromPlayer = false;
            position = Vector3.Zero;
            direction = Vector3.Zero;
            speed = 0;
            distance = 0;
            maxDist = 0;
        }

        public void setup(bool fromP, Vector3 pos, Vector3 dir, float spd, float mdist)
        {
            active = true;
            fromPlayer = fromP;
            position = pos;
            direction = dir;
            speed = spd;
            distance = 0;
            maxDist = mdist;
        }

        public void update()
        {
            if (!active) return;
            position += speed * direction;
            distance += speed;

            if (collision() || distance > maxDist)
            {
                active = false;
                return;
            }

            return;
            //false wenn getroffen oder distanz überschritten
        }

        private bool collision() 
        {
            return false;
        }

        public void draw(Camera camera)
        {
            if (!active) return;
        }
    }
}
