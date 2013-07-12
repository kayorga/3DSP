﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class PauseScreen : GameScreen
    {
        int index;
        private SpriteFont menuFont1;
        private Rectangle resumeRectangle;
        private Rectangle saveRectangle;
        private Rectangle exitRectangle;
        private Rectangle modRectangle;
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
        private ContentManager contentManager;
        private GraphicsDevice graphicsDevice;
        private int screenReturnValue=Constants.CMD_NONE;

        private World world;
        private GameData data;
        private Camera camera;


        public PauseScreen(ContentManager content, GraphicsDevice gD, GameData gameD, World w, Camera cam)
        {
            //Mouse.SetPosition(512, 384);
            data = gameD;
            world = w;
            camera = cam;
            menuFont1 = content.Load<SpriteFont>("Fonts/MenuFont1");
            resumeRectangle = new Rectangle(725, 76, 63, 58);
            saveRectangle = new Rectangle(402, 225, 228, 43);
            exitRectangle = new Rectangle(402, 505, 228, 43);
            modRectangle = new Rectangle(402, 393, 228, 43);
            charRectangle = new Rectangle(402, 281, 228, 43);
            missionRectangle = new Rectangle(402, 337, 228, 43);
            baseRectangle = new Rectangle(402, 449, 228, 43);
            frameRectangle = saveRectangle;
            
            graphicsDevice = gD;
            spriteBatch = new SpriteBatch(graphicsDevice);
            contentManager = content;
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
                    screenReturnValue = Constants.CMD_NEW;
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
            //TODO
        }

        private void onBaseClick()
        {
            if (baseRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = baseRectangle;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    world.warp(0, 0);
                    camera.reset();
                    screenReturnValue = Constants.CMD_NEW;
                }
            }
        }
        private void onModClick()
        {
            if (modRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = modRectangle;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    screenReturnValue = Constants.CMD_MOD;
            }
        }
        private void onCharClick()
        {
            if (charRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = charRectangle;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    screenReturnValue = Constants.CMD_NEW;
            }
        }
        private void onMissionClick()
        {
            if (missionRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                frameRectangle = missionRectangle;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    screenReturnValue = Constants.CMD_JOURNAL;
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
            onModClick();
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
            Texture2D texture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] color = {Color.FromNonPremultiplied(255, 255, 255, 180)};
            texture.SetData<Color>(color);
            spriteBatch.Draw(texture, fadeRectangle, Color.Black);
            
        }

    }
}
