using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Src.GameObjects;
using TeamRock.UI;
using TeamRock.Utils;
using TeamRock.Managers;
using Microsoft.Xna.Framework.Audio;

namespace TeamRock.Scene
{
    public class HomeScreen : CustomScreen
    {
        private UiImageButton _playButton;
        private ContentManager _contentManager;
        private SoundEffect _music;
        private int _musicIndex;

        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
            Texture2D playButtonTexture = contentManager.Load<Texture2D>(AssetManager.PlayImage);
            _playButton = new UiImageButton();
            _playButton.Initialize(
                playButtonTexture,
                1f,
                GameInfo.FixedWindowWidth / 2.0f,
                GameInfo.FixedWindowHeight / 2.0f
            );
            _contentManager = contentManager;
            _music = _contentManager.Load<SoundEffect>(AssetManager.Music1);
        }

        public void StartMusic()
        {
            _musicIndex = SoundManager.Instance.PlaySound(_music);
            SoundManager.Instance.SetSoundLooping(_musicIndex, true);
        }

        public void StopMusic()
        {
            SoundManager.Instance.StopSound(_musicIndex);
        }


        #endregion

        #region Render

        public override void Draw(SpriteBatch spriteBatch)
        {
            _playButton.Draw(spriteBatch);
        }

        #endregion

        #region Update

        public override bool Update(float deltaTime, float gameTime)
        {
            bool playButtonPressed = _playButton.Update();
            return playButtonPressed;
        }

        #endregion

        #region Singleton

        private static HomeScreen _instance;
        public static HomeScreen Instance => _instance ?? (_instance = new HomeScreen());

        private HomeScreen()
        {
        }

        #endregion
    }
}