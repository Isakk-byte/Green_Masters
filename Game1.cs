using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
//using Microsoft.Xna.Framework.Media;

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
        Texture2D _powerPickImg;

        private Ball _ball;
        private PowerPick _powerPick;
        private PowerBar _powerBar;
        private Flag _flag;

        private List<Cloud> _clouds;

        private Vector2 _arrowPosition;
        private float _arrowRotation;
        private Vector2 _arrowOrigin;

        private float _rotationSpeed;   // Rotationshastighet (radianer per sekund)
        private float _targetRotation;

        private string _currentShootingState = "arrowMoving"; //1. arrowMoving, 2. powerMoving, 3. shot
        private bool _initializeBallShot = false; //has the ball shooting been initialized

        private float _currentPower;

        Gamestate state = new Gamestate();
        public Gamestate.gameStates activateState = Gamestate.gameStates.startMenu;

        const int _playButtonWidth = 200;
        const int _playButtonHeight = 80;

        const int WIDTH = 1700;
        const int HEIGHT = 800;

        public Texture2D _buttonTexture;

        public Rectangle _scoreboard;
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
        public KeyboardState _currentKeyboardState;
        public KeyboardState _previousKeyboardState;

        public SpriteFont _buttonFont;

        public string _mainText = "Green Masters";

        public string _buttonText = "Play";
        public Vector2 _buttonTextPosition;

        public string _buttonText2 = "Settings";
        public Vector2 _buttonTextPosition2;

        public string _loadingText = "Loading...";
        public Vector2 _loadingTextPosition;

        private bool callLoadingOnce = false;

        private float _loadbarTimer = 0f;

        private float _score = 0;
        private float _shots = 0;
        public int _hole = 0;
        private float _totalShots = 0;
        public string _scoreText = "Score: 0";

        //Song backgroundMusic;

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
            _buttonRectangle = new Rectangle(((WIDTH / 2) - (_playButtonWidth / 2)), ((HEIGHT / 2)+55), _playButtonWidth, _playButtonHeight);
            _buttonRectangle2 = new Rectangle(((WIDTH / 2) - (_playButtonWidth / 2)), ((HEIGHT / 2) + (_playButtonHeight + 85)), _playButtonWidth, _playButtonHeight);

            _scoreboard = new Rectangle((WIDTH / 2)-250, (HEIGHT / 2)-350, 500, 600);

            _loadbar1 = new Rectangle(((WIDTH / 2) - (120)), (HEIGHT - (205)), 50, 50);
            _loadbar2 = new Rectangle(((WIDTH / 2) - (60)), (HEIGHT - (205)), 50, 50);
            _loadbar3 = new Rectangle(((WIDTH / 2) + (00)), (HEIGHT - (205)), 50, 50);
            _loadbar4 = new Rectangle(((WIDTH / 2) + (60)), (HEIGHT - (205)), 50, 50);


            _currentMouseState = Mouse.GetState();
            //if (_gameState == "Game")
            //{
            _ball = new Ball(new Vector2(0, 0), new Vector2(300, 680), _ballImg, Color.White);

            _powerBar = new PowerBar(new Vector2(170, 750), _powerbarImg, Color.White);
            _powerPick = new PowerPick(new Vector2(5, 0), _powerBar._position, _powerPickImg, Color.White);
            _flag = new Flag(new Vector2(1500, 500), _flagImg, Color.White);

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
            _powerPickImg = Texture2D.FromFile(GraphicsDevice, "../../img/PowerPick.png");

            //backgroundMusic = Content.Load<Song>("musikfilens_namn_utan_ändelse");
            
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

            _loadingTextPosition = new Vector2(((WIDTH / 2) - (textSize3.X / 2)), (HEIGHT- 270));
        }

        protected override void Update(GameTime gameTime)
        {

            //MediaPlayer.Play(backgroundMusic);
           // MediaPlayer.IsRepeating = true; // Loopa musiken om du vill

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

                if (_currentShootingState == "arrowMoving")
                {

                    //if button pressed, move to next type
                    _previousKeyboardState = _currentKeyboardState;
                    _currentKeyboardState = Keyboard.GetState();
                    if (/*_currentMouseState.LeftButton == ButtonState.Pressed || */_currentKeyboardState.IsKeyUp(Keys.Space) && _previousKeyboardState.IsKeyDown(Keys.Space))
                    {
                        _currentShootingState = "powerMoving";
                    }
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
                        if (_targetRotation == MathHelper.ToRadians(80))
                            _targetRotation = MathHelper.ToRadians(10); // Nästa mål är att peka uppåt
                        else
                            _targetRotation = MathHelper.ToRadians(80); // Nästa mål är att peka höger
                    }
                    _powerPick._position.X = _powerBar._position.X;
                }
                else if (_currentShootingState == "powerMoving")
                {

                    //_currentPower = 10f;
                    
                    //if button pressed, move to next type
                    _previousKeyboardState = _currentKeyboardState;
                    _currentKeyboardState = Keyboard.GetState();
                    if (/*_currentMouseState.LeftButton == ButtonState.Pressed || */_currentKeyboardState.IsKeyUp(Keys.Space) && _previousKeyboardState.IsKeyDown(Keys.Space))
                    {
                        _currentPower = (_powerPick._position.X - _powerBar._position.X) / 15f;
                        _currentShootingState = "shot";
                    }


                    _powerPick._position += _powerPick._velocity;

                    if (_powerPick._position.X > _powerBar._position.X + _powerbarImg.Width)
                    {
                        _powerPick._velocity.X = _powerPick._velocity.X * -1f;
                    }
                    else if (_powerPick._position.X < _powerBar._position.X)
                    {
                        _powerPick._velocity.X = _powerPick._velocity.X * -1f;
                    }

                }
                else if (_currentShootingState == "shot")
                {
                    //first time, calculate start force of the ball
                    if (!_initializeBallShot)
                    {
                        //calculate x and y velocity

                        //float angleRadians = MathF.PI / 180 * (_arrowRotation -90);
                        float angleRadians = _arrowRotation + ((MathF.PI / 180) * -90);
                        _ball._velocity = new Vector2(MathF.Cos(angleRadians) * _currentPower, MathF.Sin(angleRadians) * _currentPower);

                        _initializeBallShot = true;
                    }
                    else if (_initializeBallShot)
                    {

                        //ball position += (ball velocity.x - 9.8 *deltaTime, ball velocity.y -1 * delta time)
                        _ball._velocity = new Vector2(_ball._velocity.X, _ball._velocity.Y + (9.8f * deltaTime));
                        if (_ball._position.Y > 700 /*- _ballImg.Height*/)
                        {
                            _ball._position.Y = 700;
                            _ball._velocity.Y = _ball._velocity.Y * -0.60f; // studs
                            _ball._velocity.X = _ball._velocity.X * 0.60f; // gräsfriktion
                        }

                        //check if ball is still
                        if (_ball._position.X >= _flag._position.X && _ball._position.X < _flag._position.X + _flagImg.Width - _ballImg.Width
                            && _ball._position.Y >= 730 - (_flagImg.Width/2) && _ball._position.Y < 730 + (_flagImg.Width / 2)
                            && _ball._velocity.X < _ball._velocity.Y* 4)
                        {
                            _currentShootingState = "stopped";

                        }
                        else if(_ball._velocity.X <= 1f && _ball._velocity.Y <= 1f)
                        {
                            //to next state
                            _currentShootingState = "stopped";
                        }
                        else
                        {
                            _ball._position += _ball._velocity;
                        }
                    }
                    
                    //all other times, move ball accordingly
                }
                else if(_currentShootingState == "stopped")
                {
                    System.Threading.Thread.Sleep(1500);

                    _shots++;
                    if (_ball._position.X > WIDTH) //utanför skärm
                    {
                        //reset round and give 0 points
                        _flag._position.X = 1600f;
                        _ball._position.X = 300f;
                        _initializeBallShot = false;

                        _currentShootingState = "arrowMoving";
                    }
                    else if (MathF.Abs(_ball._position.X - _flag._position.X) <= 50f) //check if goal
                    {
                        _hole++;
                        //ball has been putted into hole! reset round and calc(short for calculation) points
                        if (_shots == 1)
                        {
                            _score += 15000;
                        }
                        else
                        {
                            _score += MathF.Round(10000 * MathF.Pow(2.7f, -0.256f * (_shots - 1)));
                        }
                            //P(slag)=10000 × e^(−0.256(slag−1))
                        _totalShots += _shots;
                        _shots = 0;
                        _scoreText = "Score: " + _score;
                        _flag._position.X = 1600f;
                        _ball._position.X = 300f;
                        _initializeBallShot = false;

                        _currentShootingState = "arrowMoving";
                    }
                    else //flytta flaggan
                    {
                        
                        _flag._position.X = 300f + MathF.Abs(_ball._position.X - _flag._position.X);

                        _ball._position.X = 300f;
                        _initializeBallShot = false;
                        
                        _currentShootingState = "arrowMoving";
                    }



                    if (_hole == 5) //Avslutar spelet efter 5 hål
                    {
                        activateState = Gamestate.gameStates.endScen;
                    }
                    //start next round and move flag towards player according to how close the shot was
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

            //menu himmel färg
            GraphicsDevice.Clear(new Color(163, 238, 255));

            if (callLoadingOnce)
            {
                GraphicsDevice.Clear(new Color(135, 179, 93));
                LoadAnimation(gameTime);
            }

            else if (activateState == Gamestate.gameStates.startMenu)
            {
                _spriteBatch.Begin();

                _spriteBatch.Draw(_groundImg, new Vector2(0, 700), Color.White);
                _spriteBatch.Draw(_buttonTexture, _scoreboard, Color.Brown);

                _spriteBatch.End();
                //DrawMenu();
            }

            else if (activateState == Gamestate.gameStates.playing)
            {
                _spriteBatch.Begin();

                _spriteBatch.Draw(_groundImg, new Vector2(0, 700), Color.White);
                _spriteBatch.Draw(_personImg, new Vector2(10, 380), Color.White);

                _spriteBatch.DrawString(_buttonFont, _scoreText, new Vector2((WIDTH/2-20), 300), Color.White);

                //_spriteBatch.Draw(_powerbarImg, new Vector2(170, 750), Color.White);



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
                _powerBar.Draw(_spriteBatch, _powerbarImg);
                _powerPick.Draw(_spriteBatch, _powerPickImg);
                _flag.Draw(_spriteBatch, _flagImg);

                _spriteBatch.End();
            }
            else if (activateState == Gamestate.gameStates.endScen)
            {
                _spriteBatch.Begin();

                _spriteBatch.Draw(_groundImg, new Vector2(0, 700), Color.White);
                _spriteBatch.Draw(_buttonTexture, _scoreboard, Color.Black);

                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        private void LoadAnimation(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_buttonFont, _loadingText, _loadingTextPosition, Color.White);
            _loadbarTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            _spriteBatch.Draw(_logoImg, new Vector2(600, 70), Color.White);

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

            _spriteBatch.Draw(_logoImg, new Vector2(600,0), Color.White);
            _spriteBatch.Draw(_groundImg, new Vector2(0, 700), Color.White);

            if (_isButton1Visible)
            {
                _spriteBatch.Draw(_buttonTexture, _buttonRectangle, Color.Red);
                _spriteBatch.DrawString(_buttonFont, _buttonText, _buttonTextPosition, Color.White);
            }

            if (_isButton2Visible)
            {
                _spriteBatch.Draw(_buttonTexture, _buttonRectangle2, Color.Red);
                _spriteBatch.DrawString(_buttonFont, _buttonText2, _buttonTextPosition2, Color.White);
            }
            _spriteBatch.End();

        }
    }
}

