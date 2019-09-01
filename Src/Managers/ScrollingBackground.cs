using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Src.GameObjects;
using TeamRock.Utils;

namespace TeamRock.Managers
{
    public class ScrollingBackground
    {
        private Vector2 _initialPosition;
        private List<Sprite> _scrollingSprites;

        private bool _isScrollingActive;

        #region Initialization

        public void Initialize(Texture2D scrollingTexture, int totalElementsCount,
            Vector2 initialPosition, float textureScale, bool useSize = false)
        {
            _scrollingSprites = new List<Sprite>();

            for (int i = 0; i < totalElementsCount; i++)
            {
                Sprite scrollingSprite = new Sprite(scrollingTexture, useSize);
                if (!useSize)
                {
                    scrollingSprite.Scale = textureScale;
                }

                scrollingSprite.Origin =
                    new Vector2(scrollingSprite.TextureWidth / 2.0f, scrollingSprite.TextureHeight);
                scrollingSprite.Position = GetInitialPositionBasedOnIndex(i, initialPosition, scrollingSprite.Height);

                _scrollingSprites.Add(scrollingSprite);
            }

            _initialPosition = initialPosition;
            _isScrollingActive = true;
        }

        public void SetSize(int width, int height)
        {
            for (int i = 0; i < _scrollingSprites.Count; i++)
            {
                _scrollingSprites[i].SetSize(width, height);
                _scrollingSprites[i].Position = GetInitialPositionBasedOnIndex(i, _initialPosition, _scrollingSprites[i].Height);
            }
        }

        #endregion

        #region Render

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite scrollingSprite in _scrollingSprites)
            {
                scrollingSprite.Draw(spriteBatch);
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
            foreach (var scrollingSprite in _scrollingSprites)
            {
                scrollingSprite.Position -= Vector2.UnitY * playerSpeed * deltaTime;
            }

            for (int i = 0; i < _scrollingSprites.Count; i++)
            {
                RePositionScrollingBackground();
            }
        }

        private void RePositionScrollingBackground()
        {
            Sprite firstElement = _scrollingSprites[0];
            Sprite lastElement = _scrollingSprites[_scrollingSprites.Count - 1];

            if (firstElement.Position.Y < GameInfo.FixedWindowHeight)
            {
                _scrollingSprites.RemoveAt(_scrollingSprites.Count - 1);
                lastElement.Position = firstElement.Position + Vector2.UnitY * firstElement.Height;
                _scrollingSprites.Insert(0, lastElement);
            }
            else if (lastElement.Position.Y > lastElement.Height)
            {
                _scrollingSprites.RemoveAt(0);
                firstElement.Position = lastElement.Position - Vector2.UnitY * lastElement.Height;
                _scrollingSprites.Add(firstElement);
            }
        }

        private Vector2 GetInitialPositionBasedOnIndex(int index, Vector2 initialPosition, float spriteHeight)
        {
            float yHeight = initialPosition.Y - spriteHeight * index;
            Vector2 position = new Vector2(initialPosition.X, yHeight);

            return position;
        }

        #endregion
    }
}