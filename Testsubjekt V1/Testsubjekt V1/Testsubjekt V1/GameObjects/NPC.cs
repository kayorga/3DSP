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
        public bool newTarget;
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
            newTarget = false;
            XP = 0;
            cooldown = 0;
            maxCooldn = 0;
            kind = 0;
            pathFinder = null;
            target = Vector3.Zero;
        }

        /// <summary>
        /// assigns NPC attributes and ModelObject and activates the NPC
        /// </summary>
        /// <param name="k">NPC kind/species</param>
        /// <param name="m">ModelObject</param>
        /// <param name="p">starting position</param>
        /// <param name="d">starting direction</param>
        /// <param name="l">level</param>
        /// <param name="pl">Player</param>
        /// <param name="npcs">parent NPCCollection (should always be "this")</param>
        public void setup(byte k, ModelObject m, Vector3 p, Vector3 d, int l, Player pl, NPCCollection npcs)
        {
            kind = k;
            model = m;
            position = p;
            direction = d;
            level = l;

            switch (kind)
            {
                case 0:
                    speed = 0.025f;
                    maxHealth = 50 * ((int)Math.Pow(1.06f, level));
                    XP = 5;
                    maxCooldn = 60;
                    break;
                case 1:
                    speed = 0.1f;
                    maxHealth = 50 * ((int)Math.Pow(1.05f, level));
                    XP = 6;
                    maxCooldn = 40;
                    break;
                case 2:
                    speed = 0.09f;
                    maxHealth = 50 * ((int)Math.Pow(1.07f, level));
                    XP = 7;
                    maxCooldn = 75;
                    break;
                case 3:
                    speed = 0.01f;
                    maxHealth = 200 * ((int)Math.Pow(1.07f, level));
                    XP = 100;
                    maxCooldn = 50;
                    model.Scaling = new Vector3(3,3,3);
                    break;
                default:
                    speed = 0.001f;
                    maxHealth = 1;
                    XP = 0;
                    maxCooldn = 1000;
                    break;
            }

            health = maxHealth;
            active = true;
            model.Position = this.position;
            moving = false;
            newTarget = false;
            //pathFinder = new AStar(world, pl, new Point((int)Math.Round((-1 * position.X + world.size - 1) * 3.0f / 2.0f), (int)Math.Round((-1 * position.Z + world.size - 1) * 3.0f / 2.0f)), npcs);
            pathFinder = npcs.PathFinder;
        }

        public /*override*/ bool update(BulletCollection bullets, Camera camera, Player p, Mission m)
        {
            //TODO
            if (health <= 0)
            {
                int exp = (int)(XP * (float)level / (float)p.level);
                exp = Math.Min(exp,(int) (XP * 1.5f));
                exp = Math.Max(exp, 1);
                p.getEXP(exp);
                m.update(kind, exp);
                active = false;
                return false;
            }

            float dist = (p.position - position).Length();

            if (dist <= world.shootDistance && cooldown == 0)
            {
                cooldown = maxCooldn;
                Vector3 dir = (p.position - position);
                dir.Normalize();
                bullets.generate(false, position + dir, dir, 1, world.shootDistance * 2, 1);
            }

            if (cooldown > 0) cooldown--;

            if (moving)
                move();
            else
            {
                if (dist < 4)
                {
                    target = position;
                    return true;
                }
                //pathFinder.setup(new Point((int)Math.Round((-1 * position.X + world.size - 1) * 3.0f / 2.0f), (int)Math.Round((-1 * position.Z + world.size - 1) * 3.0f / 2.0f)), p);
                pathFinder.setup(new Point((int)Math.Round((-1 * position.X + world.size - 1)), (int)Math.Round((-1 * position.Z + world.size - 1))), p);
                target = pathFinder.findPath();
                newTarget = true;
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

            int factor = (int)Math.Ceiling(speed / .025f);

            float remainingSpeed = speed;
            for (int i = 0; i < factor; i++)
            {
                float spd = speed / (float)factor;
                spd = Math.Min(spd, remainingSpeed);
                bool u = moveCycle(factor, spd);
                remainingSpeed -= spd;
                if (!u || remainingSpeed == 0) break;
            }

            
            //Console.WriteLine("post move : " + position.X + " ; " + position.Z);
        }

        private bool moveCycle(int factor, float spd)
        {
            this.position += spd * direction;
            model.Position = this.position;

            //if (float.IsNaN(position.X)) Console.WriteLine("X is NaN cause direction is");

            if ((this.position - target).Length() < 0.02f)
            {
                this.position = target;
                //if (float.IsNaN(position.X)) Console.WriteLine("X is NaN cause target is");
                moving = false;
                return false;
                //Console.WriteLine("done: " + position.X + "/" + position.Z);
            }
            return true;
        }

        public void getHit(Bullet b, Mission m)
        {
            //TODO/////
            int dmg = 100;
            ///////////

            health = Math.Max(health - dmg, 0);
            m.dmgOut += dmg;
        }
        
        public void getHit(Player p)
        {
            //TODO/////
            int dmg = p.lv;
            ///////////

            health = Math.Max(health - dmg, 0);
        }

        public String getLabel()
        {
            switch (kind)
            {
                default: return "Enemies";
            }
        }

        public override void draw(Camera camera)
        {
            if (!active)
                return;
            base.draw(camera);
        }

        
    }
}
