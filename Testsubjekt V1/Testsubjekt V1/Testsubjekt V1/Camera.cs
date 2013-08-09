using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class Camera
    {
               // matrices
        protected Matrix projectionMatrix;
        protected Matrix viewMatrix = Matrix.Identity;
        // vectors
        protected Vector3 viewDirection = new Vector3(0, -0.7f, -0.5f);
        protected Vector3 position = new Vector3(0, 10, 0);
        protected Vector3 upVec;
        // projection properties
        protected float aspectRatio;
        protected float fov;
        protected float nearPlane;
        protected float farPlane;
        
        
        /// <summary>
        /// creates a new camera and sets a projection matrix up
        /// </summary>
        /// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height. 
        ///                          To match aspect ratio of the viewport, the property AspectRatio.</param>
        /// <param name="fov">Field of view in the y direction, in radians.</param>
        /// <param name="nearPlane">Distance to the near view plane.</param>
        /// <param name="farPlane">Distance to the far view plane.</param>
        public Camera(float aspectRatio, float fov = 1.309f, float nearPlane = 0.1f, float farPlane = 5000.0f)
        {
            this.aspectRatio = aspectRatio;
            this.fov = fov;
            this.nearPlane = nearPlane;
            this.farPlane = farPlane;
            RebuildProjectionMatrix();
        }

        public void reset()
        {
            viewDirection = new Vector3(0, -0.7f, -0.5f);
            position = new Vector3(0, 10, 0);
            phi = .5f*MathHelper.Pi;
            theta = -.75f*MathHelper.Pi;
            lastMouseX = 511; // last x position of the mouse
            lastMouseY = 383; // last y position of the mouse
        }

        /// <summary>
        /// The projection matrix for this camera
        /// </summary>
        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
        }

        /// <summary>
        /// The view matrix for this camera
        /// </summary>
        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        /// <summary>
        /// Current position of the camera
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Current view-direction of the camera
        /// </summary>
        public Vector3 Direction
        {
            get { return viewDirection; }
        }

        public Vector3 UpDirection
        {
            get { return upVec; }
        }

        /// <summary>
        /// Updates the Camera.
        /// Handles user input intern and updates matrices.
        /// </summary>
        /// <param name="time">GameTime-Object from Game.Update</param>


        /// <summary>
        /// Speed of the camera rotation - using the mouse
        /// </summary>
        public float RotationSpeed
        {
            get { return rotationSpeed; }
            set { rotationSpeed = value; }
        }

        // movement factors variables
        protected float rotationSpeed = 0.01f;

        // some intern controlling variables
        protected float phi = .5f*MathHelper.Pi;
        protected float theta = -.75f*MathHelper.Pi;
        protected int lastMouseX = 511; // last x position of the mouse
        protected int lastMouseY = 383; // last y position of the mouse

        public float Phi
        {
            get { return phi; }
        }
        /// <summary>
        /// Updates the Camera 
        /// </summary>
        /// <param name="time">time from Game.Update</param>
        public void Update(GameTime time, Vector3 targetPosition)
        {
            // mouse movement
            UpdateThetaPhiFromMouse();

            // resulting view direction
            viewDirection = new Vector3((float)(System.Math.Cos(phi) * System.Math.Sin(theta)),
                                        (float)(System.Math.Cos(theta)),
                                        (float)(System.Math.Sin(phi) * System.Math.Sin(theta)));
            // up vector - by rotation 90°
            float theta2 = theta + (float)System.Math.PI / 2.0f;
            upVec = new Vector3((float)(System.Math.Cos(phi) * System.Math.Sin(theta2)),
                                        (float)(System.Math.Cos(theta2)),
                                        (float)(System.Math.Sin(phi) * System.Math.Sin(theta2)));
            MouseState mouseState = Mouse.GetState();

            float ScrollWheelChange = -1.0f*mouseState.ScrollWheelValue;
            float distance= (ScrollWheelChange*0.02f+7.5f);
            if (ScrollWheelChange * 0.02f + 7.5f < 2.0f) distance = 2.0f;
            else if (ScrollWheelChange * 0.02f + 7.5f > 12.5f) distance = 12.5f;
            
            Position = targetPosition - distance * viewDirection;
            float c = viewDirection.Length();
            ;
            // compute view matrix
            viewMatrix = Matrix.CreateLookAt(Position, Position + viewDirection, upVec);
        }

        /// <summary>
        /// intern helper to update view angles by mouse
        /// </summary>
        protected void UpdateThetaPhiFromMouse()
        {
            // mouse movement
            int deltaX = Mouse.GetState().X - lastMouseX;
            int deltaY = Mouse.GetState().Y - lastMouseY;
            Mouse.SetPosition(512, 384);
            phi += deltaX * rotationSpeed;
            theta -= deltaY * rotationSpeed;
            while (theta < 0) theta += 2 * MathHelper.Pi;
            while (theta > 2*MathHelper.Pi) theta -= 2 * MathHelper.Pi;
            if (theta < MathHelper.Pi) theta = MathHelper.Pi;
            if (theta > 1.5f * MathHelper.Pi) theta = 1.5f * MathHelper.Pi;
            lastMouseX = Mouse.GetState().X;
            lastMouseY = Mouse.GetState().Y;
        }

        /// <summary>
        /// Intern function for recreating the projection matrix.
        /// Capsuling the Matrix.Create... makes it easy to exchange the type of projection
        /// </summary>
        protected virtual void RebuildProjectionMatrix()
        {
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlane, farPlane);
        }
    }
}
