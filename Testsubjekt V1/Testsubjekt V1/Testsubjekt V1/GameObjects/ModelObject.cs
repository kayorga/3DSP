using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestsubjektV1
{
    /// <summary>
    /// Easy to use object with model from xna contentloader
    /// </summary>
    class ModelObject
    {
        protected Model model;

        /// <summary>
        /// Position of the object
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// by default rotation is done first around X, then Y, then Z
        /// values are expected to be in radian
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// Scaling factor applied to all 3 axis
        /// </summary>
        public Vector3 Scaling
        { get { return scaling; } set { scaling = value; } }
        private Vector3 scaling = new Vector3(1.0f, 1.0f, 1.0f);

        /// <summary>
        /// creates a modelobject by loading a specified model from the content manager
        /// </summary>
        /// <param name="contentManager">contentmanager that will load the specified model</param>
        /// <param name="modelName">name of the model - no fileending!</param>
        public ModelObject(ContentManager contentManager, string modelName)
            : this(contentManager.Load<Model>(modelName))
        {
        }

        /// <summary>
        /// creates a modelobject by using a already loaded model
        /// </summary>
        /// <param name="model">instance of an already loaded model</param>
        public ModelObject(Model model)
        {
            this.model = model;

            // some settings to the material
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    if (effect is BasicEffect)
                    {
                        BasicEffect basicEffect = (BasicEffect)effect;
                        basicEffect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                        basicEffect.DirectionalLight0.Enabled = true;
                        basicEffect.DirectionalLight0.Direction = new Vector3(0, -1.0f , 0);
                        basicEffect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                        basicEffect.DirectionalLight1.Enabled = false;
                        basicEffect.DirectionalLight2.Enabled = false;
                        basicEffect.LightingEnabled = true;
                        basicEffect.PreferPerPixelLighting = true;  // much better :)
                    }
                }
            }
        }

        public void changeLighting(Vector3 direction, Vector3 color)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    if (effect is BasicEffect)
                    {
                        BasicEffect basicEffect = (BasicEffect)effect;
                        basicEffect.AmbientLightColor = color;
                        basicEffect.DirectionalLight0.Direction = direction;
                        //basicEffect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                }
            }
        }

        /// <summary>
        /// performs a simple collision test with the bounding sphere (very inaccurate!)
        /// </summary>
        /// <param name="point">point to be tested with the bounding sphere</param>
        /// <returns>true if point is in the sphere, false otherwise</returns>
        public bool SphereCollision(Vector3 point)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                if (mesh.BoundingSphere.Contains(point - Position) == ContainmentType.Contains)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// draws the object using a specified camera
        /// </summary>
        /// <param name="usedCamera">camera in that space the object will be drawn</param>
        public void Draw(Camera usedCamera)
        {
            Draw(usedCamera.ViewMatrix, usedCamera.ProjectionMatrix);
        }

        /// <summary>
        /// sets/uses the given view and projection matrices to draw the object
        /// </summary>
        /// <param name="viewMatrix">view matrix used for the drawing</param>
        /// <param name="projectionMatrix">projection matrix applied to the object drawing</param>
        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix worldMatrix = Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z) *
                                                Matrix.CreateScale(Scaling) * Matrix.CreateTranslation(Position);

            foreach (ModelMesh mesh in model.Meshes)
            {
                
                foreach (Effect effect in mesh.Effects)
                {
                    if (effect is BasicEffect)
                    {
                        BasicEffect basicEffect = (BasicEffect)effect;
                        basicEffect.World = worldMatrix;
                        basicEffect.View = viewMatrix;
                        basicEffect.Projection = projectionMatrix;
                        basicEffect.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                    }
                    else
                    {
                        AlphaTestEffect e = (AlphaTestEffect)effect;
                        e.World = worldMatrix;
                        e.View = viewMatrix;
                        e.Projection = projectionMatrix;
                        e.ReferenceAlpha = 150;
                    }
                }
                mesh.Draw();
            }
        }
    }
}
