using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class Bullet
    {
        public Bullet() { }

        internal bool update()
        {
            return true;
            //false wenn getroffen oder distanz überschritten
        }
    }
}
