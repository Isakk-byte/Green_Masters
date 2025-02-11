using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;

namespace Green_Masters
{
    internal class PowerPick:Gameobject
    {
        private Vector2 _velocity;
        private GraphicsDevice _graphicsDevice;

        public PowerPick(GraphicsDevice graphicsDevice, Vector2 velocity, Vector2 position, Texture2D texture, Color color)
        {
            this._graphicsDevice = graphicsDevice;
            _velocity = velocity;
            _position = position;
            _texture = new Texture2D(graphicsDevice, 50, 30);
            Color[] data = new Color[50 * 30];
            for (int i = 0; i < data.Length; i++) data[i] = Color.Black;
            texture.SetData(data);


        }
        public void Update(PowerBar _powerbarImg)
        {
            _position += _velocity;

            if (_position.X > _powerbarImg._position.X) { }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
