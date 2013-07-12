using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestsubjektV1
{
    abstract class GameScreen
    {
        public virtual int update(GameTime gameTime)
        {
            return 0;
        }

        public abstract void draw();
    }
}
