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
using System.IO;

namespace StormGame
{

    public enum GameState
    {
        LevelSelect = 1,
        Gameplay = 2,
        ScoreScreen = 3,
        TitleScreen = 4
    }

    /// <summary>
    /// This is the main type for your gamea
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private SpriteFont _font;

        public GameState gameState { get; set; }

        MainMenu mainMenu;
        private Level LoadedLevel;
        private AudioManager audioManager;
        private TitleScreen titleScreen;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = Globals.SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = Globals.SCREEN_WIDTH;
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
            gameState = GameState.TitleScreen;
            mainMenu = new MainMenu();
            audioManager = new AudioManager(this);
            Texture2D titlepic = Content.Load<Texture2D>("TitleImage");
            var rec = new Rectangle((int)Globals.SCREEN_CENTER.X - (int)(titlepic.Bounds.Width / 2), (int)Globals.SCREEN_CENTER.Y - (int)(titlepic.Bounds.Height / 2), (int)titlepic.Bounds.Width, (int)titlepic.Bounds.Height);
            titleScreen = new TitleScreen(titlepic, rec, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), false);
            titleScreen.AddImage(titlepic);

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
            _font = Content.Load<SpriteFont>("DefaultFont");
            this.IsMouseVisible = true;

            //Load Globals, these should be loaded first.
            Globals.Font1 = _font;
            Globals.SpriteBatch = spriteBatch;
            Globals.Content = Content;
            Globals.GraphicsDevice = GraphicsDevice;
            Globals.gameState = gameState;
            Globals.game = this;
            LoadAudio();
            mainMenu.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }

        private void LoadAudio()
        {
            Components.Add(audioManager);
            audioManager.MusicVolume = 0.1f;
            Globals.audioManager = audioManager;

            // Make a reference to a directory.
            DirectoryInfo di = new DirectoryInfo("../../../../StormGameContent/Sounds/");

            // Get a reference to each file in that directory.
            FileInfo[] fiArr = di.GetFiles();

            // Display the names of the files.
            for (int i = 0; i < fiArr.Length; i++)
            {
                var noExtFile = fiArr[i].Name.Split('.');
                audioManager.LoadSound("Sounds/" + noExtFile[0]);
            }

            //Songs
            di = new DirectoryInfo("../../../../StormGameContent/Songs/");
            fiArr = di.GetFiles();
            for (int i = 0; i < fiArr.Length; i++)
            {
                var noExtFile = fiArr[i].Name.Split('.');
                audioManager.LoadSong("Songs/" + noExtFile[0]);
            }

            Globals.audioManager.PlaySong("Songs/3 Doors Down - Krptonite", true);
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
            Globals.GameTime = gameTime;

            GetInputs();

            switch (Globals.gameState)
            {
                case GameState.TitleScreen:
                    titleScreen.Update(gameTime);
                    if (titleScreen.Finished)
                        Globals.gameState = GameState.LevelSelect;
                    break;
                case GameState.LevelSelect:
                    mainMenu.Update();
                    break;

                case GameState.Gameplay:
                    LoadedLevel.Update(gameTime);
                    break;

                case GameState.ScoreScreen:
                    LoadedLevel.Update(gameTime);
                    break;

            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);



            switch (Globals.gameState)
            {
                case GameState.TitleScreen:
                    titleScreen.Draw(spriteBatch);
                    break;

                case GameState.LevelSelect:
                    mainMenu.Draw(spriteBatch);
                    break;

                case GameState.Gameplay:
                    LoadedLevel.Draw();
                    break;

            }

            base.Draw(gameTime);
        }

        public void LoadNewLevel(Level level)
        {
            Globals.audioManager.FadeToNewSong(0.0f, new TimeSpan(0, 0, 1), "Songs/Raptor Call of the Shadows - Bravo Sector");
            LoadedLevel = level;
            Globals.gameState = GameState.Gameplay;
        }

        // Records mouse and keyboard states each frame.
        private void GetInputs()
        {
            Globals.oldMouseState = Globals.mouseState;
            Globals.oldKeyboardState = Globals.keyboardState;

            Globals.mouseState = Mouse.GetState();
            Globals.keyboardState = Keyboard.GetState();
        }
    }
}
