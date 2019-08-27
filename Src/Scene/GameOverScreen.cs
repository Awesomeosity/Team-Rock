using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Src.GameObjects;
using TeamRock.Utils;

namespace TeamRock.Scene
{
    public class GameOverScreen : CustomScreen
    {
        private Sprite _gameOverSprite;

        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
            Texture2D gameOverTexture = contentManager.Load<Texture2D>(AssetManager.GameOverImage);
            _gameOverSprite = new Sprite(gameOverTexture)
            {
                Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight / 2.0f)
            };
            _gameOverSprite.SetOriginCenter();
        }

        #endregion

        #region Render

        public override void Draw(SpriteBatch spriteBatch)
        {
            _gameOverSprite.Draw(spriteBatch);
        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
        }

        #endregion

        #region Update

        public override bool Update(float deltaTime, float gameTime)
        {
            return false;
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