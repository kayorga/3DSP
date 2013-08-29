using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestsubjektV1
{
    struct Particle
    {
        public Vector3 position;
        public Vector3 velocity;

        public Color startColor;
        public Color endColor;

        public float totLifetime;
        public float remLifetime;

        public float startsize;
        public float endsize;

    }
}
