using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TeamRock.Src.GameObjects
{
    class GameObject
    {
        private Texture2D _sprite;

        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _acceleration;

        public GameObject(Texture2D sprite, Vector2 position, Vector2 velocity, Vector2 acceleration)
        {
            _sprite = sprite;
            _position = position;
            _velocity = velocity;
            _acceleration = acceleration;
        }

        public Texture2D Sprite
        {
            get
            {
                return _sprite;
            }
            
            set
            {
                _sprite = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return _velocity;
            }

            set
            {
                _velocity = value;
            }
        }

        public Vector2 Acceleration
        {
            get
            {
                return _acceleration;
            }

            set
            {
                _acceleration = value;
            }
        }

        public Vector2 getNewPosition(double totalSeconds)
        {
            Vector2 deltaVelocity = _acceleration * (float)totalSeconds;
            _velocity += deltaVelocity;
            _position += _velocity;
            return _position;
        }
    }
}
