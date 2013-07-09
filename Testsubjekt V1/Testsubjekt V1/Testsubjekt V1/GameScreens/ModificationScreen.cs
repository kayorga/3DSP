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
    class ModificationScreen : GameScreen
    {
        int index;
        private SpriteFont menuFont1;
        private Rectangle loadRectangle;
        private Rectangle exitRectangle;
        private Rectangle interfaceRectangle;
        private Rectangle frameRectangle;
        private Rectangle startRectangle;
        private Rectangle activeModRectangle;
        private Rectangle slot1Rectangle;
        private Rectangle slot2Rectangle;
        private Rectangle slot3Rectangle;
        private Rectangle slot4Rectangle;
        private Rectangle[][] inventoryRectangle=new Rectangle[4][];

        private Texture2D cursor;
        private Texture2D userInterface;
        private Texture2D frame;

        private SpriteBatch spriteBatch;
        private ContentManager contentManager;
        private GraphicsDevice graphicsDevice;
        private int screenReturnValue = Constants.CMD_NONE;
        private int activeMod = 21; //21-24 Slot1-4, 1-20 Inventar 1-20

        private World world;
        private GameData data;
        private Camera camera;

        private ButtonState lastMouseState;

        public class ModItem
        {
            private Mod modification;
            private Rectangle clickPosition;
            private Rectangle imagePosition;
            private Rectangle textPosition;
            private int intentoryPosition = 21; //21-24 Slot1-4, 1-20 Inventar 1-20

            public ModItem(Mod mod)
            {
                modification = mod;
            }
        }


        public ModificationScreen(ContentManager content, GraphicsDevice gD, GameData gameD, World w, Camera cam)
        {
            //Mouse.SetPosition(512, 384);
            data = gameD;
            world = w;
            camera = cam;
            lastMouseState = ButtonState.Released;
            menuFont1 = content.Load<SpriteFont>("Fonts/MenuFont1");
            loadRectangle = new Rectangle(294, 200, 224, 45);
            exitRectangle = new Rectangle(891, 78, 63, 55);
            graphicsDevice = gD;
            spriteBatch = new SpriteBatch(graphicsDevice);
            contentManager = content;
            cursor = content.Load<Texture2D>("cursor");
            userInterface = content.Load<Texture2D>("mod_interface");
            frame = content.Load<Texture2D>("briefing_frame");
            interfaceRectangle = new Rectangle(0, 0, 1024, 768);
            frameRectangle = new Rectangle(174, 334, 105, 105);
            startRectangle = new Rectangle(196, 583, 190, 67);
            slot1Rectangle = new Rectangle(174, 334, 105, 105);
            slot2Rectangle = new Rectangle(307, 334, 105, 105);
            slot3Rectangle = new Rectangle(174, 466, 105, 105);
            slot4Rectangle = new Rectangle(307, 466, 105, 105);
            #region Inventory Rectangles
            inventoryRectangle[0] = new Rectangle[5];
            inventoryRectangle[1] = new Rectangle[5];
            inventoryRectangle[2] = new Rectangle[5];
            inventoryRectangle[3] = new Rectangle[5];
            for(int i = 0; i<4; i++)
                for (int j = 0; j < 5; j++)
                {
                    inventoryRectangle[i][j]=new Rectangle(559+i*87, 217+j*86, 68, 68);
                }
            #endregion
        }

        private void onInventoryClick()
        {
            for(int i = 0; i<4; i++)
                for (int j = 0; j < 5; j++)
                {
                    if (inventoryRectangle[i][j].Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed
                        &&lastMouseState==ButtonState.Released)
                    {
                        frameRectangle = new Rectangle(559 + i * 87, 217 + j * 86, 68, 68);
                        activeMod = (i + 1) * (j + 1);
                    }
                }
        }
        #region SlotClick
        private void onSlot1Click()
        {
            if (slot1Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed
                && lastMouseState == ButtonState.Released)
            {
                frameRectangle = new Rectangle(174, 334, 105, 105);
                activeMod = 21;
            }
            if (slot1Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Released
                && lastMouseState == ButtonState.Pressed)
            {
                //TODO swap entities
            }
        }
        private void onSlot2Click()
        {
            if (slot2Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed
                && lastMouseState == ButtonState.Released)
            {
                frameRectangle = new Rectangle(307, 334, 105, 105);
                activeMod = 22;
            }
            if (slot2Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Released
                && lastMouseState == ButtonState.Pressed)
            {
                //TODO swap entities
            }

        }
        private void onSlot3Click()
        {
            if (slot3Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed
                && lastMouseState == ButtonState.Released)
            {
                frameRectangle = new Rectangle(174, 466, 105, 105);
                activeMod = 23;
            }
            if (slot3Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Released
                && lastMouseState == ButtonState.Pressed)
            {
                //TODO swap entities
            }
        }
        private void onSlot4Click()
        {
            if (slot4Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed
                && lastMouseState == ButtonState.Released)
            {
                frameRectangle = new Rectangle(307, 466, 105, 105);
                activeMod = 24;
            }
            if (slot4Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Released
                && lastMouseState == ButtonState.Pressed)
            {
                //TODO swap entities
            }
        }
        #endregion

        private void onExitClick()
        {
            if (exitRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                screenReturnValue = Constants.CMD_NEW;
        }

        private void onStartClick()
        {
            if (startRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                screenReturnValue = Constants.CMD_NEW;
        }

        public override int update(GameTime gameTime)
        {                      
            onExitClick();
            onStartClick();
            onInventoryClick();
            onSlot1Click();
            onSlot2Click();
            onSlot3Click();
            onSlot4Click();

            lastMouseState = Mouse.GetState().LeftButton;
            return screenReturnValue;
        }


        public override void draw()
        {
            //TODO
            world.draw(camera);
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

        }

        private void drawMenu()
        {

            //spriteBatch.DrawString(menuFont1, "Resume Game", new Vector2(280, 100),
            //                     (resumeRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y)) ? Color.Orange : Color.LemonChiffon);

        }
    }
}
