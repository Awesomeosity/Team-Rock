using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TeamRock.Src.GameObjects
{
    public class Sprite
    {
        private Texture2D _texture2D;

        private Rectangle _destinationRectangle;
        private bool _useSize;

        private Vector2 _position;
        private Vector2 _origin;

        private float _scale;
        private float _rotation;

        private Color _spriteColor;

        #region Constructor

        public Sprite(Texture2D texture2D, bool useSize = false)
        {
            _texture2D = texture2D;
            _useSize = useSize;

            Initialize(_useSize);
        }

        #endregion

        #region Render

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_useSize)
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
            else
            {
                spriteBatch.Draw(
                    _texture2D,
                    _destinationRectangle,
                    null,
                    _spriteColor,
                    _rotation,
                    _origin,
                    SpriteEffects.None,
                    0
                );
            }
        }

        #endregion

        #region External Functions

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;

                if (_useSize)
                {
                    _destinationRectangle.X = (int) _position.X;
                    _destinationRectangle.Y = (int) _position.Y;
                }
            }
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

        public bool UseSize
        {
            get => _useSize;
            set => _useSize = value;
        }

        public void UpdateTexture(Texture2D texture2D) => _texture2D = texture2D;

        public void SetOriginCenter() => _origin = new Vector2(_texture2D.Width / 2.0f, _texture2D.Height / 2.0f);

        public void SetSize(int width, int height)
        {
            _destinationRectangle.Width = width;
            _destinationRectangle.Height = height;
        }

        public float Width => _texture2D.Width * _scale;

        public float Height => _texture2D.Height * _scale;

        public float TextureWidth => _texture2D.Width;

        public float TextureHeight => _texture2D.Height;

        #endregion

        #region Utility Functions

        private void Initialize(bool useSize)
        {
            _position = new Vector2();
            _origin = new Vector2();

            if (useSize)
            {
                _destinationRectangle = new Rectangle
                {
                    X = (int) _position.X,
                    Y = (int) _position.Y
                };
            }

            _scale = 1;
            _spriteColor = Color.White;
        }

        #endregion
    }
}