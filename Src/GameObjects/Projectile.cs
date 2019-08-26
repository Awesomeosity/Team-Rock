using Microsoft.Xna.Framework;
using TeamRock.Utils;

namespace TeamRock.Src.GameObjects
{
    public class Projectile : GameObject
    {
        private float _lifeTime;
        private bool _isProjectileDestroyed;

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

            _lifeTime -= deltaTime;
            if (_lifeTime <= 0)
            {
                _isProjectileDestroyed = true;
            }
        }

        #endregion

        #region External Functions

        public void LaunchProjectile(Vector2 directionNormalized)
        {
            Velocity = directionNormalized *
                       ExtensionFunctions.RandomInRange(GameInfo.MinProjectileVelocity,
                           GameInfo.MaxProjectileVelocity);
            _lifeTime = GameInfo.ProjectileLifeTime;
        }

        public bool IsProjectileDestroyed => _isProjectileDestroyed;

        #endregion
    }
}