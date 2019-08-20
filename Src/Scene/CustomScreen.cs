using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TeamRock.Scene
{
    public abstract class CustomScreen
    {
        public virtual void Initialize(ContentManager contentManager) { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void UnLoadContent() { }
    }
}