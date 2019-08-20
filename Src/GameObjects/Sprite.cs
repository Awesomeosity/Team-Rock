using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TeamRock.Src.GameObjects
{
    public class Sprite : Texture2D
    {
        private Vector2 _position;
        private Vector2 _origin;

        private float _scale;
        private float _rotation;

        private Color _spriteColor;

        #region Render

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this,
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

        public void SetOriginCenter() => _origin = new Vector2(base.Width / 2.0f, base.Height / 2.0f);

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

        // Not required to override
        #region Base Members 

        public Sprite(GraphicsDevice graphicsDevice, int width, int height) : base(graphicsDevice, width, height)
        {
            Initialize();
        }

        public Sprite(GraphicsDevice graphicsDevice, int width, int height, bool mipmap, SurfaceFormat format) : base(graphicsDevice, width, height, mipmap, format)
        {
            Initialize();
        }

        public Sprite(GraphicsDevice graphicsDevice, int width, int height, bool mipmap, SurfaceFormat format, int arraySize) : base(graphicsDevice, width, height, mipmap, format, arraySize)
        {
            Initialize();
        }

        protected Sprite(GraphicsDevice graphicsDevice, int width, int height, bool mipmap, SurfaceFormat format, SurfaceType type, bool shared, int arraySize) : base(graphicsDevice, width, height, mipmap, format, type, shared, arraySize)
        {
            Initialize();
        }

        #endregion
    }
}