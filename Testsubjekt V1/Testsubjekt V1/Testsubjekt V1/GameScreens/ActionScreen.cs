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
        private Texture2D hud;
        private Rectangle hudRectangle;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private ContentManager contentManager;

        public ActionScreen(ContentManager content, GraphicsDevice gD, GameData gameData, Camera cam, World w)
        {
            Mouse.SetPosition(512, 384);
            data = gameData;
            camera = cam;
            world = w;
            contentManager = content;
            graphicsDevice = gD;

            hud = content.Load<Texture2D>("hud");
            hudRectangle = new Rectangle(0, 0, 1024, 768);
            spriteBatch = new SpriteBatch(graphicsDevice);
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

            if (Keyboard.GetState().IsKeyDown(Keys.P))
                return Constants.CMD_PAUSE;

            if (Keyboard.GetState().IsKeyDown(Keys.J))
                return Constants.CMD_JOURNAL;

            if (Keyboard.GetState().IsKeyDown(Keys.M))
                return Constants.CMD_MOD;

            return Constants.CMD_NONE;
        }

        public override void draw()
        {
            //TODO
            world.draw(camera);
            data.player.draw(camera);
            data.npcs.draw(camera);
            data.bullets.draw(camera);
            drawHUD();
        }

        private void drawHUD()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(hud, hudRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
