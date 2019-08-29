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
        private GameObject _winWrestler;
        private Player _player;
        private GameObject _distanceBackground;
        private GameObject _distanceFrame;
        private GameObject _distanceGradient;
        private GameObject _distanceIndicator;


        private SpriteSheetAnimationManager _backgroundSpriteSheet;
        private ScrollingBackground _scrollingBackground;

        private SpriteSheetAnimationManager _endExplosion;
        private SpriteSheetAnimationManager _confetti1;
        private SpriteSheetAnimationManager _confetti2;

        private List<Audience> _audiences;

        private UiTextNode _timerText;
        private SpriteFont _defaultFont;
        private float _timeToImpact;
        private float _timeToGameOver;

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
            _timeToGameOver = GameInfo.EndGameTime;
            _defaultFont = _contentManager.Load<SpriteFont>(AssetManager.Luckiest_Guy);

            _timerText = new UiTextNode()
            {
                Position = new Vector2(GameInfo.FixedWindowWidth / 4.0f, 50)
            };
            _timerText.Initialize(_defaultFont, "");

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

            Texture2D distanceBackground = _contentManager.Load<Texture2D>(AssetManager.DistanceBackground);
            Sprite backgroundSprite = new Sprite(distanceBackground)
            {
                Scale = 0.1f
            };
            backgroundSprite.SetOriginCenter();
            _distanceBackground = new GameObject(backgroundSprite, backgroundSprite.Width, backgroundSprite.Height)
            {
                Position = new Vector2(GameInfo.FixedWindowWidth - 20, GameInfo.FixedWindowHeight / 2)
            };
            Texture2D distanceGradient = _contentManager.Load<Texture2D>(AssetManager.DistanceGradient);
            Sprite gradientSprite = new Sprite(distanceGradient)
            {
                Scale = 0.1f
            };
            gradientSprite.SetOriginCenter();
            _distanceGradient = new GameObject(gradientSprite, gradientSprite.Width, gradientSprite.Height)
            {
                Position = new Vector2(GameInfo.FixedWindowWidth - 20, GameInfo.FixedWindowHeight / 2)
            };
            Texture2D distanceFrame = _contentManager.Load<Texture2D>(AssetManager.DistanceFrame);
            Sprite frameSprite = new Sprite(distanceFrame)
            {
                Scale = 0.1f
            };
            frameSprite.SetOriginCenter();

            _distanceFrame = new GameObject(frameSprite, frameSprite.Width, frameSprite.Height)
            {
                Position = new Vector2(GameInfo.FixedWindowWidth - 20, GameInfo.FixedWindowHeight / 2)
            };

            Texture2D distanceIndicator = _contentManager.Load<Texture2D>(AssetManager.DistanceIndicator);
            Sprite indicatorSprite = new Sprite(distanceIndicator)
            {
                Scale = 0.1f
            };
            indicatorSprite.SetOriginCenter();

            _distanceIndicator = new GameObject(indicatorSprite, indicatorSprite.Width, indicatorSprite.Height)
            {
                Position = new Vector2(GameInfo.FixedWindowWidth - 20, GameInfo.FixedWindowHeight / 2)
            };
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            _backgroundSpriteSheet.Draw(spriteBatch);
            _scrollingBackground.Draw(spriteBatch);

            _stage.Draw(spriteBatch);
            _winWrestler.Draw(spriteBatch);

            switch (_gameState)
            {
                case GameState.IsRunning:
                case GameState.EndStarted:
                    _player.Draw(spriteBatch);
                    break;

                case GameState.EndAnimations:
                    if(_endExplosion.IsAnimationActive == true)
                    {
                        _endExplosion.Draw(spriteBatch);
                    }
                    _confetti1.Draw(spriteBatch);
                    _confetti2.Draw(spriteBatch);
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

            _timerText.Draw(spriteBatch);
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

            SoundManager.Instance.CheckSound(_musicIndex);


            return _gameState == GameState.GameOver;
        }

        #endregion

        #region External Functions

        public void ResetScreen()
        {
            _player.GameObject.Position = GameInfo.PlayerInitialPosition;
            _player.GameObject.Acceleration = GameInfo.BaseAccelerationRate;
            _player.ResetPlayer();

            _stage.Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight + 300);

            _timeToImpact = GameInfo.TotalGameTime;
            _stage.Velocity = new Vector2(0, 0);

            _winWrestler.Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight + 250);
            _winWrestler.Velocity = new Vector2(0, 0);


            foreach (Audience audience in _audiences)
            {
                audience.IsProjectileSPawningActive = true;
                audience.ClearProjectiles();
            }

            _endExplosion.StopSpriteAnimation();
            _confetti1.StopSpriteAnimation();
            _confetti2.StopSpriteAnimation();

            _timeToGameOver = GameInfo.EndGameTime;
            
            SetGameState(GameState.IsRunning);
        }

        public void StartMusic()
        {
            _musicIndex = SoundManager.Instance.PlaySound(_music);
            SoundManager.Instance.SetSoundLooping(_musicIndex, true);
            SoundManager.Instance.SetSoundVolume(_musicIndex, 0.75f);
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
                _player.GameObject.Position = new Vector2(GameInfo.FixedWindowWidth / 2, 0);
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
            _scrollingBackground.Update(deltaTime, _player.GetScaledVelocity().Y);
            _player.Update(deltaTime, gameTime);
            _stage.Update(deltaTime, gameTime);
            _winWrestler.Update(deltaTime, gameTime);

        }

        private void UpdateStageAndPlayerEndState(float deltaTime, float gameTime)
        {
            // TODO: Change this. It is super hacky
            if (_stage.Position.Y > GameInfo.FixedWindowHeight - 100)
            {
                _stage.Velocity = -Vector2.UnitY * GameInfo.StageMoveUpSpeed;
                _stage.Update(deltaTime, gameTime);
                _winWrestler.Velocity = -Vector2.UnitY * GameInfo.StageMoveUpSpeed;
                _winWrestler.Update(deltaTime, gameTime);
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


                GamePadVibrationController.Instance.StartVibration(GameInfo.GamePadMaxIntensity, GameInfo.GamePadMaxIntensity, GameInfo.GamePadVibrationTime);

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

            if(_timeToGameOver <= 0)
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