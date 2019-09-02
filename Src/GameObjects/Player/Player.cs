using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Common;
using TeamRock.Managers;
using TeamRock.Scene;
using TeamRock.Utils;

namespace TeamRock.Src.GameObjects
{
    public class Player
    {
        public enum Poses
        {
            Normal,
            Pose_1,
            Pose_2
        }

        private PlayerController _playerController;
        private GameObject _playerGameObject;
        private SpriteSheetAnimationManager _playerFallingSpriteSheet;

        private float _velocityScaler;

        private float _dashCooldown;
        private float _dashDuration;
        private Vector2 _dashDirection;

        private float _poseDuration;
        private int _consecutivePoseCount;
        private float _boostDuration;

        private Vector2 _spriteSheetPosition;

        private bool _currentPose;
        private Texture2D _playerPose;
        private Texture2D _pose1;
        private Texture2D _pose2;

        private ColorFlashSwitcher _playerColorFlasher;

        public delegate void PlayerHitNotification();

        public PlayerHitNotification OnPlayerHitNotification;

        private bool _useAsDummy; // This is a very hacky method that handles Custom Player Hiding and Collision Boxes

        #region Initialization

        public void Initialize(ContentManager contentManager)
        {
            _playerPose = contentManager.Load<Texture2D>(AssetManager.Player);
            Sprite playerSprite = new Sprite(_playerPose)
            {
                Scale = GameInfo.PlayerAssetScale,
            };
            playerSprite.SetOriginCenter();

            _pose1 = contentManager.Load<Texture2D>(AssetManager.Pose1);
            _pose2 = contentManager.Load<Texture2D>(AssetManager.Pose2);

            _playerFallingSpriteSheet = new SpriteSheetAnimationManager();
            _playerFallingSpriteSheet.Initialize(contentManager, AssetManager.FireFallingBase,
                AssetManager.FireFallingTotalCount, 1, true);
            _playerFallingSpriteSheet.FrameTime = 0.01666667F;
            _playerFallingSpriteSheet.RenderOnStopped = false;
            _playerFallingSpriteSheet.Sprite.SetOriginCenter();
            _spriteSheetPosition = Vector2.Zero;

            _playerGameObject = new GameObject(playerSprite,
                _playerPose.Width * GameInfo.PlayerAssetScale,
                _playerPose.Height * GameInfo.PlayerAssetScale)
            {
                Acceleration = GameInfo.BaseAccelerationRate,
                Position = GameInfo.PlayerInitialPosition,
            };

            _playerController = new PlayerController();

            _velocityScaler = 1.0f;

            _dashCooldown = 0;
            _dashDuration = 0;
            _poseDuration = 0;
            _consecutivePoseCount = 0;
            _boostDuration = 0;

            _playerColorFlasher = new ColorFlashSwitcher()
            {
                StartFlash = false,
                StartColor = Color.White * 1,
                EndColor = Color.White * 0,
                FlashCount = GameInfo.PlayerFlashCount,
                LerpRate = GameInfo.PlayerFlashRate,
                ResetAutomatically = true
            };
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_useAsDummy)
            {
                return;
            }

            if (_dashCooldown <= 0 && _poseDuration <= 0)
            {
                _playerFallingSpriteSheet.Draw(spriteBatch);
            }

            _playerGameObject.Draw(spriteBatch);
        }

        public void DrawDebug(SpriteBatch spriteBatch) => _playerGameObject.DrawDebug(spriteBatch);

        #endregion

        #region Update

        public void Update(float deltaTime, float gameTime)
        {
            if (_useAsDummy)
            {
                return;
            }

            UpdateDurations(deltaTime);

            if (_playerGameObject.Velocity.Y < GameInfo.PlayerMaxYVelocity)
            {
                _playerGameObject.UpdateOnlyVelocity(deltaTime, gameTime);
            }

            _playerGameObject.Sprite.SpriteColor = _playerColorFlasher.Update(deltaTime);

            _playerController.Update();

            if (_playerGameObject.Position.X < GameInfo.PlayerLeftPosition)
            {
                _playerGameObject.Position = new Vector2(GameInfo.PlayerLeftPosition, _playerGameObject.Position.Y);
            }
            else if (_playerGameObject.Position.X > GameInfo.PlayerRightPosition)
            {
                _playerGameObject.Position = new Vector2(GameInfo.PlayerRightPosition, _playerGameObject.Position.Y);
            }
            else if (_playerGameObject.Position.Y < GameInfo.PlayerMinYPosition)
            {
                _playerGameObject.Position = new Vector2(_playerGameObject.Position.X, GameInfo.PlayerMinYPosition);
            }
            else if (_playerGameObject.Position.Y > GameInfo.PlayerMaxYPosition)
            {
                _playerGameObject.Position = new Vector2(_playerGameObject.Position.X, GameInfo.PlayerMaxYPosition);
            }

            HandleInput(deltaTime);
            UpdateSpriteSheet(deltaTime);
        }

        private void HandleInput(float deltaTime)
        {
            int speedFactor = 1;
            if (_playerController.DidPlayerPressDash && _dashCooldown <= 0 &&
                ExtensionFunctions.FloatCompare(1, _velocityScaler) && _consecutivePoseCount < GameInfo.MaxPoseCount)
            {
                if (_currentPose)
                {
                    GameObject.Sprite.UpdateTexture(_pose1);
                    _currentPose = !_currentPose;
                }
                else
                {
                    GameObject.Sprite.UpdateTexture(_pose2);
                    _currentPose = !_currentPose;
                }

                _dashDuration = GameInfo.PlayerDashDuration;
                _dashDirection = _playerController.DashDirection;
                _poseDuration = GameInfo.PlayerPoseDuration;
                _consecutivePoseCount += 1;
            }

            if (_dashDuration > 0)
            {
                if (_velocityScaler == 1)
                {
                    speedFactor = 3;
                }

                _playerGameObject.Position += _dashDirection * GameInfo.PlayerDashVelocity * deltaTime;
            }

            if (_playerController.State == PlayerController.ControllerState.Right)
            {
                _playerGameObject.Position += GameInfo.HorizontalVelocity * deltaTime * _velocityScaler * speedFactor;
            }
            else if (_playerController.State == PlayerController.ControllerState.Left)
            {
                _playerGameObject.Position -= GameInfo.HorizontalVelocity * deltaTime * _velocityScaler * speedFactor;
            }
            else if (_playerController.State == PlayerController.ControllerState.Up)
            {
                if (_playerGameObject.Velocity.Y < GameInfo.PlayerMinYVelocity)
                {
                    return;
                }

                _playerGameObject.Position -= GameInfo.VerticalVelocity * deltaTime * _velocityScaler * speedFactor;
                _playerGameObject.Acceleration -= GameInfo.AccelerationChangeRate * deltaTime;
            }
            else if (_playerController.State == PlayerController.ControllerState.Down)
            {
                _playerGameObject.Position += GameInfo.VerticalVelocity * deltaTime * _velocityScaler * speedFactor;
                _playerGameObject.Acceleration += GameInfo.AccelerationChangeRate * deltaTime;
            }
        }

        private void UpdateDurations(float deltaTime)
        {
            if (_velocityScaler < 1)
            {
                _velocityScaler += GameInfo.PlayerRecoveryRate * deltaTime;
            }

            if (_velocityScaler > 1)
            {
                _velocityScaler -= GameInfo.PlayerSlowdownRate * deltaTime;
            }

            if (_dashCooldown > 0)
            {
                _dashCooldown -= deltaTime;
            }

            if (_dashDuration > 0)
            {
                _dashDuration -= deltaTime;
            }

            if (_boostDuration > 0)
            {
                _boostDuration -= deltaTime;
                if (_boostDuration <= 0)
                {
                    _velocityScaler = 1;
                }
            }

            if (_poseDuration > 0)
            {
                _poseDuration -= deltaTime;

                if (_poseDuration <= 0)
                {
                    if (_velocityScaler <= 1 && _boostDuration <= 0)
                    {
                        _velocityScaler = GameInfo.PlayerDamageVelocity;
                    }

                    GameObject.Sprite.UpdateTexture(_playerPose);

                    _consecutivePoseCount = 0;
                    _dashCooldown = GameInfo.PlayerDashCooldown;
                }
            }
        }

        #endregion

        #region External Functions

        public void UpdateSpriteSheet(float deltaTime)
        {
            _playerFallingSpriteSheet.Update(deltaTime);
            _spriteSheetPosition.X = _playerGameObject.Position.X;
            _spriteSheetPosition.Y = _playerGameObject.Position.Y - _playerGameObject.Sprite.Height / 2.0f;
            _playerFallingSpriteSheet.Sprite.Position = _spriteSheetPosition;
        }

        public void ReduceVelocity()
        {
            // Prevent player from slowing down/speeding up while speed is different.
            if (!ExtensionFunctions.FloatCompare(_velocityScaler, 1))
            {
                return;
            }

            _velocityScaler = 1;
            if (_poseDuration > 0 && _boostDuration <= 0)
            {
                _velocityScaler = GameInfo.PlayerIncreasedVelocity;
                _boostDuration = GameInfo.PlayerBoostDuration;
                MainScreen.Instance.PlayerDashed();
            }
            else
            {
                _velocityScaler = GameInfo.PlayerDamageVelocity;
                if (_playerGameObject.Position.Y < GameInfo.PlayerMinYPosition)
                {
                    return;
                }

                _playerGameObject.Position -= new Vector2(0, GameInfo.PlayerKnockBack);
                MainScreen.Instance.PlayerCollided();
            }
        }

        public void PlayerHit()
        {
            _playerColorFlasher.StartFlash = true;
            OnPlayerHitNotification?.Invoke();
        }

        public bool IsPosing() => _poseDuration > 0;

        public Vector2 GetScaledVelocity() => _velocityScaler * _playerGameObject.Velocity;

        public void ResetPlayer()
        {
            _velocityScaler = 1.0f;

            _dashCooldown = 0;
            _dashDuration = 0;

            _poseDuration = 0;
            _currentPose = false;
            _consecutivePoseCount = 0;

            _playerGameObject.Sprite.UpdateTexture(_playerPose);

            _playerFallingSpriteSheet.StartSpriteAnimation();
            _spriteSheetPosition.X = _playerGameObject.Position.X;
            _spriteSheetPosition.Y = _playerGameObject.Position.Y - _playerGameObject.Sprite.Height / 2.0f;
            _playerFallingSpriteSheet.Sprite.Position = _spriteSheetPosition;
        }

        public void PlayerSetEndState()
        {
            _playerColorFlasher.StopAndReset();
        }

        public void SetPose(Poses pose)
        {
            switch (pose)
            {
                case Poses.Normal:
                    _playerGameObject.Sprite.UpdateTexture(_playerPose);
                    break;

                case Poses.Pose_1:
                    _playerGameObject.Sprite.UpdateTexture(_pose1);
                    break;

                case Poses.Pose_2:
                    _playerGameObject.Sprite.UpdateTexture(_pose2);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool UseAsDummy
        {
            get => _useAsDummy;
            set => _useAsDummy = value;
        }

        public SpriteSheetAnimationManager PlayerSpriteSheet
        {
            get => _playerFallingSpriteSheet;
            set => _playerFallingSpriteSheet = value;
        }

        public GameObject GameObject => _playerGameObject;

        #endregion
    }
}