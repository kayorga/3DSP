using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TestsubjektV1
{
    class NPC : Character
    {
        #region attributes
        int cooldown;
        int maxCooldn;
        public byte kind;
        public bool active;
        public int XP;
        private AStar pathFinder;
        private bool moving;
        public bool newTarget;
        public bool isMoving { get { return moving; } }
        private byte strength;
        private byte element;
        private byte elWeakness
        {
            get
            {
                switch (element)
                {
                    case Constants.ELM_PLA: return Constants.ELM_ICE;
                    case Constants.ELM_HEA: return Constants.ELM_PLA;
                    case Constants.ELM_ICE: return Constants.ELM_HEA;
                    default: return Constants.ELM_NIL;
                }
            }
        }

        private byte typeWeakness
        {
            get
            {
                switch (element)
                {
                    case Constants.ELM_PLA: return Constants.TYP_TRI;
                    case Constants.ELM_HEA: return Constants.TYP_BLA;
                    case Constants.ELM_ICE: return Constants.TYP_WAV;
                    default: return Constants.ELM_NIL;
                }
            }
        }

        private byte typeResistance
        {
            get
            {
                switch (element)
                {
                    case Constants.ELM_PLA: return Constants.TYP_WAV;
                    case Constants.ELM_HEA: return Constants.TYP_TRI;
                    case Constants.ELM_ICE: return Constants.TYP_BLA;
                    default: return Constants.ELM_NIL;
                }
            }
        }

        private List<DmgNumber> dmgNumbers;

        public Vector3 target;

        private GraphicsDevice graphicsDevice;
        private BillboardEngine billboardEngine;
        private bool isHit;
        private int hitTimer;
        private Explosion explosion;
        private bool isDead;
        private float playerDistance;

        private AudioManager audio;
        
        #endregion

        /// <summary>
        /// creates empty NPC with placeholder attributes
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="graphicsDevice"></param>
        /// <param name="world"></param>
        /// <param name="bilbo"></param>
        public NPC(ContentManager Content, GraphicsDevice graphicsDevice, World world, BillboardEngine bilbo, AudioManager audio)
        {
            model = null;
            position = Vector3.Zero;
            direction = Vector3.Zero;
            this.world = world;
            speed = 0;
            level = 0;
            maxHealth = 0;
            health = 0;
            active = false;
            newTarget = false;
            XP = 0;
            cooldown = 0;
            maxCooldn = 0;
            element = 0;
            kind = 0;
            strength = 0;
            pathFinder = null;
            target = Vector3.Zero;
            isHit = false;
            hitTimer = 0;
            isDead = false;
            billboardEngine = bilbo;
            this.graphicsDevice = graphicsDevice;
            this.audio = audio;
            explosion = new Explosion(Position, graphicsDevice, Content);
            dmgNumbers = new List<DmgNumber>();
        }

        /// <summary>
        /// assigns NPC attributes and ModelObject and activates the NPC
        /// </summary>
        /// <param name="k">NPC kind/species</param>
        /// <param name="m">ModelObject</param>
        /// <param name="p">starting position</param>
        /// <param name="d">starting direction</param>
        /// <param name="l">level</param>
        /// <param name="pl">Player</param>
        /// <param name="npcs">parent NPCCollection (should always be "this")</param>
        public void setup(byte k, ModelObject m, Vector3 p, Vector3 d, int l, Player pl, NPCCollection npcs, byte elem = Constants.ELM_NIL)
        {
            kind = k;
            model = m;
            position = p;
            direction = d;
            level = l;

            if (kind == Constants.NPC_BOSS)
                level = Math.Min(60, Math.Max(1, level));
            else level = Math.Min(50, Math.Max(1, level));

            playerDistance = (pl.Position - this.Position).Length();

            #region kind specific stats
            switch (kind)
            {
                case Constants.NPC_NONE:
                    speed = 0.04f;
                    maxHealth = (int)(50 * Math.Pow(1.06f, level - 1));
                    XP = 2;
                    maxCooldn = 70;
                    element = Constants.ELM_NIL;
                    strength = (byte)(25 * Math.Pow(1.04f, level - 1));
                    break;
                case Constants.NPC_PLAS:
                    speed = 0.05f;
                    maxHealth = (int)(50 * Math.Pow(1.05f, level - 1));
                    XP = 3;
                    maxCooldn = 80;
                    element = Constants.ELM_PLA;
                    strength = (byte)(12 * Math.Pow(1.06f, level - 1));
                    break;
                case Constants.NPC_HEAT:
                    speed = 0.06f;
                    maxHealth = (int)(50 * Math.Pow(1.04f, level - 1));
                    XP = 3;
                    maxCooldn = 75;
                    element = Constants.ELM_HEA;
                    strength = (byte)(20 * Math.Pow(1.05f, level - 1));
                    break;
                case Constants.NPC_ICE:
                    speed = 0.05f;
                    maxHealth = (int)(50 * Math.Pow(1.07f, level - 1));
                    XP = 3;
                    maxCooldn = 90;
                    element = Constants.ELM_ICE;
                    strength = (byte)Math.Min((15 * Math.Pow(1.06f, level - 1)), 255);
                    break;                
                case Constants.NPC_BOSS:
                    speed = 0.03f;
                    maxHealth = (int)(200 * Math.Pow(1.07f, level - 1));
                    XP = 70;
                    maxCooldn = 50;
                    //model.Scaling = new Vector3(3,3,3);
                    element = elem;
                    strength = (byte)(14 * Math.Pow(1.06f, level - 1));
                    break;
                default:
                    speed = 0.001f;
                    maxHealth = 1;
                    XP = 0;
                    maxCooldn = 1000;
                    break;
            }
            #endregion

            health = maxHealth;
            active = true;
            model.Position = this.position;
            moving = false;
            newTarget = false;
            isHit = false;
            hitTimer = 0;
            isDead = false;
            pathFinder = npcs.PathFinder;
            target = new Vector3(position.X, position.Y, position.Z);
        }

        public bool update(GameTime gameTime, BulletCollection bullets, Camera camera, Player p, Mission m)
        {
            #region npc death
            //die and grant xp
            if (health <= 0 && !isDead)
            {
                int exp = (int)(XP * (float)level / (float)p.level);
                exp = Math.Min(exp,(int) (XP * 1.5f));
                exp = Math.Max(exp, 1);
                if (p.lv == 50) exp = 0;
                else p.getEXP(exp);
                m.update(kind, exp);
                isDead = true;
                isHit = false;
                hitTimer = 0;
                explosion.Position = Position + new Vector3(0, 0.5f, 0);
                explosion.Initialize(camera);
            }

            //Explosion Update if NPC is down
            if (isDead && active)
            {
                hitTimer++;
                explosion.Update(gameTime, camera);
                if (hitTimer > 25)
                {
                    active = false;
                    isDead = false;
                    this.explosion.Clear();
                    dmgNumbers.Clear();
                    return false;
                }
            }
            #endregion

            #region shoot

            //shoot if player is within shooting distance and not on cooldown
            if (playerDistance <= world.shootDistance && cooldown == 0)
            {
                cooldown = maxCooldn;
                Vector3 dir = (p.position - position);
                dir.Normalize();
                bullets.generate(false, position + dir, dir, 1, world.shootDistance * 2, strength, element);
                audio.playShoot(false);
            }

            if (cooldown > 0) cooldown--;
            #endregion

            #region move
            //if (moving)
                //move();
            //else

            if (playerDistance < 4)
            {
                target = position;
                direction = p.Position - this.Position;
            }
            if (!move())
            {
                {
                    pathFinder.setup(new Point((int)Math.Round((-1 * position.X + world.size - 1)), (int)Math.Round((-1 * position.Z + world.size - 1))), p);
                    target = pathFinder.findPath(kind == Constants.NPC_BOSS);
                    newTarget = true;
                    direction = target - position;
                    if (direction.Length() != 0) direction.Normalize();
                    moving = true;
                    //move();
                }
            }
            #endregion

            #region get hit billboard/dmg number
            //Hit Notification
            if (isHit)
            {
                hitTimer++;
                billboardEngine.AddBillboard(this.position + new Vector3(0, 2, 0), Color.Red, 1.5f);
                if (hitTimer == 70)
                {
                    isHit = false;
                    hitTimer = 0;
                }
            }
            #endregion

            //Rotate Model
            double rotationAngle = Math.Acos((Vector3.Dot(direction, -1 * Vector3.UnitX)) / (direction.Length()));
            rotationAngle = (p.Position.Z < this.Position.Z) ? rotationAngle * -1.0f : rotationAngle;
            model.Rotation = new Vector3(0, (float)rotationAngle, 0);

            //Update PlayerDistance
            playerDistance = (p.Position - this.Position).Length();

            return true;
        }

        #region move
        private bool move()
        {
            //Console.WriteLine("pre move : " + position.X + " ; " + position.Z + " ; direction: " + direction.X + " ; " + direction.Z);

            int xTile = (int)Math.Round((-1 * target.X + Constants.MAP_SIZE - 1) / 2);
            int zTile = (int)Math.Round((-1 * target.Z + Constants.MAP_SIZE - 1) / 2);
            //if (kind == Constants.NPC_BOSS && world.MoveData[xTile][zTile] == 2)
            //{
            //    moving = false;
            //    return;
            //}

            int factor = (int)Math.Ceiling(speed / .025f);

            float remainingSpeed = speed;
            for (int i = 0; i < factor; i++)
            {
                float spd = speed / (float)factor;
                spd = Math.Min(spd, remainingSpeed);
                bool u = moveCycle(factor, spd);
                remainingSpeed -= spd;
                //if (!u || remainingSpeed == 0) break;

                if (!u) return false;
                else if (remainingSpeed == 0) return true;
            }

            return true;
            //Console.WriteLine("post move : " + position.X + " ; " + position.Z);
        }

        private bool moveCycle(int factor, float spd)
        {
            //if (float.IsNaN(position.X)) Console.WriteLine("X is NaN cause direction is");

            if ((this.position - target).Length() < 0.02f)
            {
                this.position = target;
                //if (float.IsNaN(position.X)) Console.WriteLine("X is NaN cause target is");
                moving = false;
                return false;
                //Console.WriteLine("done: " + position.X + "/" + position.Z);
            }

            this.position += spd * direction;
            model.Position = this.position;
            return true;
        }
        #endregion

        #region get hit
        public void getHit(Bullet b, Mission m)
        {
            if (!isDead && b.fromPlayer)
            {
                int dmg = b.Strength;

                Random ran = new Random();

                dmg = (byte) (ran.Next(9*dmg, 11*dmg) * 0.1f);

                float dmgModifier = 1;

                //apply weakness and resistance
                if (b.Element != Constants.ELM_NIL && b.Element == elWeakness) dmgModifier *= 1.5f; 
                else if (b.Element != Constants.ELM_NIL && b.Element == element) dmgModifier /= 1.5f;

                if (b.Type != Constants.TYP_NIL && b.Type == typeWeakness) dmgModifier *= 1.5f;
                else if (b.Type != Constants.TYP_NIL && b.Type == typeResistance) dmgModifier /= 1.5f;

                dmg = (int)(dmg * dmgModifier);

                //apply crit chance and play hit sound
                bool crit = false;
                if (ran.Next(100) < 8)
                {
                    crit = true;
                    dmg *= 2;
                    audio.playCrit();
                }
                else audio.playNPCHit(dmgModifier);

                //cap damage between 1 and health
                dmg = Math.Max(dmg, 1);
                dmg = Math.Min(dmg, health);

                //apply damage and add to dmg out
                health -= dmg;
                m.dmgOut += dmg;

                //knockback effect
                Vector3 push = b.Direction;
                push.Normalize();
                this.position += push * 0.5f;
                moving = false;
                //isHit = true;

                //add new dmg number for new dmg
                sbyte dx = (sbyte)(ran.Next(41) - 20);
                sbyte dy = (sbyte)(ran.Next(21) - 10);
                dmgNumbers.Add(new DmgNumber(dmg, crit, dx, dy));
            }
        }
        
        public void getHit(Player p)
        {
            if (!isDead)
            {
                int dmg = p.lv;

                health = Math.Max(health - dmg, 0);
            }
        }
        #endregion

        public override void draw(Camera camera, Queue<DmgNumber> queue)
        {
            draw(camera);

            //crit number position calculation
            for (int i = 0; i < dmgNumbers.Count; i++ )
            {
                DmgNumber dmgNum = dmgNumbers[i];
                if (!dmgNum.update())
                {
                    dmgNumbers.Remove(dmgNum);
                    i--;
                    continue;
                }

                Vector3 dmgNumPos3 = graphicsDevice.Viewport.Project(Vector3.Zero,
                                                                        camera.ProjectionMatrix,
                                                                        camera.ViewMatrix,
                                                                        Matrix.CreateTranslation(position + new Vector3(0, 2, 0)));
                
                dmgNum.setPos(dmgNumPos3.X, dmgNumPos3.Y);

                queue.Enqueue(dmgNum);
            }
        }

        public override void draw(Camera camera)
        {
            if (!active)
                return;
            if (isDead && active)
                explosion.Draw(camera);
            else base.draw(camera);
        }

        #region sorting
        public float DistanceToPlayer
        {
            get { return this.playerDistance; }
        }

        public static int compareUpward(NPC npc1, NPC npc2)
        {
            if (npc1.active && !npc2.active) return -1;
            else return npc1.DistanceToPlayer.CompareTo(npc2.DistanceToPlayer);
        }

        public static int compareDownward(NPC npc1, NPC npc2)
        {
            if (npc2.active && !npc1.active) return 1;
            else return npc2.DistanceToPlayer.CompareTo(npc1.DistanceToPlayer);
        }
        #endregion
    }
}
