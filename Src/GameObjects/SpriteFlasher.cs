using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TeamRock.Src.GameObjects
{
    public class SpriteFlasher : Sprite
    {
        private bool _incrementAlpha;
        private bool _startFlash;

        private float _alphaChangeRate;
        private float _currentAlpha;

        private float _minFlashAlpha;
        private float _maxFlashAlpha;

        private Color _spriteColor;

        #region Initialization

        public SpriteFlasher(Texture2D texture2D, bool useSize = false) : base(texture2D, useSize)
        {
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_startFlash)
            {
                return;
            }

            base.Draw(spriteBatch);
        }

        #endregion

        #region Update

        public void Update(float deltaTime)
        {
            if (!_startFlash)
            {
                return;
            }

            if (_currentAlpha <= _minFlashAlpha)
            {
                _incrementAlpha = true;
            }
            else if (_currentAlpha >= _maxFlashAlpha)
            {
                _incrementAlpha = false;
            }

            if (_incrementAlpha)
            {
                _currentAlpha += _alphaChangeRate * deltaTime;
            }
            else
            {
                _currentAlpha -= _alphaChangeRate * deltaTime;
            }

            SpriteColor = _spriteColor * _currentAlpha;
        }

        #endregion

        #region External Functions

        public void StartFlashing(float alphaChangeRate, float minFlashAlpha, float maxFlashAlpha)
        {
            _alphaChangeRate = alphaChangeRate;
            _currentAlpha = 0;
            _startFlash = true;

            _minFlashAlpha = minFlashAlpha;
            _maxFlashAlpha = maxFlashAlpha;
        }

        public float FlashingRate
        {
            get => _alphaChangeRate;
            set => _alphaChangeRate = value;
        }

        public void SetSpriteColor(Color color) => _spriteColor = color;

        public void StopFlashing() => _startFlash = false;
    }

    #endregion
}