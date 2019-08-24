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
        public const int CenterBoardWidth = 480;

        // Player State
        public const float PlayerMinYPosition = 10;
        public const float PlayerMaxYPosition = FixedWindowHeight - 10;
        public const float PlayerAssetScale = 1f;

        public static readonly Vector2 PlayerInitialPosition =
            new Vector2(FixedWindowWidth / 2.0f, 70);

        public static readonly Vector2 BaseAccelerationRate = new Vector2(0, 10);
        public static readonly Vector2 AccelerationChangeRate = new Vector2(0, 3);
        public static readonly Vector2 HorizontalVelocity = new Vector2(250, 0);
        public static readonly Vector2 VerticalVelocity = new Vector2(0, 50);
        public const int PlayerLeftPosition = AudienceWidth;
        public const int PlayerRightPosition = FixedWindowWidth - AudienceWidth;
        public const float PlayerGamePadAxisThreshold = 0.5f;

        // Audience Position
        private const int AudienceWidth = (FixedWindowWidth - CenterBoardWidth) / 2;
        public const float AudienceAssetScale = 0.5f;
        public const float LeftAudiencePos = 0;
        public const float RightAudiencePos = 480;
        public static readonly Rectangle LeftAudienceRectangle = new Rectangle(0, 0, AudienceWidth, FixedWindowHeight);

        public static readonly Rectangle RightAudienceRectangle =
            new Rectangle(FixedWindowWidth - AudienceWidth, 0, AudienceWidth, FixedWindowHeight);

        // Projectile
        public const float ProjectileVelocity = 750;
        public const float ProjectileLifeTime = 1.5f;
        public const float MinProjectileSpawnTimer = 1;
        public const float MaxProjectileSpawnTimer = 5;
        public const float ProjectileAssetScale = 0.1f;
        public const float ProjectileAimRadius = 100;

        // SpriteSheet Animations
        public const float DefaultAnimationSpeed = 0.01666667F;

        //Game State
        public const float TotalGameTime = 10; //TODO: Remove this, only for testing
    }
}
