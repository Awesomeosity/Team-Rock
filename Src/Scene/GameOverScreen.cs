using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TeamRock.Src.GameObjects;
using TeamRock.UI;
using TeamRock.Utils;


namespace TeamRock.Scene
{
    public class GameOverScreen : CustomScreen
    {
        private ContentManager _contentManager;
        private Sprite _gameOverSprite;
        private Sprite _returnSprite;

        private KeyboardState _oldState;
        private GamePadState _oldControl;

        private bool _screenActive;
        private bool _exitScreen;

        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;

            Texture2D gameOverTexture = contentManager.Load<Texture2D>(AssetManager.GameOverImage);
            _gameOverSprite = new Sprite(gameOverTexture)
            {
                Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight / 2.0f),
                Scale = 0.2f
            };
            _gameOverSprite.SetOriginCenter();

            Texture2D returnTexture = _contentManager.Load<Texture2D>(AssetManager.SpaceToReturn);
            _returnSprite = new Sprite(returnTexture)
            {
                Scale = 0.2f,
                Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight - 100)
            };
            _returnSprite.SetOriginCenter();

            _oldState = Keyboard.GetState();
            _oldControl = GamePad.GetState(PlayerIndex.One);
        }

        #endregion

        #region Render

        public override void Draw(SpriteBatch spriteBatch)
        {
            _gameOverSprite.Draw(spriteBatch);
            _returnSprite.Draw(spriteBatch);
        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
        }

        #endregion

        #region Update

        public override bool Update(float deltaTime, float gameTime)
        {
            if (_screenActive)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                GamePadCapabilities gamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);
                if (gamePadCapabilities.IsConnected)
                {
                    GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                    if (gamePadState.Buttons.A != ButtonState.Pressed && _oldControl.Buttons.A == ButtonState.Pressed)
                    {
                        _screenActive = false;
                        Fader.Instance.StartFadeIn();
                    }

                    _oldControl = gamePadState;
                }
                else
                {
                    if (keyboardState.IsKeyUp(Keys.Space) && _oldState.IsKeyDown(Keys.Space))
                    {
                        _screenActive = false;
                        Fader.Instance.StartFadeIn();
                    }

                    _oldState = keyboardState;
                }
            }


            return _exitScreen;
        }

        #endregion

        #region External Functions

        public void ResetScreen()
        {
            _oldState = Keyboard.GetState();
            _oldControl = GamePad.GetState(PlayerIndex.One);

            _exitScreen = false;
            _screenActive = false;

            Fader.Instance.OnFadeInComplete += HandleFadeIn;
            Fader.Instance.OnFadeOutComplete += HandleFadeOut;
        }

        #endregion

        #region Utility Functions

        private void HandleFadeIn()
        {
            Fader.Instance.OnFadeInComplete -= HandleFadeIn;
            _exitScreen = true;
        }

        private void HandleFadeOut()
        {
            Fader.Instance.OnFadeOutComplete -= HandleFadeOut;
            _screenActive = true;
        }

        #endregion

        #region Singleton

        private static GameOverScreen _instance;
        public static GameOverScreen Instance => _instance ?? (_instance = new GameOverScreen());

        private GameOverScreen()
        {
        }

        #endregion
    }
}