using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TeamRock.Scene;
using TeamRock.Utils;

namespace TeamRock
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        #region Screen Management

        private MainScreen _mainScreen;

        private enum GameScreen
        {
            MainScreen
        }
        private GameScreen _gameScreen;

        #endregion

        #region Constructor

        public GameMain()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = GameInfo.WindowWidth;
            _graphics.PreferredBackBufferHeight = GameInfo.WindowHeight;
            _graphics.ApplyChanges();
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SetupScreens();
            SetGameScreen(GameScreen.MainScreen);
        }

        private void SetupScreens()
        {
            _mainScreen = MainScreen.Instance;
            _mainScreen.Initialize(Content);
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);

            _spriteBatch.Begin();

            switch (_gameScreen)
            {
                case GameScreen.MainScreen:
                    _mainScreen.Draw(_spriteBatch);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _spriteBatch.End();
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);

            if (IsActive)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                float totalGameTime = (float)gameTime.TotalGameTime.TotalSeconds;

                switch (_gameScreen)
                {
                    case GameScreen.MainScreen:
                        _mainScreen.Update(deltaTime, totalGameTime);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #region UnLoad Content

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() => Content.Unload();

        #endregion

        #region Utility Functions

        private void SetGameScreen(GameScreen gameScreen)
        {
            if (_gameScreen == gameScreen)
            {
                return;
            }

            _gameScreen = gameScreen;
        }

        #endregion
    }
}
