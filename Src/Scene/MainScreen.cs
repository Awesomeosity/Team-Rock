using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TeamRock.CustomCamera;
using TeamRock.Managers;
using TeamRock.Src.GameObjects;
using TeamRock.Utils;

namespace TeamRock.Scene
{
    public class MainScreen : CustomScreen
    {
        private ContentManager _contentManager;

        private KeyboardState _oldKeyboardState;

        private Player _player;
        private ScrollingBackground _scrollingBackground;

        private List<Audience> _audiences;

        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;

            _player = new Player();
            _player.Initialize(_contentManager);


            Texture2D backgroundTexture = _contentManager.Load<Texture2D>(AssetManager.TestSeamless);
            _scrollingBackground = new ScrollingBackground();
            _scrollingBackground.Initialization(backgroundTexture, GameInfo.CenterBoardWidth);
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
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            _scrollingBackground.Draw(spriteBatch);
            _player.Draw(spriteBatch);

            foreach (Audience audience in _audiences)
            {
                audience.Draw(spriteBatch);
                audience.DrawProjectiles(spriteBatch);
            }
        }

        #endregion

        #region Update

        public override bool Update(float deltaTime, float gameTime)
        {
            _scrollingBackground.Update(deltaTime, 500);
            _player.Update(deltaTime, gameTime);

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.X) && _oldKeyboardState.IsKeyUp(Keys.X)) {
            GamePadVibrationController.Instance.StartVibration(0.5f, 0.7f, 2);
                CameraShaker.Instance.StartShake(10, 5);
            }

            _oldKeyboardState = keyboardState;

            foreach(Audience audience in _audiences)
            {
                audience.Update(deltaTime, gameTime);
            }

            return false;
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
