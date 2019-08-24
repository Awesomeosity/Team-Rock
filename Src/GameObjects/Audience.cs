using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TeamRock.Utils;

namespace TeamRock.Src.GameObjects
{
    class Audience : GameObject
    {
        private ContentManager _contentManager;
        private float _randomTimer;
        private List<Projectile> _enemies;
        private Player _player;

        #region Initialization

        public Audience(Sprite sprite, Player player, ContentManager contentManager, int collisionHeight,
            int collisionWidth) : base(sprite, collisionHeight, collisionWidth)
        {
            _contentManager = contentManager;
            _randomTimer =
                ExtensionFunctions.RandomInRange(GameInfo.MinProjectileSpawnTimer, GameInfo.MaxProjectileSpawnTimer);

            _player = player;
            _enemies = new List<Projectile>();
        }

        #endregion


        #region Update

        public override void Update(float deltaTime, float gameTime)
        {
            _randomTimer -= deltaTime;
            if (_randomTimer <= 0)
            {
                SpawnProjectileAndResetTimer();
            }

            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                _enemies[i].Update(deltaTime, gameTime);

                if (_enemies[i].IsProjectileDestroyed || _enemies[i].DidCollide(_player.GameObject))
                {
                    _enemies.RemoveAt(i);
                }
            }
        }

        #endregion

        #region Draw

        public void DrawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (Projectile enemy in _enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }

        #endregion

        #region Utility Functions

        private void SpawnProjectileAndResetTimer()
        {
            _randomTimer =
                ExtensionFunctions.RandomInRange(GameInfo.MinProjectileSpawnTimer, GameInfo.MaxProjectileSpawnTimer);

            Texture2D projectileTexture = _contentManager.Load<Texture2D>(AssetManager.WhitePixel);
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

            _enemies.Add(projectile);
        }

        #endregion
    }
}
