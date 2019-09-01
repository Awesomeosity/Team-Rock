using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Scene.Screen_Items.Cinematics;
using TeamRock.Src.GameObjects;
using TeamRock.Utils;

namespace TeamRock.Scene
{
    public class CinematicScreen : CustomScreen
    {
        private ContentManager _contentManager;

        private CinematicBackgroundScroller _cinematicBackgroundScroller;

        private Sprite _stage;
        private Sprite _player;

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

            _stage = new Sprite(stageTexture) {Scale = GameInfo.StageScale};
            _stage.SetOriginCenter();

            _player = new Sprite(playerTexture) {Scale = GameInfo.PlayerAssetScale};
            _player.SetOriginCenter();

            _cinematicBackgroundScroller = new CinematicBackgroundScroller();
            _cinematicBackgroundScroller.Initialize(scrollerRope, 0.5f, GameInfo.TotalCinematicRopes,
                GameInfo.CinematicStageInitialPosition, GameInfo.CinematicRowFinalPosition);
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            _cinematicBackgroundScroller.Draw(spriteBatch);
            _stage.Draw(spriteBatch);
            _player.Draw(spriteBatch);
        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
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
                    UpdatePlayerClimbing(deltaTime);
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
            _player.Position -= Vector2.UnitY * GameInfo.PlayerInitialClimbSpeed * deltaTime;
            if (_player.Position.Y <= GameInfo.PlayerMoveToRopePosition)
            {
                _cinematicBackgroundScroller.StartScrolling = true;
                _cinematicBackgroundScroller.ScrollingSpeed = GameInfo.CinematicScrollMoveSpeed;
                _cinematicBackgroundScroller.OnPositionReached += PlayerClimbingPositionReached;

                SetCinematicState(CinematicState.Climbing);
            }

            _genericTimer -= deltaTime;
            if (_genericTimer <= 0)
            {
                _genericTimer = GameInfo.PlayerSpriteFlipRate;

                SpriteEffects playerSpriteEffects = _player.SpriteEffects;
                switch (playerSpriteEffects)
                {
                    case SpriteEffects.None:
                        _player.SpriteEffects = SpriteEffects.FlipHorizontally;
                        break;

                    case SpriteEffects.FlipHorizontally:
                        _player.SpriteEffects = SpriteEffects.None;
                        break;

                    case SpriteEffects.FlipVertically:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void UpdatePlayerClimbing(float deltaTime)
        {
            _genericTimer -= deltaTime;
            if (_genericTimer <= 0)
            {
                _genericTimer = GameInfo.PlayerSpriteFlipRate;

                SpriteEffects playerSpriteEffects = _player.SpriteEffects;
                switch (playerSpriteEffects)
                {
                    case SpriteEffects.None:
                        _player.SpriteEffects = SpriteEffects.FlipHorizontally;
                        break;

                    case SpriteEffects.FlipHorizontally:
                        _player.SpriteEffects = SpriteEffects.None;
                        break;

                    case SpriteEffects.FlipVertically:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _stage.Position += Vector2.UnitY * GameInfo.CinematicScrollMoveSpeed * deltaTime;

            _cinematicBackgroundScroller.Update(deltaTime);
        }

        private void PlayerClimbingPositionReached()
        {
            _cinematicBackgroundScroller.OnPositionReached -= PlayerClimbingPositionReached;
            _genericTimer = GameInfo.StageTopWaitTimer;

            SetCinematicState(CinematicState.PlayerReachedTop);
        }

        private void UpdatePlayerReachedTop(float deltaTime)
        {
            _genericTimer -= deltaTime;
            if (_genericTimer <= 0)
            {
                _player.SpriteEffects = SpriteEffects.FlipVertically;
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
            _player.Position = new Vector2(GameInfo.FixedWindowWidth / 2.0f, GameInfo.FixedWindowHeight - 200);
            _player.SpriteEffects = SpriteEffects.None;

            _cinematicBackgroundScroller.Reset();

            SetCinematicState(CinematicState.StageDisplay);
            _genericTimer = GameInfo.InitialStageDisplayWaitTimer;
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