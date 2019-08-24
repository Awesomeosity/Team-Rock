﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        private SoundEffect _testSoundEffect;
        private SoundEffect _music;
        private SoundEffect _clap;
        private KeyboardState _oldKeyboardState; // TODO: Remove this later on... Just added for testing

        private Player _player;
        private ScrollingBackground _scrollingBackground;

        private List<Audience> _audiences;

        private SpriteFont _font;
        private float _timeLeft; //TODO: Remove this? only for testing purposes

        #region Initialization

        public override void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;

            CreateSounds();
            CreatePlayerAndBackground();
            CreateAudiences();

            _font = _contentManager.Load<SpriteFont>(AssetManager.Arial);
            _timeLeft = GameInfo.TotalGameTime;
        }

        private void CreateSounds()
        {
            _testSoundEffect = _contentManager.Load<SoundEffect>(AssetManager.TestSound);
            _music = _contentManager.Load<SoundEffect>(AssetManager.Music2);
            _clap = _contentManager.Load<SoundEffect>(AssetManager.Clap);
        }

        private void CreatePlayerAndBackground()
        {
            _player = new Player();
            _player.Initialize(_contentManager);


            Texture2D backgroundTexture = _contentManager.Load<Texture2D>(AssetManager.TestSeamless);
            _scrollingBackground = new ScrollingBackground();
            _scrollingBackground.Initialization(backgroundTexture, GameInfo.CenterBoardWidth);
        }

        private void CreateAudiences()
        {
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

            spriteBatch.DrawString(_font, "Hello World! " + _timeLeft, new Vector2(GameInfo.FixedWindowWidth / 2, GameInfo.FixedWindowHeight / 2), Color.White);
        }

        #endregion

        #region Update

        public override bool Update(float deltaTime, float gameTime)
        {
            _scrollingBackground.Update(deltaTime, 500);
            _player.Update(deltaTime, gameTime);

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.X) && _oldKeyboardState.IsKeyUp(Keys.X))
            {
                SoundManager.Instance.PlaySound(_testSoundEffect);
                GamePadVibrationController.Instance.StartVibration(0.5f, 0.7f, 2);
                CameraShaker.Instance.StartShake(2, 5);
            }

            _oldKeyboardState = keyboardState;

            foreach (Audience audience in _audiences)
            {
                audience.Update(deltaTime, gameTime);
            }

            _timeLeft -= deltaTime;
            if (_timeLeft <= 0)
            {
                SoundManager.Instance.PlaySound(_clap);
                ResetScreen();
            }

            return false;
        }


        #endregion

        #region External Functions

        public void ResetScreen()
        {
            //Possibly abuse? Might need to change later.
            Initialize(_contentManager);
        }

        public void StartMusic()
        {
            _musicIndex = SoundManager.Instance.PlaySound(_music);
            SoundManager.Instance.SetSoundLooping(_musicIndex, true);
        }

        public void StopMusic()
        {
            SoundManager.Instance.StopSound(_musicIndex);
        }

        #endregion

        #region Utility Functions

        #endregion

        #region Singleton

        private static MainScreen _instance;
        private int _musicIndex;

        public static MainScreen Instance => _instance ?? (_instance = new MainScreen());

        private MainScreen()
        {
        }


        #endregion
    }
}