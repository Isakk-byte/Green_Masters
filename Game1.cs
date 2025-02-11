using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        Gamestate state = new Gamestate();
        public Gamestate.gameStates activateState = Gamestate.gameStates.startMenu;

        const int _playButtonWidth = 200;
        const int _playButtonHeight = 80;

        const int WIDTH = 1700;
        const int HEIGHT = 800;

        public Texture2D _buttonTexture;

        public Rectangle _buttonRectangle;
        public Rectangle _buttonRectangle2;
        public Rectangle _loadbar1;
        public Rectangle _loadbar2;
        public Rectangle _loadbar3;
        public Rectangle _loadbar4;

        public bool _isButton1Visible = true;
        public bool _isButton2Visible = true;

        public MouseState _currentMouseState;
        public MouseState _previousMouseState;

        public SpriteFont _buttonFont;

        public string _buttonText = "Play";
        public Vector2 _buttonTextPosition;

        public string _buttonText2 = "Settings";
        public Vector2 _buttonTextPosition2;

        public string _loadingText = "Loading...";
        public Vector2 _loadingTextPosition;

        private bool callLoadingOnce = false;

        private float _loadbarTimer = 0f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = WIDTH; // Bredd
            _graphics.PreferredBackBufferHeight = HEIGHT; // Höjd
            _graphics.ApplyChanges();


        }

        protected override void Initialize()
        {
            _buttonRectangle = new Rectangle(((WIDTH / 2) - (_playButtonWidth / 2)), ((HEIGHT / 2)), _playButtonWidth, _playButtonHeight);
            _buttonRectangle2 = new Rectangle(((WIDTH / 2) - (_playButtonWidth / 2)), ((HEIGHT / 2) + (_playButtonHeight + 30)), _playButtonWidth, _playButtonHeight);

            _loadbar1 = new Rectangle(((WIDTH / 2) - (120)), ((HEIGHT / 2) - (25)), 50, 50);
            _loadbar2 = new Rectangle(((WIDTH / 2) - (60)), ((HEIGHT / 2) - (25)), 50, 50);
            _loadbar3 = new Rectangle(((WIDTH / 2) + (00)), ((HEIGHT / 2) - (25)), 50, 50);
            _loadbar4 = new Rectangle(((WIDTH / 2) + (60)), ((HEIGHT / 2) - (25)), 50, 50);


            _currentMouseState = Mouse.GetState();
            //if (_gameState == "Game")
            //{
            _ball = new Ball(new Vector2(0, 0), new Vector2(300, 680), _ballImg, Color.White);

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
            //}


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
            _arrowOrigin = new Vector2(_arrowImg.Width / 2, _arrowImg.Height);
            //_arrowOrigin = new Vector2(_ballImg.Width / 2, _ballImg.Height / 2);

            _buttonTexture = new Texture2D(GraphicsDevice, 1, 1);
            _buttonTexture.SetData(new[] { Color.White });
            _buttonFont = Content.Load<SpriteFont>("buttonFont");
            Vector2 textSize = _buttonFont.MeasureString(_buttonText);
            Vector2 textSize2 = _buttonFont.MeasureString(_buttonText2);
            Vector2 textSize3 = _buttonFont.MeasureString(_loadingText);
            _buttonTextPosition = new Vector2(
                _buttonRectangle.X + (_playButtonWidth - textSize.X) / 2,
                _buttonRectangle.Y + (_playButtonHeight - textSize.Y) / 2
            );

            _buttonTextPosition2 = new Vector2(
            _buttonRectangle2.X + (_playButtonWidth - textSize2.X) / 2,
            _buttonRectangle2.Y + (_playButtonHeight - textSize2.Y) / 2);

            _loadingTextPosition = new Vector2(((WIDTH / 2) - (textSize3.X / 2)), ((HEIGHT / 2) + 50));
        }

        protected override void Update(GameTime gameTime)
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (activateState == Gamestate.gameStates.startMenu)
            {
                UpdateMenu();
            }
            else if (activateState == Gamestate.gameStates.playing)
            {
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
            }

            base.Update(gameTime);
        }

        void UpdateMenu()
        {
            if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                if (_buttonRectangle.Contains(_currentMouseState.Position))
                {
                    _isButton1Visible = false;
                    _isButton2Visible = true;
                    LoadData loadingScreen = new LoadData();

                    callLoadingOnce = true;
                    Thread t = new Thread(() => loadingScreen.LoadMethod(out activateState, ref callLoadingOnce));
                    t.Start();
                }
                if (_buttonRectangle2.Contains(_currentMouseState.Position))
                {
                    _isButton2Visible = false;
                    _isButton1Visible = true;
                }
            }
            _previousMouseState = _currentMouseState;
        }

        protected override void Draw(GameTime gameTime)
        {
            //loading color
            //GraphicsDevice.Clear(new Color(135,179,93));

            GraphicsDevice.Clear(new Color(163, 238, 255));

            if (callLoadingOnce)
            {
                GraphicsDevice.Clear(Color.GreenYellow);
                LoadAnimation(gameTime);
            }

            else if (activateState == Gamestate.gameStates.startMenu)
            {
                DrawMenu();
            }

            else if (activateState == Gamestate.gameStates.playing)
            {
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
                    _arrowImg,
                    _arrowPosition,
                    null,
                    Color.White,
                    _arrowRotation,
                    _arrowOrigin,
                    1.0f,
                    SpriteEffects.None,
                    0f
                );

                _ball.Draw(_spriteBatch, _ballImg);

                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        private void LoadAnimation(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_buttonFont, _loadingText, _loadingTextPosition, Color.White);
            _loadbarTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_loadbarTimer < 1f)
            {
                _spriteBatch.Draw(_buttonTexture, _loadbar1, Color.White);
            }
            if (_loadbarTimer > 0.5f && _loadbarTimer < 1.5f)
            {
                _spriteBatch.Draw(_buttonTexture, _loadbar2, Color.White);
            }
            if (_loadbarTimer > 1f && _loadbarTimer < 2f)
            {
                _spriteBatch.Draw(_buttonTexture, _loadbar3, Color.White);
            }
            if (_loadbarTimer > 1.5f && _loadbarTimer < 2.5f)
            {
                _spriteBatch.Draw(_buttonTexture, _loadbar4, Color.White);
            }
            if (_loadbarTimer > 3f)
            {
                _loadbarTimer = 0f;
            }

            _spriteBatch.End();
        }

        private void DrawMenu()
        {
            _spriteBatch.Begin();
            if (_isButton1Visible)
            {
                _spriteBatch.Draw(_buttonTexture, _buttonRectangle, Color.CornflowerBlue);
                _spriteBatch.DrawString(_buttonFont, _buttonText, _buttonTextPosition, Color.White);
            }

            if (_isButton2Visible)
            {
                _spriteBatch.Draw(_buttonTexture, _buttonRectangle2, Color.CornflowerBlue);
                _spriteBatch.DrawString(_buttonFont, _buttonText2, _buttonTextPosition2, Color.White);
            }
            _spriteBatch.End();

        }
    }
}

