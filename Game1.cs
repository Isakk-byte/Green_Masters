using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System.Collections.Generic;
using System.Threading;

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
        Texture2D _arrowImg;
        Texture2D _groundImg;
        Texture2D _powerbarImg;

        private Ball _ball;

        private List<Cloud> _clouds;

        private Vector2 _arrowPosition;
        private float _arrowRotation;   
        private Vector2 _arrowOrigin;

        private float _rotationSpeed;   // Rotationshastighet (radianer per sekund)
        private float _targetRotation;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
           
            _graphics.PreferredBackBufferWidth = 1700; // Bredd
            _graphics.PreferredBackBufferHeight = 800; // Höjd
            _graphics.ApplyChanges();


    }

        protected override void Initialize()
        {

            _ball= new Ball(new Vector2(0,0), new Vector2(300, 680), _ballImg, Color.White);

            _clouds = new List<Cloud>
            {
                new Cloud(new Vector2(30, 0), new Vector2(0, 50), _cloudImg, Color.White),
                new Cloud(new Vector2(30, 0), new Vector2(1100, 50), _cloudImg, Color.White),
                new Cloud(new Vector2(30, 0), new Vector2(560, 50), _cloudImg, Color.White)
            };

            _arrowPosition = new Vector2(315, 695);
            _arrowRotation = 0f;

            //konverterar en vinkel från grader till radianer
            _rotationSpeed = MathHelper.ToRadians(90); // 90 grader per sekund
            _targetRotation = MathHelper.ToRadians(90);


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
            _arrowImg = Texture2D.FromFile(GraphicsDevice, "../../img/Arrow.png");
            _groundImg = Texture2D.FromFile(GraphicsDevice, "../../img/Ground.png");
            _powerbarImg = Texture2D.FromFile(GraphicsDevice, "../../img/Powerbar.png");

            //sätter punkten att rotera runt
            _arrowOrigin = new Vector2(_arrowImg.Width/2,_arrowImg.Height);
            //_arrowOrigin = new Vector2(_ballImg.Width / 2, _ballImg.Height / 2);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //updatera moln
            foreach (var cloud in _clouds)
            {
                cloud.MoveCloud(gameTime, _graphics.PreferredBackBufferWidth);
            }

            //updatera pil
            //sekunder som gått sedan senaste framen
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_arrowRotation < _targetRotation)
            {
                // Öka rotationen gradvis
                _arrowRotation += _rotationSpeed * deltaTime;

            }
            //rotera tillbaka
            else if (_arrowRotation > _targetRotation)
            {
                _arrowRotation -= _rotationSpeed * deltaTime;

                if (_arrowRotation < _targetRotation)
                    _arrowRotation = _targetRotation;
            }

            
            if (_arrowRotation == _targetRotation)
            {
                // Växla mellan uppåt (0 radianer) och höger (90 grader)
                if (_targetRotation == MathHelper.ToRadians(90))
                    _targetRotation = 0f; // Nästa mål är att peka uppåt
                else
                    _targetRotation = MathHelper.ToRadians(90); // Nästa mål är att peka höger
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //loading color
            //GraphicsDevice.Clear(new Color(135,179,93));

            GraphicsDevice.Clear(new Color(163, 238, 255));


            _spriteBatch.Begin();

            _spriteBatch.Draw(_groundImg, new Vector2(0, 700), Color.White);
            _spriteBatch.Draw(_personImg, new Vector2(10, 380), Color.White);

            _spriteBatch.Draw(_logoImg, new Vector2(580, 120), Color.White);
            _spriteBatch.Draw(_flagImg, new Vector2(1500, 500), Color.White);
            _spriteBatch.Draw(_powerbarImg, new Vector2(170, 750), Color.White);



            foreach (var cloud in _clouds)
            {
                cloud.Draw(_spriteBatch, _cloudImg);
            }

            _spriteBatch.Draw(
                _arrowImg,                // Texturen
                _arrowPosition,               // Positionen på skärmen
                null,                         // Källrektangel (null = hela texturen används)
                Color.White,                  // Färg (White = originalfärg)
                _arrowRotation,               // Rotationsvinkeln (45 grader)
                _arrowOrigin,                 // Ursprunget för rotation (nedre vänstra hörnet)
                1.0f,                         // Skalning
                SpriteEffects.None,           // Inga spegeleffekter
                0f                            // Layer depth (0 = längst fram)
            );
            _ball.Draw(_spriteBatch, _ballImg);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

