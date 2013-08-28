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

        byte xTile;
        byte zTile;

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

        public void update(World world, NPCCollection npcs, Player p, Mission m)
        {
            if (!active) return;

            int factor = (int) Math.Ceiling(speed);

            float remainingSpeed = speed;
            float spd = speed / (float)factor;

            for (int i = 0; i < factor; i++)
            {
                spd = Math.Min(spd, remainingSpeed);
                bool u = updateCycle(world, npcs, p, m, factor, spd);
                remainingSpeed -= spd;
                if (!u || remainingSpeed == 0) break;
            }

                return;
            //false wenn getroffen oder distanz überschritten
        }

        private bool updateCycle(World world, NPCCollection npcs, Player p, Mission m, int factor, float spd)
        {
            position += spd * direction;
            distance += spd;

            bulletOb.Position = position;

            xTile = (byte)Math.Round(-1 * (position.X) + (Constants.MAP_SIZE - 1));
            zTile = (byte)Math.Round(-1 * (position.Z) + (Constants.MAP_SIZE - 1));

            if (collision(world) || collision(npcs, m) || collision(p, m) || distance > maxDist)
            {
                active = false;
                return false;
            }
            return true;
        }

        private bool collision(World world) 
        {
            if (world.MoveData[xTile / 2][zTile / 2] == 1)
                return true;
            return false;
        }

        private bool collision(NPCCollection npcs, Mission m)
        {
            byte tile = npcs.npcMoveData[xTile][zTile];
            if (tile != 0 && tile != 255)
            {
                npcs[tile - 1].getHit(this, m);
                return true;
            }
            return false;
        }

        private bool collision(Player p, Mission m)
        {
            if (!fromPlayer && xTile == p.xTile && zTile == p.zTile)
            {
                p.getHit(this, m);
                return true;
            }
            return false;
        }

        public void draw(Camera camera)
        {
            if (!active) return;
            bulletOb.Draw(camera);
        }
    }
}
