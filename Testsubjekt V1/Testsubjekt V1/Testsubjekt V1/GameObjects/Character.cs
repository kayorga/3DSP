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
        Model model;
        Vector3 position;
        Vector3 direction;
        float speed;
        int level;
        int maxHealth;
        int health;

        public abstract bool update();

        public virtual void draw()
        {
            //TODO
        }
    }
}
