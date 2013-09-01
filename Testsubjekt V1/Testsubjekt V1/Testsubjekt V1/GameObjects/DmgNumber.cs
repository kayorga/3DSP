using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestsubjektV1
{
    class DmgNumber
    {
        public readonly int dmg;
        public readonly bool crit;
        public Vector2 position;
        private byte lifetime;
        private sbyte dx;
        private sbyte dy;

        public DmgNumber(int dmg, bool crit, sbyte dx, sbyte dy)
        {
            this.dmg = dmg;
            this.crit = crit;
            this.position = Vector2.Zero;
            this.dx = dx;
            this.dy = dy;
            lifetime = 30;
        }

        public void setPos(float x, float y)
        {
            position.X = x + dx;
            position.Y = y + dy;
        }

        public bool update()
        {
            lifetime--;
            if (lifetime > 0) return true;
            else return false;
        }
    }
}
