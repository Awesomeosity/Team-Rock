using System;
using Microsoft.Xna.Framework;
using TeamRock.Common;
using TeamRock.Utils;

namespace TeamRock.Src.GameObjects
{
    public class Projectile : GameObject
    {
        private ColorFlashSwitcher _colorFlashSwitcher;

        private float _lifeTime;
        private bool _isProjectileDestroyed;

        private Vector2 _positionToTarget;
        private float _initialDistanceToTarget;

        private float _rotationSpeed;

        public enum ProjSprite
        {
            Popcorn,
            Soda,
            Girl,
            Jordan
        }

        private ProjSprite _projSprite;

        #region Constructor

        public Projectile(Sprite sprite, int collisionWidth, int collisionHeight) : base(sprite, collisionWidth,
            collisionHeight)
        {
        }

        public ColorFlashSwitcher ColorFlashSwitcher
        {
            get => _colorFlashSwitcher;
            set => _colorFlashSwitcher = value;
        }

        #endregion

        #region Update

        public override void Update(float deltaTime, float gameTime)
        {
            base.Update(deltaTime, gameTime);

            Color spriteColor = _colorFlashSwitcher.Update(deltaTime);
            Sprite.SpriteColor = spriteColor;

            UpdateRotation(deltaTime);
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

            if (_projSprite != ProjSprite.Girl && _projSprite != ProjSprite.Jordan)
            {
                _rotationSpeed = ExtensionFunctions.RandomInRange(GameInfo.ProjectileMinRotationSpeed,
                    GameInfo.ProjectileMaxRotationSpeed);
            }
            else
            {
                _rotationSpeed = 0;
                Sprite.Rotation = (float) Math.Atan2(directionNormalized.Y, directionNormalized.X) + 90;
            }
        }

        public bool IsProjectileDestroyed => _isProjectileDestroyed;

        public void SetSprite(ProjSprite projSprite)
        {
            if (projSprite == _projSprite)
            {
                return;
            }

            _projSprite = projSprite;
        }

        public ProjSprite GetSprite()
        {
            return _projSprite;
        }

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

        private void UpdateRotation(float deltaTime) => Sprite.Rotation += _rotationSpeed * deltaTime;

        #endregion
    }
}