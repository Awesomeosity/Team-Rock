using Microsoft.Xna.Framework;
using MonoGame.Extended;
using TeamRock.Utils;

namespace TeamRock.CustomCamera
{
    public class CameraShaker
    {
        private float _currentShakeTimer;
        private float _shakeMultiplier;

        private bool _isShaking;
        private Vector2 _cameraInitialPosition;
        private Vector2 _shakeOffset;

        private OrthographicCamera _camera;

        #region Initialization

        public void Initialize(OrthographicCamera camera)
        {
            _camera = camera;
            _shakeOffset = Vector2.Zero;
        }

        #endregion

        #region Update

        public void Update(float deltaTime)
        {
            if (!_isShaking)
            {
                return;
            }

            _currentShakeTimer -= deltaTime;
            if (_currentShakeTimer <= 0)
            {
                StopShake();
            }

            UpdateCameraShake(deltaTime);
        }

        #endregion

        #region External Functions

        public void StartShake(float maxShakeTime, float shakeMultiplier)
        {
            _isShaking = true;

            _cameraInitialPosition = _camera.Position;
            _shakeOffset = Vector2.Zero;

            _currentShakeTimer = maxShakeTime;
            _shakeMultiplier = shakeMultiplier;
        }

        public void StopShake()
        {
            _isShaking = false;
            _camera.Position = _cameraInitialPosition;
        }

        #endregion

        #region Utility Functions

        private void UpdateCameraShake(float deltaTime)
        {
            _shakeOffset.X = ExtensionFunctions.RandomInRange(-_shakeMultiplier, _shakeMultiplier);
            _shakeOffset.Y = ExtensionFunctions.RandomInRange(-_shakeMultiplier, _shakeMultiplier);

            _camera.Position = _cameraInitialPosition + _shakeOffset;
        }

        #endregion

        #region Singleton

        private static CameraShaker _instance;
        public static CameraShaker Instance => _instance ?? (_instance = new CameraShaker());

        private CameraShaker()
        {
        }

        #endregion
    }
}