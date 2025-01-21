using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Green_Masters
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        Texture2D _logoImg;
        Texture2D _personImg;
        Texture2D _ballImg;
        Texture2D _cloudImg;
        Texture2D _buttonImg;
        Texture2D _flagImg;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
           
            _graphics.PreferredBackBufferWidth = 1700; // Bredd
            _graphics.PreferredBackBufferHeight = 800; // Höjd
            _graphics.ApplyChanges(); // Tillämpar ändringarna
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _logoImg = Texture2D.FromFile(GraphicsDevice, "../../img/Logo.png");
            _ballImg = Texture2D.FromFile(GraphicsDevice, "../../img/Ball.png");
            _personImg = Texture2D.FromFile(GraphicsDevice, "../../img/Person.png");
            _cloudImg = Texture2D.FromFile(GraphicsDevice, "../../img/Cloud.png");
            _flagImg = Texture2D.FromFile(GraphicsDevice, "../../img/Flag.png");


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(135,179,93));

            _spriteBatch.Begin();

            _spriteBatch.Draw(_logoImg, new Vector2(600, 50), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
