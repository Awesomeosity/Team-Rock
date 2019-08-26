using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TeamRock.Scene
{
    public abstract class CustomScreen
    {
        public abstract void Initialize(ContentManager contentManager);

        public abstract bool Update(float deltaTime, float gameTime); // The return value tells when to switch screens

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void DrawDebug(SpriteBatch spriteBatch);
    }
}