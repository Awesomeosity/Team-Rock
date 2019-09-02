using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Src.GameObjects;
using TeamRock.Utils;

namespace TeamRock.UI
{
    public class Fader
    {
        private Sprite _faderSprite;

        private float _fadeInRate;
        private float _fadeOutRate;

        private bool _fadeInActive;
        private bool _fadeOutActive;

        private float _currentAlpha;
        private Color _spriteColor;

        public delegate void FadeInComplete();
        public delegate void FadeOutComplete();

        public FadeOutComplete OnFadeOutComplete;
        public FadeInComplete OnFadeInComplete;

        #region Initialization

        public void Initialize(ContentManager contentManager, float fadeInRate, float fadeOutRate)
        {
            Texture2D whitePixel = contentManager.Load<Texture2D>(AssetManager.WhitePixel);
            _faderSprite = new Sprite(whitePixel, true);
            _faderSprite.SetSize(GameInfo.FixedWindowWidth, GameInfo.FixedWindowHeight);

            _fadeInRate = fadeInRate;
            _fadeOutRate = fadeOutRate;
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch) => _faderSprite.Draw(spriteBatch);

        #endregion

        #region Update

        public void Update(float deltaTime)
        {
            if (_fadeInActive)
            {
                FadeIn(deltaTime);
            }
            else if (_fadeOutActive)
            {
                FadeOut(deltaTime);
            }
        }

        private void FadeIn(float deltaTime)
        {
            _currentAlpha += _fadeInRate * deltaTime;

            if (_currentAlpha >= 1)
            {
                OnFadeInComplete?.Invoke();
                _fadeInActive = false;
            }

            _faderSprite.SpriteColor = _spriteColor * _currentAlpha;
        }

        private void FadeOut(float deltaTime)
        {
            _currentAlpha -= _fadeOutRate * deltaTime;

            if (_currentAlpha <= 0)
            {
                OnFadeOutComplete?.Invoke();
                _fadeOutActive = false;
            }

            _faderSprite.SpriteColor = _spriteColor * _currentAlpha;
        }

        #endregion

        #region External Functions

        public void StartFadeIn()
        {
            _fadeInActive = true;
            _fadeOutActive = false;

            _currentAlpha = 0;
        }

        public void StartFadeOut()
        {
            _fadeInActive = false;
            _fadeOutActive = true;

            _currentAlpha = 1;
        }

        public void SetSpriteColor(Color color)
        {
            _faderSprite.SpriteColor = color;
            _spriteColor = color;
        }

        #endregion

        #region Singleton

        private static Fader _instance;
        public static Fader Instance => _instance ?? (_instance = new Fader());

        private Fader()
        {
        }

        #endregion
    }
}