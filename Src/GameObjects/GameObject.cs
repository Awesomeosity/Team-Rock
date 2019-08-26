using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace TeamRock.Src.GameObjects
{
    public class GameObject
    {
        private Sprite _sprite;

        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _acceleration;
        private RectangleF _collisionObject;

        #region Initialization

        public GameObject(Sprite sprite, float collisionWidth, float collisionHeight)
        {
            _sprite = sprite;
            GenerateHitBox(collisionWidth, collisionHeight);
        }

        private void GenerateHitBox(float width, float height)
        {
            _collisionObject = new RectangleF
            {
                Width = width,
                Height = height
            };
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch) => _sprite?.Draw(spriteBatch);

        public void DrawDebug(SpriteBatch spriteBatch) => spriteBatch.DrawRectangle(_collisionObject, Color.Red, 2);

        #endregion

        #region Update

        public virtual void Update(float deltaTime, float gameTime)
        {
            _velocity += _acceleration;
            Position += _velocity * deltaTime;
        }

        public void UpdateOnlyVelocity(float deltaTime, float gameTime)
        {
            _velocity += _acceleration;
        }

        #endregion

        #region External Functions

        public bool DidCollide(GameObject other) => _collisionObject.Intersects(other._collisionObject);

        public Sprite Sprite
        {
            get => _sprite;
            set => _sprite = value;
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;

                _collisionObject.X = _position.X - _collisionObject.Width / 2.0f;
                _collisionObject.Y = _position.Y - _collisionObject.Height / 2.0f;

                _sprite.Position = _position;
            }
        }

        public Vector2 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        public Vector2 Acceleration
        {
            get => _acceleration;
            set => _acceleration = value;
        }

        #endregion
    }
}