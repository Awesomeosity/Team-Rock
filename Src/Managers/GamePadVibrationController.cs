using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TeamRock.Managers
{
    public class GamePadVibrationController
    {
        private float _controllerVibrationTime;
        private bool _isVibrationActive;

        #region Update

        public void Update(float deltaTime)
        {
            if (!_isVibrationActive)
            {
                return;
            }

            _controllerVibrationTime -= deltaTime;
            if (_controllerVibrationTime <= 0)
            {
                StopVibration();
            }
        }

        #endregion

        #region External Functions

        public void StartVibration(float leftIntensity, float rightIntensity, float vibrationTime)
        {
            _isVibrationActive = true;

            GamePadCapabilities gamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (gamePadCapabilities.IsConnected)
            {
                GamePad.SetVibration(PlayerIndex.One, leftIntensity, rightIntensity);
            }

            _controllerVibrationTime = vibrationTime;
        }

        public void StopVibration()
        {
            _isVibrationActive = false;

            GamePadCapabilities gamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (gamePadCapabilities.IsConnected)
            {
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }
        }

        #endregion

        #region Singleton

        private static GamePadVibrationController _instance;

        public static GamePadVibrationController Instance =>
            _instance ?? (_instance = new GamePadVibrationController());

        private GamePadVibrationController()
        {
        }

        #endregion
    }
}