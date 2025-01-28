using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Green_Masters
{
    internal class Arrow:Gameobject
    {
        public Vector2 _velocity;
        public Arrow(Vector2 velocity, Vector2 position, Texture2D texture, Color color)
        {
            _velocity = velocity;
            _color = color;
            _position = position;
            _texture = texture;
        }


        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, _position, Color.White);
        }
    }
}
