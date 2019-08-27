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

        private bool _gameStarted;

        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;

            Texture2D headerTexture = _contentManager.Load<Texture2D>(AssetManager.HeaderImage);
            _headerImage = new Sprite(headerTexture);
            _headerImage.Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, 100);
            _headerImage.SetOriginCenter();

            _defaultFont = _contentManager.Load<SpriteFont>(AssetManager.Arial);
            _pressToPlayText = new UiTextNode();
            _pressToPlayText.Initialize(_defaultFont, "PRESS <SPACE> TO START");
            _pressToPlayText.Position =
                new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight / 2.0f);

            _music = _contentManager.Load<SoundEffect>(AssetManager.HomeScreenMusic);
        }

        #endregion

        #region Render

        public override void Draw(SpriteBatch spriteBatch)
        {
            _headerImage.Draw(spriteBatch);
            _pressToPlayText.Draw(spriteBatch);
        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
        }

        #endregion

        #region Update

        public override bool Update(float deltaTime, float gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadCapabilities gamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);

            if (gamePadCapabilities.IsConnected)
            {
                _pressToPlayText.Text = "PRESS <A> TO START";

                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                if (gamePadState.Buttons.A == ButtonState.Pressed)
                {
                    _gameStarted = true;
                }
            }
            else
            {
                _pressToPlayText.Text = "PRESS <SPACE> TO START";

                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    _gameStarted = true;
                }
            }

            return _gameStarted;
        }

        #endregion

        #region External Functions

        public void StartMusic()
        {
            _musicIndex = SoundManager.Instance.PlaySound(_music);
            SoundManager.Instance.SetSoundLooping(_musicIndex, true);
        }

        public void StopMusic() => SoundManager.Instance.StopSound(_musicIndex);

        public void ResetScreen() => _gameStarted = false;

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