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
        private UiTextNode _pressToPlayText;
        private SpriteFont _defaultFont;

        private KeyboardState _oldState;
        private GamePadState _oldControl;

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

            _defaultFont = _contentManager.Load<SpriteFont>(AssetManager.Luckiest_Guy);
            _pressToPlayText = new UiTextNode();
            _pressToPlayText.Initialize(_defaultFont, "PRESS <SPACE> TO RESTART");
            _pressToPlayText.Position =
                new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight - 100);

            _oldState = Keyboard.GetState();
            _oldControl = GamePad.GetState(PlayerIndex.One);
        }

        #endregion

        #region Render

        public override void Draw(SpriteBatch spriteBatch)
        {
            _pressToPlayText.Draw(spriteBatch);
            _gameOverSprite.Draw(spriteBatch);
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
                _pressToPlayText.Text = "PRESS <A> TO RETURN";

                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                if (gamePadState.Buttons.A != ButtonState.Pressed && _oldControl.Buttons.A == ButtonState.Pressed)
                {
                    return true;
                }

                _oldControl = gamePadState;
            }
            else
            {
                _pressToPlayText.Text = "PRESS <SPACE> TO RETURN";

                if (keyboardState.IsKeyUp(Keys.Space) && _oldState.IsKeyDown(Keys.Space))
                {
                    return true;
                }

                _oldState = keyboardState;
            }

            return false;
        }

        #endregion

        #region External Functions

        public void ResetScreen()
        {
            _oldState = Keyboard.GetState();
            _oldControl = GamePad.GetState(PlayerIndex.One);
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