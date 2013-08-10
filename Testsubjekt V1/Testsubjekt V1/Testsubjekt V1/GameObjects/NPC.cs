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
        public byte kind;
        public bool active;
        public int XP;
        private AStar pathFinder;
        private bool moving;
        public bool isMoving { get { return moving; } }
        public Vector3 target;

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
            pathFinder = null;
            target = Vector3.Zero;
        }

        public void setup(byte k, ModelObject m, Vector3 p, Vector3 d, float s, int l, int mh, int x, int mc, Player pl, NPCCollection npcs)
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
            moving = false;
            //pathFinder = new AStar(world, pl, new Point((int)Math.Round((-1 * position.X + world.size - 1) * 3.0f / 2.0f), (int)Math.Round((-1 * position.Z + world.size - 1) * 3.0f / 2.0f)), npcs);
            pathFinder = npcs.PathFinder;
        }

        public /*override*/ bool update(BulletCollection bullets, Camera camera, Player p, Mission m)
        {
            //TODO
            if (health <= 0)
            {
                p.getEXP((int)(XP * (float)level/(float)p.level));
                m.update(kind);
                active = false;
                return false;
            }

            if (moving)
                move();
            else
            {
                if ((p.position - position).Length() < 4) return true;
                //pathFinder.setup(new Point((int)Math.Round((-1 * position.X + world.size - 1) * 3.0f / 2.0f), (int)Math.Round((-1 * position.Z + world.size - 1) * 3.0f / 2.0f)), p);
                pathFinder.setup(new Point((int)Math.Round((-1 * position.X + world.size - 1)), (int)Math.Round((-1 * position.Z + world.size - 1))), p);
                target = pathFinder.findPath();
                direction = target - position;
                if (direction.Length() != 0) direction.Normalize();
                moving = true;
                move();
            }

            //double rotationAngle = Math.Acos((Vector3.Dot(direction, -1*Vector3.UnitX))/(direction.Length()));
            //model.Rotation = new Vector3(0, (float)rotationAngle, 0);

            //Console.WriteLine("act move dist: " + direction.Length() + "x: " + direction.X + " y: " + direction.Y + " z: " + direction.Z + " * " + speed);

            return true;
        }


        private void move()
        {
            //Console.WriteLine("pre move : " + position.X + " ; " + position.Z + " ; direction: " + direction.X + " ; " + direction.Z);
            this.position += speed * direction;
            model.Position = this.position;

            if (float.IsNaN(position.X)) Console.WriteLine("X is NaN cause direction is");

            if ((this.position - target).Length() < 0.02f)
            {
                this.position = target;
                if (float.IsNaN(position.X)) Console.WriteLine("X is NaN cause target is");
                moving = false;
                //Console.WriteLine("done: " + position.X + "/" + position.Z);
            }
            //Console.WriteLine("post move : " + position.X + " ; " + position.Z);
        }

        public void getHit(Bullet b)
        {
            //TODO
            health = Math.Max(health - 100, 0);
        }

        public override void draw(Camera camera)
        {
            if (!active)
                return;
            base.draw(camera);
        }
    }
}
