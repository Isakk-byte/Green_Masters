using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;

namespace Green_Masters
{
    internal class Ball:Gameobject
    {
        public Vector2 _velocity;
        public Ball(Vector2 velocity, Vector2 position, Texture2D texture, Color color)
        {
            _velocity = velocity;
            _position = position;
            _texture = texture;
            _color = color;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, _position, Color.White);
        }
    }
}
