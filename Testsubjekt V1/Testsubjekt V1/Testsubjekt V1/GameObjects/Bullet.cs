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

        private ParticleEffect particleEffect;
        //BillboardEngine billboardEngine;
        GraphicsDevice gd;

        public Vector3 Direction { get { return direction; } }

        public Bullet(ContentManager Content, GraphicsDevice graphicsDevice)
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

            Texture2D texture = Content.Load<Texture2D>("Particles/fire");
            //billboardEngine = new BillboardEngine(5, graphicsDevice);
            //billboardEngine.Effect.Texture = texture;
            particleEffect = new ParticleEffect(10000, graphicsDevice, texture, Color.Gold, Color.Crimson, 0.2f);
            particleEffect.VelocityScaling = 8.0f;
            gd = graphicsDevice;
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

        public void update(GameTime gameTime, Camera camera, World world, NPCCollection npcs, Player p, Mission m)
        {
            if (!active) return;

            int factor = (int) Math.Ceiling(speed);

            float remainingSpeed = speed;
            float spd = speed / (float)factor;

            for (int i = 0; i < factor; i++)
            {
                spd = Math.Min(spd, remainingSpeed);
                bool u = updateCycle(gameTime, camera, world, npcs, p, m, factor, spd);
                remainingSpeed -= spd;
                if (!u || remainingSpeed == 0) break;
            }

                return;
            //false wenn getroffen oder distanz überschritten
        }

        private bool updateCycle(GameTime gameTime, Camera camera, World world, NPCCollection npcs, Player p, Mission m, int factor, float spd)
        {
            position += spd * direction;
            distance += spd;

            bulletOb.Position = position;

            particleEffect.Update((float)gameTime.ElapsedGameTime.TotalSeconds, camera.ViewMatrix, position, direction);

            xTile = (byte)Math.Round(-1 * (position.X) + (Constants.MAP_SIZE - 1));
            zTile = (byte)Math.Round(-1 * (position.Z) + (Constants.MAP_SIZE - 1));

            if (collision(world) || collision(npcs, m) || collision(p, m) || distance > maxDist)
            {
                active = false;
                particleEffect.Clear();
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
            if (tile != 0 && tile != 255 && fromPlayer)
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
            if (fromPlayer) particleEffect.Draw(camera);
            else bulletOb.Draw(camera);
        }
    }
}
