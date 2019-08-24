using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Utils;

namespace TeamRock.Managers
{
    public class ScrollingBackground
    {
        private Texture2D _backgroundTexture;
        private int _textureHeight;
        private Vector2 _origin;

        private List<Rectangle> _backgroundRectangles;

        private bool _isScrollingActive;

        #region Initialization

        public void Initialization(Texture2D backgroundTexture, int textureWidth)
        {
            _backgroundTexture = backgroundTexture;
            float widthRatio = textureWidth / (float) _backgroundTexture.Width;
            _textureHeight = (int) (widthRatio * backgroundTexture.Height);

            int xPosition = (int) (GameInfo.FixedWindowWidth / 2.0f);

            _backgroundRectangles = new List<Rectangle>();
            for (int i = 0; i < GameInfo.MaxBackgroundElements; i++)
            {
                Rectangle backgroundRectangle = new Rectangle
                {
                    X = xPosition,
                    Y = i * _textureHeight,
                    Width = textureWidth,
                    Height = _textureHeight
                };

                _backgroundRectangles.Add(backgroundRectangle);
            }

            _origin = new Vector2(_backgroundTexture.Width / 2.0f, 0);
            _isScrollingActive = true;
        }

        #endregion

        #region Render

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var destinationRectangle in _backgroundRectangles)
            {
                spriteBatch.Draw(_backgroundTexture, destinationRectangle, null, Color.White, 0, _origin,
                    SpriteEffects.None, 0);
            }
        }

        #endregion

        #region Update

        public void Update(float deltaTime, float playerSpeed)
        {
            if (!_isScrollingActive)
            {
                return;
            }

            UpdateScrolling(deltaTime, playerSpeed);
        }

        #endregion

        #region External Functions

        public void ActivateScrolling() => _isScrollingActive = true;

        public void DeActivateScrolling() => _isScrollingActive = false;

        #endregion

        #region Utility Functions

        private void UpdateScrolling(float deltaTime, float playerSpeed)
        {
            for (int i = 0; i < _backgroundRectangles.Count; i++)
            {
                Rectangle backgroundPosition = _backgroundRectangles[i];
                backgroundPosition.Y -= (int) (playerSpeed * deltaTime);

                _backgroundRectangles[i] = backgroundPosition;
            }

            for (int i = 0; i < _backgroundRectangles.Count; i++)
            {
                RePositionScrollingBackground();
            }
        }

        private void RePositionScrollingBackground()
        {
            Rectangle firstPosition = _backgroundRectangles[0];
            Rectangle lastPosition = _backgroundRectangles[_backgroundRectangles.Count - 1];

            if (firstPosition.Y <= -_textureHeight)
            {
                _backgroundRectangles.RemoveAt(0);
                firstPosition.Y = lastPosition.Y + _textureHeight;
                _backgroundRectangles.Add(firstPosition);
            }
            else if (lastPosition.Y > GameInfo.FixedWindowHeight)
            {
                _backgroundRectangles.RemoveAt(_backgroundRectangles.Count - 1);
                lastPosition.Y = firstPosition.Y - _textureHeight;
                _backgroundRectangles.Insert(0, lastPosition);
            }
        }

        #endregion
    }
}