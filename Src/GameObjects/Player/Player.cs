using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using TeamRock.Managers;
using TeamRock.Utils;

namespace TeamRock.Src.GameObjects
{
    public class Player
    {
        private PlayerController _playerController;
        private GameObject _playerGameObject;
        private SpriteSheetAnimationManager _playerFallingSpriteSheet;

        #region Initialization

        public void Initialize(ContentManager contentManager)
        {
            Texture2D playerTexture = contentManager.Load<Texture2D>($"{AssetManager.FireFallingBase}1");
            Sprite playerSprite = new Sprite(playerTexture)
            {
                Scale = 2,
            };
            playerSprite.SetOriginCenter();

            _playerFallingSpriteSheet = new SpriteSheetAnimationManager();
            _playerFallingSpriteSheet.Initialize(contentManager, AssetManager.FireFallingBase,
                AssetManager.FireFallingTotalCount, 1, true);
            _playerFallingSpriteSheet.StartSpriteAnimation();

            _playerGameObject = new GameObject(playerSprite,
                playerTexture.Width * GameInfo.PlayerAssetScale / 2.0f,
                (playerTexture.Height) * GameInfo.PlayerAssetScale)
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
            _playerGameObject.Sprite.UpdateTexture(_playerFallingSpriteSheet.GetCurrentFrameTexture());
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
            _playerFallingSpriteSheet.Update(deltaTime);

            HandleInput(deltaTime);
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
                if (_playerGameObject.Position.Y < GameInfo.PlayerMinYPosition)
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

        public GameObject GameObject => _playerGameObject;

        #endregion
    }
}