using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace TeamRock.Managers
{
    public class MouseController
    {
        private Game _game;
        private OrthographicCamera _camera;

        public void Initialize(Game game, OrthographicCamera camera)
        {
            _game = game;
            _camera = camera;
        }

        #region External Functions

        public void DisplayMouse() => _game.IsMouseVisible = true;

        public void HideMouse() => _game.IsMouseVisible = false;

        public Vector2 GetMouseWorldPosition()
        {
            MouseState mouseState = Mouse.GetState();
            return _camera.ScreenToWorld(mouseState.X, mouseState.Y);
        }

        #endregion

        #region Singleton

        private static MouseController _instance;
        public static MouseController Instance => _instance ?? (_instance = new MouseController());

        private MouseController()
        {
        }

        #endregion
    }
}