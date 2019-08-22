using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TeamRock.Src.GameObjects;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TeamRock.Managers;
using TeamRock.Utils;

namespace TeamRock.Scene
{
    public class MainScreen : CustomScreen
    {
        private ContentManager _contentManager;

        private List<Projectile> _enemies; // TODO: Change this later to handle Audience which will spawn projectiles...
        private float _randomTimer; // TODO: Remove this later on. Will also be for each Audience...

        private SpriteSheetAnimationManager
            _testSpriteSheetAnimation; // TODO: Remove this later on. Just added to test SpriteSheet Animations...

        private Player _player;

        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;

            _enemies = new List<Projectile>();
            _randomTimer =
                ExtensionFunctions.RandomInRange(GameInfo.MinProjectileSpawnTimer, GameInfo.MaxProjectileSpawnTimer);

            _player = new Player();
            _player.Initialize(_contentManager);

            _testSpriteSheetAnimation = new SpriteSheetAnimationManager();
            _testSpriteSheetAnimation.Initialize(_contentManager, AssetManager.TestExplosionBase,
                AssetManager.TestExplosionTotalCount, true);
            Sprite animationSprite = _testSpriteSheetAnimation.Sprite;
            animationSprite.Position = new Vector2(GameInfo.WindowWidth / 2.0f, GameInfo.WindowHeight / 2.0f);
            animationSprite.SetOriginCenter();
            _testSpriteSheetAnimation.FrameTime = 0.0834f;
            _testSpriteSheetAnimation.StartSpriteAnimation();
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            _player.Draw(spriteBatch);

            foreach (Projectile enemy in _enemies)
            {
                enemy.Draw(spriteBatch);
            }

            DrawEffects(spriteBatch);
        }

        private void DrawEffects(SpriteBatch spriteBatch)
        {
            _testSpriteSheetAnimation.Draw(spriteBatch);
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

            _player.Update(deltaTime, gameTime);
            _testSpriteSheetAnimation.Update(deltaTime);

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

        #region External Functions

        public void ResetScreen()
        {
            _enemies.Clear();

            _player = new Player();
            _player.Initialize(_contentManager);
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

            float xPosition;
            float yPosition = ExtensionFunctions.RandomInRange(0, GameInfo.WindowHeight);
            if (ExtensionFunctions.Random() <= 0.5f)
            {
                xPosition = ExtensionFunctions.RandomInRange(0, GameInfo.LeftAudienceRectangle.Width);
            }
            else
            {
                xPosition = ExtensionFunctions.RandomInRange(GameInfo.RightAudienceRectangle.X,
                    GameInfo.RightAudienceRectangle.X + GameInfo.RightAudienceRectangle.Width);
            }

            Vector2 launchPosition = new Vector2(xPosition, yPosition);
            Vector2 launchDirection = _player.GameObject.Position - launchPosition;
            launchDirection.Normalize();

            Projectile projectile = new Projectile(projectileSprite,
                (int)(projectileTexture.Width * GameInfo.ProjectileAssetScale),
                (int)(projectileTexture.Height * GameInfo.ProjectileAssetScale))
            {
                Position = launchPosition
            };
            projectile.LaunchProjectile(launchDirection);

            _enemies.Add(projectile);
        }

        #endregion

        #region Singleton

        private static MainScreen _instance;
        public static MainScreen Instance => _instance ?? (_instance = new MainScreen());

        private MainScreen()
        {
        }

        #endregion
    }
}