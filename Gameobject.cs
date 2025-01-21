using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Green_Masters
{
    internal class Gameobject
    {
        public Vector2 _position;
        public Texture2D _texture;
        protected Color _color;

        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
           
            spriteBatch.Draw(_texture, _position, _color);
            
        }
    }
}
