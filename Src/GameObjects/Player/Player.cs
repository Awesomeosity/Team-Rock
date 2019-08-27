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
            _playerFallingSpriteSheet.FrameTime = 0.08333334F;
            _playerFallingSpriteSheet.Sprite.SetOriginCenter();
            _playerFallingSpriteSheet.Sprite.UseSize = true;
            _playerFallingSpriteSheet.Sprite.SetSize((int) playerSprite.Width + 30,
                (int) playerSprite.Height + 50);
            _spriteSheetPosition = Vector2.Zero;

            _playerGameObject = new GameObject(playerSprite,
                playerTexture.Width * GameInfo.PlayerAssetScale,
                playerTexture.Height * GameInfo.PlayerAssetScale)
            {
                Acceleration = GameInfo.BaseAccelerationRate,
                Position = GameInfo.PlayerInitialPosition,
            };

            _playerController = new PlayerController();
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            _playerFallingSpriteSheet.Draw(spriteBatch);
            _playerGameObject.Draw(spriteBatch);
        }

        #endregion

        #region Update

        public void Update(float deltaTime, float gameTime)
        {
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
            if (_playerController.State == PlayerController.ControllerState.Right)
            {
                if (_playerGameObject.Position.X > GameInfo.PlayerRightPosition)
                {
                    return;
                }

                _playerGameObject.Position += GameInfo.HorizontalVelocity * deltaTime;
            }
            else if (_playerController.State == PlayerController.ControllerState.Left)
            {
                if (_playerGameObject.Position.X < GameInfo.PlayerLeftPosition)
                {
                    return;
                }

                _playerGameObject.Position -= GameInfo.HorizontalVelocity * deltaTime;
            }
            else if (_playerController.State == PlayerController.ControllerState.Up)
            {
                if (_playerGameObject.Position.Y < GameInfo.PlayerMinYPosition ||
                    _playerGameObject.Velocity.Y < GameInfo.PlayerMinYVelocity)
                {
                    return;
                }

                _playerGameObject.Position -= GameInfo.VerticalVelocity * deltaTime;
                _playerGameObject.Acceleration -= GameInfo.AccelerationChangeRate * deltaTime;
            }
            else if (_playerController.State == PlayerController.ControllerState.Down)
            {
                if (_playerGameObject.Position.Y > GameInfo.PlayerMaxYPosition)
                {
                    return;
                }

                _playerGameObject.Position += GameInfo.VerticalVelocity * deltaTime;
                _playerGameObject.Acceleration += GameInfo.AccelerationChangeRate * deltaTime;
            }
        }

        #endregion

        #region External Functions

        public void UpdateSpriteSheet(float deltaTime)
        {
            _playerFallingSpriteSheet.Update(deltaTime);
            _spriteSheetPosition.X = _playerGameObject.Position.X;
            _spriteSheetPosition.Y = _playerGameObject.Position.Y - _playerGameObject.Sprite.Height;
            _playerFallingSpriteSheet.Sprite.Position = _spriteSheetPosition;
        }

        public GameObject GameObject => _playerGameObject;

        #endregion
    }
}