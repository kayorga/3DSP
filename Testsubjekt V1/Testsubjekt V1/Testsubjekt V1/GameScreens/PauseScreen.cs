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
            resumeRectangle = new Rectangle(234, 100, 246, 45);
            loadRectangle = new Rectangle(294, 200, 224, 45);
            exitRectangle = new Rectangle(313, 300, 197, 45);
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
            world.draw(camera);
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
            
            spriteBatch.DrawString(menuFont1, "Resume Game", new Vector2(280, 100),
                                   (resumeRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y)) ? Color.White : Color.Orange);
            spriteBatch.DrawString(menuFont1, "Load Game", new Vector2(294, 200),
                                   (loadRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y)) ? Color.White : Color.Orange);
            spriteBatch.DrawString(menuFont1, "Exit Game", new Vector2(313, 300),
                                   (exitRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y)) ? Color.White : Color.Orange);

        }
    }
}
