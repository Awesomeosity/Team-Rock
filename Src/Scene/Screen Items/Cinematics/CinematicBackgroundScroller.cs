using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Src.GameObjects;

namespace TeamRock.Scene.Screen_Items.Cinematics
{
    public class CinematicBackgroundScroller
    {
        private List<Sprite> _backgroundSprites;
        private float _scrollingSpeed;

        private bool _startScrolling;

        private Vector2 _initialPosition;
        private Vector2 _finalPosition;

        public delegate void PositionReached();

        public PositionReached OnPositionReached;

        #region Initialization

        public void Initialize(Texture2D scrollingTexture, float textureScale, int totalElementsCount,
            Vector2 initialPosition, Vector2 finalPosition)
        {
            _backgroundSprites = new List<Sprite>();

            for (int i = 0; i < totalElementsCount; i++)
            {
                Sprite backgroundSprite = new Sprite(scrollingTexture)
                {
                    Scale = textureScale
                };
                backgroundSprite.Origin =
                    new Vector2(backgroundSprite.TextureWidth / 2.0f, backgroundSprite.TextureHeight);
                backgroundSprite.Position = GetPositionBasedOnIndex(i, initialPosition, backgroundSprite.Height);

                _backgroundSprites.Add(backgroundSprite);
            }

            _initialPosition = initialPosition;
            _finalPosition = finalPosition;
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var backgroundSprite in _backgroundSprites)
            {
                backgroundSprite.Draw(spriteBatch);
            }
        }

        #endregion

        #region Update

        public void Update(float deltaTime)
        {
            if (!_startScrolling)
            {
                return;
            }

            foreach (var backgroundSprite in _backgroundSprites)
            {
                backgroundSprite.Position += Vector2.UnitY * _scrollingSpeed * deltaTime;
            }

            if (_backgroundSprites[_backgroundSprites.Count - 1].Position.Y >= _finalPosition.Y)
            {
                _startScrolling = false;
                NotifyPositionReached();
            }
        }

        #endregion

        #region External Functions

        public void Reset()
        {
            for (int i = 0; i < _backgroundSprites.Count; i++)
            {
                _backgroundSprites[i].Position =
                    GetPositionBasedOnIndex(i, _initialPosition, _backgroundSprites[i].Height);
            }

            _startScrolling = false;
            _scrollingSpeed = 0;
        }

        public bool StartScrolling
        {
            get => _startScrolling;
            set => _startScrolling = value;
        }

        public float ScrollingSpeed
        {
            get => _scrollingSpeed;
            set => _scrollingSpeed = value;
        }

        #endregion

        #region Utility Functions

        private void NotifyPositionReached() => OnPositionReached?.Invoke();

        private Vector2 GetPositionBasedOnIndex(int index, Vector2 initialPosition, float spriteHeight)
        {
            float yHeight = initialPosition.Y - index * spriteHeight;
            Vector2 position = new Vector2(initialPosition.X, yHeight);

            return position;
        }

        #endregion
    }
}