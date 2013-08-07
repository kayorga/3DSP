using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestsubjektV1
{
    class NPC : Character
    {
        int cooldown;
        int maxCooldn;
        public int kind;
        public bool active;
        public int XP;

        public NPC(World world)
        {
            //TODO
            model = null;
            position = Vector3.Zero;
            direction = Vector3.Zero;
            this.world = world;
            speed = 0;
            level = 0;
            maxHealth = 0;
            health = 0;
            active = false;
            XP = 0;
            cooldown = 0;
            maxCooldn = 0;
            kind = 0;
        }

        public void setup(int k, ModelObject m, Vector3 p, Vector3 d, float s, int l, int mh, int x, int mc)
        {
            kind = k;
            model = m;
            position = p;
            direction = d;
            speed = s;
            level = l;
            maxHealth = mh;
            health = mh;
            XP = x;
            maxCooldn = mc;
            active = true;
            model.Position = this.position;
        }

        public /*override*/ bool update(BulletCollection bullets, Camera camera, Player p)
        {
            //TODO
            if (health <= 0)
            {
                active = false;
                return false;
            }

            direction = p.Position - position;

            moveAndCollide();
            //Console.WriteLine("act move dist: " + direction.Length() + "x: " + direction.X + " y: " + direction.Y + " z: " + direction.Z + " * " + speed);
            
            return true;
        }

        public override void draw(Camera camera)
        {
            if (!active) return;
            base.draw(camera);
        }
    }
}
