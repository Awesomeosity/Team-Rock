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

        private SpriteSheetAnimationManager
            _testSpriteSheetAnimation; // TODO: Remove this later on. Just added to test SpriteSheet Animations...

        private Player _player;

        private List<Audience> _audiences;

        private SpriteFont _font;
        private float _timeLeft; //TODO: Remove this? only for testing purposes

        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;

            _player = new Player();
            _player.Initialize(_contentManager);

            _audiences = new List<Audience>();

            Texture2D audienceTexture = _contentManager.Load<Texture2D>(AssetManager.WhitePixel);
            Sprite audienceSprite = new Sprite(audienceTexture)
            {
                Scale = GameInfo.AudienceAssetScale
            };

            Sprite audienceSprite2 = new Sprite(audienceTexture)
            {
                Scale = GameInfo.AudienceAssetScale
            };

            _audiences.Add(new Audience(audienceSprite, _player, _contentManager,
                (int)(audienceTexture.Width * GameInfo.AudienceAssetScale),
                (int)(audienceTexture.Height * GameInfo.AudienceAssetScale))
            {
                Position = new Vector2(GameInfo.LeftAudiencePos, 0)
            });

            _audiences.Add(new Audience(audienceSprite2, _player, _contentManager,
                (int)(audienceTexture.Width * GameInfo.AudienceAssetScale),
                (int)(audienceTexture.Height * GameInfo.AudienceAssetScale))
            {
                Position = new Vector2(GameInfo.RightAudiencePos, 0)
            });


            _testSpriteSheetAnimation = new SpriteSheetAnimationManager();
            _testSpriteSheetAnimation.Initialize(_contentManager, AssetManager.TestBlastBase,
                AssetManager.TestBlastTotalCount, true);
            Sprite animationSprite = _testSpriteSheetAnimation.Sprite;
            animationSprite.Position = new Vector2(GameInfo.WindowWidth / 2.0f, GameInfo.WindowHeight / 2.0f);
            animationSprite.SetOriginCenter();
            _testSpriteSheetAnimation.FrameTime = 0.01538462F;
            _testSpriteSheetAnimation.StartSpriteAnimation();

            _font = _contentManager.Load<SpriteFont>(AssetManager.Arial);
            _timeLeft = GameInfo.TotalGameTime;
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            _player.Draw(spriteBatch);

            foreach (Audience audience in _audiences)
            {
                audience.Draw(spriteBatch);
                audience.DrawProjectiles(spriteBatch);
            }

            spriteBatch.DrawString(_font, "Hello World!" + _timeLeft, new Vector2(GameInfo.WindowWidth / 2, GameInfo.WindowHeight / 2), Color.White);

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
            foreach(Audience audience in _audiences)
            {
                audience.Update(deltaTime, gameTime);
            }

            _timeLeft -= deltaTime;
            if(_timeLeft <= 0)
            {
                ResetScreen();
            }
        }

        #endregion

        #region External Functions

        public void ResetScreen()
        {
            //Possibly abuse? Might need to change later.
            Initialize(_contentManager);
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