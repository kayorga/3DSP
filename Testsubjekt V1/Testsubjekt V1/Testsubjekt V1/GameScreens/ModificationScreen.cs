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
        private SpriteFont modFont;
        private Rectangle loadRectangle;
        private Rectangle exitRectangle;
        private Rectangle interfaceRectangle;
        private Rectangle frameRectangle;
        private Rectangle startRectangle;
        private Rectangle activeModRectangle;
        private Rectangle activeModTextRectangle;
        private Rectangle slot1Rectangle;
        private Rectangle slot2Rectangle;
        private Rectangle slot3Rectangle;
        private Rectangle slot4Rectangle;
        private Rectangle dragDropRectangle;
        private Rectangle[][] inventoryRectangle=new Rectangle[4][];
        private ModItem[][] inventoryItems = new ModItem[4][];
        private ModItem slot1Item;
        private ModItem slot2Item;
        private ModItem slot3Item;
        private ModItem slot4Item;

        private Texture2D cursor;
        private Texture2D userInterface;
        private Texture2D frame;
        private Texture2D i_mod_nil;
        private Texture2D i_mod_str;
        private Texture2D i_mod_spd;
        private Texture2D i_mod_rcg;
        private Texture2D i_mod_acp;


        private SpriteBatch spriteBatch;
        private ContentManager contentManager;
        private GraphicsDevice graphicsDevice;
        private int screenReturnValue = Constants.CMD_NONE;
        private int activeMod = 21; //21-24 Slot1-4, 1-20 Inventar 1-20

        private World world;
        private GameData data;
        private Camera camera;

        private ButtonState lastMouseState;

        private bool dragDropActive;

        public class ModItem
        {
            public Mod modification;
            public Texture2D icon;
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
            modFont = content.Load<SpriteFont>("Fonts/ModFont");
            loadRectangle = new Rectangle(294, 200, 224, 45);
            exitRectangle = new Rectangle(891, 78, 63, 55);
            graphicsDevice = gD;
            spriteBatch = new SpriteBatch(graphicsDevice);
            contentManager = content;
            cursor = content.Load<Texture2D>("cursor");
            userInterface = content.Load<Texture2D>("mod_interface");
            frame = content.Load<Texture2D>("briefing_frame");
            i_mod_nil = content.Load<Texture2D>("Icons/mod_nil");
            i_mod_str = content.Load<Texture2D>("Icons/mod_str");
            i_mod_spd = content.Load<Texture2D>("Icons/mod_spd");
            i_mod_rcg = content.Load<Texture2D>("Icons/mod_rcg");
            i_mod_acp = content.Load<Texture2D>("Icons/mod_acp");

            interfaceRectangle = new Rectangle(0, 0, 1024, 768);
            frameRectangle = new Rectangle(174, 334, 105, 105);
            startRectangle = new Rectangle(196, 583, 190, 67);
            slot1Rectangle = new Rectangle(174, 334, 105, 105);
            slot2Rectangle = new Rectangle(307, 334, 105, 105);
            slot3Rectangle = new Rectangle(174, 466, 105, 105);
            slot4Rectangle = new Rectangle(307, 466, 105, 105);
            activeModRectangle=new Rectangle(264,186,60,60);
            activeModTextRectangle = new Rectangle(234, 258, 117, 49);
            dragDropRectangle = new Rectangle(174, 334, 40, 40);
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

            #region build modlist
            inventoryItems[0] = new ModItem[5];
            inventoryItems[1] = new ModItem[5];
            inventoryItems[2] = new ModItem[5];
            inventoryItems[3] = new ModItem[5];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 5; j++)
                {
                    inventoryItems[i][j] = new ModItem(data.mods[i+j*4]);
                    switch (data.mods[i + j * 4].type)
                    {
                        case Constants.MOD_NIL: inventoryItems[i][j].icon = i_mod_nil; break;
                        case Constants.MOD_ELM: inventoryItems[i][j].icon = i_mod_nil; break;
                        case Constants.MOD_TYP: inventoryItems[i][j].icon = i_mod_nil; break;
                        case Constants.MOD_STR: inventoryItems[i][j].icon = i_mod_str; break;
                        case Constants.MOD_SPD: inventoryItems[i][j].icon = i_mod_spd; break;
                        case Constants.MOD_RCG: inventoryItems[i][j].icon = i_mod_rcg; break;
                        case Constants.MOD_ACP: inventoryItems[i][j].icon = i_mod_acp; break;
                        default: inventoryItems[i][j].icon = i_mod_nil; break;
                    }
                }
            if (data.player.myWeapon.mods != null)
            {
                slot1Item = new ModItem(data.player.myWeapon.mods[0]);
                switch (slot1Item.modification.type)
                {
                    case Constants.MOD_NIL: slot1Item.icon = i_mod_nil; break;
                    case Constants.MOD_ELM: slot1Item.icon = i_mod_nil; break;
                    case Constants.MOD_TYP: slot1Item.icon = i_mod_nil; break;
                    case Constants.MOD_STR: slot1Item.icon = i_mod_str; break;
                    case Constants.MOD_SPD: slot1Item.icon = i_mod_spd; break;
                    case Constants.MOD_RCG: slot1Item.icon = i_mod_rcg; break;
                    case Constants.MOD_ACP: slot1Item.icon = i_mod_acp; break;
                    default: slot1Item.icon = i_mod_nil; break;
                }
                slot2Item = new ModItem(data.player.myWeapon.mods[1]);
                switch (slot2Item.modification.type)
                {
                    case Constants.MOD_NIL: slot2Item.icon = i_mod_nil; break;
                    case Constants.MOD_ELM: slot2Item.icon = i_mod_nil; break;
                    case Constants.MOD_TYP: slot2Item.icon = i_mod_nil; break;
                    case Constants.MOD_STR: slot2Item.icon = i_mod_str; break;
                    case Constants.MOD_SPD: slot2Item.icon = i_mod_spd; break;
                    case Constants.MOD_RCG: slot2Item.icon = i_mod_rcg; break;
                    case Constants.MOD_ACP: slot2Item.icon = i_mod_acp; break;
                    default: slot2Item.icon = i_mod_nil; break;
                }
                slot3Item = new ModItem(data.player.myWeapon.mods[2]);
                switch (slot3Item.modification.type)
                {
                    case Constants.MOD_NIL: slot3Item.icon = i_mod_nil; break;
                    case Constants.MOD_ELM: slot3Item.icon = i_mod_nil; break;
                    case Constants.MOD_TYP: slot3Item.icon = i_mod_nil; break;
                    case Constants.MOD_STR: slot3Item.icon = i_mod_str; break;
                    case Constants.MOD_SPD: slot3Item.icon = i_mod_spd; break;
                    case Constants.MOD_RCG: slot3Item.icon = i_mod_rcg; break;
                    case Constants.MOD_ACP: slot3Item.icon = i_mod_acp; break;
                    default: slot3Item.icon = i_mod_nil; break;
                }
                slot4Item = new ModItem(data.player.myWeapon.mods[3]);
                switch (slot4Item.modification.type)
                {
                    case Constants.MOD_NIL: slot4Item.icon = i_mod_nil; break;
                    case Constants.MOD_ELM: slot4Item.icon = i_mod_nil; break;
                    case Constants.MOD_TYP: slot4Item.icon = i_mod_nil; break;
                    case Constants.MOD_STR: slot4Item.icon = i_mod_str; break;
                    case Constants.MOD_SPD: slot4Item.icon = i_mod_spd; break;
                    case Constants.MOD_RCG: slot4Item.icon = i_mod_rcg; break;
                    case Constants.MOD_ACP: slot4Item.icon = i_mod_acp; break;
                    default: slot4Item.icon = i_mod_nil; break;
                }
            }

            #endregion

            dragDropActive = false;

        }

        private void updateDragDrop()
        {
            if (dragDropActive)
            {
                dragDropRectangle.X = Mouse.GetState().X-20;
                dragDropRectangle.Y = Mouse.GetState().Y-20;
                if (Mouse.GetState().LeftButton == ButtonState.Released
                        && lastMouseState == ButtonState.Pressed) dragDropActive = false;
            }
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
                        activeMod = (i + 1)+j*4;
                        dragDropActive = true;
                    }

                    if (inventoryRectangle[i][j].Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Released
                        && lastMouseState == ButtonState.Pressed)
                    {
                        dragDropActive = false;
                        if (activeMod > 20)
                        {
                            ModItem token=slot1Item;
                            switch (activeMod)
                            {
                                case 21: token = slot1Item; slot1Item = inventoryItems[i][j]; break;
                                case 22: token = slot2Item; slot2Item = inventoryItems[i][j]; break;
                                case 23: token = slot3Item; slot3Item = inventoryItems[i][j]; break;
                                case 24: token = slot4Item; slot4Item = inventoryItems[i][j]; break;
                                default: break;
                                    
                            }
                            inventoryItems[i][j] = token;
                            activeMod = (i+1)+j*4;
                            frameRectangle = new Rectangle(559 + i * 87, 217 + j * 86, 68, 68);
                        }
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
                dragDropActive = true;
            }
            
            if (slot1Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Released
                && lastMouseState == ButtonState.Pressed)
            {
                if (activeMod < 21)
                {
                    dragDropActive = false;
                    ModItem token = inventoryItems[(activeMod - 1) % 4][(activeMod - 1) / 4];
                    inventoryItems[(activeMod - 1) % 4][(activeMod - 1) / 4] = slot1Item;
                    slot1Item = token;
                    activeMod = 21;
                    frameRectangle = new Rectangle(174, 334, 105, 105);
                }
                
            }
        }
        private void onSlot2Click()
        {
            if (slot2Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed
                && lastMouseState == ButtonState.Released)
            {
                dragDropActive = true;
                frameRectangle = new Rectangle(307, 334, 105, 105);
                activeMod = 22;
            }
            if (slot2Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Released
                && lastMouseState == ButtonState.Pressed)
            {
                dragDropActive = false;
                if (activeMod < 21)
                {
                    ModItem token = inventoryItems[(activeMod - 1) % 4][(activeMod - 1) / 4];
                    inventoryItems[(activeMod - 1) % 4][(activeMod - 1) / 4] = slot2Item;
                    slot2Item = token;
                    activeMod = 22;
                    frameRectangle = new Rectangle(307, 334, 105, 105);
                }
            }

        }
        private void onSlot3Click()
        {
            if (slot3Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed
                && lastMouseState == ButtonState.Released)
            {
                frameRectangle = new Rectangle(174, 466, 105, 105);
                activeMod = 23;
                dragDropActive = true;
            }
            if (slot3Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Released
                && lastMouseState == ButtonState.Pressed)
            {
                dragDropActive = false;
                if (activeMod < 21)
                {
                    ModItem token = inventoryItems[(activeMod - 1) % 4][(activeMod - 1) / 4];
                    inventoryItems[(activeMod - 1) % 4][(activeMod - 1) / 4] = slot3Item;
                    slot3Item = token;
                    activeMod = 23;
                    frameRectangle = new Rectangle(174, 466, 105, 105);
                }
            }
        }
        private void onSlot4Click()
        {
            if (slot4Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed
                && lastMouseState == ButtonState.Released)
            {
                dragDropActive = true;
                frameRectangle = new Rectangle(307, 466, 105, 105);
                activeMod = 24;
            }
            if (slot4Rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Released
                && lastMouseState == ButtonState.Pressed)
            {
                dragDropActive = false;
                if (activeMod < 21)
                {
                    ModItem token = inventoryItems[(activeMod - 1) % 4][(activeMod - 1) / 4];
                    inventoryItems[(activeMod - 1) % 4][(activeMod - 1) / 4] = slot4Item;
                    slot4Item = token;
                    activeMod = 24;
                    frameRectangle = new Rectangle(307, 466, 105, 105);
                }
            }
        }
        #endregion

        private void onExitClick()
        {
            if (exitRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                screenReturnValue = Constants.CMD_BACK;
        }

        //click finish button
        private void onStartClick()
        {
            if (startRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 5; j++)
                    {
                        data.mods[i + j * 4] = inventoryItems[i][j].modification;
                    }
                data.player.myWeapon.mods.Clear();
                data.player.myWeapon.mods.Add(slot1Item.modification);
                data.player.myWeapon.mods.Add(slot2Item.modification);
                data.player.myWeapon.mods.Add(slot3Item.modification);
                data.player.myWeapon.mods.Add(slot4Item.modification);

                data.player.myWeapon.setup(); //applies mods to weapon

                screenReturnValue = Constants.CMD_BACK;
            }
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
            updateDragDrop();

            lastMouseState = Mouse.GetState().LeftButton;
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
            drawItems();
            spriteBatch.Draw(frame, frameRectangle, Color.White);
            drawActiveMod();

            //Draw DragDrop
            if (dragDropActive)
                if(activeMod<21)
                    spriteBatch.Draw(inventoryItems[(activeMod - 1) % 4][(activeMod - 1) / 4].icon, dragDropRectangle, Color.White);
                else switch (activeMod)
                    {
                        case 21: spriteBatch.Draw(slot1Item.icon, dragDropRectangle, Color.White); break;
                        case 22: spriteBatch.Draw(slot2Item.icon, dragDropRectangle, Color.White); break;
                        case 23: spriteBatch.Draw(slot3Item.icon, dragDropRectangle, Color.White); break;
                        case 24: spriteBatch.Draw(slot4Item.icon, dragDropRectangle, Color.White); break;
                        default: break;
                    }

            //Draw Cursor
            spriteBatch.Draw(cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 38, 50), Color.White);
            spriteBatch.End();

        }

        private void drawInterface()
        {

            spriteBatch.Draw(userInterface, interfaceRectangle, Color.White);
            
        }

        private void drawItems()
        {
            for(int i = 0; i<4; i++)
                for (int j = 0; j < 5; j++)
                {
                    spriteBatch.Draw(inventoryItems[i][j].icon, inventoryRectangle[i][j], Color.White);
                }

            spriteBatch.Draw(slot1Item.icon, slot1Rectangle, Color.White);
            spriteBatch.Draw(slot2Item.icon, slot2Rectangle, Color.White);
            spriteBatch.Draw(slot3Item.icon, slot3Rectangle, Color.White);
            spriteBatch.Draw(slot4Item.icon, slot4Rectangle, Color.White);
        }

        private void drawActiveMod()
        {
            String modDescription = "";
            Texture2D icon = i_mod_nil;
            if (activeMod < 21)
            {
                modDescription = inventoryItems[(activeMod - 1) % 4][(activeMod - 1) / 4].modification.getLabel();
                icon = inventoryItems[(activeMod - 1) % 4][(activeMod - 1) / 4].icon;
            }
            else switch (activeMod)
                {
                    case 21: modDescription = slot1Item.modification.getLabel(); icon = slot1Item.icon; break;
                    case 22: modDescription = slot2Item.modification.getLabel(); icon = slot2Item.icon; break;
                    case 23: modDescription = slot3Item.modification.getLabel(); icon = slot3Item.icon; break;
                    case 24: modDescription = slot4Item.modification.getLabel(); icon = slot4Item.icon; break;
                    default: break;
                }
            spriteBatch.DrawString(modFont, modDescription, new Vector2(activeModTextRectangle.X, activeModTextRectangle.Y), Color.LemonChiffon);
            spriteBatch.Draw(icon, activeModRectangle, Color.White);


        }
    }
}
