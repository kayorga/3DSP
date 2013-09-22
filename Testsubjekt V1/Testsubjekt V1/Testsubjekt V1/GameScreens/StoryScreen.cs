using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class StoryScreen :GameScreen
    {
        private SpriteBatch spriteBatch;
        private Texture2D cursor;
        private Texture2D userInterface;
        private Texture2D frame;
        private SpriteFont font;
        private Rectangle startRectangle;
        private Rectangle interfaceRectangle;
        private TimeSpan time;
        private String[] voice;
        private  String voiceDraw;

        
        
        public StoryScreen(ContentManager content, GraphicsDevice device, AudioManager audio, GameData data, SpriteBatch spriteBatch)
            : base(content, device, audio, data)
        {

            this.spriteBatch = spriteBatch;
            font = content.Load<SpriteFont>("Fonts/MenuFont1");
            cursor = content.Load<Texture2D>("cursor");
            frame = content.Load<Texture2D>("briefing_frame");
            userInterface = content.Load<Texture2D>("ai_interface");
            interfaceRectangle = new Rectangle(0, 0, 1024, 768);
            startRectangle = new Rectangle(120, 580, 780, 135);

            time = new TimeSpan();
            voice = new String[20];

            voice[0] = "Welcome! You must be T,\nthe new research robot.";
            voice[1] = "I am Ai, the Mainframe.\nI will be watching over your progress.";
            voice[2] = "You are a new model and must be tested,\nbefore you can go on real missions.";
            voice[3] = "Your tests in this phase\n consist of defeating enemies.";
            voice[4] = "Try to get as far as you can!";
            voice[5] = "Click to continue...";
            


        }

        private int onStartClick()
        {
            if (startRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {   
                saveGame(data);
                return Constants.CMD_NEW;
            }
        return Constants.CMD_NONE;
        }
        

        public override int update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime;

            if (time.TotalSeconds < 3) voiceDraw = voice[0];
            else if (time.TotalSeconds < 6) voiceDraw = voice[1];
            else if (time.TotalSeconds < 9) voiceDraw = voice[2];
            else if (time.TotalSeconds < 12) voiceDraw = voice[3];
            else if (time.TotalSeconds < 15) voiceDraw = voice[4];
            else if (time.TotalSeconds < 18) voiceDraw = voice[5];

            else if (onStartClick() == Constants.CMD_NEW) return Constants.CMD_NEW;

            return Constants.CMD_NONE;
        }

        public override void draw()
        {
            device.Clear(Color.DarkSlateBlue);
            drawInterface();
            spriteBatch.Begin();
            //Draw Cursor
            spriteBatch.Draw(cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 38, 50), Color.White);
            spriteBatch.End();
        }

        private void drawInterface()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(userInterface, interfaceRectangle, Color.White);
            if (voiceDraw == null) voiceDraw = " ";
            spriteBatch.DrawString(font, voiceDraw, new Vector2(150, 605), Color.LemonChiffon);
            spriteBatch.End();
        }

    }
}

    


