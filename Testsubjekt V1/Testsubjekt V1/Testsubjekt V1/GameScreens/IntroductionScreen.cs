using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class IntroductionScreen : GameScreen
    {
        private TimeSpan time;
        private String[] onesAndZeroes;
        private String onesAndZeroesDraw;
        private String voiceDraw;
        private String[] voice;
        private SpriteBatch spriteBatch;
        private int onesAndZeroesDrawTimer;
        private SpriteFont font;
        private Texture2D userInterface;
        private Rectangle interfaceRectangle;
        private Texture2D heatIcon;
        private Texture2D plasmaIcon;
        private Texture2D iceIcon;
        private Rectangle heatRectangle;
        private Rectangle plasmaRectangle;
        private Rectangle iceRectangle;
        private Rectangle startRectangle;
        private Texture2D cursor;
        private bool isElementSelected;
        private byte selectedElement; //0 heat, 1 plasma, 2 ice


        public IntroductionScreen(ContentManager content, GraphicsDevice device, AudioManager audio, GameData data, SpriteBatch spriteBatch)
            : base(content, device, audio, data)
        {
            this.spriteBatch = spriteBatch;
            
            font = content.Load<SpriteFont>("Fonts/MenuFont1");
            userInterface = content.Load<Texture2D>("Introduction");
            heatIcon = content.Load<Texture2D>("Icons/mod_hea");
            plasmaIcon = content.Load<Texture2D>("Icons/mod_pla");
            iceIcon = content.Load<Texture2D>("Icons/mod_ice");
            cursor = content.Load<Texture2D>("cursor");

            time = new TimeSpan();
            interfaceRectangle = new Rectangle(0, 0, 1024, 768);
            heatRectangle = new Rectangle(265, 256, 150, 150);
            plasmaRectangle = new Rectangle(435, 256, 150, 150);
            iceRectangle = new Rectangle(605, 256, 150, 150);
            startRectangle = new Rectangle(120, 580, 780, 135);
            onesAndZeroes = new String[20];
            voice = new String[20];
            Random random = new Random();
            onesAndZeroesDrawTimer = 0;
            selectedElement = 4;
            
            for (int i=0; i<20;i++)
            {
                onesAndZeroes[i] = "";
                for (int j = 0; j < 50; j++)
                {
                    onesAndZeroes[i] += random.Next(2);
                }
            }

            voice[0] = "uploading program files";
            voice[1] = "connecting to main computer";
            voice[2] = "initiate training program";
            voice[3] = "testsubject is running now";
            voice[4] = "select an element of your choice";
            voice[5] = "heat element \nclick here to start training program";
            voice[6] = "plasma element \nclick here to start training program";
            voice[7] = "ice element. \nclick here to start training program";

        }

        private void onHeatClick()
        {
            if (heatRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                isElementSelected = true;
                selectedElement = 0;
                voiceDraw = voice[5];
                audio.playClick();
            }
        }
        private void onPlasmaClick()
        {
            if (plasmaRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                isElementSelected = true;
                selectedElement = 1;
                voiceDraw = voice[6];
                audio.playClick();
            }
        }
        private void onIceClick()
        {
            if (iceRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                isElementSelected = true;
                selectedElement = 2;
                voiceDraw = voice[7];
                audio.playClick();
            }
        }
        private int onStartClick()
        {
            if (startRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                audio.playClick();
                switch (selectedElement)
                {
                    case 0: data.mods.generate((int)Constants.ELM_HEA, Constants.MOD_ELM); break;
                    case 1: data.mods.generate((int)Constants.ELM_PLA, Constants.MOD_ELM); break;
                    case 2: data.mods.generate((int)Constants.ELM_ICE, Constants.MOD_ELM); break;
                    default: break;
                }
                data.mods.firstMod = data.mods[0];
                data.mods.setupESpecials();
                return Constants.CMD_NEW;
            }
            return Constants.CMD_NONE;
        }

        public override int update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime;
            onesAndZeroesDrawTimer++;

            if (time.TotalSeconds < 2) voiceDraw = voice[0];
            else if (time.TotalSeconds < 4) voiceDraw = voice[1];
            else if (time.TotalSeconds < 6) voiceDraw = voice[2];
            else if (time.TotalSeconds < 8) voiceDraw = voice[3];
            else if (time.TotalSeconds < 10) voiceDraw = voice[4];
            else
            {
                if (!isElementSelected) voiceDraw = voice[4];
                onHeatClick();
                onIceClick();
                onPlasmaClick();
                if (isElementSelected)
                {
                    if (onStartClick() == Constants.CMD_NEW) return Constants.CMD_NEW;
                }
            }

            if (time.TotalSeconds < 10)
            {
                if (onesAndZeroesDrawTimer % 80 > 20) voiceDraw += ".";
                if (onesAndZeroesDrawTimer % 80 > 40) voiceDraw += ".";
                if (onesAndZeroesDrawTimer % 80 > 60) voiceDraw += ".";
            }
 
            int countRow = 0;
            int countChar = 0;
            onesAndZeroesDraw = "";
            for (int i = 0; i < Math.Min(onesAndZeroesDrawTimer, 20 * 50); i++)
            {
                if (i % 50 == 0 && i != 0)
                {
                    countRow++;
                    countChar = 0;
                    onesAndZeroesDraw += "\n";
                }
               
                onesAndZeroesDraw += onesAndZeroes[countRow][countChar];
                countChar++;
            }

            return Constants.CMD_NONE;
        }

        public override void draw()
        {
            device.Clear(Color.DarkSlateBlue);
            drawBackground();
            drawInterface();
            spriteBatch.Begin();
            //Draw Cursor
            spriteBatch.Draw(cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 38, 50), Color.White);
            spriteBatch.End();
        }

        private void drawBackground()
        {
            spriteBatch.Begin();
            if (onesAndZeroesDraw == null) onesAndZeroesDraw = " ";
            spriteBatch.DrawString(font, onesAndZeroesDraw, new Vector2(28, 15), Color.LightBlue);
            spriteBatch.End();
        }

        private void drawInterface()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(userInterface, interfaceRectangle, Color.White);
            if (voiceDraw == null) voiceDraw = " ";
            spriteBatch.DrawString(font, voiceDraw, new Vector2(150, 605), Color.LemonChiffon);
            if (time.TotalSeconds > 8)
            {
                spriteBatch.Draw(heatIcon, heatRectangle, Color.White);
                spriteBatch.Draw(plasmaIcon, plasmaRectangle, Color.White);
                spriteBatch.Draw(iceIcon, iceRectangle, Color.White);
            }
            spriteBatch.End();
        }

        private void drawVoice()
        {

        }
    }
}
