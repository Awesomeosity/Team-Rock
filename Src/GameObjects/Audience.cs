using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TeamRock.Utils;

namespace TeamRock.Src.GameObjects
{
    class Audience
    {
        private readonly ContentManager _contentManager;

        private readonly List<Projectile> _projectiles;
        private readonly Player _player;

        private Vector2 _position;
        private float _randomTimer;

        #region Initialization

        public Audience(Player player, ContentManager contentManager)
        {
            _contentManager = contentManager;
            _randomTimer =
                ExtensionFunctions.RandomInRange(GameInfo.MinProjectileSpawnTimer, GameInfo.MaxProjectileSpawnTimer);

            _player = player;
            _projectiles = new List<Projectile>();
        }

        #endregion

        #region Update

        public void Update(float deltaTime, float gameTime)
        {
            _randomTimer -= deltaTime;
            if (_randomTimer <= 0)
            {
                SpawnProjectileAndResetTimer();
            }

            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                _projectiles[i].Update(deltaTime, gameTime);

                if (_projectiles[i].IsProjectileDestroyed || _projectiles[i].DidCollide(_player.GameObject))
                {
                    _projectiles.RemoveAt(i);
                }
            }
        }

        #endregion

        #region Draw

        public void DrawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (Projectile projectile in _projectiles)
            {
                projectile.Draw(spriteBatch);
            }
        }

        #endregion

        #region External Functions

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        #endregion

        #region Utility Functions

        private void SpawnProjectileAndResetTimer()
        {
            _randomTimer =
                ExtensionFunctions.RandomInRange(GameInfo.MinProjectileSpawnTimer, GameInfo.MaxProjectileSpawnTimer);

            Texture2D projectileTexture =
                _contentManager.Load<Texture2D>(ExtensionFunctions.Random() <= 0.5f
                    ? AssetManager.Soda
                    : AssetManager.Popcorn);
            Sprite projectileSprite = new Sprite(projectileTexture)
            {
                Scale = GameInfo.ProjectileAssetScale
            };

            //This might be a little hacky... Might want to change depending on number of Audiences instantiated, which means passing in more parameters for Audience's reference?
            float xPosition =
                ExtensionFunctions.RandomInRange(Position.X, Position.X + GameInfo.LeftAudienceRectangle.Width);
            float yPosition = ExtensionFunctions.RandomInRange(0, GameInfo.FixedWindowHeight);

            Vector2 launchPosition = new Vector2(xPosition, yPosition);
            float offsetX = _player.GameObject.Position.X +
                            (ExtensionFunctions.RandomInRange(-1 * GameInfo.ProjectileAimRadius,
                                GameInfo.ProjectileAimRadius));
            float offsetY = _player.GameObject.Position.Y +
                            (ExtensionFunctions.RandomInRange(-1 * GameInfo.ProjectileAimRadius,
                                GameInfo.ProjectileAimRadius));
            Vector2 offsetAim = new Vector2(offsetX, offsetY);
            Vector2 launchDirection = offsetAim - launchPosition;
            launchDirection.Normalize();

            Projectile projectile = new Projectile(projectileSprite,
                (int) (projectileTexture.Width * GameInfo.ProjectileAssetScale),
                (int) (projectileTexture.Height * GameInfo.ProjectileAssetScale))
            {
                Position = launchPosition
            };
            projectile.LaunchProjectile(launchDirection);

            _projectiles.Add(projectile);
        }

        #endregion
    }
}