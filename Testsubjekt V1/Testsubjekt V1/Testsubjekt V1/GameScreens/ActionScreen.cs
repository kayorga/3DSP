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
        private Rectangle hudRectangle;
        private Rectangle HP1Rectangle;
        private Rectangle HPRectangle;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private ContentManager contentManager;
        private SpriteFont font;
        private AStar pathFinder;
        int test = 0;
        private int i=0;

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
            pathFinder = new AStar(world, data.player, new Point(7, 16));
            Vector3 goPos = pathFinder.findPath();
        }

        private void testUpdate()
        {
            Vector3 goPos = pathFinder.list.ElementAt(i);
            data.player.setPosition(goPos);
            if(i<7)i++;
        }

        public override int update(GameTime gameTime)
        {
            //TODO
            camera.Update(gameTime, data.player.Position);
            data.player.update(data.bullets, camera);
            data.bullets.update(world);
            data.missions.update(data.player.level);
            data.npcs.update(data.bullets, camera, data.player, data.missions.activeMission);
            world.update(data.npcs, data.player);

            /*test++;
            if (test == 50)
            {
                testUpdate();
                test = 0;
            }*/
            if (isConsoleInFront() == Constants.CMD_JOURNAL) return Constants.CMD_JOURNAL;

            if (Keyboard.GetState().IsKeyDown(Keys.P))
                return Constants.CMD_PAUSE;

            if (Keyboard.GetState().IsKeyDown(Keys.J))
                return Constants.CMD_JOURNAL;

            if (Keyboard.GetState().IsKeyDown(Keys.M))
                return Constants.CMD_MOD;

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

            return Constants.CMD_NONE;
        }

        private int isConsoleInFront()
        {
            if (world.theme == 0)
            {
                int xtile = (int)Math.Round(-1 * data.player.position.X + Constants.MAP_SIZE - 1) / 2;
                int ztile = (int)Math.Round(-1 * data.player.position.Z + Constants.MAP_SIZE - 1) / 2;

                if (ztile == 14 && (xtile == 11 || xtile == 12 || xtile == 13))
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.E))
                        return Constants.CMD_JOURNAL;
                }
            }
            return Constants.CMD_NONE;
        }

        public override void draw()
        {
            //TODO
            world.draw(camera, graphicsDevice);
            data.player.draw(camera);
            data.npcs.draw(camera);
            data.bullets.draw(camera);
            drawHUD();
        }

        private void drawHUD()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(hud, hudRectangle, Color.White);
            if (data.player.health > 0)
            {
                int x1 = (int)Math.Round(-1 * data.player.position.X + Constants.MAP_SIZE - 1) / 2;
                int z1 = (int)Math.Round(-1 * data.player.position.Z + Constants.MAP_SIZE - 1) / 2;
                spriteBatch.Draw(HP1, HP1Rectangle, Color.White);
                HPRectangle.Width = (MAX_HP_WIDTH * data.player.health) / data.player.maxHealth;
                spriteBatch.DrawString(font, data.player.level.ToString(), new Vector2(79, 24), Color.LemonChiffon);
                spriteBatch.DrawString(font, x1+" "+z1/*"0 / 76"*/, new Vector2(178, 24), Color.LemonChiffon);
                spriteBatch.Draw(HP, HPRectangle, Color.White);
            }
            spriteBatch.End();
        }
    }
}
