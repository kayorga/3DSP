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

    }
}
