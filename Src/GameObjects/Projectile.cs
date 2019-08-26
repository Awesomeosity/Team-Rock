using Microsoft.Xna.Framework;
using TeamRock.Utils;

namespace TeamRock.Src.GameObjects
{
    public class Projectile : GameObject
    {
        private float _lifeTime;
        private bool _isProjectileDestroyed;

        private Vector2 _positionToTarget;
        private float _initialDistanceToTarget;

        #region Constructor

        public Projectile(Sprite sprite, int collisionWidth, int collisionHeight) : base(sprite, collisionWidth,
            collisionHeight)
        {
        }

        #endregion

        #region Update

        public override void Update(float deltaTime, float gameTime)
        {
            base.Update(deltaTime, gameTime);
            UpdateAssetScale();

            _lifeTime -= deltaTime;
            if (_lifeTime <= 0)
            {
                _isProjectileDestroyed = true;
            }
        }

        #endregion

        #region External Functions

        public void LaunchProjectile(Vector2 directionNormalized, Vector2 positionToTarget)
        {
            Velocity = directionNormalized *
                       ExtensionFunctions.RandomInRange(GameInfo.MinProjectileVelocity,
                           GameInfo.MaxProjectileVelocity);
            _lifeTime = GameInfo.ProjectileLifeTime;

            _positionToTarget = positionToTarget;
            _initialDistanceToTarget = Vector2.DistanceSquared(Position, _positionToTarget);
        }

        public bool IsProjectileDestroyed => _isProjectileDestroyed;

        #endregion

        #region Utility Functions

        private void UpdateAssetScale()
        {
            float currentDistance = Vector2.DistanceSquared(Position, _positionToTarget);
            float scaledAsset = ExtensionFunctions.Map(currentDistance, _initialDistanceToTarget,
                GameInfo.MaxProjectileAssetScaleDistanceSq,
                GameInfo.ProjectileStartAssetScale, GameInfo.ProjectileFinalAssetScale);

            if (scaledAsset > Sprite.Scale)
            {
                Sprite.Scale = scaledAsset;
            }
        }

        #endregion
    }
}