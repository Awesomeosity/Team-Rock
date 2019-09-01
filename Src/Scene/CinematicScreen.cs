using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Managers;
using TeamRock.Scene.Screen_Items.Cinematics;
using TeamRock.Src.GameObjects;
using TeamRock.Utils;

namespace TeamRock.Scene
{
    public class CinematicScreen : CustomScreen
    {
        private ContentManager _contentManager;

        private Sprite _backgroundSprite;
        private ScrollingBackground _audienceScrolling;
        private CinematicBackgroundScroller _cinematicBackgroundScroller;

        private List<Audience> _audiences;

        private Sprite _winWrestler;
        private Sprite _stage;

        private Player _dummyPlayer;
        private Sprite _playerSprite;

        private float _genericTimer;

        private enum CinematicState
        {
            StageDisplay,
            PlayerMoveToPosition,
            Climbing,
            PlayerReachedTop,
            PlayerJump
        }

        private CinematicState _cinematicState;
        private bool _exitScreen;

        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;

            Texture2D stageTexture = _contentManager.Load<Texture2D>(AssetManager.Stage);
            Texture2D playerTexture = _contentManager.Load<Texture2D>(AssetManager.PlayerFlipped);
            Texture2D scrollerRope = _contentManager.Load<Texture2D>(AssetManager.BackgroundRopes);
            Texture2D winWrestler = _contentManager.Load<Texture2D>(AssetManager.WinWrestler);
            Texture2D backgroundTexture = _contentManager.Load<Texture2D>(AssetManager.GameBG);
            Texture2D wrestlingBackground = _contentManager.Load<Texture2D>(AssetManager.WrestingBackground);

            _stage = new Sprite(stageTexture) {Scale = GameInfo.StageScale};
            _stage.SetOriginCenter();

            _playerSprite = new Sprite(playerTexture) {Scale = GameInfo.PlayerAssetScale};
            _playerSprite.SetOriginCenter();

            _backgroundSprite = new Sprite(backgroundTexture, true);
            _backgroundSprite.SetOriginCenter();
            _backgroundSprite.SetSize(GameInfo.FixedWindowWidth, GameInfo.FixedWindowHeight);

            _audienceScrolling = new ScrollingBackground();
            _audienceScrolling.Initialize(wrestlingBackground, GameInfo.MaxBackgroundElements,
                new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight), 1, true);
            _audienceScrolling.SetSize(GameInfo.FixedWindowWidth, GameInfo.FixedWindowHeight);

            _cinematicBackgroundScroller = new CinematicBackgroundScroller();
            _cinematicBackgroundScroller.Initialize(scrollerRope, 0.5f, GameInfo.TotalCinematicRopes,
                GameInfo.CinematicStageInitialPosition, GameInfo.CinematicRowFinalPosition);

            _winWrestler = new Sprite(winWrestler)
            {
                Scale = GameInfo.WrestlerScale
            };
            _winWrestler.SetOriginCenter();

            _dummyPlayer = new Player();
            _dummyPlayer.Initialize(_contentManager);

            CreateAudiences();
        }

        private void CreateAudiences()
        {
            _audiences = new List<Audience>
            {
                new Audience(_dummyPlayer, _contentManager)
                {
                    Position = new Vector2(GameInfo.LeftAudiencePos, 0), SpawnPeople = false,
                    IsProjectileSpawningActive = false,
                    IsCollisionActive = false
                },
                new Audience(_dummyPlayer, _contentManager)
                {
                    Position = new Vector2(GameInfo.RightAudiencePos, 0), SpawnPeople = false,
                    IsProjectileSpawningActive = false,
                    IsCollisionActive = false
                }
            };
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            _backgroundSprite.Draw(spriteBatch);
            _audienceScrolling.Draw(spriteBatch);

            _cinematicBackgroundScroller.Draw(spriteBatch);

            _stage.Draw(spriteBatch);
            _winWrestler.Draw(spriteBatch);

            _playerSprite.Draw(spriteBatch);

            foreach (Audience audience in _audiences)
            {
                audience.DrawProjectiles(spriteBatch);
            }
        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
            _dummyPlayer.DrawDebug(spriteBatch);
        }

        #endregion

        #region Update

        public override bool Update(float deltaTime, float gameTime)
        {
            switch (_cinematicState)
            {
                case CinematicState.StageDisplay:
                    UpdateStageDisplay(deltaTime);
                    break;

                case CinematicState.PlayerMoveToPosition:
                    UpdatePlayerMoveToPosition(deltaTime);
                    break;

                case CinematicState.Climbing:
                    UpdatePlayerClimbing(deltaTime, gameTime);
                    break;

                case CinematicState.PlayerReachedTop:
                    UpdatePlayerReachedTop(deltaTime);
                    break;

                case CinematicState.PlayerJump:
                    UpdatePlayerJump(deltaTime);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (Audience audience in _audiences)
            {
                audience.Update(deltaTime, gameTime);
            }

            return _exitScreen;
        }

        private void UpdateStageDisplay(float deltaTime)
        {
            _genericTimer -= deltaTime;

            if (_genericTimer <= 0)
            {
                _genericTimer = GameInfo.PlayerSpriteFlipRate;
                SetCinematicState(CinematicState.PlayerMoveToPosition);
            }
        }

        private void UpdatePlayerMoveToPosition(float deltaTime)
        {
            _playerSprite.Position -= Vector2.UnitY * GameInfo.PlayerInitialClimbSpeed * deltaTime;
            if (_playerSprite.Position.Y <= GameInfo.PlayerMoveToRopePosition)
            {
                _cinematicBackgroundScroller.StartScrolling = true;
                _cinematicBackgroundScroller.ScrollingSpeed = GameInfo.CinematicScrollMoveSpeed;
                _cinematicBackgroundScroller.OnPositionReached += PlayerClimbingPositionReached;

                foreach (Audience audience in _audiences)
                {
                    audience.IsProjectileSpawningActive = true;
                }

                SetCinematicState(CinematicState.Climbing);
            }

            _genericTimer -= deltaTime;
            if (_genericTimer <= 0)
            {
                _genericTimer = GameInfo.PlayerSpriteFlipRate;

                SpriteEffects playerSpriteEffects = _playerSprite.SpriteEffects;
                switch (playerSpriteEffects)
                {
                    case SpriteEffects.None:
                        _playerSprite.SpriteEffects = SpriteEffects.FlipHorizontally;
                        break;

                    case SpriteEffects.FlipHorizontally:
                        _playerSprite.SpriteEffects = SpriteEffects.None;
                        break;

                    case SpriteEffects.FlipVertically:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void UpdatePlayerClimbing(float deltaTime, float gameTime)
        {
            _genericTimer -= deltaTime;
            if (_genericTimer <= 0)
            {
                _genericTimer = GameInfo.PlayerSpriteFlipRate;

                SpriteEffects playerSpriteEffects = _playerSprite.SpriteEffects;
                switch (playerSpriteEffects)
                {
                    case SpriteEffects.None:
                        _playerSprite.SpriteEffects = SpriteEffects.FlipHorizontally;
                        break;

                    case SpriteEffects.FlipHorizontally:
                        _playerSprite.SpriteEffects = SpriteEffects.None;
                        break;

                    case SpriteEffects.FlipVertically:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _dummyPlayer.GameObject.Position = _playerSprite.Position;

            _winWrestler.Position += Vector2.UnitY * GameInfo.CinematicScrollMoveSpeed * deltaTime;
            _stage.Position += Vector2.UnitY * GameInfo.CinematicScrollMoveSpeed * deltaTime;

            _audienceScrolling.Update(deltaTime, -GameInfo.CinematicScrollMoveSpeed);
            _cinematicBackgroundScroller.Update(deltaTime);
        }

        private void PlayerClimbingPositionReached()
        {
            _cinematicBackgroundScroller.OnPositionReached -= PlayerClimbingPositionReached;
            _genericTimer = GameInfo.StageTopWaitTimer;

            foreach (Audience audience in _audiences)
            {
                audience.IsProjectileSpawningActive = false;
            }

            SetCinematicState(CinematicState.PlayerReachedTop);
        }

        private void UpdatePlayerReachedTop(float deltaTime)
        {
            _genericTimer -= deltaTime;
            if (_genericTimer <= 0)
            {
                _playerSprite.SpriteEffects = SpriteEffects.FlipVertically;
                _genericTimer = GameInfo.StageDivingWaitTimer;

                SetCinematicState(CinematicState.PlayerJump);
            }
        }

        private void UpdatePlayerJump(float deltaTime)
        {
            _genericTimer -= deltaTime;
            if (_genericTimer <= 0)
            {
                _exitScreen = true;
            }
        }

        #endregion

        #region External Functions

        public void ResetScreen()
        {
            _stage.Position = GameInfo.CinematicStageInitialPosition;
            _winWrestler.Position = _stage.Position - Vector2.UnitY * 50;

            _playerSprite.Position = _stage.Position - Vector2.UnitY * 150;
            _playerSprite.SpriteEffects = SpriteEffects.None;

            foreach (Audience audience in _audiences)
            {
                audience.ClearProjectiles();
            }

            _exitScreen = false;

            _cinematicBackgroundScroller.Reset();
            _genericTimer = GameInfo.InitialStageDisplayWaitTimer;

            SetCinematicState(CinematicState.StageDisplay);
        }

        #endregion

        #region Utility Functions

        private void SetCinematicState(CinematicState cinematicState)
        {
            if (_cinematicState == cinematicState)
            {
                return;
            }

            _cinematicState = cinematicState;
        }

        #endregion

        #region Singleton

        private static CinematicScreen _instance;
        public static CinematicScreen Instance => _instance ?? (_instance = new CinematicScreen());

        private CinematicScreen()
        {
        }

        #endregion
    }
}