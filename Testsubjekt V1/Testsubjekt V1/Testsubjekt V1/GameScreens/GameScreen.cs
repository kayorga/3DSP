using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    abstract class GameScreen
    {
        public virtual int update()
        {
            return 0;
        }

        public abstract void draw();
    }
}
