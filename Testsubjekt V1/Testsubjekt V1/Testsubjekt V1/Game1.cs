using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
        GameScreen nextScreen;
        ActionScreen myAction;
        AudioManager audio;

        Camera camera;

        bool fading;
        bool fadeOut;
        byte fadeFrames;
        byte maxFadeFrames;
        Rectangle fadeRectangle;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;

            graphics.IsFullScreen = true;
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

            audio = new AudioManager(Content);
            camera = new Camera(GraphicsDevice.Viewport.AspectRatio);
            world = new World(Content, GraphicsDevice);
            data = new GameData(Content, GraphicsDevice, audio, world);
            screen = new TitleScreen(Content, GraphicsDevice, audio, data);
            myAction = new ActionScreen(Content, GraphicsDevice, audio, data, camera, world);

            fading = false;
            fadeOut = true;
            fadeFrames = 0;
            maxFadeFrames = 30;
            fadeRectangle = new Rectangle(0, 0, 1024, 768);
            
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

        private bool fade()
        {
            fadeFrames = (byte)Math.Max(fadeFrames - 1, 0);
            return (fadeFrames == 0);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Constants.DEBUG && (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Tab)))
                this.Exit();

            if (fading) //is fading at the moment
            {
                if (fade()) //if fade ends
                {
                    if (fadeOut)    //if fading out
                    {
                        if (nextScreen == myAction)
                        {
                            if (screen.nextZone != world.mapID)
                            {
                                world.warp(screen.nextZone, screen.nextTheme);
                                world.setupSpawners(data.missions.activeMission);
                            }
                            myAction.reset();
                            camera.reset();
                            myAction.update(gameTime);
                            data.player.update(gameTime, data.npcs, data.bullets, camera, false);
                        }
                        screen = nextScreen;

                        fadeFrames = maxFadeFrames;
                        fadeOut = false;
                    }
                    else            //if fading in
                        fading = false;
                }
            }
            else
            {
                int updateState = screen.update(gameTime);
                switch (updateState)
                {
                    case Constants.CMD_EXIT: Exit(); break;
                    case Constants.CMD_NONE: break;
                    case Constants.CMD_NEW:
                        {
                            nextScreen = myAction;
                            fading = true;
                            fadeFrames = maxFadeFrames;
                            fadeOut = true;
                            //myAction.reset();
                            break;
                        }
                    case Constants.CMD_PAUSE:
                        screen = new PauseScreen(Content, GraphicsDevice, audio, data, world, camera); break;
                    case Constants.CMD_JOURNAL:
                        screen = new BriefingScreen(Content, GraphicsDevice, audio, data, world, camera); break;
                    case Constants.CMD_MOD:
                        screen = new ModificationScreen(Content, GraphicsDevice, audio, data, world, camera); break;
                    case Constants.CMD_MISSIONCOMPLETE:
                        screen = new MissionCompleteScreen(Content, GraphicsDevice, audio, data, world, camera); break;
                    case Constants.CMD_BACK:
                        {
                            screen = myAction;
                            myAction.canShoot = false;
                            break;
                        }
                    case Constants.CMD_MISSIONINFO:
                        screen = new MissionInfoScreen(Content, GraphicsDevice, audio, data, world, camera); break;
                    case Constants.CMD_INTRO:
                        screen = new IntroductionScreen(Content, GraphicsDevice, audio, data, spriteBatch); break;
                    case Constants.CMD_DEX:
                        screen = new DexScreen(Content, GraphicsDevice, data, audio, world, camera); break;
                    case Constants.CMD_CREDITS:
                        screen = new CreditsScreen(Content, GraphicsDevice, audio, data); break;
                    case Constants.CMD_TITLE:
                        screen = new TitleScreen(Content, GraphicsDevice, audio, data); break;
                    case Constants.CMD_GAMEOVER:
                        screen = new GameOverScreen(Content, GraphicsDevice, audio, data, world, camera); break;
                    case Constants.CMD_CHARINFO:
                        screen = new CharScreen(Content, GraphicsDevice, audio, data, world, camera); break;
                    case Constants.CMD_HELP:
                        {
                            if (screen is TitleScreen)
                                screen = new HelpScreen(Content, GraphicsDevice, audio, data, Constants.CMD_TITLE);
                            else
                                screen = new HelpScreen(Content, GraphicsDevice, audio, data, Constants.CMD_PAUSE);
                            break;
                        }
                    default: break;
                }

                if (updateState != Constants.CMD_NONE)
                    Mouse.SetPosition(512, 384);

                audio.update();
            }
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
                case 0: clear = new Color(90, 101, 137); break;
                case 1: clear = Color.DarkSlateBlue; break;
                case 2: clear = Color.Yellow; break;
                case 3: clear = Color.LightBlue; break;
                default: clear = Color.Red; break;
            }
            GraphicsDevice.Clear(clear);
            screen.draw();

            if (fading) 
            {
                byte alpha = (byte)((float)fadeFrames/(float)maxFadeFrames * 255);
                alpha = (fadeOut)? (byte)(255 - alpha) : alpha;
                Texture2D texture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                Color[] color = { Color.FromNonPremultiplied(255, 255, 255, alpha) };
                texture.SetData<Color>(color);
                spriteBatch.Begin();
                spriteBatch.Draw(texture, fadeRectangle, Color.Black);
                spriteBatch.End();
            }

            // reset render states that were manipulated from the sprite batch
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            base.Draw(gameTime);
        }
    }
}
