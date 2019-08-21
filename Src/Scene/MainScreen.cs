using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TeamRock.Src.GameObjects;
using System.Collections.Generic;

namespace TeamRock.Scene
{
    public class MainScreen : CustomScreen
    {
        #region Initialization
        private List<GameObject> _enemies;
        private GameObject _player;
        private int _power = 0;
        
        private const float _accelSpeed = 1f;
        private const float _decelSpeed = 0.5f;
        private const int _damageCount = 500;
        private const int _powerRate = 200;

        public override void Initialize(ContentManager contentManager)
        {
            _player = new GameObject(null, new Vector2(0), new Vector2(0), new Vector2(0), 10);
			_enemies = new List<GameObject>();
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            HandleInput((float)gameTime.ElapsedGameTime.TotalSeconds);
            
			PositionCheck((float)gameTime.ElapsedGameTime.TotalSeconds);

            _power += (int)(_powerRate * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void HandleInput(float gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            bool xPressed = false;
			bool yPressed = false;

            //Will probably change later if we will it so
            if(state.IsKeyDown(Keys.Right))
            {
                _player.Acceleration += new Vector2(_accelSpeed * gameTime, 0);
                xPressed = true;
            }
            if(state.IsKeyDown(Keys.Left))
            {
                _player.Acceleration += new Vector2(-1 * _accelSpeed * gameTime, 0);
                xPressed = true;
            }
            if(state.IsKeyDown(Keys.Up))
            {
                _player.Acceleration += new Vector2(0, -1 * _accelSpeed * gameTime);
                yPressed = true;
            }
            if(state.IsKeyDown(Keys.Down))
            {
                _player.Acceleration += new Vector2(0, 1 * _accelSpeed * gameTime);
                yPressed = true;
            }

			//Apply deceleration if direction isn't pressed
            if(!xPressed)
            {
                if(_player.Acceleration.X > _decelSpeed)
                {
                    _player.Acceleration = new Vector2(_player.Acceleration.X - _decelSpeed, _player.Acceleration.Y);
                }
                else if(_player.Acceleration.X < _decelSpeed * -1)
                {
                    _player.Acceleration = new Vector2(_player.Acceleration.X - _decelSpeed, _player.Acceleration.Y);
                }
				else
				{
                    _player.Acceleration = new Vector2(0, _player.Acceleration.Y);
				}
            }

			if(!yPressed)
            {
                if(_player.Acceleration.Y > _decelSpeed)
                {
                    _player.Acceleration = new Vector2(_player.Acceleration.X - _decelSpeed, _player.Acceleration.Y);
                }
                else if(_player.Acceleration.Y < _decelSpeed * -1)
                {
                    _player.Acceleration = new Vector2(_player.Acceleration.X - _decelSpeed, _player.Acceleration.Y);
                }
				else
				{
                    _player.Acceleration = new Vector2(0, _player.Acceleration.Y);
				}
            }
        }

        public void PositionCheck(float gameTime)
        {
            _player.GetNewPosition(gameTime);
            Rectangle playerHitbox = _player.GenerateHitbox();

            Stack<int> toDelete = new Stack<int>();
            for(int i = 0; i < _enemies.Count; i++)
	        {
                GameObject enemy = _enemies[i];
                enemy.GetNewPosition(gameTime);
                if(playerHitbox.Intersects(enemy.GenerateHitbox()))
                {
                    _power -= _damageCount;
                    toDelete.Push(i);
                }
	        }

            while(toDelete.Count != 0)
            {
                int target = toDelete.Pop();
                _enemies.RemoveAt(target);
            }
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
			
        }

        #endregion

        #region UnLoad

        public override void UnLoadContent()
        {

        }

        #endregion

        #region Singleton

        private static MainScreen _instance;
        public static MainScreen Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainScreen();
                }

                return _instance;
            }
        }

        private MainScreen() { }

        #endregion
    }
}