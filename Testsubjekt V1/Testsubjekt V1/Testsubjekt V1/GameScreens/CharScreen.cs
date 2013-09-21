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
    class CharScreen : GameScreen
    {
        private SpriteFont menuFont1;
        private Rectangle exitRectangle;
        private Rectangle interfaceRectangle;
        private Rectangle Mod1Rectangle;
        private Rectangle Mod2Rectangle;
        private Rectangle Mod3Rectangle;
        private Rectangle Mod4Rectangle;
        private Rectangle[] ModRectangles;

        private SpriteBatch spriteBatch;

        private World world;
        private Camera camera;

        private Texture2D green;
        private Texture2D violet;
        private Texture2D red;
        private Texture2D blue;
        private Texture2D darkblue;
        private Texture2D lightgreen;
        private Texture2D foxred;
        private Texture2D white;
        private Texture2D cyan;
        private Texture2D orange;
        private Texture2D cursor;
        private Texture2D userInterface;
        
        public CharScreen(ContentManager content, GraphicsDevice device, GameData data, AudioManager audio, World w, Camera cam)
            : base(content, device, audio, data)
        {
            world = w;
            camera = cam;

            menuFont1 = content.Load<SpriteFont>("Fonts/MenuFont1");
            cursor = content.Load<Texture2D>("cursor");
            userInterface = content.Load<Texture2D>("CharInfo");
            
            //stat mod icons
            red = content.Load<Texture2D>("Icons/redslot");
            blue = content.Load<Texture2D>("Icons/blueslot");
            violet = content.Load<Texture2D>("Icons/violetslot");
            green = content.Load<Texture2D>("Icons/greenslot");

            //type mod icons
            cyan = content.Load<Texture2D>("Icons/cyanslot");
            darkblue = content.Load<Texture2D>("Icons/darkblueslot");
            orange = content.Load<Texture2D>("Icons/orangeslot");

            //element mod icons
            lightgreen = content.Load<Texture2D>("Icons/lightgreenslot");
            foxred = content.Load<Texture2D>("Icons/foxredslot");
            white = content.Load<Texture2D>("Icons/whiteslot");
            
            spriteBatch = new SpriteBatch(device);

            interfaceRectangle = new Rectangle(0, 0, 1024, 768);
            
            exitRectangle = new Rectangle(726, 78, 63, 55);

            Mod1Rectangle = new Rectangle(296, 542, 40, 39);
            Mod2Rectangle = new Rectangle(335, 515, 40, 39);
            Mod3Rectangle = new Rectangle(378, 538, 40, 39);
            Mod4Rectangle = new Rectangle(337, 565, 40, 39);
            ModRectangles = new Rectangle[] { Mod1Rectangle, Mod2Rectangle, Mod3Rectangle, Mod4Rectangle };

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

            return Constants.CMD_NONE;
        }


        public override void draw()
        {
            //TODO
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
            drawMods();

        }

        private void drawMods()
        {
            Texture2D icon = null;

            for (int i = 0; i < 4; i++)
            {
                switch (data.player.myWeapon.mods[i].type)
                {
                    case Constants.MOD_NIL: icon = null; break;
                    case Constants.MOD_ELM:
                        switch (data.player.myWeapon.mods[i].value)
                        {
                            case Constants.ELM_PLA: icon = lightgreen; break;
                            case Constants.ELM_HEA: icon = foxred; break;
                            case Constants.ELM_ICE: icon = white; break;
                            default: icon = null; break;
                        } break;
                    case Constants.MOD_TYP:
                        switch (data.player.myWeapon.mods[i].value)
                        {
                            case Constants.TYP_BLA: icon = orange; break;
                            case Constants.TYP_WAV: icon = darkblue; break;
                            case Constants.TYP_TRI: icon = cyan; break;
                            default: icon = null; break;
                        } break;
                    case Constants.MOD_STR: icon = red; break;
                    case Constants.MOD_SPD: icon = blue; break;
                    case Constants.MOD_RCG: icon = green; break;
                    case Constants.MOD_ACP: icon = violet; break;
                    default: icon = null; break;
                }
                if (icon != null) spriteBatch.Draw(icon, ModRectangles[i], Color.White);
            }
        }

    }
}
