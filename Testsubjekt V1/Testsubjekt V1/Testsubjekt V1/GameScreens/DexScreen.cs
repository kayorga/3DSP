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
    class DexScreen : GameScreen
    {
        private SpriteFont menuFont1;
        private Rectangle exitRectangle;
        private Rectangle interfaceRectangle;
        private Rectangle bossRectangle;
        private Rectangle heatRectangle;
        private Rectangle iceRectangle;
        private Rectangle plasmaRectangle;
        private Rectangle neutralRectangle;
        private Rectangle frameRectangle;
        private Rectangle activeEnemyRectangle;

        private Texture2D cursor;
        private Texture2D userInterface;
        private Texture2D frame;
        private Texture2D heatEnemy;
        private Texture2D iceEnemy;
        private Texture2D plasmaEnemy;
        private Texture2D bossEnemy;
        private Texture2D neutralEnemy;

        private SpriteBatch spriteBatch;

        private World world;
        private Camera camera;

        private String descriptionString;
        private String frameworkString;
        private Texture2D activeEnemyTexture;

        public DexScreen(ContentManager content, GraphicsDevice device, GameData data, AudioManager audio, World w, Camera cam)
            : base(content, device, audio, data)
        {
            world = w;
            camera = cam;

            menuFont1 = content.Load<SpriteFont>("Fonts/MenuFont1");
            cursor = content.Load<Texture2D>("cursor");
            userInterface = content.Load<Texture2D>("dex_interface");
            frame = content.Load<Texture2D>("briefing_frame");
            heatEnemy = content.Load<Texture2D>("Icons\\heat_enemy");
            iceEnemy = content.Load<Texture2D>("Icons\\ice_enemy");
            plasmaEnemy = content.Load<Texture2D>("Icons\\plasma_enemy");
            bossEnemy = content.Load<Texture2D>("Icons\\boss_enemy");
            neutralEnemy = content.Load<Texture2D>("Icons\\neutral_enemy");

            spriteBatch = new SpriteBatch(device);
            
            interfaceRectangle = new Rectangle(0, 0, 1024, 768);
            bossRectangle = new Rectangle(106, 495, 154, 156);
            heatRectangle = new Rectangle(270, 495, 154, 156);
            iceRectangle = new Rectangle(434, 495, 154, 156);
            plasmaRectangle = new Rectangle(598, 495, 154, 156);
            neutralRectangle = new Rectangle(762, 495, 154, 156);
            activeEnemyRectangle = new Rectangle(153, 196, 223, 223);
            exitRectangle = new Rectangle(891, 78, 63, 55);

            frameRectangle = bossRectangle;
            activeEnemyTexture = bossEnemy;

            descriptionString = "Boss\nSpecific\n++++\n+++\n+";
            frameworkString = "Name:\nType:\nHealth:\nStrength:\nSpeed:";
            
            
        }

        
        private void onBossClick()
        {
            if (bossRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                frameRectangle = bossRectangle;
                activeEnemyTexture = bossEnemy;
                descriptionString = "Cobra Commander\n???\n+++++\n+++++\n+";
            }
        }

        private void onHeatClick()
        {
            if (heatRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                frameRectangle = heatRectangle;
                activeEnemyTexture = heatEnemy;
                descriptionString = "Sun Andreas\nHeat\n+\n++++\n+++";
            }
        }

        private void onIceClick()
        {
            if (iceRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                frameRectangle = iceRectangle;
                activeEnemyTexture = iceEnemy;
                descriptionString = "Spyke Spykleton\nIce\n++++\n+\n++";
            }
        }

        private void onPlasmaClick()
        {
            if (plasmaRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                frameRectangle = plasmaRectangle;
                activeEnemyTexture = plasmaEnemy;
                descriptionString = "Doom Seed\nPlasma\n++\n+++\n+++";
            }
        }

        private void onNeutralClick()
        {
            if (neutralRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                frameRectangle = neutralRectangle;
                activeEnemyTexture = neutralEnemy;
                descriptionString = "Fogs McCloud\nNeutral\n+++\n++\n+++";
            }
        }

        private int onExitClick()
        {
            if (exitRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                return Constants.CMD_BACK;
            else return Constants.CMD_NONE;
        }
        
        public override int update(GameTime gameTime)
        {
            if (onExitClick() == Constants.CMD_BACK) return Constants.CMD_BACK;
            onBossClick();
            onHeatClick();
            onIceClick();
            onPlasmaClick();
            onNeutralClick();

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                audio.playClick();
                return Constants.CMD_MOD;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                audio.playClick();
                return Constants.CMD_JOURNAL;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                audio.playClick();
                return Constants.CMD_BACK;
                
            }
            return Constants.CMD_NONE;
        }

        public override void draw()
        {
            skybox.Draw(device, camera, data.player.Position);
            world.draw(camera, device);
            data.player.draw(camera);

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
            spriteBatch.Draw(activeEnemyTexture, activeEnemyRectangle, Color.White);
            spriteBatch.DrawString(menuFont1, frameworkString, new Vector2(410, 220), Color.LemonChiffon);
            spriteBatch.DrawString(menuFont1, descriptionString, new Vector2(610, 220), Color.LemonChiffon);

        }

    }
}
