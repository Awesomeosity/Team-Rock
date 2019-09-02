using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Managers;
using TeamRock.Src.GameObjects;
using TeamRock.UI;
using TeamRock.Utils;

namespace TeamRock.Scene
{
    public class InstructionScreen : CustomScreen
    {
        private ContentManager _contentManager;

        private SoundEffect _voiceOver_6;
        private Sprite _dashTutorial;

        private float _countDownTimer;
        private bool _screenActive;
        private bool _exitScreen;

        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;

            _voiceOver_6 = _contentManager.Load<SoundEffect>(AssetManager.VOScene6);

            Texture2D dashTutorial = _contentManager.Load<Texture2D>(AssetManager.DashControls);
            _dashTutorial = new Sprite(dashTutorial)
            {
                Scale = 0.2f,
                Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight / 2.0f)
            };
            _dashTutorial.SetOriginCenter();
        }

        #endregion

        #region Render

        public override void Draw(SpriteBatch spriteBatch)
        {
            _dashTutorial.Draw(spriteBatch);
        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
        }

        #endregion

        #region Update

        public override bool Update(float deltaTime, float gameTime)
        {
            if (_screenActive)
            {
                _countDownTimer -= deltaTime;
                if (_countDownTimer <= 0)
                {
                    _screenActive = false;
                    Fader.Instance.StartFadeIn();
                }
            }

            return _exitScreen;
        }

        #endregion

        #region External Functions

        public void ResetScreen()
        {
            _screenActive = false;
            _exitScreen = false;

            _countDownTimer = GameInfo.InstructionScreenWaitTimer;

            Fader.Instance.OnFadeInComplete += HandleFadeIn;
            Fader.Instance.OnFadeOutComplete += HandleFadeOut;
        }

        #endregion

        #region Utility Functions

        private void HandleFadeIn()
        {
            Fader.Instance.OnFadeInComplete -= HandleFadeIn;
            _exitScreen = true;
        }

        private void HandleFadeOut()
        {
            Fader.Instance.OnFadeOutComplete -= HandleFadeOut;
            _screenActive = true;

            SoundManager.Instance.PlaySound(_voiceOver_6);
        }

        #endregion

        #region Singleton

        private static InstructionScreen _instance;
        public static InstructionScreen Instance => _instance ?? (_instance = new InstructionScreen());

        private InstructionScreen()
        {
        }

        #endregion
    }
}