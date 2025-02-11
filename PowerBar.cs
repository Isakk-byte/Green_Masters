using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Green_Masters
{
    internal class PowerBar:Gameobject
    {
        public PowerBar(Vector2 position, Texture2D texture, Color color)
        {
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
