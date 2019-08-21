using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TeamRock.Src.GameObjects
{
    public class Sprite
    {
        private Texture2D _texture2D;

        private Vector2 _position;
        private Vector2 _origin;

        private float _scale;
        private float _rotation;

        private Color _spriteColor;

        #region Constructor

        public Sprite(Texture2D texture2D)
        {
            _texture2D = texture2D;
            Initialize();
        }

        #endregion

        #region Render

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture2D,
                _position,
               null,
                _spriteColor,
                _rotation,
                _origin,
                _scale,
                SpriteEffects.None,
                0
                );
        }

        #endregion

        #region External Functions

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public Vector2 Origin
        {
            get => _origin;
            set => _origin = value;
        }

        public float Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        public Color SpriteColor
        {
            get => _spriteColor;
            set => _spriteColor = value;
        }

        public float Scale
        {
            get => _scale;
            set => _scale = value;
        }

        public void UpdateTexture(Texture2D texture2D) => _texture2D = texture2D;

        public void SetOriginCenter() => _origin = new Vector2(_texture2D.Width / 2.0f, _texture2D.Height / 2.0f);

        #endregion

        #region Utility Functions

        private void Initialize()
        {
            _position = new Vector2();
            _origin = new Vector2();

            _scale = 1;
            _spriteColor = Color.White;

        }

        #endregion
    }
}