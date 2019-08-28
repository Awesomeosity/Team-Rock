using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TeamRock.Managers;
using TeamRock.Utils;

namespace TeamRock.Src.GameObjects
{
    public class PlayerController
    {
        public enum ControllerState
        {
            None,
            Left,
            Right,
            Up,
            Down
        }

        public bool IsDashing = false;

        private bool _pressed = false;

        private ControllerState _controllerState;

        #region Update

        public void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadCapabilities gamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);
            bool holding = false;

            // Check if GamePad is connected else use the Keyboard
            if (gamePadCapabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

                if (gamePadState.ThumbSticks.Left.X < -GameInfo.PlayerGamePadAxisThreshold)
                {
                    SetControllerState(ControllerState.Left);
                }
                else if (gamePadState.ThumbSticks.Left.X > GameInfo.PlayerGamePadAxisThreshold)
                {
                    SetControllerState(ControllerState.Right);
                }
                else if (gamePadState.ThumbSticks.Left.Y < -GameInfo.PlayerGamePadAxisThreshold)
                {
                    SetControllerState(ControllerState.Down);
                }
                else if (gamePadState.ThumbSticks.Left.Y > GameInfo.PlayerGamePadAxisThreshold)
                {
                    SetControllerState(ControllerState.Up);
                }
                else
                {
                    SetControllerState(ControllerState.None);
                }

                holding = gamePadState.IsButtonDown(Buttons.A);

            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    SetControllerState(ControllerState.Left);
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    SetControllerState(ControllerState.Right);
                }
                else if (keyboardState.IsKeyDown(Keys.Up))
                {
                    SetControllerState(ControllerState.Up);
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    SetControllerState(ControllerState.Down);
                }
                else
                {
                    SetControllerState(ControllerState.None);
                }

                holding = keyboardState.IsKeyDown(Keys.Space);
            }


            if(holding == false && _pressed == true)
            {
                IsDashing = true;
            }

            _pressed = holding;

            
        }

        #endregion

        #region External Functions

        public ControllerState State => _controllerState;

        #endregion

        #region Utility Functions

        private void SetControllerState(ControllerState controllerState)
        {
            if (_controllerState == controllerState)
            {
                return;
            }

            _controllerState = controllerState;
        }

        #endregion
    }
}