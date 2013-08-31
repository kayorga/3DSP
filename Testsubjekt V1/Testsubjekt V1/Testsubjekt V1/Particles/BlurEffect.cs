using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TestsubjektV1
{
    class BlurEffect
    {
        private List<Particle2D> blurs;
        private GraphicsDevice graphicsDevice;
        private Texture2D blurTexture;

        public BlurEffect(GraphicsDevice gD, ContentManager Content)
        {
            graphicsDevice = gD;
            blurs = new List<Particle2D>();
            blurTexture = Content.Load<Texture2D>("Particles/particle");
        }

        public void AddBlur()
        {
            Random random = new Random();
            int positionX = random.Next(900)+50;
            int positionY = random.Next(500) + 50;
            int width = random.Next(300) + 100;
            float lifeTime = (float) random.NextDouble() * 10000;

            Particle2D mainBlur = new Particle2D(new Point(positionX, positionY), new Point(width, width), lifeTime);
            Particle2D secondaryBlur = new Particle2D(new Point(positionX + random.Next(10) - 5, positionY + random.Next(10) - 5), new Point(width, width), (float)(lifeTime + random.NextDouble() * 500 - 250));

            blurs.Add(mainBlur);
            blurs.Add(secondaryBlur);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < blurs.Count; i++)
            {
                blurs[i].remLifetime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (blurs[i].remLifetime <= 0.0f)
                {
                    blurs.Remove(blurs[i]);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.GraphicsDevice.BlendState = BlendState.Additive;
            spriteBatch.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            
            foreach (Particle2D blur in blurs)
            {
                float interpolation = blur.remLifetime / blur.totLifetime;
                Color color = Color.Lerp(Color.Transparent, Color.Black, interpolation);
                spriteBatch.Draw(blurTexture, new Rectangle(blur.position.X, blur.position.Y, blur.size.X, blur.size.Y), color);
            }

            spriteBatch.End();
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        }
    }
}
