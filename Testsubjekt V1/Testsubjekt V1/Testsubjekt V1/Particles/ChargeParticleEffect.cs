using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestsubjektV1
{
    class ChargeParticleEffect
    {
        private Random random;
        private GraphicsDevice graphicsDevice;


        private BillboardEngine billboardEngine;

        private Particle[] particles;
        private int numParticles = 0;

        private Color startColor;
        private Color endColor;
        private float particleLifetime;

        private Vector3 previousPosition;

        public float SizeScaling { get; set; }

        public float VelocityScaling { get; set; }

        public void setColors(Color start, Color end)
        {
            startColor = start;
            endColor = end;
        }

        public BasicEffect Effect
        { get { return billboardEngine.Effect; } }

        public ChargeParticleEffect(int maxNumParticles, GraphicsDevice gd, Texture2D texture, Color startCol, Color endCol, float lifetime)
        {
            graphicsDevice = gd;
            random = new Random();
            billboardEngine = new BillboardEngine(maxNumParticles, graphicsDevice);
            particles = new Particle[maxNumParticles];
            Effect.Texture = texture;
            startColor = startCol;
            endColor = endCol;
            particleLifetime = lifetime;
            this.VelocityScaling = 1.5f;
            this.SizeScaling = 0.8f;
        }

        public void Clear()
        {
            numParticles = 0;
        }

        public void Update(float time, Matrix cameraMatrix, Vector3 position)
        {
            emitParticles(position);

            billboardEngine.Begin(cameraMatrix);

            for (int i = 0; i <= numParticles; i++)
            {
                particles[i].remLifetime -= time;
                if (particles[i].remLifetime < 0.0f)
                {
                    numParticles--;
                    particles[i] = particles[numParticles];
                    i--;
                    continue;
                }

                float interpolation = particles[i].remLifetime / particles[i].totLifetime;
                particles[i].position += (previousPosition == Vector3.Zero? Vector3.Zero:(position - previousPosition)) + particles[i].velocity * time;
                Color color = Color.Lerp(particles[i].endColor, particles[i].startColor, interpolation);
                float size = MathHelper.Lerp(particles[i].endsize, particles[i].startsize, interpolation);
                billboardEngine.AddBillboard(particles[i].position, color, size);
            }

            previousPosition = position;
        }

        private void addParticle(ref Particle newParticle)
        {
            int particleindex;
            if (numParticles == particles.Length)
                particleindex = random.Next(particles.Length);
            else
            {
                particleindex = numParticles;
                numParticles++;
            }

            particles[particleindex] = newParticle;
        }

        private void emitParticles(Vector3 position)
        {
            Particle particle;
            for (int i = 0; i < 72; i++)
            {
                particle.position = RotateAboutOrigin(position, new Vector3 (position.X+1.1f, position.Y, position.Z), (float) (i*5*Math.PI/180));
                particle.totLifetime = (float)random.NextDouble() * particleLifetime + 0.1f;
                particle.remLifetime = particle.totLifetime;
                particle.startColor = startColor;
                particle.endColor = endColor;
                particle.startsize = (float)random.NextDouble() * this.SizeScaling + 0.1f;
                particle.endsize = 0.0f;
                particle.velocity = Vector3.Up * this.VelocityScaling + new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()) * this.VelocityScaling/2 - new Vector3(VelocityScaling / 4, VelocityScaling / 4, VelocityScaling / 4);
                addParticle(ref particle);
            }
        }

        private Vector3 RotateAboutOrigin(Vector3 point, Vector3 origin, float rotation)
        {
            Vector3 u = point - origin; //point relative to origin  

            if (u == Vector3.Zero)
                return point;

            float a = (float)Math.Atan2(u.Z, u.X); //angle relative to origin  
            a += rotation; //rotate  

            //u is now the new point relative to origin  
            u = u.Length() * new Vector3((float)Math.Cos(a), 0, (float)Math.Sin(a));
            return u + origin - new Vector3(1.1f, 0.4f, 0);
        } 

        public void Draw(Camera camera)
        {
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            graphicsDevice.BlendState = BlendState.Additive;
            graphicsDevice.RasterizerState = RasterizerState.CullNone;
            if (numParticles > 0) billboardEngine.Draw(graphicsDevice, camera);
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

    }
}

