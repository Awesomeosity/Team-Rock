using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TeamRock.Scene
{
    public abstract class CustomScreen
    {
        public virtual void Initialize(ContentManager contentManager) { }

        public virtual void Update(float deltaTime, float gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}