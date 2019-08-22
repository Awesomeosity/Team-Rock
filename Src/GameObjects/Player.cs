using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TeamRock.Utils;

namespace TeamRock.Src.GameObjects
{
    public class Player
    {
        private GameObject _playerGameObject;

        #region Initialization

        public void Initialize(ContentManager contentManager)
        {
            Texture2D playerTexture = contentManager.Load<Texture2D>(AssetManager.WhitePixel);
            Sprite playerSprite = new Sprite(playerTexture)
            {
                Scale = GameInfo.PlayerAssetScale,
            };
            playerSprite.SetOriginCenter();

            _playerGameObject = new GameObject(playerSprite,
                (int)(playerTexture.Width * GameInfo.PlayerAssetScale),
                (int)(playerTexture.Height * GameInfo.PlayerAssetScale))
            {
                Acceleration = GameInfo.BaseAccelerationRate,
                Position = GameInfo.PlayerInitialPosition,
            };
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            _playerGameObject.Draw(spriteBatch);
        }

        #endregion

        #region Update

        public void Update(float deltaTime, float gameTime)
        {
            _playerGameObject.UpdateOnlyVelocity(deltaTime, gameTime);
            HandleInput(deltaTime);
        }

        private void HandleInput(float deltaTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Right))
            {
                if (_playerGameObject.Position.X > GameInfo.PlayerRightPosition)
                {
                    return;
                }

                _playerGameObject.Position += GameInfo.HorizontalVelocity * deltaTime;
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                if (_playerGameObject.Position.X < GameInfo.PlayerLeftPosition)
                {
                    return;
                }

                _playerGameObject.Position -= GameInfo.HorizontalVelocity * deltaTime;
            }
            else if (state.IsKeyDown(Keys.Up))
            {
                if (_playerGameObject.Position.Y < GameInfo.PlayerMinYPosition)
                {
                    return;
                }

                _playerGameObject.Position -= GameInfo.VerticalVelocity * deltaTime;
                _playerGameObject.Acceleration -= GameInfo.AccelerationChangeRate * deltaTime;
            }
            else if (state.IsKeyDown(Keys.Down))
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