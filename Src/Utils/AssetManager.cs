using Microsoft.Xna.Framework.Graphics;

namespace TeamRock.Utils
{
    public static class AssetManager
    {
        #region Basic Textures

        public const string WhitePixel = "Textures/White Pixel";
        public const string TestSeamless = "Textures/TestSeamless";

        public const string PlayImage = "Textures/Play";
        public const string HeaderImage = "Textures/Header";

        public const string Stage = "Textures/Stage Prop";
        public const string BackgroundRopes = "Textures/Background Ropes Tiling";
        public const string WrestingBackground = "Textures/WrestlingBackground";

        public const string Soda = "Textures/Soda";
        public const string Popcorn = "Textures/PopCorn";

        #endregion

        #region Fonts

        public const string Arial = "Fonts/Arial";

        #endregion

        #region Sprite Sheets

        // Test Explosion
        public const string TestExplosionBase = "SpriteSheets/TestExplosion/exp2_";
        public const int TestExplosionTotalCount = 7;

        // Test Blast
        public const string TestBlastBase = "SpriteSheets/TestBlast/image_part_0";
        public const int TestBlastTotalCount = 64;

        // Fire Falling
        public const string FireFallingBase = "SpriteSheets/FireFalling/image_part_0";
        public const int FireFallingTotalCount = 81;

        #endregion

        #region Sounds

        public const string TestSound = "Sounds/Cutting line";
        public const string Boo = "Sounds/Boo";
        public const string Clap = "Sounds/Crowd_Clap";
        public const string Dizzy = "Sounds/Dizzy";
        public const string Clap2 = "Sounds/Hand_Claps";

        #endregion

        #region Music

        public const string HomeScreenMusic = "Music/Hermanos_Ranchero";
        public const string MainScreenMusic = "Music/Mariachiando";

        #endregion
    }
}