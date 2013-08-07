using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TestsubjektV1
{
    abstract class Character
    {
        internal ModelObject model;
        internal Vector3 position;
        internal Vector3 direction;
        internal World world;
        internal float speed;
        internal int level;
        internal int maxHealth;
        internal int health;
        public int lv { get { return level; } }

        //public abstract bool update(BulletCollection bullets, Camera camera);

        public Vector3 Position
        {
            get { return position; }
        }

        public virtual void draw(Camera camera)
        {
            model.Draw(camera);
        }

        public virtual void moveAndCollide()
        {
            this.position += speed * direction;
            int x1 = (int)Math.Round(-1 * this.position.X + Constants.MAP_SIZE - 1) / 2;
            int x2 = (int)Math.Round(-1 * this.position.X + Constants.MAP_SIZE) / 2;
            int z1 = (int)Math.Round(-1 * this.position.Z + Constants.MAP_SIZE - 1) / 2;
            int z2 = (int)Math.Round(-1 * this.position.Z + Constants.MAP_SIZE) / 2;
            if ((world.MoveData[x1][z1] == 1) || (world.MoveData[x2][z2] == 1) || (world.MoveData[x2][z1] == 1) || (world.MoveData[x1][z2] == 1))
            {
                this.position = this.model.Position;
                //this.model.Position = this.position;
            }
            else model.Position = this.position;
        }
    }
}
