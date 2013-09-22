using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class PauseScreen : GameScreen
    {
        private SpriteFont menuFont1;
        private Rectangle resumeRectangle;
        private Rectangle saveRectangle;
        private Rectangle exitRectangle;
        private Rectangle titleRectangle;
        private Rectangle charRectangle;
        private Rectangle missionRectangle;
        private Rectangle fadeRectangle;
        private Rectangle baseRectangle;
        private Rectangle interfaceRectangle;
        private Rectangle frameRectangle;


        Texture2D cursor;
        private Texture2D userInterface;
        private Texture2D frame;

        private SpriteBatch spriteBatch;
        private int screenReturnValue=Constants.CMD_NONE;

        private World world;
        private Camera camera;


        public PauseScreen(ContentManager content, GraphicsDevice device, AudioManager audio, GameData data, World w, Camera cam)
            : base(content, device, audio, data)
        {
            //Mouse.SetPosition(512, 384);
            world = w;
            camera = cam;
            menuFont1 = content.Load<SpriteFont>("Fonts/MenuFont1");
            resumeRectangle = new Rectangle(725, 76, 63, 58);
            saveRectangle = new Rectangle(402, 225, 228, 43);
            exitRectangle = new Rectangle(402, 505, 228, 43);
            baseRectangle = new Rectangle(402, 393, 228, 43);
            charRectangle = new Rectangle(402, 281, 228, 43);
            missionRectangle = new Rectangle(402, 337, 228, 43);
            titleRectangle = new Rectangle(402, 449, 228, 43);
            frameRectangle = saveRectangle;
            
            spriteBatch = new SpriteBatch(device);
            cursor = content.Load<Texture2D>("cursor");
            fadeRectangle = new Rectangle(0, 0, 1024, 768);

            userInterface = content.Load<Texture2D>("PauseMenu");
            frame = content.Load<Texture2D>("briefing_frame");
            interfaceRectangle = new Rectangle(0, 0, 1024, 768);
        }

        private void onNewGameClick()
        {
            if (resumeRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    screenReturnValue = Constants.CMD_BACK;
            }
        }

        private void onExitClick()
        {
            if (exitRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = exitRectangle;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    screenReturnValue = Constants.CMD_EXIT;
            }
        }

        private void onSaveClick()
        {
            if (saveRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = saveRectangle;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    saveGame(data);
                    screenReturnValue = Constants.CMD_BACK;
                }
            }
        }

        private void onBaseClick()
        {
            if (baseRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = baseRectangle;
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
        private void onTitleClick()
        {
            if (titleRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = titleRectangle;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    screenReturnValue = Constants.CMD_TITLE;
            }
        }
        private void onCharClick()
        {
            if (charRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = charRectangle;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    screenReturnValue = Constants.CMD_CHARINFO;
            }
        }
        private void onMissionClick()
        {
            if (missionRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = missionRectangle;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    screenReturnValue = Constants.CMD_MISSIONINFO;
            }
        }
       
        public override int update(GameTime gameTime)
        {
            onSaveClick();
            onExitClick();
            onNewGameClick();
            onBaseClick();
            onCharClick();
            onMissionClick();
            onTitleClick();
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
            spriteBatch.Draw(frame, frameRectangle, Color.White);
            
            //Draw Cursor
            spriteBatch.Draw(cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 38, 50), Color.White);
            spriteBatch.End();

        }

        private void drawFade()
        {
            Texture2D texture = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            Color[] color = {Color.FromNonPremultiplied(255, 255, 255, 180)};
            texture.SetData<Color>(color);
            spriteBatch.Draw(texture, fadeRectangle, Color.Black);
        }

    }
}
