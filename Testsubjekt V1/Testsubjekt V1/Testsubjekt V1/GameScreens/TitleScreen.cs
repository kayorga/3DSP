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
    class TitleScreen : GameScreen
    {
        int index;
        private SpriteFont menuFont1;
        private Rectangle newGameRectangle;
        private Rectangle loadRectangle;
        private Rectangle exitRectangle;

        Texture2D cursor;

        private SpriteBatch spriteBatch;
        private ContentManager contentManager;
        private GraphicsDevice graphicsDevice;
        private int screenReturnValue=Constants.CMD_NONE;


        public TitleScreen(ContentManager content, GraphicsDevice gD)
        {
            //Mouse.SetPosition(512, 384);
            menuFont1 = content.Load<SpriteFont>("Fonts/MenuFont1");
            newGameRectangle = new Rectangle(254, 100, 296, 45);
            loadRectangle = new Rectangle(294, 200, 224, 45);
            exitRectangle = new Rectangle(313, 300, 197, 45);
            graphicsDevice = gD;
            spriteBatch = new SpriteBatch(graphicsDevice);
            contentManager = content;
            cursor = content.Load<Texture2D>("cursor");
        }

        private void onNewGameClick()
        {
            if (newGameRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
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
            
            //Draw Menu
            drawMenu(); 
            
            //Draw Cursor
            spriteBatch.Begin();
            spriteBatch.Draw(cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 38, 50), Color.White);
            spriteBatch.End();

        }

        private void drawMenu()
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(menuFont1, "Start New Game", new Vector2(250, 100),
                                   (newGameRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y)) ? Color.White : Color.Orange);
            spriteBatch.DrawString(menuFont1, "Load Game", new Vector2(294, 200),
                                   (loadRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y)) ? Color.White : Color.Orange);
            spriteBatch.DrawString(menuFont1, "Exit Game", new Vector2(313, 300),
                                   (exitRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y)) ? Color.White : Color.Orange);

            spriteBatch.End();
        }
    }
}
