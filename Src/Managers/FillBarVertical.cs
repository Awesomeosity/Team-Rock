using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Src.GameObjects;
using TeamRock.Utils;

namespace TeamRock.Managers
{
    public class FillBarVertical
    {
        private Sprite _background;
        private Sprite _topBar;
        private Sprite _unFilledBar;

        private float _maxTopBarHeight;

        private float _maxValue;
        private float _currentValue;

        private float _barHeight;

        #region Initialization

        public void Initialize(Sprite background, Sprite unFilledBar, Sprite topBar,
            float maxValue)
        {
            _maxValue = maxValue;
            _currentValue = 0;

            _background = background;
            _unFilledBar = unFilledBar;

            _topBar = topBar;
            _topBar.Origin = new Vector2(_topBar.TextureWidth / 2.0f, 0);
            _topBar.UseSize = true;

            _maxTopBarHeight = _topBar.ScaledHeight;
            _topBar.SetSize((int) _topBar.ScaledWidth, 0);
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            _background.Draw(spriteBatch);
            _unFilledBar.Draw(spriteBatch);
            _topBar.Draw(spriteBatch);
        }

        #endregion

        #region Update

        public void Update(float deltaTime)
        {
            float ratio = _currentValue / _maxValue;
            _barHeight = ExtensionFunctions.Map(ratio, 0, 1, 0, _maxTopBarHeight);

            _topBar.SetSize((int) _topBar.ScaledWidth, (int) _barHeight);
        }

        #endregion

        #region External Function

        public float MaxValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }

        public float CurrentValue
        {
            get => _currentValue;
            set
            {
                if (value > _maxValue)
                {
                    _currentValue = _maxValue;
                }
                else if (value < 0)
                {
                    _currentValue = 0;
                }
                else
                {
                    _currentValue = value;
                }
            }
        }

        public Sprite Background
        {
            get => _background;
            set => _background = value;
        }

        public Sprite TopBar
        {
            get => _topBar;
            set => _topBar = value;
        }

        public Sprite UnFilledBar
        {
            get => _unFilledBar;
            set => _unFilledBar = value;
        }

        public float BarHeight => _barHeight;

        #endregion
    }
}