using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class HelpScreen : GameScreen
    {
        private Rectangle backRectangle;
        private Rectangle screenRectangle;
        private Texture2D screenTexture;

        Texture2D cursor;

        private SpriteBatch spriteBatch;
        private int screenReturnValue = Constants.CMD_NONE;
        private int backCommand;

        public HelpScreen(ContentManager content, GraphicsDevice device, AudioManager audio, GameData data, int backCMD)
            : base(content, device, audio, data)
        {
            //Mouse.SetPosition(512, 384);

            screenTexture = content.Load<Texture2D>("help");
            screenRectangle = new Rectangle(0, 0, 1024, 768);
            backRectangle = new Rectangle(0, 33, 234, 56);

            spriteBatch = new SpriteBatch(device);
            cursor = content.Load<Texture2D>("cursor");
            this.data = data;
            backCommand = backCMD;
        }

        private void onBackClick()
        {
            if (backRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            screenReturnValue = backCommand;
        }

        public override int update(GameTime gameTime)
        {
            onBackClick();
            return screenReturnValue;
        }


        public override void draw()
        {
            //Draw Screen and Cursor
            spriteBatch.Begin();
            spriteBatch.Draw(screenTexture, screenRectangle, Color.White);
            spriteBatch.Draw(cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 38, 50), Color.White);
            spriteBatch.End();

        }
    }
}
