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

        public Player(/*BulletCollection bullets,*/ World world, ContentManager Content)
            : base()
        {
            this.model= new ModelObject(Content.Load<Model>("cube_rounded"));
            this.position=new Vector3(0,0,0);
            this.direction=new Vector3(1,0,0);
            this.speed=0.2f;
            this.level=1;
            this.maxHealth=100;
            this.health=100;
            weapon = new Weapon();
            this.world = world;
        }

        public int ground
        {
            get
            {
                //TODO
                return 0;
            }
        }

        public override bool update(BulletCollection bullets, Camera camera)
        {
            if (health <= 0) return false;

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

            model.Rotation = new Vector3(0, -camera.Phi,0);

            Vector3 front = new Vector3(camera.Direction.X, 0, camera.Direction.Z);
            front.Normalize();
            float forward = (Keyboard.GetState().IsKeyDown(Keys.W) ? 1.0f : 0.0f) + (Keyboard.GetState().IsKeyDown(Keys.Up) ? 1.0f : 0.0f) -
                            (Keyboard.GetState().IsKeyDown(Keys.S) ? 1.0f : 0.0f) - (Keyboard.GetState().IsKeyDown(Keys.Down) ? 1.0f : 0.0f);

            Vector3 sideVec = Vector3.Cross(front, new Vector3(0,1,0));
            float side = (Keyboard.GetState().IsKeyDown(Keys.D) ? 1.0f : 0.0f) + (Keyboard.GetState().IsKeyDown(Keys.Right) ? 1.0f : 0.0f) -
                         (Keyboard.GetState().IsKeyDown(Keys.A) ? 1.0f : 0.0f) - (Keyboard.GetState().IsKeyDown(Keys.Left) ? 1.0f : 0.0f);

            this.position += forward * speed * front;
            this.position += side * speed * sideVec;

            int x1 = (int)Math.Round(-1 * this.position.X + Constants.MAP_SIZE-1) / 2;
            int x2 = (int)Math.Round(-1 * this.position.X + Constants.MAP_SIZE) / 2;
            int z1 = (int)Math.Round(-1 * this.position.Z + Constants.MAP_SIZE-1) / 2;
            int z2 = (int)Math.Round(-1 * this.position.Z + Constants.MAP_SIZE) / 2;

            /*if (world.MapData[x2][z2] == '1')
            { 
                if(world.MapData[x1][z2] == '0')
                {
                    front = new Vector3(0, 0, front.Z);
                    this.model.Position += forward * speed * front;
                    this.position = this.model.Position;
                }
                else if(world.MapData[x2][z1] == '0')
                {
                    front = new Vector3(front.X, 0, 0);
                    this.model.Position += forward * speed * front;
                    this.position = this.model.Position;
                }
                else this.position = this.model.Position;
            }
            else if (world.MapData[x1][z2] == '1')
            {
                if (world.MapData[x2][z2] == '0')
                {
                    front = new Vector3(0, 0, front.Z);
                    this.model.Position += forward * speed * front;
                    this.position = this.model.Position;
                }
                else if (world.MapData[x1][z1] == '0')
                {
                    front = new Vector3(front.X, 0, 0);
                    this.model.Position += forward * speed * front;
                    this.position = this.model.Position;
                }
                else this.position = this.model.Position;
            }
            else if (world.MapData[x1][z1] == '1')
            {
                if (world.MapData[x2][z1] == '0')
                {
                    front = new Vector3(0, 0, front.Z);
                    this.model.Position += forward * speed * front;
                    this.position = this.model.Position;
                }
                else if (world.MapData[x1][z2] == '0')
                {
                    front = new Vector3(front.X, 0, 0);
                    this.model.Position += forward * speed * front;
                    this.position = this.model.Position;
                }
                else this.position = this.model.Position;
            }
            else if (world.MapData[x2][z1] == '1')
            {
                if (world.MapData[x1][z1] == '0')
                {
                    front = new Vector3(0, 0, front.Z);
                    this.model.Position += forward * speed * front;
                    this.position = this.model.Position;
                }
                else if (world.MapData[x2][z1] == '0')
                {
                    front = new Vector3(front.X, 0, 0);
                    this.model.Position += forward * speed * front;
                    this.position = this.model.Position;
                }
                else this.position = this.model.Position;
            }*/

            if ((world.MapData[x1][z1] == '1') || (world.MapData[x1][z2] == '1') || (world.MapData[x2][z1] == '1') || (world.MapData[x2][z2] == '1'))
                this.position = this.model.Position;
            else this.model.Position = this.position;

            weapon.update(bullets, position, direction);

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
