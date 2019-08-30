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

        private bool _didPlayerPressDash;
        private Vector2 _dashDirection = Vector2.Zero;
        private ControllerState _controllerState;

        private GamePadState _oldGamePadState;
        private KeyboardState _oldKeyboardState;


        #region Update

        public void Update()
        {
            _didPlayerPressDash = false;
            _dashDirection = Vector2.Zero;

            GamePadCapabilities gamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);

            // Check if GamePad is connected else use the Keyboard
            if (gamePadCapabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

                if (gamePadState.ThumbSticks.Left.X < -GameInfo.PlayerGamePadAxisThreshold)
                {
                    _dashDirection.X = -1;
                    SetControllerState(ControllerState.Left);
                }
                else if (gamePadState.ThumbSticks.Left.X > GameInfo.PlayerGamePadAxisThreshold)
                {
                    _dashDirection.X = 1;
                    SetControllerState(ControllerState.Right);
                }
                else if (gamePadState.ThumbSticks.Left.Y < -GameInfo.PlayerGamePadAxisThreshold)
                {
                    _dashDirection.Y = 1;
                    SetControllerState(ControllerState.Down);
                }
                else if (gamePadState.ThumbSticks.Left.Y > GameInfo.PlayerGamePadAxisThreshold)
                {
                    _dashDirection.Y = -1;
                    SetControllerState(ControllerState.Up);
                }
                else
                {
                    SetControllerState(ControllerState.None);
                }

                if (gamePadState.IsButtonDown(Buttons.A) && !_oldGamePadState.IsButtonDown(Buttons.A))
                {
                    _didPlayerPressDash = true;
                }

                _oldGamePadState = gamePadState;
            }
            else
            {
                KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    _dashDirection.X = -1;
                    SetControllerState(ControllerState.Left);
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    _dashDirection.X = 1;
                    SetControllerState(ControllerState.Right);
                }
                else if (keyboardState.IsKeyDown(Keys.Up))
                {
                    _dashDirection.Y = -1;
                    SetControllerState(ControllerState.Up);
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    _dashDirection.Y = 1;
                    SetControllerState(ControllerState.Down);
                }
                else
                {
                    SetControllerState(ControllerState.None);
                }

                if (keyboardState.IsKeyDown(Keys.Space) && !_oldKeyboardState.IsKeyDown(Keys.Space))
                {
                    _didPlayerPressDash = true;
                }

                _oldKeyboardState = keyboardState;
            }
        }

        #endregion

        #region External Functions

        public ControllerState State => _controllerState;

        public bool DidPlayerPressDash => _didPlayerPressDash;

        public Vector2 DashDirection => _dashDirection;

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