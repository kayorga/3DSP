using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class GameOverScreen : GameScreen
    {
        private Rectangle baseRectangle;
        private Rectangle interfaceRectangle;
        private Rectangle fadeRectangle;

        Texture2D cursor;
        private Texture2D userInterface;

        private SpriteBatch spriteBatch;
        private int screenReturnValue = Constants.CMD_NONE;

        private World world;
        private Camera camera;


        public GameOverScreen(ContentManager content, GraphicsDevice device, AudioManager audio, GameData data, World w, Camera cam)
            : base(content, device, audio, data)
        {
            //Mouse.SetPosition(512, 384);
            world = w;
            camera = cam;
            baseRectangle = new Rectangle(390, 475, 245, 50);

            spriteBatch = new SpriteBatch(device);
            cursor = content.Load<Texture2D>("cursor");
            fadeRectangle = new Rectangle(0, 0, 1024, 768);

            userInterface = content.Load<Texture2D>("gameover");
            interfaceRectangle = new Rectangle(0, 0, 1024, 768);
        }

        private void onBaseClick()
        {
            if (baseRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    //world.warp(0, 0);
                    prepareWarp(0, 0);
                    data.npcs.clear();
                    data.bullets.clear();
                    //camera.reset();
                    screenReturnValue = Constants.CMD_NEW;
                }
            }
        }
        
        public override int update(GameTime gameTime)
        {
            onBaseClick();
            return screenReturnValue;
        }


        public override void draw()
        {
            world.draw(camera, device);
            data.player.draw(camera);
            data.npcs.draw(camera);
            data.bullets.draw(camera);

            spriteBatch.Begin();
            //Draw Fade
            drawFade();
            //Draw Menu
            spriteBatch.Draw(userInterface, interfaceRectangle, Color.White);

            //Draw Cursor
            spriteBatch.Draw(cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 38, 50), Color.White);
            spriteBatch.End();

        }

        private void drawFade()
        {
            Texture2D texture = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            Color[] color = { Color.FromNonPremultiplied(255, 255, 255, 180) };
            texture.SetData<Color>(color);
            spriteBatch.Draw(texture, fadeRectangle, Color.Black);
        }

    }
}
