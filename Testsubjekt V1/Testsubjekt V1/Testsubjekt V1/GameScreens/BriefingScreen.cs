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
    class BriefingScreen : GameScreen
    {
        int index;
        private SpriteFont menuFont1;
        private Rectangle resumeRectangle;
        private Rectangle loadRectangle;
        private Rectangle exitRectangle;
        private Rectangle interfaceRectangle;
        private Rectangle arcticRectangle;
        private Rectangle forestRectangle;
        private Rectangle caveRectangle;
        private Rectangle frameRectangle;
        private Rectangle startRectangle;
        private Rectangle briefingRectangle;

        private Texture2D cursor;
        private Texture2D userInterface;
        private Texture2D frame;

        private SpriteBatch spriteBatch;
        private ContentManager contentManager;
        private GraphicsDevice graphicsDevice;
        private int screenReturnValue = Constants.CMD_NONE;
        private int activeStage = 0; //0 - forest, 1 - arctic, 2 - cave

        private World world;
        private GameData data;
        private Camera camera;


        public BriefingScreen(ContentManager content, GraphicsDevice gD, GameData gameD, World w, Camera cam)
        {
            //Mouse.SetPosition(512, 384);
            data = gameD;
            world = w;
            camera = cam;
            menuFont1 = content.Load<SpriteFont>("Fonts/MenuFont1");
            resumeRectangle = new Rectangle(234, 100, 246, 45);
            loadRectangle = new Rectangle(294, 200, 224, 45);
            exitRectangle = new Rectangle(891, 78, 63, 55);
            graphicsDevice = gD;
            spriteBatch = new SpriteBatch(graphicsDevice);
            contentManager = content;
            cursor = content.Load<Texture2D>("cursor");
            userInterface = content.Load<Texture2D>("briefing_interface");
            frame = content.Load<Texture2D>("briefing_frame");
            interfaceRectangle = new Rectangle(0, 0, 1024, 768);
            forestRectangle = new Rectangle(145, 173, 200, 144);
            arcticRectangle = new Rectangle(145, 329, 200, 144);
            caveRectangle = new Rectangle(145, 485, 200, 144);
            frameRectangle = new Rectangle(144, 171, 203, 149);
            startRectangle = new Rectangle(563, 464, 232, 47);

            data.missions.generate(0);
        }

        private void onNewGameClick()
        {
            if (resumeRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                screenReturnValue = Constants.CMD_BACK;
        }

        private void onExitClick()
        {
            if (exitRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                screenReturnValue = Constants.CMD_BACK;
        }


        private void onForestClick()
        {
            if (forestRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                activeStage = 0;
                frameRectangle = new Rectangle(144, 171, 203, 149);
            }
        }
        private void onArcticClick()
        {
            if (arcticRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                activeStage = 1;
                frameRectangle = new Rectangle(144, 327, 203, 149);
            }
        }
        private void onCaveClick()
        {
            if (caveRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                activeStage = 2;
                frameRectangle = new Rectangle(144, 483, 203, 149);
            }
        }
        private void onStartClick()
        {
            if (startRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                camera.reset();  world.warp(1, 1);
                data.missions.activeMission = data.missions[activeStage];
                screenReturnValue = Constants.CMD_NEW;
            }
        }

        public override int update(GameTime gameTime)
        {
            //TODO

            onExitClick();
            onForestClick();
            onCaveClick();
            onArcticClick();
            onStartClick();
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
            //Draw Interface
            drawInterface();
            //Draw Menu
            drawMenu();

            //Draw Cursor
            spriteBatch.Draw(cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 38, 50), Color.White);
            spriteBatch.End();

        }

        private void drawInterface()
        {
            
            spriteBatch.Draw(userInterface, interfaceRectangle, Color.White);
            spriteBatch.Draw(frame, frameRectangle, Color.White);
            spriteBatch.DrawString(menuFont1, data.missions[activeStage].getLabel(), new Vector2(410, 200), Color.LemonChiffon);

        }

        private void drawMenu()
        {

            //spriteBatch.DrawString(menuFont1, "Resume Game", new Vector2(280, 100),
              //                     (resumeRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y)) ? Color.Orange : Color.LemonChiffon);
            
        }
    }
}
