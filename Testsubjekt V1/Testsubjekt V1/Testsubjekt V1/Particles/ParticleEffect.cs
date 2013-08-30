using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestsubjektV1
{
    class ParticleEffect
    {
        /*private int particleLifespan;
        private int emitAmount;
        private float particleSpeed;
        private Queue<Particle> freeParticles;*/

        private Random random;
        private GraphicsDevice graphicsDevice;


        private BillboardEngine billboardEngine;

        private Particle[] particles;
        private int numParticles = 0;

        private Color startColor;
        private Color endColor;
        private float particleLifetime;

        public float VelocityScaling { get; set; }

        public void setColors(Color start, Color end)
        {
            startColor = start;
            endColor = end;
        }

        public BasicEffect Effect
        { get { return billboardEngine.Effect; } }

        public ParticleEffect(int maxNumParticles, GraphicsDevice gd, Texture2D texture, Color startCol, Color endCol, float lifetime)
        {
            graphicsDevice = gd;
            random = new Random();
            billboardEngine = new BillboardEngine(maxNumParticles, graphicsDevice);
            particles = new Particle[maxNumParticles];
            Effect.Texture = texture;
            startColor = startCol;
            endColor = endCol;
            particleLifetime = lifetime;
            this.VelocityScaling = 1.0f;
        }

        public void Clear()
        {
            numParticles = 0;
        }

        public void Update(float time, Matrix cameraMatrix, Vector3 position, Vector3 direction, GraphicsDevice graphicsDevice)
        {
            emitParticles(position, direction);

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
                particles[i].position += particles[i].velocity * time;
                Color color = Color.Lerp(particles[i].endColor, particles[i].startColor, interpolation);
                float size = MathHelper.Lerp(particles[i].endsize, particles[i].startsize, interpolation);
                billboardEngine.AddBillboard(particles[i].position, color, size);
            }
        }

        public void Update(float time, Matrix cameraMatrix, Vector3 position, Vector3 direction)
        {
            emitParticles(position, direction);

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
                particles[i].position += particles[i].velocity * time;
                Color color = Color.Lerp(particles[i].endColor, particles[i].startColor, interpolation);
                float size = MathHelper.Lerp(particles[i].endsize, particles[i].startsize, interpolation);
                billboardEngine.AddBillboard(particles[i].position, color, size);
            }
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

        private void emitParticles(Vector3 position, Vector3 direction)
        {
            Particle particle;
            for (int i = 0; i < 25; i++)
            {
                particle.position = position;
                particle.totLifetime = (float)random.NextDouble() * particleLifetime + 0.01f;
                particle.remLifetime = particle.totLifetime;
                particle.startColor = startColor;
                particle.endColor = endColor;
                particle.startsize = (float)random.NextDouble() * 0.8f + 0.1f;
                particle.endsize = 0.0f;
                particle.velocity = direction+ new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()) * this.VelocityScaling - new Vector3(VelocityScaling/2,VelocityScaling/2,VelocityScaling/2);
                addParticle(ref particle);
            }
        }

        public void Draw(Camera camera, GraphicsDevice graphicsDevice)
        {
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            graphicsDevice.BlendState = BlendState.Additive;
            graphicsDevice.RasterizerState = RasterizerState.CullNone;
            if(numParticles>0) billboardEngine.Draw(graphicsDevice, camera);
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
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

