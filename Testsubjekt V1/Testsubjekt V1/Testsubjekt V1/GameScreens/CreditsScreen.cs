using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class CreditsScreen : GameScreen
    {
        private Rectangle backRectangle;
        private Rectangle screenRectangle;
        private Texture2D screenTexture;

        Texture2D cursor;

        private SpriteBatch spriteBatch;
        private int screenReturnValue = Constants.CMD_NONE;

        public CreditsScreen(ContentManager content, GraphicsDevice device, AudioManager audio, GameData data)
            : base(content, device, audio, data)
        {
            //Mouse.SetPosition(512, 384);

            screenTexture = content.Load<Texture2D>("credits");
            screenRectangle = new Rectangle(0, 0, 1024, 768);
            backRectangle = new Rectangle(0, 33, 440, 56);

            spriteBatch = new SpriteBatch(device);
            cursor = content.Load<Texture2D>("cursor");
            this.data = data;
        }

        private void onBackClick()
        {
            if (backRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            screenReturnValue = Constants.CMD_TITLE;
        }

        public override int update(GameTime gameTime)
        {
            //TODO
            onBackClick();
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

            spriteBatch.Draw(screenTexture, screenRectangle, Color.White);

            spriteBatch.End();
        }
    }
}
