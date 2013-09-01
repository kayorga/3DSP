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
        private SpriteFont menuFont1;
        private Rectangle resumeRectangle;
        private Rectangle loadRectangle;
        private Rectangle exitRectangle;
        private Rectangle interfaceRectangle;
        private Rectangle arcticRectangle;
        private Rectangle forestRectangle;
        private Rectangle caveRectangle;
        private Rectangle bossRectangle;
        private Rectangle frameRectangle;
        private Rectangle startRectangle;

        private Texture2D cursor;
        private Texture2D userInterface;
        private Texture2D frame;

        private SpriteBatch spriteBatch;
        private int screenReturnValue = Constants.CMD_NONE;
        private int activeStage = 0; //0 - forest, 1 - arctic, 2 - cave

        private World world;
        private Camera camera;


        public BriefingScreen(ContentManager content, GraphicsDevice device, AudioManager audio, GameData data, World w, Camera cam)
            : base(content, device, audio, data)
        {
            //Mouse.SetPosition(512, 384);
            world = w;
            camera = cam;
            menuFont1 = content.Load<SpriteFont>("Fonts/MenuFont1");
            resumeRectangle = new Rectangle(234, 100, 246, 45);
            loadRectangle = new Rectangle(294, 200, 224, 45);
            exitRectangle = new Rectangle(891, 78, 63, 55);
            spriteBatch = new SpriteBatch(device);
            cursor = content.Load<Texture2D>("cursor");
            userInterface = content.Load<Texture2D>("briefing_interface");
            frame = content.Load<Texture2D>("briefing_frame");
            interfaceRectangle = new Rectangle(0, 0, 1024, 768);
            forestRectangle = new Rectangle(145, 173, 200, 144);
            arcticRectangle = new Rectangle(145, 329, 200, 144);
            caveRectangle = new Rectangle(145, 485, 200, 144);
            bossRectangle = new Rectangle(379, 543, 542, 111);
            frameRectangle = new Rectangle(144, 171, 203, 149);
            startRectangle = new Rectangle(563, 464, 232, 47);

            data.missions.generate((byte)data.player.level);
            data.missions.update();
        }

        private void onNewGameClick()
        {
            if (resumeRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                screenReturnValue = Constants.CMD_BACK;
                audio.playClick();
            }
        }

        private void onExitClick()
        {
            if (exitRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                screenReturnValue = Constants.CMD_BACK;
                audio.playClick();
            }
        }


        private void onForestClick()
        {
            if (forestRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                activeStage = 0;
                frameRectangle = new Rectangle(144, 171, 203, 149);
                audio.playClick();
            }
        }
        private void onArcticClick()
        {
            if (arcticRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                activeStage = 1;
                frameRectangle = new Rectangle(144, 327, 203, 149);
                audio.playClick();
            }
        }
        private void onCaveClick()
        {
            if (caveRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                activeStage = 2;
                frameRectangle = new Rectangle(144, 483, 203, 149);
                audio.playClick();
            }
        }
        private void onBossClick()
        {
            if (bossRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                activeStage = 3;
                frameRectangle = new Rectangle(378, 542, 544, 113);
                audio.playClick();
            }
        }
        private void onStartClick()
        {
            if (startRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (data.missions[activeStage].blocked) return;
                Mission m = data.missions[activeStage];
                m.reset();
                data.missions.activeMission = m;

                camera.reset(); 
                world.warp(m.Area, m.Zone);
                world.setupSpawners(m);
                data.npcs.clear();
                data.bullets.clear();
                data.player.myWeapon.reload();
                data.missions.activeMission.reset(data.player.lv);
                screenReturnValue = Constants.CMD_NEW;
                audio.playClick();
            }
        }

        private void onKeyboard()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                screenReturnValue = Constants.CMD_MOD;
                audio.playClick();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                screenReturnValue = Constants.CMD_MOD;
                audio.playClick();
            }
        }

        public override int update(GameTime gameTime)
        {
            //TODO

            onExitClick();
            onForestClick();
            onCaveClick();
            onArcticClick();
            onBossClick();
            onStartClick();
            onKeyboard();
            return screenReturnValue;
        }


        public override void draw()
        {
            //TODO
            world.draw(camera, device);
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
