using System.Collections.Generic;
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

        internal ModelObject ModelObject
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public virtual void draw(Camera camera)
        {
            model.Draw(camera);
        }

        public virtual void draw(Camera camera, Queue<DmgNumber> queue)
        {
            model.Draw(camera);
        }
    }
}
