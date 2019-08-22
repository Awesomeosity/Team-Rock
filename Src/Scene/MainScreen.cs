﻿using Microsoft.Xna.Framework.Content;
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

        private SpriteSheetAnimationManager
            _testSpriteSheetAnimation; // TODO: Remove this later on. Just added to test SpriteSheet Animations...

        private Player _player;

        private List<Audience> _audiences;

        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;

            _player = new Player();
            _player.Initialize(_contentManager);

            _audiences = new List<Audience>();

            _testSpriteSheetAnimation = new SpriteSheetAnimationManager();
            _testSpriteSheetAnimation.Initialize(_contentManager, AssetManager.TestBlastBase,
                AssetManager.TestBlastTotalCount, true);
            Sprite animationSprite = _testSpriteSheetAnimation.Sprite;
            animationSprite.Position = new Vector2(GameInfo.WindowWidth / 2.0f, GameInfo.WindowHeight / 2.0f);
            animationSprite.SetOriginCenter();
            _testSpriteSheetAnimation.FrameTime = 0.01538462F;
            _testSpriteSheetAnimation.StartSpriteAnimation();
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            _player.Draw(spriteBatch);

            foreach (Audience audience in _audiences)
            {
                audience.DrawProjectiles(spriteBatch);
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
            _player.Update(deltaTime, gameTime);
            _testSpriteSheetAnimation.Update(deltaTime);

        }

        #endregion

        #region External Functions

        public void ResetScreen()
        {
            _audiences.Clear();

            _player = new Player();
            _player.Initialize(_contentManager);
        }

        #endregion

        #region Utility Functions

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