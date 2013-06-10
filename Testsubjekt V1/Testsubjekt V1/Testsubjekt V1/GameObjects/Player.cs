using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class Player : Character
    {
        int exp;
        Weapon weapon;
        //protected int lastMouseX = 0; // last x position of the mouse
        //protected float phi = 0.0f;//MathHelper.Pi;

        public Player(/*BulletCollection bullets, World world,*/ContentManager Content)
            : base()
        {
            this.model= new ModelObject(Content.Load<Model>("cube_rounded"));
            this.position=new Vector3(0,0,0);
            this.direction=new Vector3(1,0,0);
            this.speed=0.2f;
            this.level=1;
            this.maxHealth=100;
            this.health=100;
        }

        public int ground
        {
            get
            {
                //TODO
                return 0;
            }
        }

        public override bool update(Camera camera)
        {
            this.direction = camera.Direction;
            /*Vector3 sideVec = Vector3.Cross(camera.Direction, camera.UpDirection);
            Vector3 front = Vector3.Cross(camera.UpDirection, sideVec);
            
            float forward = (Keyboard.GetState().IsKeyDown(Keys.W) ? 1.0f : 0.0f) + (Keyboard.GetState().IsKeyDown(Keys.Up) ? 1.0f : 0.0f) -
                            (Keyboard.GetState().IsKeyDown(Keys.S) ? 1.0f : 0.0f) - (Keyboard.GetState().IsKeyDown(Keys.Down) ? 1.0f : 0.0f);


            this.position += forward * speed * front;

            float side = (Keyboard.GetState().IsKeyDown(Keys.D) ? 1.0f : 0.0f) + (Keyboard.GetState().IsKeyDown(Keys.Right) ? 1.0f : 0.0f) -
                         (Keyboard.GetState().IsKeyDown(Keys.A) ? 1.0f : 0.0f) - (Keyboard.GetState().IsKeyDown(Keys.Left) ? 1.0f : 0.0f);

            this.position += side * speed * sideVec;

            int deltaX = Mouse.GetState().X - lastMouseX;
            phi -= deltaX * 0.01f;
            lastMouseX = Mouse.GetState().X;*/

            //model.Rotation = new Vector3(0, phi, 0);

            Vector3 front = new Vector3(camera.Direction.X, 0, camera.Direction.Z);
            float forward = (Keyboard.GetState().IsKeyDown(Keys.W) ? 1.0f : 0.0f) + (Keyboard.GetState().IsKeyDown(Keys.Up) ? 1.0f : 0.0f) -
                            (Keyboard.GetState().IsKeyDown(Keys.S) ? 1.0f : 0.0f) - (Keyboard.GetState().IsKeyDown(Keys.Down) ? 1.0f : 0.0f);


            this.position += forward * speed * front;
            Vector3 sideVec = Vector3.Cross(front, new Vector3(0,1,0));
            float side = (Keyboard.GetState().IsKeyDown(Keys.D) ? 1.0f : 0.0f) + (Keyboard.GetState().IsKeyDown(Keys.Right) ? 1.0f : 0.0f) -
                         (Keyboard.GetState().IsKeyDown(Keys.A) ? 1.0f : 0.0f) - (Keyboard.GetState().IsKeyDown(Keys.Left) ? 1.0f : 0.0f);

            this.position += side * speed * sideVec;
            model.Position = this.position;

            return true;
        }

        public void getEXP(int exp)
        {
            //TODO
        }

        public void lvUP()
        {
            //TODO
        }

    }
}
