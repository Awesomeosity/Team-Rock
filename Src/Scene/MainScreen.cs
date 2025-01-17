﻿using System;
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

        private Sprite _starSprite;
        private SpriteSheetAnimationManager _starSpriteSheetAnimationManager;
        private Vector2 _starsOffset;

        private GameObject _stage;
        private GameObject _winWrestler;
        private Player _player;

        private Sprite _backgroundSprite;
        private ScrollingBackground _backgroundAudience;
        private ScrollingBackground _scrollingBackground;

        private SpriteSheetAnimationManager _endExplosion;
        private SpriteSheetAnimationManager _confetti1;
        private SpriteSheetAnimationManager _confetti2;

        private List<Audience> _audiences;

        private float _hinderedPlayerTimer;
        private float _currentIncrementModifierTimer;
        private float _playerCollisionTimerRate;

        private float _unhinderedTimeToImpact;
        private float _timeToGameOver;

        private Sprite _fillBarPointer;
        private Vector2 _fillBarPointerInitialPosition;
        private Vector2 _fillBarPointerFinalPosition;

        private SpriteFlasher _fillBarFlasher;
        private Sprite _fillBarGradient;
        private FillBarVertical _fillBarVertical;

        private bool _exitScreen;
        private bool _screenActive;

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
            CreateFillBarGradient();
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

            _playerCollisionTimerRate = GameInfo.PlayerTimerChangeRate;

            Texture2D backgroundTexture = _contentManager.Load<Texture2D>(AssetManager.GameBG);
            Texture2D wrestlingBackground = _contentManager.Load<Texture2D>(AssetManager.WrestingBackground);

            _backgroundSprite = new Sprite(backgroundTexture, true);
            _backgroundSprite.SetOriginCenter();
            _backgroundSprite.SetSize(GameInfo.FixedWindowWidth, GameInfo.FixedWindowHeight);
            _backgroundSprite.Position =
                new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight / 2.0f);

            _backgroundAudience = new ScrollingBackground();
            _backgroundAudience.Initialize(wrestlingBackground, GameInfo.MaxBackgroundElements,
                new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight), 1, true);
            _backgroundAudience.SetSize(GameInfo.FixedWindowWidth, GameInfo.FixedWindowHeight);

            Texture2D scrollingBackgroundTexture = _contentManager.Load<Texture2D>(AssetManager.BackgroundRopes);
            _scrollingBackground = new ScrollingBackground();
            _scrollingBackground.Initialize(scrollingBackgroundTexture, GameInfo.MaxBackgroundElements,
                new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight), 0.5f);

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

            Texture2D winWrestler = _contentManager.Load<Texture2D>(AssetManager.WinWrestler);
            Sprite winSprite = new Sprite(winWrestler)
            {
                Scale = GameInfo.WrestlerScale
            };
            winSprite.SetOriginCenter();
            _winWrestler = new GameObject(winSprite, winSprite.Width, winSprite.Height)
            {
                Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight + 250)
            };

            _starSpriteSheetAnimationManager = new SpriteSheetAnimationManager();
            _starSpriteSheetAnimationManager.Initialize(_contentManager, AssetManager.StarBase,
                AssetManager.StarTotalCount, 0, true);
            _starSpriteSheetAnimationManager.FrameTime = AssetManager.StarAnimationSpeed;
            _starSprite = _starSpriteSheetAnimationManager.Sprite;
            _starSprite.Scale = 0.4f;
            _starsOffset = new Vector2(_winWrestler.Sprite.Width - 50, 70);
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
            _unhinderedTimeToImpact = GameInfo.TotalGameTime;
            _hinderedPlayerTimer = GameInfo.TotalGameTime;

            _timeToGameOver = GameInfo.EndGameTime;

            _endExplosion = new SpriteSheetAnimationManager();
            _endExplosion.Initialize(_contentManager, AssetManager.EndExplosionBase,
                AssetManager.EndExplosionTotalCount, 1, false, false);
            _endExplosion.Sprite.SetOriginCenter();
            _endExplosion.Sprite.Position =
                new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight + 300);

            _confetti1 = new SpriteSheetAnimationManager();
            _confetti1.Initialize(_contentManager, AssetManager.ConfettiBase_1,
                AssetManager.ConfettiTotalCount_1, 1, false, false);
            _confetti1.Sprite.SetOriginCenter();
            _confetti1.Sprite.Position =
                new Vector2(GameInfo.FixedWindowWidth / 4.0f, GameInfo.FixedWindowHeight + 300);
            _confetti1.FrameTime = AssetManager.ConfettiAnimationSpeed_1;

            _confetti2 = new SpriteSheetAnimationManager();
            _confetti2.Initialize(_contentManager, AssetManager.ConfettiBase_2,
                AssetManager.ConfettiTotalCount_2, 1, false, false);
            _confetti2.Sprite.SetOriginCenter();
            _confetti2.Sprite.Position =
                new Vector2(GameInfo.FixedWindowWidth / 4.0f * 3.0f, GameInfo.FixedWindowHeight + 300);
            _confetti2.FrameTime = AssetManager.ConfettiAnimationSpeed_2;
        }

        private void CreateFillBarGradient()
        {
            Texture2D fillBarBackgroundTexture = _contentManager.Load<Texture2D>(AssetManager.FillBarBackground);
            Texture2D fillBarFrameTexture = _contentManager.Load<Texture2D>(AssetManager.FillBarFrame);
            Texture2D fillBarGradientTexture = _contentManager.Load<Texture2D>(AssetManager.FillBarGradient);

            Sprite fillBarBackground = new Sprite(fillBarBackgroundTexture);
            Sprite fillBarFrame = new Sprite(fillBarFrameTexture);
            _fillBarGradient = new Sprite(fillBarGradientTexture);

            fillBarBackground.SetOriginCenter();
            fillBarFrame.SetOriginCenter();
            _fillBarGradient.Origin = new Vector2(_fillBarGradient.TextureWidth / 2.0f, 0);

            fillBarFrame.Scale = 0.5f;
            fillBarBackground.Scale = 0.5f;
            _fillBarGradient.Scale = 0.5f;

            _fillBarVertical = new FillBarVertical();
            _fillBarVertical.Initialize(fillBarFrame, fillBarBackground, _fillBarGradient, GameInfo.TotalGameTime);

            float barXPosition = GameInfo.FixedWindowWidth / 2.0f + GameInfo.CenterBoardWidth / 2.0f;

            fillBarBackground.Position = new Vector2(barXPosition, GameInfo.FixedWindowHeight / 2.0f);
            fillBarFrame.Position = new Vector2(barXPosition, GameInfo.FixedWindowHeight / 2.0f);
            _fillBarGradient.Position = new Vector2(barXPosition,
                GameInfo.FixedWindowHeight / 2.0f - _fillBarGradient.ScaledHeight / 2.0f);

            Texture2D whitePixel = _contentManager.Load<Texture2D>(AssetManager.WhitePixel);
            _fillBarFlasher = new SpriteFlasher(whitePixel, true);
            _fillBarFlasher.Origin = new Vector2(_fillBarFlasher.TextureWidth / 2.0f, 0);
            _fillBarFlasher.SetSize((int) _fillBarGradient.Width, 0);
            _fillBarFlasher.StartFlashing(GameInfo.InitialBarFlashRate, GameInfo.FlashBarMinAlpha,
                GameInfo.FlashBarMaxAlpha);
            _fillBarFlasher.Position = new Vector2(barXPosition,
                GameInfo.FixedWindowHeight / 2.0f - _fillBarGradient.ScaledHeight / 2.0f);
            _fillBarFlasher.SetSpriteColor(GameInfo.FlashBarColor);

            Texture2D fillBarPointer = _contentManager.Load<Texture2D>(AssetManager.FillBarPointer);
            _fillBarPointer = new Sprite(fillBarPointer);
            _fillBarPointer.SetOriginCenter();
            _fillBarPointer.Scale = 0.5f;

            _fillBarPointerInitialPosition =
                new Vector2(barXPosition + 35,
                    GameInfo.FixedWindowHeight / 2.0f - _fillBarGradient.ScaledHeight / 2.0f);
            _fillBarPointerFinalPosition =
                new Vector2(barXPosition + 35,
                    GameInfo.FixedWindowHeight / 2.0f + _fillBarGradient.ScaledHeight / 2.0f);

            _fillBarPointer.Position = _fillBarPointerInitialPosition;
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            _backgroundAudience.Draw(spriteBatch);
            _scrollingBackground.Draw(spriteBatch);

            _fillBarVertical.Draw(spriteBatch);
            _fillBarFlasher.Draw(spriteBatch);
            _fillBarPointer.Draw(spriteBatch);

            _stage.Draw(spriteBatch);
            _winWrestler.Draw(spriteBatch);

            if (_unhinderedTimeToImpact > 0)
            {
                _starSpriteSheetAnimationManager.Draw(spriteBatch);
            }

            switch (_gameState)
            {
                case GameState.IsRunning:
                case GameState.EndStarted:
                    _player.Draw(spriteBatch);
                    break;

                case GameState.EndAnimations:
                    if (_endExplosion.IsAnimationActive)
                    {
                        _endExplosion.Draw(spriteBatch);
                    }

                    if (_hinderedPlayerTimer <= 0)
                    {
                        _confetti1.Draw(spriteBatch);
                        _confetti2.Draw(spriteBatch);
                    }

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
            _player.DrawDebug(spriteBatch);

            foreach (Audience audience in _audiences)
            {
                audience.DrawDebug(spriteBatch);
            }
        }

        #endregion

        #region Update

        public override bool Update(float deltaTime, float gameTime)
        {
            if (_screenActive)
            {
                UpdateFillBarColor(deltaTime);
                _starSpriteSheetAnimationManager.Update(deltaTime);

                switch (_gameState)
                {
                    case GameState.IsRunning:
                        UpdateGameEndTimer(deltaTime);
                        UpdatePlayerRacingTimer(deltaTime);
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
            }

            return _exitScreen;
        }

        #endregion

        #region External Functions

        public void ResetScreen()
        {
            _player.GameObject.Position = GameInfo.PlayerInitialPosition;
            _player.GameObject.Acceleration = GameInfo.BaseAccelerationRate;
            _player.ResetPlayer();

            _playerCollisionTimerRate = GameInfo.PlayerTimerChangeRate;

            _stage.Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight + 300);

            _unhinderedTimeToImpact = GameInfo.TotalGameTime;
            _hinderedPlayerTimer = GameInfo.TotalGameTime;

            _stage.Velocity = new Vector2(0, 0);

            _winWrestler.Sprite.UpdateTexture(_contentManager.Load<Texture2D>(AssetManager.WinWrestler));
            _winWrestler.Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight + 250);
            _winWrestler.Velocity = new Vector2(0, 0);
            _starSprite.Position = _winWrestler.Position - _starsOffset;

            _fillBarPointer.Position = _fillBarPointerInitialPosition;
            _fillBarVertical.Reset();

            foreach (Audience audience in _audiences)
            {
                audience.IsProjectileSpawningActive = true;
                audience.ClearProjectiles();
            }

            _endExplosion.StopSpriteAnimation();
            _confetti1.StopSpriteAnimation();
            _confetti2.StopSpriteAnimation();

            _timeToGameOver = GameInfo.EndGameTime;

            _exitScreen = false;
            _screenActive = false;

            Fader.Instance.OnFadeInComplete += HandleFadeIn;
            Fader.Instance.OnFadeOutComplete += HandleFadeOut;

            SetGameState(GameState.IsRunning);
        }

        public void StartMusic()
        {
            _musicIndex = SoundManager.Instance.PlaySound(_music, true);
            SoundManager.Instance.SetSoundVolume(_musicIndex, 0.75f);
        }

        public void StopMusic() => SoundManager.Instance.StopSound(_musicIndex);

        public void PlayerCollided()
        {
            _currentIncrementModifierTimer = GameInfo.PlayerHitTimerAffectTime;
            _playerCollisionTimerRate = GameInfo.PlayerHitTimerChangeRate;
        }

        public void PlayerDashed()
        {
            _currentIncrementModifierTimer = GameInfo.PlayerDashAffectTime;
            _playerCollisionTimerRate = GameInfo.PlayerDashTimerChangeRate;
        }

        #endregion

        #region Utility Functions

        private void UpdateFillBarColor(float deltaTime)
        {
            _fillBarVertical.Update(deltaTime);
            _fillBarVertical.CurrentValue = GameInfo.TotalGameTime - _unhinderedTimeToImpact;

            float barHeight = _fillBarVertical.BarHeight;
            _fillBarFlasher.SetSize((int) _fillBarGradient.Width, (int) barHeight);

            float mappedFlashRate = ExtensionFunctions.Map(_unhinderedTimeToImpact, 0, 30, GameInfo.MaxBarFlashRate,
                GameInfo.InitialBarFlashRate);
            _fillBarFlasher.FlashingRate = mappedFlashRate;
            _fillBarFlasher.Update(deltaTime);

            float reachedRatio = (GameInfo.TotalGameTime - _unhinderedTimeToImpact) / GameInfo.TotalGameTime;
            Color lerpedColor = Color.Lerp(GameInfo.InitialTimerBarColor, GameInfo.FinalTimerBarColor, reachedRatio);
            _fillBarGradient.SpriteColor = lerpedColor;
        }

        private void UpdatePlayerRacingTimer(float deltaTime)
        {
            if (_currentIncrementModifierTimer > 0)
            {
                _currentIncrementModifierTimer -= deltaTime;

                if (_currentIncrementModifierTimer <= 0)
                {
                    _playerCollisionTimerRate = GameInfo.PlayerTimerChangeRate;
                }
            }

            if (_hinderedPlayerTimer > 0)
            {
                _hinderedPlayerTimer -= _playerCollisionTimerRate * deltaTime;

                float ratio = ExtensionFunctions.Map(_hinderedPlayerTimer, 0, GameInfo.TotalGameTime, 1, 0);
                Vector2 playerMarkerPosition =
                    Vector2.Lerp(_fillBarPointerInitialPosition, _fillBarPointerFinalPosition, ratio);
                _fillBarPointer.Position = playerMarkerPosition;

                if (_hinderedPlayerTimer <= 0)
                {
                    SetGameState(GameState.EndStarted);

                    _player.GameObject.Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, 0);
                    _player.SetPose(Player.Poses.Pose_1);
                    _player.PlayerSpriteSheet.StopSpriteAnimation();
                    _player.PlayerSetEndState();

                    foreach (Audience audience in _audiences)
                    {
                        audience.IsProjectileSpawningActive = false;
                        audience.ClearProjectiles();
                    }

                    _player.GameObject.Acceleration = Vector2.Zero;
                    _player.GameObject.Velocity = Vector2.Zero;
                }
            }
        }

        private void UpdateGameEndTimer(float deltaTime)
        {
            if (_unhinderedTimeToImpact > 0)
            {
                _unhinderedTimeToImpact -= deltaTime;
            }
            else if (_unhinderedTimeToImpact <= 0)
            {
                SetGameState(GameState.EndStarted);
                _winWrestler.Sprite.UpdateTexture(_contentManager.Load<Texture2D>(AssetManager.LoseWrestler));
                _winWrestler.Position += new Vector2(100, -100);

                _player.GameObject.Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, 0);
                _player.SetPose(Player.Poses.Normal);
                _player.PlayerSetEndState();

                foreach (Audience audience in _audiences)
                {
                    audience.IsProjectileSpawningActive = false;
                    audience.ClearProjectiles();
                }

                _player.GameObject.Acceleration = Vector2.Zero;
                _player.GameObject.Velocity = Vector2.Zero;
            }
        }

        private void UpdateGameObjectsBeforeTime(float deltaTime, float gameTime)
        {
            _backgroundAudience.Update(deltaTime, _player.GetScaledVelocity().Y);
            _scrollingBackground.Update(deltaTime, _player.GetScaledVelocity().Y);
            _player.Update(deltaTime, gameTime);
            _stage.Update(deltaTime, gameTime);
            _winWrestler.Update(deltaTime, gameTime);
        }

        private void UpdateStageAndPlayerEndState(float deltaTime, float gameTime)
        {
            if (_stage.Position.Y > GameInfo.FixedWindowHeight - 100)
            {
                _stage.Velocity = -Vector2.UnitY * GameInfo.StageMoveUpSpeed;
                _stage.Update(deltaTime, gameTime);
                _winWrestler.Velocity = -Vector2.UnitY * GameInfo.StageMoveUpSpeed;
                _winWrestler.Update(deltaTime, gameTime);
                _starSprite.Position = _winWrestler.Position - _starsOffset;
            }

            _player.GameObject.Velocity = Vector2.UnitY * GameInfo.PlayerMaxYVelocity;
            _player.GameObject.Update(deltaTime, gameTime);
            _player.UpdateSpriteSheet(deltaTime);

            if (_player.GameObject.DidCollide(_stage))
            {
                _endExplosion.StartSpriteAnimation();
                _endExplosion.Sprite.Position = _player.GameObject.Position;

                _confetti1.StartSpriteAnimation();
                _confetti1.Sprite.Position = _player.GameObject.Position + new Vector2(100, 0);
                _confetti2.StartSpriteAnimation();
                _confetti2.Sprite.Position = _player.GameObject.Position + new Vector2(-100, 0);


                GamePadVibrationController.Instance.StartVibration(GameInfo.GamePadMaxIntensity,
                    GameInfo.GamePadMaxIntensity, GameInfo.GamePadVibrationTime);

                CameraShaker.Instance.StartShake(1, 5);
                SoundManager.Instance.PlaySound(_explosionSound);

                SetGameState(GameState.EndAnimations);
            }
        }

        private void UpdateEndStateAnimations(float deltaTime)
        {
            _endExplosion.Update(deltaTime);
            _confetti1.Update(deltaTime);
            _confetti2.Update(deltaTime);
            if (!_endExplosion.IsAnimationActive)
            {
                _timeToGameOver -= deltaTime;
            }

            if (_timeToGameOver <= 0)
            {
                _screenActive = false;
                Fader.Instance.StartFadeIn();

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

        private void HandleFadeIn()
        {
            Fader.Instance.OnFadeInComplete -= HandleFadeIn;
            _exitScreen = true;
        }

        private void HandleFadeOut()
        {
            Fader.Instance.OnFadeOutComplete -= HandleFadeOut;
            _screenActive = true;
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