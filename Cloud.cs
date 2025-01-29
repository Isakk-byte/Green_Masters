using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Green_Masters
{
    internal class Cloud : Gameobject
    {
        public Vector2 _velocity;
        public Cloud(Vector2 velocity, Vector2 position, Texture2D texture, Color color)
        {
            _velocity = velocity;
            _color = color;
            _position = position;
            _texture = texture;
        }

        public void MoveCloud(GameTime gameTime, int screenWidth)
        {
            _position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Om molnet går utanför skärmen, flytta det tillbaka till vänster
            if (_position.X > screenWidth)
            {
                _position.X = -300; 
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, _position, Color.White);
        }
    }
}
