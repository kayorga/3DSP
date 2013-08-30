using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TestsubjektV1
{


    class ActionScreen : GameScreen
    {
        GameData data;
        Camera camera;
        World world;
        private const int MAX_HP_WIDTH = 200;
        private Texture2D hud;
        private Texture2D HP1;
        private Texture2D HP;
        private Texture2D green;
        private Texture2D violet;
        private Texture2D red;
        private Texture2D blue;

        private Rectangle hudRectangle;
        private Rectangle HP1Rectangle;
        private Rectangle HPRectangle;
        private Rectangle Mod1Rectangle;
        private Rectangle Mod2Rectangle;
        private Rectangle Mod3Rectangle;
        private Rectangle Mod4Rectangle;
        
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private ContentManager contentManager;
        private SpriteFont font;
        private bool spawnNewEnemies;
        private TimeSpan timeInMission;

        public ActionScreen(ContentManager content, GraphicsDevice gD, GameData gameData, Camera cam, World w)
        {
            Mouse.SetPosition(512, 384);
            data = gameData;
            camera = cam;
            world = w;
            contentManager = content;
            graphicsDevice = gD;

            font = content.Load<SpriteFont>("Fonts/ModFont");

            hud = content.Load<Texture2D>("hud");
            hudRectangle = new Rectangle(0, 0, 1024, 768);
            HP1 = content.Load<Texture2D>("hp_1");
            HP1Rectangle = new Rectangle(37, 49, 30, 30);
            HP = content.Load<Texture2D>("hp");
            HPRectangle = new Rectangle(67, 56, 200, 17);
            spriteBatch = new SpriteBatch(graphicsDevice);

            red = content.Load<Texture2D>("Icons/redslot");
            blue = content.Load<Texture2D>("Icons/blueslot");
            violet = content.Load<Texture2D>("Icons/violetslot");
            green = content.Load<Texture2D>("Icons/greenslot");

            Mod1Rectangle = new Rectangle(52, 676, 40, 39);
            Mod2Rectangle = new Rectangle(91, 649, 40, 39);
            Mod3Rectangle = new Rectangle(134, 672, 40, 39);
            Mod4Rectangle = new Rectangle(93, 699, 40, 39);

            spawnNewEnemies = false;
            timeInMission = new TimeSpan(0);
            data.missions.activeMission.timeSpent = timeInMission;
        }

        public void reset()
        {
            data.missions.activeMission.actCount = 0;
            data.npcs.clear();
            data.bullets.clear();
            timeInMission = new TimeSpan(0);
            data.missions.activeMission.timeSpent = timeInMission;
        }

        public override int update(GameTime gameTime)
        {
            //TODO
            if (world.mapID != 0)
                timeInMission+=gameTime.ElapsedGameTime;
            camera.Update(gameTime, data.player.Position);
            if (!data.player.update(data.npcs, data.bullets, camera))
            {
                world.warp(0, 0);
                camera.reset();
                return Constants.CMD_NONE;
            }

            data.bullets.update(gameTime, camera, world, data.npcs, data.player, data.missions.activeMission);
            data.npcs.update(gameTime, data.bullets, camera, data.player, data.missions.activeMission);

            if (spawnNewEnemies)
                if (data.missions.activeMission == data.missions.mainMission)
                    world.update(data.npcs, data.player, data.missions.activeMission, true);
                else world.update(data.npcs, data.player, data.missions.activeMission);

            if (data.missions.activeMission != null)
            {
                if (data.missions.activeMission.complete())
                {
                    if (spawnNewEnemies)
                    {
                        spawnNewEnemies = false;
                        data.missions.activeMission.timeSpent = timeInMission;
                        data.missions.activeMission.reward(data.player, data.mods);
                        return Constants.CMD_MISSIONCOMPLETE;
                    }
                }
                else if (world.theme != 0)
                        spawnNewEnemies = true;
            }

            
            if (isConsoleInFront() == Constants.CMD_JOURNAL) return Constants.CMD_JOURNAL;

            if (Keyboard.GetState().IsKeyDown(Keys.P))
                return Constants.CMD_PAUSE;

            if (Keyboard.GetState().IsKeyDown(Keys.J))
                return Constants.CMD_MISSIONINFO;

            if (Keyboard.GetState().IsKeyDown(Keys.M))
                return Constants.CMD_MOD;
            #region DEBUG commands
            if (Constants.DEBUG)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    Console.WriteLine("Paused");
                }

                if (Keyboard.GetState().IsKeyDown(Keys.U))
                {
                    world.warp(1, 1);
                    camera.reset();
                    return Constants.CMD_NONE;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Z))
                {
                    world.warp(0, 0);
                    camera.reset();
                    return Constants.CMD_NONE;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    data.player.level = 20;
                    data.missions.clear();
                    //data.missions.mainMission.level = 0;
                    //data.missions.generate(1);
                }
                if (Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    data.player.health = data.player.maxHealth;
                    data.player.myWeapon.reload();
                }
            }
            #endregion
            
            return Constants.CMD_NONE;
        }

        private int isConsoleInFront()
        {
            if (world.theme == 0)
            {
                data.npcs.clear();
                int xtile = (int)Math.Round((-1 * data.player.position.X + Constants.MAP_SIZE - 1) / 2.0f);
                int ztile = (int)Math.Round((-1 * data.player.position.Z + Constants.MAP_SIZE - 1) / 2.0f);

                //int xtile = data.player.xTile;
                //int ztile = data.player.zTile;

                if (ztile == 14 && (xtile == 11 || xtile == 12 || xtile == 13))
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.E))
                        return Constants.CMD_JOURNAL; // activate Journal
                    return Constants.CMD_NEW; // console is only in front but nothing done
                }
            }
            return Constants.CMD_NONE; // console is not in front
        }

        public override void draw()
        {
            //TODO
            world.draw(camera, graphicsDevice);
            data.player.draw(camera);
            data.npcs.draw(camera, spriteBatch, font);
            data.bullets.draw(camera);
            drawHUD();
        }

        private void drawHUD()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(hud, hudRectangle, Color.White);
            if (data.player.health > 0)
            {
                int x1 = (int)Math.Round((-1 * data.player.position.X + Constants.MAP_SIZE - 1));
                int z1 = (int)Math.Round((-1 * data.player.position.Z + Constants.MAP_SIZE - 1));
                spriteBatch.Draw(HP1, HP1Rectangle, Color.White);
                HPRectangle.Width = (MAX_HP_WIDTH * data.player.health) / data.player.maxHealth;
                spriteBatch.DrawString(font, data.player.level.ToString(), new Vector2(79, 24), Color.LemonChiffon);
                spriteBatch.DrawString(font, data.player.XP.ToString()/*x1+" "+z1*/, new Vector2(178, 24), Color.LemonChiffon);
                spriteBatch.Draw(HP, HPRectangle, Color.White);
            }

            drawMods();

            String time = (timeInMission.Minutes<10? "0":"") + timeInMission.Minutes + ":" + (timeInMission.Seconds<10? "0":"")+timeInMission.Seconds + ":" + timeInMission.Milliseconds;
            spriteBatch.DrawString(font, time, new Vector2(894,43), Color.LemonChiffon);

            if (world.theme != 0 && data.missions.activeMission != null)
                spriteBatch.DrawString(font, data.missions.activeMission.getShortLabel(), new Vector2(800, 450), Color.LemonChiffon);
            if (isConsoleInFront()==Constants.CMD_NEW)
                spriteBatch.DrawString(font, "Press 'E' Button", new Vector2(800, 450), Color.LemonChiffon);


            spriteBatch.End();
        }

        private void drawMods()
        {
            Texture2D icon = null;
            switch (data.player.myWeapon.mods[0].type)
            {
                case Constants.MOD_NIL: icon = null; break;
                case Constants.MOD_ELM: icon = null; break;
                case Constants.MOD_TYP: icon = null; break;
                case Constants.MOD_STR: icon = red; break;
                case Constants.MOD_SPD: icon = blue; break;
                case Constants.MOD_RCG: icon = green; break;
                case Constants.MOD_ACP: icon = violet; break;
                default: icon = null; break;
            }
            if (icon != null) spriteBatch.Draw(icon, Mod1Rectangle, Color.White);
            
            switch (data.player.myWeapon.mods[1].type)
            {
                case Constants.MOD_NIL: icon = null; break;
                case Constants.MOD_ELM: icon = null; break;
                case Constants.MOD_TYP: icon = null; break;
                case Constants.MOD_STR: icon = red; break;
                case Constants.MOD_SPD: icon = blue; break;
                case Constants.MOD_RCG: icon = green; break;
                case Constants.MOD_ACP: icon = violet; break;
                default: icon = null; break;
            }
            if (icon != null) spriteBatch.Draw(icon, Mod2Rectangle, Color.White);
            
            switch (data.player.myWeapon.mods[2].type)
            {
                case Constants.MOD_NIL: icon = null; break;
                case Constants.MOD_ELM: icon = null; break;
                case Constants.MOD_TYP: icon = null; break;
                case Constants.MOD_STR: icon = red; break;
                case Constants.MOD_SPD: icon = blue; break;
                case Constants.MOD_RCG: icon = green; break;
                case Constants.MOD_ACP: icon = violet; break;
                default: icon = null; break;
            }
            if (icon != null) spriteBatch.Draw(icon, Mod3Rectangle, Color.White);

            switch (data.player.myWeapon.mods[3].type)
            {
                case Constants.MOD_NIL: icon = null; break;
                case Constants.MOD_ELM: icon = null; break;
                case Constants.MOD_TYP: icon = null; break;
                case Constants.MOD_STR: icon = red; break;
                case Constants.MOD_SPD: icon = blue; break;
                case Constants.MOD_RCG: icon = green; break;
                case Constants.MOD_ACP: icon = violet; break;
                default: icon = null; break;
            }
            if (icon != null) spriteBatch.Draw(icon, Mod4Rectangle, Color.White);
        }
    }
}
