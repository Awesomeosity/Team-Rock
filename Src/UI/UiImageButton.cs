using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using TeamRock.Managers;
using TeamRock.Src.GameObjects;

namespace TeamRock.UI
{
    public class UiImageButton
    {
        private Sprite _sprite;

        private float _xPosition;
        private float _yPosition;

        private RectangleF _buttonRectangle;
        private bool _displayButton;

        private MouseState _oldMouseState;

        #region Initialization

        public void Initialize(Texture2D buttonTexture, float imageScale, float xPosition, float yPosition)
        {
            _xPosition = xPosition;
            _yPosition = yPosition;

            _sprite = new Sprite(buttonTexture)
            {
                Scale = imageScale,
                Position = new Vector2(xPosition, yPosition)
            };
            _sprite.SetOriginCenter();

            _buttonRectangle = new Rectangle();
            UpdateButtonRectangle();

            _displayButton = true;
        }

        #endregion

        #region Render

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_displayButton)
            {
                return;
            }

            _sprite.Draw(spriteBatch);
        }

        #endregion

        #region Update

        public bool Update()
        {
            if (!_displayButton)
            {
                return false;
            }

            return ButtonClicked();
        }

        #endregion

        #region External Functions

        public void UpdateTexture(Texture2D texture2D)
        {
            _sprite.UpdateTexture(texture2D);
            UpdateButtonRectangle();
        }

        public void UpdatePosition(float xPosition, float yPosition)
        {
            _xPosition = xPosition;
            _yPosition = yPosition;
            UpdateButtonRectangle();
        }

        public void UpdateScale(float imageScale)
        {
            _sprite.Scale = imageScale;
            UpdateButtonRectangle();
        }

        public void ActivateButton() => _displayButton = true;

        public void DeActivateButton() => _displayButton = false;

        #endregion

        #region Utility Functions

        private void UpdateButtonRectangle()
        {
            float rectangleWidth = _sprite.Width;
            float rectangleHeight = _sprite.Height;

            _buttonRectangle.X = (int) (_xPosition - rectangleWidth / 2.0f);
            _buttonRectangle.Y = (int) (_yPosition - rectangleHeight / 2.0f);
            _buttonRectangle.Width = rectangleWidth;
            _buttonRectangle.Height = rectangleHeight;
        }

        private bool ButtonClicked()
        {
            MouseState mouseState = Mouse.GetState();
            bool buttonClicked = false;

            // This ensures that the mouse button was just released
            if (mouseState.LeftButton == ButtonState.Released && _oldMouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 mousePosition = MouseController.Instance.GetMouseWorldPosition();
                buttonClicked = _buttonRectangle.Contains(new Point2(mousePosition.X, mousePosition.Y));
            }

            _oldMouseState = mouseState;

            return buttonClicked;
        }

        #endregion
    }
}