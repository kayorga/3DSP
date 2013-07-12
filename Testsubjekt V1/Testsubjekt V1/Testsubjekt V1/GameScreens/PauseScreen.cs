using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class PauseScreen : GameScreen
    {
                int index;
        private SpriteFont menuFont1;
        private Rectangle resumeRectangle;
        private Rectangle loadRectangle;
        private Rectangle exitRectangle;
        private Rectangle fadeRectangle;

        Texture2D cursor;

        private SpriteBatch spriteBatch;
        private ContentManager contentManager;
        private GraphicsDevice graphicsDevice;
        private int screenReturnValue=Constants.CMD_NONE;

        private World world;
        private GameData data;
        private Camera camera;


        public PauseScreen(ContentManager content, GraphicsDevice gD, GameData gameD, World w, Camera cam)
        {
            //Mouse.SetPosition(512, 384);
            data = gameD;
            world = w;
            camera = cam;
            menuFont1 = content.Load<SpriteFont>("Fonts/MenuFont1");
            resumeRectangle = new Rectangle(358, 290, 286, 45);
            loadRectangle = new Rectangle(402, 390, 214, 45);
            exitRectangle = new Rectangle(409, 490, 187, 45);
            graphicsDevice = gD;
            spriteBatch = new SpriteBatch(graphicsDevice);
            contentManager = content;
            cursor = content.Load<Texture2D>("cursor");
            fadeRectangle = new Rectangle(0, 0, 1024, 768);
        }

        private void onNewGameClick()
        {
            if (resumeRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                screenReturnValue = Constants.CMD_NEW;
        }

        private void onExitClick()
        {
            if (exitRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                screenReturnValue = Constants.CMD_EXIT;
        }

        private void onLoadClick()
        { 
            //TODO
        }
       
        public override int update(GameTime gameTime)
        {
            //TODO
            onExitClick();
            onLoadClick();
            onNewGameClick();
            return screenReturnValue;
        }


        public override void draw()
        {
            //TODO
            world.draw(camera, graphicsDevice);
            data.player.draw(camera);
            data.npcs.draw(camera);
            data.bullets.draw(camera);

            spriteBatch.Begin();
            //Draw Fade
            drawFade();
            //Draw Menu
            drawMenu(); 
            
            //Draw Cursor
            spriteBatch.Draw(cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 38, 50), Color.White);
            spriteBatch.End();

        }

        private void drawFade()
        {
            Texture2D texture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] color = {Color.FromNonPremultiplied(255, 255, 255, 180)};
            texture.SetData<Color>(color);
            spriteBatch.Draw(texture, fadeRectangle, Color.Black);
            
        }

        private void drawMenu()
        {

            spriteBatch.DrawString(menuFont1, "Resume Game", new Vector2(401, 295),
                                   (resumeRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y)) ? Color.Orange : Color.LemonChiffon);
            spriteBatch.DrawString(menuFont1, "Load Game", new Vector2(415, 395),
                                   (loadRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y)) ? Color.Orange : Color.LemonChiffon);
            spriteBatch.DrawString(menuFont1, "Exit Game", new Vector2(424, 495),
                                   (exitRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y)) ? Color.Orange : Color.LemonChiffon);

        }
    }
}
