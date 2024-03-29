﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class MissionInfoScreen : GameScreen
    {
        private SpriteFont menuFont1;

        private Rectangle exitRectangle;
        private Rectangle interfaceRectangle;
        private Rectangle frameRectangle;
        private Rectangle baseRectangle;
        private Rectangle missionRectangle;
        private Rectangle imageRectangle;
        private Rectangle helpRectangle;

        private Texture2D cursor;
        private Texture2D userInterface;
        private Texture2D frame;
        private Texture2D forestImg;
        private Texture2D wasteImg;
        private Texture2D arcticImg;

        private SpriteBatch spriteBatch;

        private World world;
        private Camera camera;

        private int screenReturnValue = Constants.CMD_NONE;


        public MissionInfoScreen(ContentManager content, GraphicsDevice device, AudioManager audio, GameData data, World w, Camera cam)
            : base(content, device, audio, data)
        {
            //Mouse.SetPosition(512, 384);
            world = w;
            camera = cam;
            menuFont1 = content.Load<SpriteFont>("Fonts/MenuFont1");
            spriteBatch = new SpriteBatch(device);
            cursor = content.Load<Texture2D>("cursor");
            userInterface = content.Load<Texture2D>("missionInfo");
            frame = content.Load<Texture2D>("briefing_frame");
            forestImg = content.Load<Texture2D>("Icons/forrest");
            wasteImg = content.Load<Texture2D>("Icons/wasteland");
            arcticImg = content.Load<Texture2D>("Icons/artic");

            interfaceRectangle = new Rectangle(0, 0, 1024, 768);
            exitRectangle = new Rectangle(891, 78, 63, 55);
            baseRectangle = new Rectangle(412, 579, 228, 43);
            missionRectangle = new Rectangle(664, 579, 228, 43);
            imageRectangle = new Rectangle(135, 474, 191, 137);
            helpRectangle = new Rectangle(837, 81, 56, 47);
            frameRectangle = baseRectangle;
        }

        private void onExitClick()
        {
            if (exitRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                screenReturnValue = Constants.CMD_BACK;
        }

        private void onHelpClick()
        {
            if (helpRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                screenReturnValue = Constants.CMD_HELP;
                audio.playClick();
            }
        }

        private void onBaseClick()
        {
            if (baseRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = baseRectangle;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    screenReturnValue = Constants.CMD_NEW;
                    data.npcs.clear();
                    prepareWarp(0, 0);
                    data.npcs.clear();
                    data.bullets.clear();
                    //camera.reset();
                }
            }
        }
        private void onMissionClick()
        {
            if (missionRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = missionRectangle;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    screenReturnValue = Constants.CMD_BACK;
                }
            }
        }

        public override int update(GameTime gameTime)
        {
            onHelpClick();
            onExitClick();
            onBaseClick();
            onMissionClick();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                screenReturnValue = Constants.CMD_BACK;
                audio.playClick();
            }

            return screenReturnValue;
        }


        public override void draw()
        {
            world.draw(camera, device);
            data.player.draw(camera);
            data.npcs.draw(camera);
            data.bullets.draw(camera);

            spriteBatch.Begin();

            //Draw Interface
            drawInterface();

            //Draw Cursor
            spriteBatch.Draw(cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 38, 50), Color.White);

            spriteBatch.End();

        }

        private void drawInterface()
        {
            spriteBatch.Draw(userInterface, interfaceRectangle, Color.White);
            spriteBatch.Draw(frame, frameRectangle, Color.White);
            spriteBatch.DrawString(menuFont1, data.missions.activeMission.getLabel(), new Vector2(410, 200), Color.LemonChiffon);
            if (world.theme == 1)
                spriteBatch.Draw(forestImg, imageRectangle, Color.White);
            else if (world.theme == 2)
                spriteBatch.Draw(wasteImg, imageRectangle, Color.White);
            else if (world.theme == 3)
                spriteBatch.Draw(arcticImg, imageRectangle, Color.White);

        }

    }
}
