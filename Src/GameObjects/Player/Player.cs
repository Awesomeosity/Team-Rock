using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Managers;
using TeamRock.Utils;

namespace TeamRock.Src.GameObjects
{
    public class Player
    {
        private PlayerController _playerController;
        private GameObject _playerGameObject;
        private SpriteSheetAnimationManager _playerFallingSpriteSheet;

        private float _velocityScaler;

        private float _dashCooldown;
        private float _dashDuration;
        private Vector2 _dashDirection;

        private Vector2 _spriteSheetPosition;

        #region Initialization

        public void Initialize(ContentManager contentManager)
        {
            Texture2D playerTexture = contentManager.Load<Texture2D>(AssetManager.Player);
            Sprite playerSprite = new Sprite(playerTexture)
            {
                Scale = GameInfo.PlayerAssetScale,
            };
            playerSprite.SetOriginCenter();

            _playerFallingSpriteSheet = new SpriteSheetAnimationManager();
            _playerFallingSpriteSheet.Initialize(contentManager, AssetManager.FireFallingBase,
                AssetManager.FireFallingTotalCount, 1, true);
            _playerFallingSpriteSheet.FrameTime = 0.01666667F;
            _playerFallingSpriteSheet.Sprite.SetOriginCenter();
            _spriteSheetPosition = Vector2.Zero;

            _playerGameObject = new GameObject(playerSprite,
                playerTexture.Width * GameInfo.PlayerAssetScale,
                playerTexture.Height * GameInfo.PlayerAssetScale)
            {
                Acceleration = GameInfo.BaseAccelerationRate,
                Position = GameInfo.PlayerInitialPosition,
            };

            _playerController = new PlayerController();

            _velocityScaler = 1.0f;

            _dashCooldown = 0;
            _dashDuration = 0;
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_dashCooldown <= 0)
            {
                _playerFallingSpriteSheet.Draw(spriteBatch);
            }

            _playerGameObject.Draw(spriteBatch);
        }

        #endregion

        #region Update

        public void Update(float deltaTime, float gameTime)
        {
            UpdateDurations(deltaTime);

            if (_playerGameObject.Velocity.Y < GameInfo.PlayerMaxYVelocity)
            {
                _playerGameObject.UpdateOnlyVelocity(deltaTime, gameTime);
            }

            _playerController.Update();

            HandleInput(deltaTime);
            UpdateSpriteSheet(deltaTime);
        }

        private void HandleInput(float deltaTime)
        {
            int speedFactor = 1;
            if (_playerController.DidPlayerPressDash && _dashCooldown <= 0)
            {
                _dashDuration = GameInfo.PlayerDashDuration;
                _dashDirection = _playerController.DashDirection;
            }

            if (_dashDuration > 0)
            {
                if (_velocityScaler == 1)
                {
                    speedFactor = 3;
                }

                Console.WriteLine($"Dash Velocity: {_dashDirection}");
                _playerGameObject.Position += _dashDirection * GameInfo.PlayerDashVelocity * deltaTime;
            }

            if (_dashDuration > 0)
            {
                return;
            }

            if (_playerController.State == PlayerController.ControllerState.Right)
            {
                if (_playerGameObject.Position.X > GameInfo.PlayerRightPosition)
                {
                    return;
                }

                _playerGameObject.Position += GameInfo.HorizontalVelocity * deltaTime * _velocityScaler * speedFactor;
            }
            else if (_playerController.State == PlayerController.ControllerState.Left)
            {
                if (_playerGameObject.Position.X < GameInfo.PlayerLeftPosition)
                {
                    return;
                }

                _playerGameObject.Position -= GameInfo.HorizontalVelocity * deltaTime * _velocityScaler * speedFactor;
            }
            else if (_playerController.State == PlayerController.ControllerState.Up)
            {
                if (_playerGameObject.Position.Y < GameInfo.PlayerMinYPosition ||
                    _playerGameObject.Velocity.Y < GameInfo.PlayerMinYVelocity)
                {
                    return;
                }

                _playerGameObject.Position -= GameInfo.VerticalVelocity * deltaTime * _velocityScaler * speedFactor;
                _playerGameObject.Acceleration -= GameInfo.AccelerationChangeRate * deltaTime;
            }
            else if (_playerController.State == PlayerController.ControllerState.Down)
            {
                if (_playerGameObject.Position.Y > GameInfo.PlayerMaxYPosition)
                {
                    return;
                }

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
                _velocityScaler = 1.0f;
            }

            if (_dashCooldown > 0)
            {
                _dashCooldown -= deltaTime;
            }

            if (_dashDuration > 0)
            {
                _dashDuration -= deltaTime;

                if (_dashDuration <= 0)
                {
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
            _velocityScaler = GameInfo.PlayerDamageVelocity;
            _playerGameObject.Position -= new Vector2(0, GameInfo.PlayerKnockBack);
        }

        public Vector2 GetScaledVelocity() => _velocityScaler * _playerGameObject.Velocity;

        public void ResetPlayer()
        {
            _velocityScaler = 1.0f;
            _dashCooldown = 0;
            _dashDuration = 0;
        }

    public GameObject GameObject => _playerGameObject;

        #endregion
    }
}