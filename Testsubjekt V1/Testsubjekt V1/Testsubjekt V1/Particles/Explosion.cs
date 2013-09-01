using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TestsubjektV1
{
    class Explosion
    {
        BillboardEngine billboardEngine;
        ParticleEffect particleEffect;
        GraphicsDevice graphicsDevice;

        public Vector3 Position {get; set;}

        public Explosion(Vector3 position, GraphicsDevice gDevice, ContentManager Content)
        {
            graphicsDevice = gDevice;
            Position = position;
            Texture2D texture = Content.Load<Texture2D>("Particles/fire");
            billboardEngine = new BillboardEngine(5, graphicsDevice);
            billboardEngine.Effect.Texture = texture;
            particleEffect = new ParticleEffect(10000, graphicsDevice, texture, Color.Goldenrod, Color.DarkMagenta, 1.0f);
            particleEffect.VelocityScaling = 7.0f;
        }

        public void Update(GameTime gameTime, Camera camera)
        {
            billboardEngine.Begin(camera.ViewMatrix);
            billboardEngine.AddBillboard(new Vector3(0.0f, 1.0f, 0.0f), Color.Honeydew, 0.01f);

            particleEffect.Update((float)gameTime.ElapsedGameTime.TotalSeconds, camera.ViewMatrix, Position, Vector3.Zero);
        }

        public void Draw(Camera camera)
        {
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            graphicsDevice.BlendState = BlendState.Additive;
            billboardEngine.Draw(graphicsDevice, camera);
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            particleEffect.Draw(camera);
        }

        public void Clear()
        {
            particleEffect.Clear();
        }

        public void Initialize(Camera camera)
        {
            billboardEngine.Begin(camera.ViewMatrix);
            billboardEngine.AddBillboard(new Vector3(0.0f, 1.0f, 0.0f), Color.Honeydew, 0.01f);
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            graphicsDevice.BlendState = BlendState.Additive;
            billboardEngine.Draw(graphicsDevice, camera);
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
