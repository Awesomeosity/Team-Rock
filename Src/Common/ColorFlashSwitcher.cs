using Microsoft.Xna.Framework;

namespace TeamRock.Common
{
    public class ColorFlashSwitcher
    {
        private Color _startColor;
        private Color _endColor;

        private float _currentLerpAmount;
        private float _lerpRate;

        private bool _isIncreasing;
        private bool _startFlash;

        #region Update

        public Color Update(float deltaTime)
        {
            if (_isIncreasing)
            {
                _currentLerpAmount += _lerpRate * deltaTime;

                if (_currentLerpAmount >= 1)
                {
                    _isIncreasing = false;
                }
            }
            else
            {
                _currentLerpAmount -= _lerpRate * deltaTime;

                if (_currentLerpAmount <= 0)
                {
                    _isIncreasing = true;
                }
            }

            return Color.Lerp(_startColor, _endColor, _currentLerpAmount);
        }

        #endregion

        #region External Functions

        public Color StartColor
        {
            get => _startColor;
            set => _startColor = value;
        }

        public Color EndColor
        {
            get => _endColor;
            set => _endColor = value;
        }

        public float LerpRate
        {
            get => _lerpRate;
            set => _lerpRate = value;
        }

        public bool StartFlash
        {
            get => _startFlash;
            set => _startFlash = value;
        }

        #endregion
    }
}