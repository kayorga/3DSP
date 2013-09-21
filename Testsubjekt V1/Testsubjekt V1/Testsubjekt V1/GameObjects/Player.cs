using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class Player : Character
    {
        int exp;
        int lastMapID;

        byte invincibleTimer;
        byte maxInvincibility;

        byte restore;
        byte maxRest;
        byte hitDelay;
        byte maxHitDelay;

        public byte xTile { get { return (byte)Math.Round(-1 * (position.X) + (Constants.MAP_SIZE - 1)); } }
        public byte zTile { get { return (byte)Math.Round(-1 * (position.Z) + (Constants.MAP_SIZE - 1)); } }

        public bool gotHit { get; set; }

        public bool charging;

        Weapon weapon;

        private Charge charge;

        public Player(World world, ContentManager Content, AudioManager audio, GraphicsDevice graphicsDevice)
            : base()
        {
            this.world = world;
            this.model = new ModelObject(Content.Load<Model>("Models\\T"));
            this.speed=0.15f;
            this.level=1;
            this.maxHealth=100;
            this.health=100;
            maxRest = 15;
            restore = 0;
            maxHitDelay = 45;
            hitDelay = 0;

            charging = false;

            weapon = new Weapon(audio);
            exp = 0;

            invincibleTimer = 0;
            maxInvincibility = Constants.PLAYER_INVINCIBILITY;

            this.charge = new Charge(this.position, graphicsDevice, Content);

            reset();
        }

        /// <summary>
        /// sets position to world coordinates according to map coordinates in world.player_start
        /// </summary>
        private void reset()
        {
            int xtile = world.player_start[0];
            int ztile = world.player_start[1];
            this.position  = new Vector3(Constants.MAP_SIZE - 2 * xtile - 1, 0, Constants.MAP_SIZE - 2 * ztile - 1);
            this.direction = new Vector3(1,0,0);
            lastMapID = world.mapID;
            invincibleTimer = 0;
            health = maxHealth;
            weapon.reload();
            gotHit = false;
            charging = false;
        }

        public void setPosition(Vector3 X) { this.position = X; }

        public Weapon myWeapon
        {
            get { return weapon; }
            set { weapon = value; }
        }

        public int XP
        {
            get { return exp; }
            set { exp = value; }
        }

        public bool update(GameTime gameTime, NPCCollection npcs, BulletCollection bullets, Camera camera, bool canShoot)
        {
            if (lastMapID != world.mapID)
            {
                reset();
                return true;
            }
            
            if (health <= 0) return false;

            restore = (byte) Math.Max(restore - 1, 0);
            hitDelay = (byte)Math.Max(hitDelay - 1, 0);

            if (health < maxHealth && hitDelay == 0
                && Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                charging = true;
                charge.Update(gameTime, camera, this.Position);
                if (restore <= 0)
                {
                    int h = Math.Max((int)(maxHealth * .01f), 1);
                    health = Math.Min(health + h, maxHealth);
                    restore = maxRest;
                }
            }
            else charging = false;

            /*if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                charging = true;
                charge.Update(gameTime, camera, this.Position);
            }*/

            model.Rotation = new Vector3(0, -camera.Phi,0);

            Vector3 front = new Vector3(camera.Direction.X, 0, camera.Direction.Z);
            front.Normalize();
            float forward = (Keyboard.GetState().IsKeyDown(Keys.W) ? 1.0f : 0.0f) - (Keyboard.GetState().IsKeyDown(Keys.S) ? 1.0f : 0.0f);

            Vector3 sideVec = Vector3.Cross(front, new Vector3(0,1,0));
            float side = (Keyboard.GetState().IsKeyDown(Keys.D) ? 1.0f : 0.0f) - (Keyboard.GetState().IsKeyDown(Keys.A) ? 1.0f : 0.0f);

            this.direction = front * forward + sideVec * side;
            if (this.direction != Vector3.Zero) this.direction.Normalize();

            moveAndCollide(npcs);

            if (invincibleTimer > 0)
                invincibleTimer--;

            if (world.mapID != 0) 
                weapon.update(bullets, position, front, canShoot);
            return true;
        }

        public void getEXP(int xp)
        {
            exp += xp;
            while (exp >= 100)
            {
                lvUP();
                exp -= 100;
            }

            if (level == 50) exp = 0;
        }

        public void lvUP()
        {
            level++;
            int d = (int) (maxHealth * .05f);
            maxHealth += d;
            health = maxHealth;
            weapon.reload();
        }

        public void setupStats()
        {
            for (int i = 1; i < level; i++)
            {
                int d = (int)(maxHealth * .05f);
                maxHealth += d;
                health = maxHealth;
            }
        }

        public void getHit(Bullet b, Mission m)
        {
            if (invincibleTimer > 0)
                return;
            //TODO///////
            int dmg = b.Strength;
            /////////////
            gotHit = true;
            health = Math.Max(health - dmg, 0);
            m.dmgIn += dmg;
            invincibleTimer = maxInvincibility;
            hitDelay = maxHitDelay;
        }

        private void getHit(NPC npc)
        {
            //TODO/////
            int dmg = 1 + (int)(npc.lv * npc.speed * 50);
            ///////////
            gotHit = true;
            health = Math.Max(health - dmg, 0);
            invincibleTimer = maxInvincibility;
            hitDelay = maxHitDelay;
        }

        //public void moveAndCollide1(NPCCollection npcs)
        //{
        //    this.position += speed * direction;
        //    int x1 = (int)Math.Round(-1 * this.position.X + Constants.MAP_SIZE - 1) / 2;
        //    int x2 = (int)Math.Round(-1 * this.position.X + Constants.MAP_SIZE) / 2;
        //    int z1 = (int)Math.Round(-1 * this.position.Z + Constants.MAP_SIZE - 1) / 2;
        //    int z2 = (int)Math.Round(-1 * this.position.Z + Constants.MAP_SIZE) / 2;
        //    if ((world.MoveData[x1][z1] == 1) || (world.MoveData[x2][z2] == 1) || (world.MoveData[x2][z1] == 1) || (world.MoveData[x1][z2] == 1))
        //        this.position = this.model.Position;
        //    else model.Position = this.position;

        //    byte tile = npcs.npcMoveData[xTile][zTile];

        //    if (tile != 0 && tile != 255 && invincibleTimer == 0)
        //    {
        //        getHit(npcs[tile - 1]);
        //        npcs[tile - 1].getHit(this);
        //    }
        //}

        public void moveAndCollide(NPCCollection npcs)
        {
            //calculate tiles before movement
            int oldx1 = (int)Math.Round(-1 * this.position.X + Constants.MAP_SIZE - 1) / 2;
            int oldx2 = (int)Math.Round(-1 * this.position.X + Constants.MAP_SIZE) / 2;
            int oldz1 = (int)Math.Round(-1 * this.position.Z + Constants.MAP_SIZE - 1) / 2;
            int oldz2 = (int)Math.Round(-1 * this.position.Z + Constants.MAP_SIZE) / 2;

            //apply speed and direction to move character
            this.position += speed * direction;

            //calculate tiles after movement
            int x1 = (int)Math.Round(-1 * this.position.X + Constants.MAP_SIZE - 1) / 2;
            int x2 = (int)Math.Round(-1 * this.position.X + Constants.MAP_SIZE) / 2;
            int z1 = (int)Math.Round(-1 * this.position.Z + Constants.MAP_SIZE - 1) / 2;
            int z2 = (int)Math.Round(-1 * this.position.Z + Constants.MAP_SIZE) / 2;

            //determine Movement bools for tiles; true means not passable
            bool moveDataX1Z1 = world.MoveData[x1][z1] == 1;
            bool moveDataX1Z2 = world.MoveData[x1][z2] == 1;
            bool moveDataX2Z2 = world.MoveData[x2][z2] == 1;
            bool moveDataX2Z1 = world.MoveData[x2][z1] == 1;

            if (moveDataX1Z1 || moveDataX2Z2 || moveDataX2Z1 || moveDataX1Z2)
            {
                #region check for single Collisions

                //Bottom Right
                if (moveDataX1Z1 && !moveDataX2Z2 && !moveDataX2Z1 && !moveDataX1Z2)
                {
                    if (oldz1 - 1 == z1)
                    {
                        this.position = this.model.Position;
                        this.position += speed * new Vector3(direction.X, direction.Y, 0);
                    }
                    else if (oldx1 - 1 == x1)
                    {
                        this.position = this.model.Position;
                        this.position += speed * new Vector3(0, direction.Y, direction.Z);
                    }
                }

                //Top Left
                else if (!moveDataX1Z1 && moveDataX2Z2 && !moveDataX2Z1 && !moveDataX1Z2)
                {
                    if (oldz2 + 1 == z2)
                    {
                        this.position = this.model.Position;
                        this.position += speed * new Vector3(direction.X, direction.Y, 0);
                    }
                    else if (oldx2 + 1 == x2)
                    {
                        this.position = this.model.Position;
                        this.position += speed * new Vector3(0, direction.Y, direction.Z);
                    }
                }

                //Top Right
                else if (!moveDataX1Z1 && !moveDataX2Z2 && !moveDataX2Z1 && moveDataX1Z2)
                {
                    if (oldz2 + 1 == z2)
                    {
                        this.position = this.model.Position;
                        this.position += speed * new Vector3(direction.X, direction.Y, 0);
                    }
                    else if (oldx1 - 1 == x1)
                    {
                        this.position = this.model.Position;
                        this.position += speed * new Vector3(0, direction.Y, direction.Z);
                    }
                }

                //Bottom Left
                else if (!moveDataX1Z1 && !moveDataX2Z2 && moveDataX2Z1 && !moveDataX1Z2)
                {
                    if (oldz1 - 1 == z1)
                    {
                        this.position = this.model.Position;
                        this.position += speed * new Vector3(direction.X, direction.Y, 0);
                    }
                    else if (oldx2 + 1 == x2)
                    {
                        this.position = this.model.Position;
                        this.position += speed * new Vector3(0, direction.Y, direction.Z);
                    }
                }
                #endregion

                #region check for collision on one whole side

                //Bottom
                else if (moveDataX1Z1 && !moveDataX2Z2 && moveDataX2Z1 && !moveDataX1Z2)
                {
                    this.position = this.model.Position;
                    this.position += speed * new Vector3(direction.X, direction.Y, 0);
                }

                //Right
                else if (moveDataX1Z1 && !moveDataX2Z2 && !moveDataX2Z1 && moveDataX1Z2)
                {
                    this.position = this.model.Position;
                    this.position += speed * new Vector3(0, direction.Y, direction.Z);
                }

                //Top
                else if (!moveDataX1Z1 && moveDataX2Z2 && moveDataX2Z1 && !moveDataX1Z2)
                {
                    this.position = this.model.Position;
                    this.position += speed * new Vector3(0, direction.Y, direction.Z);
                }

                //Left
                else if (!moveDataX1Z1 && moveDataX2Z2 && !moveDataX2Z1 && moveDataX1Z2)
                {
                    this.position = this.model.Position;
                    this.position += speed * new Vector3(direction.X, direction.Y, 0);
                }
                #endregion

                // corner, nowhere to slide to
                else this.position = this.model.Position;
            }

            //Set Model Position at last
            this.model.Position = this.position;

            //check for player - npc collision
            byte tile = npcs.npcMoveData[xTile][zTile];

            if (tile != 0 && tile != 255 && invincibleTimer == 0)
            {
                getHit(npcs[tile - 1]);
                npcs[tile - 1].getHit(this);
            }
        }

        internal Weapon Weapon
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }


        public override void draw(Camera camera)
        {
            base.draw(camera);
            if (charging) charge.Draw(camera);
        }

    }
}
