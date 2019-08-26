using Microsoft.Xna.Framework;

namespace TeamRock.Utils
{
    public static class GameInfo
    {
        // Window Size
        public const int FixedWindowWidth = 600;
        public const int FixedWindowHeight = 800;

        // Background
        public const int MaxBackgroundElements = 3;

        // Center Board
        private const int CenterBoardWidth = 300;

        // Player State
        public const float PlayerMinYPosition = 10;
        public const float PlayerMaxYPosition = FixedWindowHeight - 10;
        public const float PlayerAssetScale = 1f;

        public static readonly Vector2 PlayerInitialPosition =
            new Vector2(FixedWindowWidth / 2.0f, 70);

        public const float PlayerMaxYVelocity = 500;
        public static readonly Vector2 BaseAccelerationRate = new Vector2(0, 5);
        public static readonly Vector2 AccelerationChangeRate = new Vector2(0, 3);
        public static readonly Vector2 HorizontalVelocity = new Vector2(250, 0);
        public static readonly Vector2 VerticalVelocity = new Vector2(0, 50);
        public const int PlayerLeftPosition = AudienceWidth;
        public const int PlayerRightPosition = FixedWindowWidth - AudienceWidth;
        public const float PlayerGamePadAxisThreshold = 0.5f;

        // Stage
        public const float StageScale = 0.3f;

        // Audience Position
        public const int AudienceWidth = (FixedWindowWidth - CenterBoardWidth) / 2;
        public const float AudienceTopBuffer = 100;
        public const float LeftAudiencePos = 0;
        public const float RightAudiencePos = 480;

        // Projectile
        public const float MinProjectileVelocity = 400;
        public const float MaxProjectileVelocity = 700;
        public const float ProjectileLifeTime = 5f;
        public const float MinProjectileSpawnTimer = 1;
        public const float MaxProjectileSpawnTimer = 5;
        public const float MaxProjectileAssetScaleDistanceSq = 400;
        public const float ProjectileStartAssetScale = 0.05f;
        public const float ProjectileFinalAssetScale = 0.2f;
        public const float ProjectileAimRadius = 100;
        public const float ProjectileMinRotationSpeed = 3;
        public const float ProjectileMaxRotationSpeed = 10;

        // SpriteSheet Animations
        public const float DefaultAnimationSpeed = 0.01666667F;

        //Game State
        public const float TotalGameTime = 10; //TODO: Remove this, only for testing
    }
}