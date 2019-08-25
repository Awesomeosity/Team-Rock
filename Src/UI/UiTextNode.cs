using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TeamRock.UI
{
    public class UiTextNode
    {
        private SpriteFont _spriteFont;

        private string _text;
        private Color _textColor;

        private Vector2 _position;

        #region Initialization

        public void Initialization(SpriteFont spriteFont, string text)
        {
            _spriteFont = spriteFont;
            _text = text;
            _textColor = Color.White;
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 stringSize = _spriteFont.MeasureString(_text);

            float xPosition = _position.X - stringSize.X / 2.0f;
            float yPosition = _position.Y - stringSize.Y / 2.0f;

            spriteBatch.DrawString(_spriteFont, _text, new Vector2(xPosition, yPosition), _textColor);
        }

        #endregion

        #region External Functions

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public string Text
        {
            get => _text;
            set => _text = value;
        }

        public Color Color
        {
            get => _textColor;
            set => _textColor = value;
        }

        #endregion
    }
}