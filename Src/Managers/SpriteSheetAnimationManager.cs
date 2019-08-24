using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Src.GameObjects;
using TeamRock.Utils;

namespace TeamRock.Managers
{
    public class SpriteSheetAnimationManager
    {
        private int _totalAssetIndex;
        private int _currentAssetIndex;

        private string _assetBaseName;
        private List<Texture2D> _animationTextures;
        private Sprite _sprite;

        private float _currentFrameTime;
        private float _frameTime;

        private bool _animationActive;
        private bool _isRepeating;

        #region Initialization

        public void Initialize(ContentManager contentManager, string assetBaseName, int totalAssetIndex,
            int assetStartIndex, bool isRepeating)
        {
            _isRepeating = isRepeating;

            _assetBaseName = assetBaseName;
            _totalAssetIndex = totalAssetIndex;

            _currentFrameTime = 0;
            _currentAssetIndex = 0;

            _animationTextures = new List<Texture2D>();
            for (int i = assetStartIndex; i < totalAssetIndex + assetStartIndex; i++)
            {
                _animationTextures.Add(contentManager.Load<Texture2D>($"{assetBaseName}{i}"));
            }

            _sprite = new Sprite(_animationTextures[0]);
            _frameTime = GameInfo.DefaultAnimationSpeed;
        }

        #endregion

        #region Render

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch);
            Console.WriteLine("Hello");
        }

        #endregion

        #region Update

        public void Update(float deltaTime)
        {
            if (!_animationActive)
            {
                return;
            }

            _currentFrameTime += deltaTime;
            if (_currentFrameTime >= _frameTime)
            {
                _currentFrameTime = 0;
                _currentAssetIndex += 1;

                if (_currentAssetIndex >= _totalAssetIndex)
                {
                    if (_isRepeating)
                    {
                        _currentAssetIndex = 0;
                    }
                    else
                    {
                        _animationActive = false;
                    }
                }

                _sprite.UpdateTexture(_animationTextures[_currentAssetIndex]);
            }
        }

        #endregion

        #region External Functions

        public Texture2D GetCurrentFrameTexture() => _animationTextures[_currentAssetIndex];

        public void StartSpriteAnimation()
        {
            _animationActive = true;
        }

        public void StopSpriteAnimation()
        {
            _animationActive = false;
        }

        public void ResetSpriteAnimation()
        {
            _animationActive = false;
            _currentAssetIndex = 0;
            _currentFrameTime = 0;
        }

        public void SetRepeatingState(bool isRepeating) => _isRepeating = isRepeating;

        public Sprite Sprite => _sprite;

        public float FrameTime
        {
            get => _frameTime;
            set => _frameTime = value;
        }

        #endregion
    }
}