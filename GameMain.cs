using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using TeamRock.CustomCamera;
using TeamRock.Managers;
using TeamRock.Scene;
using TeamRock.Utils;

namespace TeamRock
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GamePadVibrationController _gamePadVibrationController;
        private MouseController _mouseController;

        private BoxingViewportAdapter _mainViewport;
        private OrthographicCamera _mainCamera;

        private SoundManager _soundManager;
        private CameraShaker _cameraShaker;

        private bool _drawDebug = false; // TODO: Change this later on...

        #region Screen Management

        private HomeScreen _homeScreen;
        private CinematicScreen _cinematicScreen;
        private MainScreen _mainScreen;
        private GameOverScreen _gameOverScreen;

        private enum GameScreen
        {
            HomeScreen,
            CinematicScreen,
            MainScreen,
            GameOverScreen
        }

        private GameScreen _gameScreen;

        #endregion

        #region Constructor

        public GameMain()
        {
            Content.RootDirectory = "Content";

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = GameInfo.FixedWindowWidth,
                PreferredBackBufferHeight = GameInfo.FixedWindowHeight
            };
            Window.AllowUserResizing = true;
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
            Window.Title = "POCO LOCO";
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SetupCamerasAndViewports();
            SetupSpecialControllers();
            SetupScreens();
            SetupOtherItems();

            SetGameScreen(GameScreen.HomeScreen);
            _homeScreen.StartMusic();
        }

        private void SetupCamerasAndViewports()
        {
            _mainViewport =
                new BoxingViewportAdapter(Window, GraphicsDevice, GameInfo.FixedWindowWidth,
                    GameInfo.FixedWindowHeight);
            _mainCamera = new OrthographicCamera(_mainViewport)
            {
                Zoom = 1
            };

            _cameraShaker = CameraShaker.Instance;
            _cameraShaker.Initialize(_mainCamera);

            // This is a hack that was required to reset the ViewPort so that textures scaled properly
            _mainViewport.Reset();
        }

        private void SetupSpecialControllers()
        {
            _mouseController = MouseController.Instance;
            _mouseController.Initialize(this, _mainCamera);
            _mouseController.DisplayMouse();

            _gamePadVibrationController = GamePadVibrationController.Instance;
        }

        private void SetupScreens()
        {
            _homeScreen = HomeScreen.Instance;
            _homeScreen.Initialize(Content);

            _cinematicScreen = CinematicScreen.Instance;
            _cinematicScreen.Initialize(Content);

            _mainScreen = MainScreen.Instance;
            _mainScreen.Initialize(Content);

            _gameOverScreen = GameOverScreen.Instance;
            _gameOverScreen.Initialize(Content);
        }

        private void SetupOtherItems()
        {
            _soundManager = SoundManager.Instance;
            _soundManager.Initialize();
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

            _spriteBatch.Begin(transformMatrix: _mainCamera.GetViewMatrix(), samplerState: SamplerState.LinearClamp,
                blendState: BlendState.AlphaBlend);

            switch (_gameScreen)
            {
                case GameScreen.HomeScreen:
                    _homeScreen.Draw(_spriteBatch);
                    break;

                case GameScreen.CinematicScreen:
                    _cinematicScreen.Draw(_spriteBatch);
                    break;

                case GameScreen.MainScreen:
                    _mainScreen.Draw(_spriteBatch);
                    break;

                case GameScreen.GameOverScreen:
                    _gameOverScreen.Draw(_spriteBatch);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _spriteBatch.End();

            if (_drawDebug)
            {
                DrawDebug();
            }
        }

        private void DrawDebug()
        {
            _spriteBatch.Begin(transformMatrix: _mainCamera.GetViewMatrix());

            switch (_gameScreen)
            {
                case GameScreen.HomeScreen:
                    _homeScreen.DrawDebug(_spriteBatch);
                    break;

                case GameScreen.CinematicScreen:
                    _cinematicScreen.DrawDebug(_spriteBatch);
                    break;

                case GameScreen.MainScreen:
                    _mainScreen.DrawDebug(_spriteBatch);
                    break;

                case GameScreen.GameOverScreen:
                    _gameOverScreen.DrawDebug(_spriteBatch);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            base.Update(gameTime);

            if (IsActive)
            {
                float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
                float totalGameTime = (float) gameTime.TotalGameTime.TotalSeconds;

                _gamePadVibrationController.Update(deltaTime);
                _cameraShaker.Update(deltaTime);

                switch (_gameScreen)
                {
                    case GameScreen.HomeScreen:
                    {
                        bool switchScreen = _homeScreen.Update(deltaTime, totalGameTime);
                        if (switchScreen)
                        {
                            _homeScreen.StopMusic();

                            _mainScreen.ResetScreen();
                            SetGameScreen(GameScreen.MainScreen);
                            _mainScreen.StartMusic();

//                                _cinematicScreen.ResetScreen();
//                            SetGameScreen(GameScreen.CinematicScreen);
                        }
                    }
                        break;

                    case GameScreen.CinematicScreen:
                    {
                        bool switchScreen = _cinematicScreen.Update(deltaTime, totalGameTime);

                        if (switchScreen)
                        {
                            _mainScreen.ResetScreen();
                            SetGameScreen(GameScreen.MainScreen);
                            _mainScreen.StartMusic();
                        }
                    }
                        break;

                    case GameScreen.MainScreen:
                    {
                        bool switchScreens = _mainScreen.Update(deltaTime, totalGameTime);
                        if (switchScreens)
                        {
                            _mainScreen.StopMusic();
                            _gameOverScreen.ResetScreen();
                            SetGameScreen(GameScreen.GameOverScreen);
                        }
                    }
                        break;

                    case GameScreen.GameOverScreen:
                    {
                        bool switchScreens = _gameOverScreen.Update(deltaTime, totalGameTime);
                        if (switchScreens)
                        {
                            _homeScreen.StartMusic();
                            _homeScreen.ResetScreen();
                            SetGameScreen(GameScreen.HomeScreen);
                        }
                    }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                _gamePadVibrationController.StopVibration();
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