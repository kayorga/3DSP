using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TestsubjektV1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        World world;
        GameData data;
        GameScreen screen;
        ActionScreen myAction;

        Camera camera;
        Character player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            camera = new Camera(GraphicsDevice.Viewport.AspectRatio);
            world = new World(Content, GraphicsDevice);
            data = new GameData(Content, world);
            screen = new TitleScreen(Content, GraphicsDevice);
            myAction = new ActionScreen(Content, GraphicsDevice, data, camera, world);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Tab))
                this.Exit();
            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) screen = myAction;

            int updateState = screen.update(gameTime);
            switch (updateState)
            {
                case Constants.CMD_EXIT: Exit(); break;
                case Constants.CMD_NONE: break;
                case Constants.CMD_NEW:
                    screen = myAction; myAction.reset(); break;
                case Constants.CMD_PAUSE: 
                    screen = new PauseScreen(Content, GraphicsDevice, data, world, camera); break;
                case Constants.CMD_JOURNAL: 
                    screen = new BriefingScreen(Content, GraphicsDevice, data, world, camera); break;
                case Constants.CMD_MOD: 
                    screen = new ModificationScreen(Content, GraphicsDevice, data, world, camera); break;
                case Constants.CMD_MISSIONCOMPLETE: 
                    screen = new MissionCompleteScreen(Content, GraphicsDevice, data, world, camera); break;
                case Constants.CMD_BACK: 
                    screen = myAction; break;
                case Constants.CMD_MISSIONINFO: 
                    screen = new MissionInfoScreen(Content, GraphicsDevice, data, world, camera); break;
                default: break;
            }

            if (updateState != Constants.CMD_NONE)
                Mouse.SetPosition(512, 384); 

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Color clear = Color.DarkSlateBlue;
            switch (world.theme)
            {
                case 0: clear = Color.DarkSlateGray; break;
                case 1: clear = Color.DarkSlateBlue; break;
                case 2: clear = Color.Yellow; break;
                case 3: clear = Color.LightBlue; break;
                default: clear = Color.Red; break;
            }
            GraphicsDevice.Clear(clear);
            screen.draw();

            // TODO: Add your drawing code here
            // reset render states that were manipulated from the sprite batch
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            base.Draw(gameTime);
        }
    }
}
