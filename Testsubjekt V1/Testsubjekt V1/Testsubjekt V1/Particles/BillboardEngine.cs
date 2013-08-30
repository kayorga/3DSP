using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestsubjektV1
{
    class BillboardEngine
    {
        public BasicEffect Effect { private set; get; }

        private VertexBuffer vb;
        private int currentSpriteNumber = 0;

        public BillboardEngine(int maxSpriteNumber, GraphicsDevice device)
        {
            Effect = new BasicEffect(device);
            Effect.LightingEnabled = false;
            Effect.VertexColorEnabled = true;
            Effect.TextureEnabled = true;

            vb = new VertexBuffer(device, VertexPositionColorTexture.VertexDeclaration, maxSpriteNumber * 6, BufferUsage.WriteOnly);
        }


        private Vector3 camX;
        private Vector3 camY;

        /// <summary>
        /// begin with adding new sprites
        /// you need to re-add all sprites every time the camera changes its orientation
        /// </summary>
        /// <param name="cameraMatrix"></param>
        public void Begin(Matrix cameraMatrix)
        {
            currentSpriteNumber = 0;
            camX = new Vector3(cameraMatrix.M11, cameraMatrix.M21, cameraMatrix.M31);
            camY = new Vector3(cameraMatrix.M12, cameraMatrix.M22, cameraMatrix.M32);
        }

        /// <summary>
        /// adds a sprite and orientates it properly
        /// </summary>
        public void AddBillboard(Vector3 position, Color color, float size)
        {
            try
            {

                float halfsize = size * 0.5f;

                VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[6];

                vertices[0].Color = vertices[1].Color = vertices[4].Color = vertices[2].Color = color;

                vertices[0].TextureCoordinate = new Vector2(0, 0);
                vertices[1].TextureCoordinate = new Vector2(0, 1);
                vertices[4].TextureCoordinate = new Vector2(1, 1);
                vertices[2].TextureCoordinate = new Vector2(1, 0);

                vertices[0].Position = position + (-camX - camY) * halfsize;    //0
                vertices[1].Position = position + (-camX + camY) * halfsize;    //1
                vertices[2].Position = position + (camX - camY) * halfsize;     //3

                vertices[3] = vertices[1];
                vertices[4].Position = position + (camX + camY) * halfsize;
                vertices[5] = vertices[2];

                GraphicsDevice device = vb.GraphicsDevice;

                device.SetVertexBuffers(null);

                vb.SetData<VertexPositionColorTexture>(currentSpriteNumber * VertexPositionColorTexture.VertexDeclaration.VertexStride * 6, vertices, 0, 6, VertexPositionColorTexture.VertexDeclaration.VertexStride);
                currentSpriteNumber++;
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// draws all sprites with given effect settings
        /// </summary>
        public void Draw(GraphicsDevice device, Camera camera)
        {
            Effect.VertexColorEnabled = true;
            Effect.World = Matrix.Identity;
            Effect.View = camera.ViewMatrix;
            Effect.Projection = camera.ProjectionMatrix;
            Effect.LightingEnabled = false;
            device.SetVertexBuffer(vb);
            //device.DrawPrimitives(PrimitiveType.TriangleList, 0, currentSpriteNumber * 2);
            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                try { device.DrawPrimitives(PrimitiveType.TriangleList, 0, currentSpriteNumber * 2); }
                catch { ArgumentOutOfRangeException e = new ArgumentOutOfRangeException(); }
            }
        }
    }
}
