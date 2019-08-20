using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TeamRock.Scene
{
    public class MainScreen : CustomScreen
    {
        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {

        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {

        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        #endregion

        #region UnLoad

        public override void UnLoadContent()
        {

        }

        #endregion

        #region Singleton

        private static MainScreen _instance;
        public static MainScreen Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainScreen();
                }

                return _instance;
            }
        }

        private MainScreen() { }

        #endregion
    }
}