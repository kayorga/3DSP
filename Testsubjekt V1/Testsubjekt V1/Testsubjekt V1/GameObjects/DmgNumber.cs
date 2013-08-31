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
        public readonly Vector2 position;

        public DmgNumber(int dmg, bool crit, Vector2 position)
        {
            this.dmg = dmg;
            this.crit = crit;
            this.position = position;
        }
    }
}
