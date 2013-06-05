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
        protected ModelObject model;
        protected Vector3 position;
        protected Vector3 direction;
        protected float speed;
        protected int level;
        protected int maxHealth;
        protected int health;

        public abstract bool update(Camera camera);

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
