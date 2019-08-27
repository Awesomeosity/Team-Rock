using System.Collections.Generic;
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
    public class MainScreen : CustomScreen
    {
        private ContentManager _contentManager;

        private SoundEffect _music;
        private SoundEffect _clap;

        private GameObject _stage;
        private Player _player;

        private SpriteSheetAnimationManager _backgroundSpriteSheet;
        private ScrollingBackground _scrollingBackground;

        private List<Audience> _audiences;

        private UiTextNode _timerText;
        private SpriteFont _defaultFont;
        private float _timeToImpact;

        private bool _stopControlledMovement; // TODO: Probably Switch to an Enum
        private bool _gameOver;

        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;

            CreateSounds();
            CreatePlayerAndBackground();
            CreateAudiences();
            CreateOtherSceneItems();
        }

        private void CreateSounds()
        {
            _music = _contentManager.Load<SoundEffect>(AssetManager.MainScreenMusic);
            _clap = _contentManager.Load<SoundEffect>(AssetManager.Clap);
        }

        private void CreatePlayerAndBackground()
        {
            _player = new Player();
            _player.Initialize(_contentManager);

            Texture2D backgroundTexture = _contentManager.Load<Texture2D>(AssetManager.WrestingBackground);
            _backgroundSpriteSheet = new SpriteSheetAnimationManager();
            _backgroundSpriteSheet.Initialize(_contentManager, AssetManager.WrestlingBackgroundBase,
                AssetManager.WrestlingBackgroundTotalCount, 0, true);
            _backgroundSpriteSheet.Sprite.UseSize = true;
            _backgroundSpriteSheet.Sprite.SetSize(GameInfo.FixedWindowWidth, GameInfo.FixedWindowHeight);
            _backgroundSpriteSheet.FrameTime = AssetManager.WrestlingBackgroundAnimationSpeed;

            Texture2D scrollingBackgroundTexture = _contentManager.Load<Texture2D>(AssetManager.BackgroundRopes);
            _scrollingBackground = new ScrollingBackground();
            _scrollingBackground.Initialization(scrollingBackgroundTexture, GameInfo.FixedWindowWidth);

            Texture2D stage = _contentManager.Load<Texture2D>(AssetManager.Stage);
            Sprite stageSprite = new Sprite(stage)
            {
                Scale = GameInfo.StageScale
            };
            stageSprite.SetOriginCenter();
            _stage = new GameObject(stageSprite, stageSprite.Width, stageSprite.Height)
            {
                Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight - 100)
            };
        }

        private void CreateAudiences()
        {
            _audiences = new List<Audience>
            {
                new Audience(_player, _contentManager) {Position = new Vector2(GameInfo.LeftAudiencePos, 0)},
                new Audience(_player, _contentManager) {Position = new Vector2(GameInfo.RightAudiencePos, 0)}
            };
        }

        private void CreateOtherSceneItems()
        {
            _timeToImpact = GameInfo.TotalGameTime;
            _defaultFont = _contentManager.Load<SpriteFont>(AssetManager.Arial);

            _timerText = new UiTextNode()
            {
                Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, 20)
            };
            _timerText.Initialize(_defaultFont, "");
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            _backgroundSpriteSheet.Draw(spriteBatch);
            _scrollingBackground.Draw(spriteBatch);

            _timerText.Draw(spriteBatch);

            _player.Draw(spriteBatch);
            _stage.Draw(spriteBatch);

            foreach (Audience audience in _audiences)
            {
                audience.DrawProjectiles(spriteBatch);
            }
        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
            _stage.DrawDebug(spriteBatch);
            _player.GameObject.DrawDebug(spriteBatch);

            foreach (Audience audience in _audiences)
            {
                audience.DrawDebug(spriteBatch);
            }
        }

        #endregion

        #region Update

        public override bool Update(float deltaTime, float gameTime)
        {
            _scrollingBackground.Update(deltaTime, _player.GameObject.Velocity.Y);
            _backgroundSpriteSheet.Update(deltaTime);

            if (_timeToImpact > 0)
            {
                _timeToImpact -= deltaTime;
                _timerText.Text = $"Time To Impact: {ExtensionFunctions.Format2DecimalPlace(_timeToImpact)}";
            }
            else if (_timeToImpact <= 0)
            {
                _stopControlledMovement = true;
                _timerText.Text = "Smack Down!!!";
            }

            _player.Update(deltaTime, gameTime);
            _stage.Update(deltaTime, gameTime);

            foreach (Audience audience in _audiences)
            {
                audience.Update(deltaTime, gameTime);
            }

            return _gameOver;
        }

        #endregion

        #region External Functions

        public void ResetScreen()
        {
            // TODO: Implement Actual Screen Reset Logic
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

        #region Utility Functions

        #endregion

        #region Singleton

        private static MainScreen _instance;
        private int _musicIndex;

        public static MainScreen Instance => _instance ?? (_instance = new MainScreen());

        private MainScreen()
        {
        }

        #endregion
    }
}