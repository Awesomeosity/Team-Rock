using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.CustomCamera;
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
        private SoundEffect _explosionSound;

        private GameObject _stage;
        private Player _player;

        private SpriteSheetAnimationManager _backgroundSpriteSheet;
        private ScrollingBackground _scrollingBackground;

        private SpriteSheetAnimationManager _endExplosion;

        private List<Audience> _audiences;

        private UiTextNode _timerText;
        private SpriteFont _defaultFont;
        private float _timeToImpact;

        private enum GameState
        {
            IsRunning,
            EndStarted,
            EndAnimations,
            GameOver
        }

        private GameState _gameState;

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
            _explosionSound = _contentManager.Load<SoundEffect>(AssetManager.Explosion);
        }

        private void CreatePlayerAndBackground()
        {
            _player = new Player();
            _player.Initialize(_contentManager);

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
                Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight + 300)
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

            _endExplosion = new SpriteSheetAnimationManager();
            _endExplosion.Initialize(_contentManager, AssetManager.EndExplosionBase,
                AssetManager.EndExplosionTotalCount, 1, false, false);
            _endExplosion.Sprite.SetOriginCenter();
            _endExplosion.Sprite.Position =
                new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight + 300);
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            _backgroundSpriteSheet.Draw(spriteBatch);
            _scrollingBackground.Draw(spriteBatch);

            _timerText.Draw(spriteBatch);
            _stage.Draw(spriteBatch);


            switch (_gameState)
            {
                case GameState.IsRunning:
                case GameState.EndStarted:
                    _player.Draw(spriteBatch);
                    break;

                case GameState.EndAnimations:
                    _endExplosion.Draw(spriteBatch);
                    break;

                case GameState.GameOver:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

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
            _backgroundSpriteSheet.Update(deltaTime);

            switch (_gameState)
            {
                case GameState.IsRunning:
                    UpdateGameEndTimer(deltaTime);
                    UpdateGameObjectsBeforeTime(deltaTime, gameTime);
                    break;

                case GameState.EndStarted:
                    UpdateStageAndPlayerEndState(deltaTime, gameTime);
                    break;

                case GameState.EndAnimations:
                    UpdateEndStateAnimations(deltaTime);
                    break;

                case GameState.GameOver:
                    // Don't do Anything
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (Audience audience in _audiences)
            {
                audience.Update(deltaTime, gameTime);
            }

            return _gameState == GameState.GameOver;
        }

        #endregion

        #region External Functions

        public void ResetScreen()
        {
            _player.GameObject.Position = GameInfo.PlayerInitialPosition;
            _player.GameObject.Acceleration = GameInfo.BaseAccelerationRate;

            _stage.Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight + 300);

            _timeToImpact = GameInfo.TotalGameTime;
            _stage.Velocity = new Vector2(0, 0);


            foreach (Audience audience in _audiences)
            {
                audience.IsProjectileSPawningActive = true;
                audience.ClearProjectiles();
            }


            SetGameState(GameState.IsRunning);
        }

        public void StartMusic()
        {
            _musicIndex = SoundManager.Instance.PlaySound(_music);
            SoundManager.Instance.SetSoundLooping(_musicIndex, true);
            SoundManager.Instance.SetSoundVolume(_musicIndex, 0.5f);
        }

        public void StopMusic() => SoundManager.Instance.StopSound(_musicIndex);

        #endregion

        #region Utility Functions

        private void UpdateGameEndTimer(float deltaTime)
        {
            if (_timeToImpact > 0)
            {
                _timeToImpact -= deltaTime;
                _timerText.Text = $"Time To Impact: {ExtensionFunctions.Format2DecimalPlace(_timeToImpact)}";
            }
            else if (_timeToImpact <= 0)
            {
                SetGameState(GameState.EndStarted);
                _timerText.Text = "Smack Down!!!";

                foreach (Audience audience in _audiences)
                {
                    audience.IsProjectileSPawningActive = false;
                }

                _player.GameObject.Acceleration = Vector2.Zero;
                _player.GameObject.Velocity = Vector2.Zero;
            }
        }

        private void UpdateGameObjectsBeforeTime(float deltaTime, float gameTime)
        {
            _scrollingBackground.Update(deltaTime, _player.GameObject.Velocity.Y);
            _player.Update(deltaTime, gameTime);
            _stage.Update(deltaTime, gameTime);
        }

        private void UpdateStageAndPlayerEndState(float deltaTime, float gameTime)
        {
            // TODO: Change this. It is super hacky
            if (_stage.Position.Y > GameInfo.FixedWindowHeight - 100)
            {
                _stage.Velocity = -Vector2.UnitY * GameInfo.StageMoveUpSpeed;
                _stage.Update(deltaTime, gameTime);
            }

            _player.GameObject.Velocity = Vector2.UnitY * GameInfo.PlayerMaxYVelocity;
            _player.GameObject.Update(deltaTime, gameTime);
            _player.UpdateSpriteSheet(deltaTime);

            if (_player.GameObject.DidCollide(_stage))
            {
                _endExplosion.StartSpriteAnimation();
                _endExplosion.Sprite.Position = _player.GameObject.Position;

                GamePadVibrationController.Instance.StartVibration(GameInfo.GamePadMaxIntensity, GameInfo.GamePadMaxIntensity, GameInfo.GamePadVibrationTime);

                CameraShaker.Instance.StartShake(1, 5);
                SoundManager.Instance.PlaySound(_explosionSound);

                SetGameState(GameState.EndAnimations);
            }
        }

        private void UpdateEndStateAnimations(float deltaTime)
        {
            _endExplosion.Update(deltaTime);
            if (!_endExplosion.IsAnimationActive)
            {
                SetGameState(GameState.GameOver);
            }
        }

        private void SetGameState(GameState gameState)
        {
            if (gameState == _gameState)
            {
                return;
            }

            _gameState = gameState;
        }

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