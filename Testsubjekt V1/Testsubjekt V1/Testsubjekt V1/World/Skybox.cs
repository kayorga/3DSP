using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestsubjektV1
{
    class Skybox
    {
        // containers
        private Texture2D[] skyboxTextures;
        private VertexBuffer skyboxBuffer;
        private BasicEffect effect;

        // const skybox size, may be changed if convenient
        private const float size = Constants.MAP_SIZE+20.0f;

        // sampler states for texture clamping
        private SamplerState clampState;
        private SamplerState backupState;

        // constructor
        public Skybox(GraphicsDevice graphics, ContentManager contentManager, byte theme)
        {
            // create basic effect
            effect = new BasicEffect(graphics);

            // initailize effect
            effect.World = Matrix.Identity;
            effect.LightingEnabled = false;
            effect.TextureEnabled = true;
            effect.VertexColorEnabled = false;

            effect.AmbientLightColor = new Vector3(0.6f, 0.6f, 0.6f);
            effect.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
            effect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            effect.SpecularPower = 5.0f;
            effect.Alpha = 1.0f;

            // load Skybox textures
            skyboxTextures = new Texture2D[6];
            skyboxTextures[0] = contentManager.Load<Texture2D>("Skybox/theme"+ theme + "_Top");
            skyboxTextures[1] = contentManager.Load<Texture2D>("Skybox/theme" + theme + "_Bottom");
            skyboxTextures[2] = contentManager.Load<Texture2D>("Skybox/theme" + theme + "_Left");
            skyboxTextures[3] = contentManager.Load<Texture2D>("Skybox/theme" + theme + "_Right");
            skyboxTextures[4] = contentManager.Load<Texture2D>("Skybox/theme" + theme + "_Front");
            skyboxTextures[5] = contentManager.Load<Texture2D>("Skybox/theme" + theme + "_Back");

            // define skybox vertices
            float upTranslation = 0.9f;
            float downTranslation = -1*(2-upTranslation);
            Vector3[] vertices = new Vector3[8];
            vertices[0] = new Vector3(downTranslation*size, downTranslation*size, downTranslation*size);
            vertices[1] = new Vector3(downTranslation*size, downTranslation*size, upTranslation*size);
            vertices[2] = new Vector3(upTranslation*size, downTranslation*size, downTranslation*size);
            vertices[3] = new Vector3(upTranslation*size, downTranslation*size, upTranslation*size);
            vertices[4] = new Vector3(downTranslation*size, upTranslation*size, downTranslation*size);
            vertices[5] = new Vector3(downTranslation*size, upTranslation*size, upTranslation*size);
            vertices[6] = new Vector3(upTranslation*size, upTranslation*size, downTranslation*size);
            vertices[7] = new Vector3(upTranslation*size, upTranslation*size, upTranslation*size);

            // create SamplerState that clamps textures at borders
            clampState = new SamplerState();
            clampState.AddressU = TextureAddressMode.Clamp;
            clampState.AddressV = TextureAddressMode.Clamp;

            // define skybox faces as triangle fans
            // TODO: if texture winding or orientation is wrong, adjust vertex order or texture coordinates here
            VertexPositionTexture[] skyboxModel = new VertexPositionTexture[24];
            // Top
            skyboxModel[0] = new VertexPositionTexture(vertices[4], new Vector2(0.0f, 0.0f));
            skyboxModel[1] = new VertexPositionTexture(vertices[6], new Vector2(0.0f, 1.0f));
            skyboxModel[2] = new VertexPositionTexture(vertices[5], new Vector2(1.0f, 0.0f));
            skyboxModel[3] = new VertexPositionTexture(vertices[7], new Vector2(1.0f, 1.0f));
            // Bottom
            skyboxModel[4] = new VertexPositionTexture(vertices[2], new Vector2(0.0f, 0.0f));
            skyboxModel[5] = new VertexPositionTexture(vertices[0], new Vector2(0.0f, 1.0f));
            skyboxModel[6] = new VertexPositionTexture(vertices[3], new Vector2(1.0f, 0.0f));
            skyboxModel[7] = new VertexPositionTexture(vertices[1], new Vector2(1.0f, 1.0f));
            // Left
            skyboxModel[8] = new VertexPositionTexture(vertices[4], new Vector2(0.0f, 0.0f));
            skyboxModel[9] = new VertexPositionTexture(vertices[0], new Vector2(0.0f, 1.0f));
            skyboxModel[10] = new VertexPositionTexture(vertices[6], new Vector2(1.0f, 0.0f));
            skyboxModel[11] = new VertexPositionTexture(vertices[2], new Vector2(1.0f, 1.0f));
            // Right
            skyboxModel[12] = new VertexPositionTexture(vertices[7], new Vector2(0.0f, 0.0f));
            skyboxModel[13] = new VertexPositionTexture(vertices[3], new Vector2(0.0f, 1.0f));
            skyboxModel[14] = new VertexPositionTexture(vertices[5], new Vector2(1.0f, 0.0f));
            skyboxModel[15] = new VertexPositionTexture(vertices[1], new Vector2(1.0f, 1.0f));
            // Front
            skyboxModel[16] = new VertexPositionTexture(vertices[6], new Vector2(0.0f, 0.0f));
            skyboxModel[17] = new VertexPositionTexture(vertices[2], new Vector2(0.0f, 1.0f));
            skyboxModel[18] = new VertexPositionTexture(vertices[7], new Vector2(1.0f, 0.0f));
            skyboxModel[19] = new VertexPositionTexture(vertices[3], new Vector2(1.0f, 1.0f));
            // Back
            skyboxModel[20] = new VertexPositionTexture(vertices[5], new Vector2(0.0f, 0.0f));
            skyboxModel[21] = new VertexPositionTexture(vertices[1], new Vector2(0.0f, 1.0f));
            skyboxModel[22] = new VertexPositionTexture(vertices[4], new Vector2(1.0f, 0.0f));
            skyboxModel[23] = new VertexPositionTexture(vertices[0], new Vector2(1.0f, 1.0f));

            // create vertexbuffer
            skyboxBuffer = new VertexBuffer(graphics, VertexPositionTexture.VertexDeclaration, 24, BufferUsage.WriteOnly);
            skyboxBuffer.SetData(skyboxModel);
        }

        public void Draw(GraphicsDevice graphics, Camera camera, Vector3 center)
        {
            Draw(graphics, camera.ViewMatrix, camera.ProjectionMatrix, Matrix.CreateTranslation(new Vector3(size / 2 -10.0f, 0, size / 2-10.0f)));
        }

        private void Draw(GraphicsDevice graphics, Matrix viewMatrix, Matrix projectionMatrix, Matrix WorldMatrix)
        {
            // disable depth buffer
            graphics.DepthStencilState = DepthStencilState.None;
            graphics.RasterizerState = RasterizerState.CullClockwise;

            // clamp texture
            backupState = graphics.SamplerStates[0];
            graphics.SamplerStates[0] = clampState;

            // set vertexbuffer
            graphics.SetVertexBuffer(skyboxBuffer);

            // world matrix should contain camera translation, not orientation
            // this results in the skybox moving with the viewer e.g. being "infinitely far away"
            effect.World = WorldMatrix;
            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;

            // render skybox
            effect.Texture = skyboxTextures[0];
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }
            effect.Texture = skyboxTextures[1];
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 4, 2);
            }
             effect.Texture = skyboxTextures[2];
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
             {
                 pass.Apply();
                 graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 8, 2);
             }
             effect.Texture = skyboxTextures[3];
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
             {
                 pass.Apply();
                 graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 12, 2);
             }
             effect.Texture = skyboxTextures[4];
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
             {
                 pass.Apply();
                 graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 16, 2);
             }
             effect.Texture = skyboxTextures[5];
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
             {
                 pass.Apply();
                 graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 20, 2);
             }

            // return to default
             graphics.SamplerStates[0] = backupState;

            // enable depth buffer again
            graphics.DepthStencilState = DepthStencilState.Default;
            graphics.RasterizerState = RasterizerState.CullCounterClockwise;
        }

    }
}
