using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TeamRock.Scene
{
    public class GameOverScreen : CustomScreen

    {
        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
        }

        #endregion

        #region Render

        public override void Draw(SpriteBatch spriteBatch)
        {
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