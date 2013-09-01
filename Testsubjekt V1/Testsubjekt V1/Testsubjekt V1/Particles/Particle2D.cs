using Microsoft.Xna.Framework;

namespace TestsubjektV1
{
    class Particle2D
    {
        public Point position;
        public Point size;
        public float totLifetime;
        public float remLifetime;

        public Particle2D(Point pos, Point siz, float lifetime)
        {
            this.position = pos;
            this.size = siz;
            this.remLifetime = lifetime;
            this.totLifetime = lifetime;
        }
    }
}
