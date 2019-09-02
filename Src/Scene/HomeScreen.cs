using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.UI;
using TeamRock.Utils;
using TeamRock.Managers;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using TeamRock.Src.GameObjects;

namespace TeamRock.Scene
{
    public class HomeScreen : CustomScreen
    {
        private ContentManager _contentManager;

        private SoundEffect _music;
        private int _musicIndex;

        private SpriteFont _defaultFont;
        private UiTextNode _pressToPlayText;
        private Sprite _headerImage;
        private Sprite _luchadorSprite;
        private KeyboardState _oldKeyboardState;
        private GamePadState _oldGamePadState;

        private bool _screenActive;
        private bool _exitScreen;

        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;

            Texture2D headerTexture = _contentManager.Load<Texture2D>(AssetManager.Logo);
            _headerImage = new Sprite(headerTexture)
            {
                Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, 100),
                Scale = 0.2f
            };
            _headerImage.SetOriginCenter();

            Texture2D luchadorTexture = _contentManager.Load<Texture2D>(AssetManager.LuchadorStartScreen);
            _luchadorSprite = new Sprite(luchadorTexture)
            {
                Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight / 2.0f),
                Scale = 0.20f
            };
            _luchadorSprite.SetOriginCenter();

            _defaultFont = _contentManager.Load<SpriteFont>(AssetManager.Luckiest_Guy);
            _pressToPlayText = new UiTextNode();
            _pressToPlayText.Initialize(_defaultFont, "PRESS <SPACE> TO START");
            _pressToPlayText.Position =
                new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight - 100);

            _music = _contentManager.Load<SoundEffect>(AssetManager.HomeScreenMusic);
        }

        #endregion

        #region Render

        public override void Draw(SpriteBatch spriteBatch)
        {
            _headerImage.Draw(spriteBatch);
            _luchadorSprite.Draw(spriteBatch);
            _pressToPlayText.Draw(spriteBatch);
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
                UpdateControls();
            }

            return _exitScreen;
        }

        private void UpdateControls()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadCapabilities gamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);

            if (gamePadCapabilities.IsConnected)
            {
                _pressToPlayText.Text = "PRESS <A> TO START";

                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                if (gamePadState.Buttons.A != ButtonState.Pressed && _oldGamePadState.Buttons.A == ButtonState.Pressed)
                {
                    _screenActive = false;
                    Fader.Instance.StartFadeIn();
                }

                _oldGamePadState = gamePadState;
            }
            else
            {
                _pressToPlayText.Text = "PRESS <SPACE> TO START";

                if (keyboardState.IsKeyUp(Keys.Space) && _oldKeyboardState.IsKeyDown(Keys.Space))
                {
                    _screenActive = false;
                    Fader.Instance.StartFadeIn();
                }

                _oldKeyboardState = keyboardState;
            }
        }

        #endregion

        #region External Functions

        public void StartMusic()
        {
            _musicIndex = SoundManager.Instance.PlaySound(_music, true);
            SoundManager.Instance.SetSoundVolume(_musicIndex, 0.5f);
        }

        public void StopMusic() => SoundManager.Instance.StopSound(_musicIndex);

        public void ResetScreen()
        {
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

        private static HomeScreen _instance;
        public static HomeScreen Instance => _instance ?? (_instance = new HomeScreen());

        private HomeScreen()
        {
        }

        #endregion
    }
}