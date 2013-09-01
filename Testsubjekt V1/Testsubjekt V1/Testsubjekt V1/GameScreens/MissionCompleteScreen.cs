using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class MissionCompleteScreen : GameScreen
    {
        private SpriteFont menuFont1;

        private Rectangle exitRectangle;
        private Rectangle interfaceRectangle;
        private Rectangle frameRectangle;
        private Rectangle baseRectangle;
        private Rectangle exploreRectangle;
        private Rectangle imageRectangle;

        private Texture2D cursor;
        private Texture2D userInterface;
        private Texture2D frame;
        private Texture2D forestImg;

        private SpriteBatch spriteBatch;

        private World world;
        private Camera camera;

        private int screenReturnValue = Constants.CMD_NONE;


        public MissionCompleteScreen(ContentManager content, GraphicsDevice device, AudioManager audio, GameData data, World w, Camera cam)
            : base(content, device, audio, data)
        {
            //Mouse.SetPosition(512, 384);
            world = w;
            camera = cam;
            menuFont1 = content.Load<SpriteFont>("Fonts/MenuFont1");
            spriteBatch = new SpriteBatch(device);
            cursor = content.Load<Texture2D>("cursor");
            userInterface = content.Load<Texture2D>("missionCompleteInterface");
            frame = content.Load<Texture2D>("briefing_frame");
            forestImg = content.Load<Texture2D>("Icons/forrest");
            
            interfaceRectangle = new Rectangle(0, 0, 1024, 768);
            exitRectangle = new Rectangle(891, 78, 63, 55);
            baseRectangle = new Rectangle(412,579,228,43);
            exploreRectangle = new Rectangle(664, 579, 228, 43);
            imageRectangle =new Rectangle(135,474,191,137);
            frameRectangle = baseRectangle;
        }

        private void onExitClick()
        {
            if (exitRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                screenReturnValue = Constants.CMD_BACK;
        }

        private void onBaseClick()
        {
            if (baseRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = baseRectangle;
                if(Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    screenReturnValue = Constants.CMD_NEW;
                    data.npcs.clear();
                    world.warp(0, 0);
                    data.npcs.clear();
                    data.bullets.clear();
                    camera.reset();
                }
            }
        }
        private void onExploreClick()
        {
            if (exploreRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = exploreRectangle;
                if(Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    screenReturnValue = Constants.CMD_BACK;
                }
            }
        }

        public override int update(GameTime gameTime)
        {
            //TODO

            onExitClick();
            onBaseClick();
            onExploreClick();
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
            
                //Draw Cursor
                spriteBatch.Draw(cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 38, 50), Color.White);
            
            spriteBatch.End();

        }

        private void drawInterface()
        {

            spriteBatch.Draw(userInterface, interfaceRectangle, Color.White);
            spriteBatch.Draw(frame, frameRectangle, Color.White);
            String time = (data.missions.activeMission.timeSpent.Minutes < 10 ? "0" : "") + data.missions.activeMission.timeSpent.Minutes + ":" + (data.missions.activeMission.timeSpent.Seconds < 10 ? "0" : "") + data.missions.activeMission.timeSpent.Seconds + ":" + data.missions.activeMission.timeSpent.Milliseconds;
            spriteBatch.DrawString(menuFont1, time, new Vector2(693, 205), Color.LemonChiffon);
            spriteBatch.DrawString(menuFont1, data.missions.activeMission.countKilledEnemies.ToString(), new Vector2(693, 235), Color.LemonChiffon);
            spriteBatch.DrawString(menuFont1, data.missions.activeMission.countXPGained.ToString(), new Vector2(693, 355), Color.LemonChiffon);
            spriteBatch.DrawString(menuFont1, data.missions.activeMission.dmgOut.ToString(), new Vector2(693, 265), Color.LemonChiffon);
            spriteBatch.DrawString(menuFont1, data.missions.activeMission.dmgIn.ToString(), new Vector2(693, 295), Color.LemonChiffon);
            spriteBatch.DrawString(menuFont1, data.missions.activeMission.Reward, new Vector2(693, 385), Color.LemonChiffon);
            if (world.theme == 1)
                spriteBatch.Draw(forestImg, imageRectangle, Color.White);

        }

    }
}
