using Microsoft.Xna.Framework;

namespace TeamRock.Utils
{
    public static class GameInfo
    {
        // Window Size
        public const int FixedWindowWidth = 1334;
        public const int FixedWindowHeight = 800;

        // Background
        public const int MaxBackgroundElements = 3;

        // Center Board
        public const int CenterBoardWidth = 400;

        // Player State
        public static readonly Vector2 PlayerInitialPosition =
            new Vector2(FixedWindowWidth / 2.0f, 70);

        public const float PlayerMinYPosition = 50;
        public const float PlayerMaxYPosition = FixedWindowHeight - 300;
        public const float PlayerAssetScale = 0.1f;
        public const float PlayerMinYVelocity = 50;
        public const float PlayerMaxYVelocity = 500;
        public static readonly Vector2 BaseAccelerationRate = new Vector2(0, 5);
        public static readonly Vector2 AccelerationChangeRate = new Vector2(0, 1);
        public static readonly Vector2 HorizontalVelocity = new Vector2(200, 0);
        public static readonly Vector2 VerticalVelocity = new Vector2(0, 200);
        public const int PlayerLeftPosition = AudienceWidth;
        public const int PlayerRightPosition = FixedWindowWidth - AudienceWidth;
        public const float PlayerGamePadAxisThreshold = 0.5f;

        public const float PlayerDamageVelocity = 0.25f;
        public const float PlayerRecoveryRate = 0.5f;
        public const float PlayerIncreasedVelocity = 3;
        public const float PlayerSlowdownRate = 0.5f;

        public const float PlayerDashDuration = 0.2f;
        public const float PlayerDashCooldown = 1;
        public const float PlayerDashVelocity = 750;
        public const float PlayerPoseDuration = 2;
        public const float PlayerBoostDuration = 3;

        public const float PlayerKnockBack = 5;

        // Stage
        public const float StageScale = 0.3f;
        public const float WrestlerScale = 0.2f;

        // Game Pad
        public const float GamePadVibrationTime = 0.3f;
        public const float GamePadMinIntensity = 0.3f;
        public const float GamePadMaxIntensity = 0.7f;

        // Audience Position
        public const int AudienceWidth = (FixedWindowWidth - CenterBoardWidth) / 2;
        public const float AudienceTopBuffer = 300;
        public const float LeftAudiencePos = 0;
        public const float RightAudiencePos = FixedWindowWidth - AudienceWidth;

        // Projectile
        public const float MinProjectileVelocity = 150;
        public const float MaxProjectileVelocity = 300;
        public const float ProjectileLifeTime = 5f;
        public const float MinProjectileSpawnTimer = 1;
        public const float MaxProjectileSpawnTimer = 5;
        public const float MaxProjectileAssetScaleDistanceSq = 400;
        public const float ProjectileStartAssetScale = 0.05f;
        public const float ProjectileFinalAssetScale = 0.2f;
        public const float ProjectileAimRadius = 100;
        public const float ProjectileMinRotationSpeed = 3;
        public const float ProjectileMaxRotationSpeed = 10;
        public const float ProjectileFlashRate = 5;
        public static readonly Color ProjectileFlashStartColor = Color.White;
        public static readonly Color ProjectileFlashEndColor = Color.Red;

        // SpriteSheet Animations
        public const float DefaultAnimationSpeed = 0.01666667F;

        // Timer Bar
        public static readonly Color InitialTimerBarColor = Color.White;
        public static readonly Color FinalTimerBarColor = new Color(120, 89, 169);

        public const float FlashBarMinAlpha = 0;
        public const float FlashBarMaxAlpha = 0.3f;
        public const float InitialBarFlashRate = 0;
        public const float MaxBarFlashRate = 7;
        public static readonly Color FlashBarColor = new Color(204, 0, 0);

        public const float PlayerTimerChangeRate = 1.2f;
        public const float PlayerHitTimerChangeRate = 0.5f;
        public const float PlayerHitTimerAffectTime = 2f;
        public const float PlayerDashTimerChangeRate = 1.5f;
        public const float PlayerDashAffectTime = 0.3f;

        // Game Ending Control
        public const float TotalGameTime = 30;
        public const float EndGameTime = 2;
        public const float StageMoveUpSpeed = 500;
        public const float IncreasedItemPosition = 480F;
        public const int IncreasedItemFrequency = 5;

        // Cinematics
        public const int TotalCinematicRopes = 5;
        public const float CinematicScrollMoveSpeed = 250;
        public const float PlayerMoveToRopePosition = FixedWindowHeight / 2.0f;
        public const float PlayerSpriteFlipRate = 0.125F;
        public const float PlayerInitialClimbSpeed = 250;

        public static readonly Vector2 CinematicStageInitialPosition =
            new Vector2(FixedWindowWidth / 2.0f, FixedWindowHeight - 100);

        public static readonly Vector2 CinematicRowFinalPosition =
            new Vector2(FixedWindowWidth / 2.0f, FixedWindowHeight + 300);

        public const float InitialStageDisplayWaitTimer = 3f;
        public const float StageTopWaitTimer = 3f;
        public const float StageDivingWaitTimer = 3f;
    }
}