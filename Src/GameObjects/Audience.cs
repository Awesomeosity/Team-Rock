using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended;
using TeamRock.CustomCamera;
using TeamRock.Managers;
using TeamRock.Scene;
using TeamRock.Utils;

namespace TeamRock.Src.GameObjects
{
    class Audience
    {
        private readonly ContentManager _contentManager;

        private readonly List<Projectile> _projectiles;
        private readonly Player _player;

        private Vector2 _position;
        private float _randomTimer;

        private RectangleF _audienceRectangle;

        private SoundEffect _cheer;
        private SoundEffect _hitSound;
        private SoundEffect _hitSound2;
        private SoundEffect _hitSound3;

        private bool _isProjectileSpawningActive;

        #region Initialization

        public Audience(Player player, ContentManager contentManager)
        {
            _contentManager = contentManager;
            _randomTimer =
                ExtensionFunctions.RandomInRange(GameInfo.MinProjectileSpawnTimer, GameInfo.MaxProjectileSpawnTimer);

            _player = player;
            _projectiles = new List<Projectile>();
            _audienceRectangle = new RectangleF();


            _cheer = contentManager.Load<SoundEffect>(AssetManager.Cheer);
            _hitSound = contentManager.Load<SoundEffect>(AssetManager.Hit);
            _hitSound2 = contentManager.Load<SoundEffect>(AssetManager.Boo);
            _hitSound3 = contentManager.Load<SoundEffect>(AssetManager.Oof_Girl);

            _isProjectileSpawningActive = true;
        }

        #endregion

        #region Update

        public void Update(float deltaTime, float gameTime)
        {
            _randomTimer -= deltaTime * (_player.GameObject.Position.Y > GameInfo.IncreasedItemPosition
                                ? GameInfo.IncreasedItemFrequency
                                : 1);

            if (_randomTimer <= 0)
            {
                SpawnProjectileAndResetTimer();
            }

            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                _projectiles[i].Update(deltaTime, gameTime);

                if (_projectiles[i].IsProjectileDestroyed || _projectiles[i].DidCollide(_player.GameObject))
                {
                    if (_projectiles[i].DidCollide(_player.GameObject))
                    {

                        if (_projectiles[i].Position.X < _player.GameObject.Position.X)
                        {
                            GamePadVibrationController.Instance.StartVibration(GameInfo.GamePadMaxIntensity,
                                GameInfo.GamePadMinIntensity, GameInfo.GamePadVibrationTime);
                        }
                        else
                        {
                            GamePadVibrationController.Instance.StartVibration(GameInfo.GamePadMinIntensity,
                                GameInfo.GamePadMaxIntensity, GameInfo.GamePadVibrationTime);
                        }

                        CameraShaker.Instance.StartShake(GameInfo.GamePadVibrationTime, 5);
                        int soundIndex = 0;
                        switch (_projectiles[i].GetSprite())
                        {
                            case Projectile.ProjSprite.Popcorn:
                                soundIndex = SoundManager.Instance.PlaySound(_hitSound);
                                break;

                            case Projectile.ProjSprite.Soda:
                                soundIndex = SoundManager.Instance.PlaySound(_hitSound);
                                break;

                            case Projectile.ProjSprite.Girl:
                                soundIndex = SoundManager.Instance.PlaySound(_hitSound3);
                                break;

                            default:
                                break;
                        }

                        SoundManager.Instance.SetSoundVolume(soundIndex, 0.25f);

                        if (_player.IsPosing())
                        {
                            soundIndex = SoundManager.Instance.PlaySound(_hitSound2);
                            SoundManager.Instance.SetSoundVolume(soundIndex, 0.3f);
                        }
                        else
                        {
                            soundIndex = SoundManager.Instance.PlaySound(_cheer);
                        }

                        _player.ReduceVelocity();
                    }

                    _projectiles.RemoveAt(i);
                }
            }
        }

        #endregion

        #region Draw

        public void DrawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (Projectile projectile in _projectiles)
            {
                projectile.Draw(spriteBatch);
            }
        }

        public void DrawDebug(SpriteBatch spriteBatch)
        {
            foreach (Projectile projectile in _projectiles)
            {
                projectile.DrawDebug(spriteBatch);
            }

            spriteBatch.DrawRectangle(_audienceRectangle, Color.Red, 2);
        }

        #endregion

        #region External Functions

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;

                _audienceRectangle.X = _position.X;
                _audienceRectangle.Y = GameInfo.AudienceTopBuffer;
                _audienceRectangle.Width = GameInfo.AudienceWidth;
                _audienceRectangle.Height = Math.Abs(GameInfo.FixedWindowHeight - GameInfo.AudienceTopBuffer);
            }
        }

        public bool IsProjectileSPawningActive
        {
            get => _isProjectileSpawningActive;
            set => _isProjectileSpawningActive = value;
        }

        public void ClearProjectiles() => _projectiles.Clear();

        #endregion

        #region Utility Functions

        private void SpawnProjectileAndResetTimer()
        {
            if (!_isProjectileSpawningActive)
            {
                return;
            }

            _randomTimer =
                ExtensionFunctions.RandomInRange(GameInfo.MinProjectileSpawnTimer, GameInfo.MaxProjectileSpawnTimer);

            float xPosition =
                ExtensionFunctions.RandomInRange(Position.X, Position.X + GameInfo.AudienceWidth);
            float yPosition = ExtensionFunctions.RandomInRange(GameInfo.AudienceTopBuffer, GameInfo.FixedWindowHeight);
            Vector2 launchPosition = new Vector2(xPosition, yPosition);

            float offsetX = _player.GameObject.Position.X +
                            (ExtensionFunctions.RandomInRange(-GameInfo.ProjectileAimRadius,
                                GameInfo.ProjectileAimRadius));
            float offsetY = _player.GameObject.Position.Y +
                            (ExtensionFunctions.RandomInRange(-GameInfo.ProjectileAimRadius,
                                GameInfo.ProjectileAimRadius));
            Vector2 offsetAim = new Vector2(offsetX, offsetY);

            Vector2 launchDirection = offsetAim - launchPosition;
            launchDirection.Normalize();

            float random = ExtensionFunctions.Random();
            Projectile.ProjSprite projSprite;
            string textureLoad;
            if(random < 0.33)
            {
                projSprite = Projectile.ProjSprite.Soda;
                textureLoad = AssetManager.Soda;
            }
            else if(random >= 0.33 && random < 0.66)
            {
                projSprite = Projectile.ProjSprite.Popcorn;
                textureLoad = AssetManager.Popcorn;
            }
            else
            {
                projSprite = Projectile.ProjSprite.Girl;
                textureLoad = AssetManager.Girl;

            }

            Texture2D projectileTexture;
            Sprite projectileSprite;
            if (textureLoad == AssetManager.Girl && launchDirection.X < 0)
            {
                projectileTexture = _contentManager.Load<Texture2D>(AssetManager.FlipGirl);
                projectileSprite = new Sprite(projectileTexture)
                {
                    Scale = GameInfo.ProjectileStartAssetScale
                };
                projectileSprite.SetOriginCenter();
            }
            else
            {
                projectileTexture = _contentManager.Load<Texture2D>(textureLoad);
                projectileSprite = new Sprite(projectileTexture)
                {
                    Scale = GameInfo.ProjectileStartAssetScale
                };
                projectileSprite.SetOriginCenter();
            }

            Projectile projectile = new Projectile(projectileSprite,
                (int) (projectileTexture.Width * GameInfo.ProjectileFinalAssetScale),
                (int) (projectileTexture.Height * GameInfo.ProjectileFinalAssetScale))
            {
                Position = launchPosition
            };

            projectile.SetSprite(projSprite);
            projectile.LaunchProjectile(launchDirection, _player.GameObject.Position);

            _projectiles.Add(projectile);
        }

        #endregion
    }
}