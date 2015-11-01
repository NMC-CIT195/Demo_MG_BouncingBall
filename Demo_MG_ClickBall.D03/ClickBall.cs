using System; // add to allow Windows message box
using System.Runtime.InteropServices; // add to allow Windows message box
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Demo_MG_ClickBall
{
    /// <summary>
    /// initial layout of game including walls and a ball
    /// </summary>
    public class ClickBall : Game
    {
        // add code to allow Windows message boxes when running in a Windows environment
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        // set the cell size in pixels
        private const int CELL_WIDTH = 64;
        private const int CELL_HEIGHT = 64;

        // set the map size in cells
        private const int MAP_CELL_ROW_COUNT = 8;
        private const int MAP_CELL_COLUMN_COUNT = 10;

        // set the window size
        private const int WINDOW_WIDTH = MAP_CELL_COLUMN_COUNT * CELL_WIDTH;
        private const int WINDOW_HEIGHT = MAP_CELL_ROW_COUNT * CELL_HEIGHT;

        // set the location for the score
        private const int SCORE_X_POSTION = 500;
        private const int SCORE_Y_POSTION = 20;

        // set the winning score
        private const int WINNING_SCORE = 5;

        // set the time limit seconds
        private const double TIME_LIMIT = 5;

        // create a random number set
        private Random _randomNumbers = new Random();

        // declare instance variables for the background
        private Texture2D _background;
        private Rectangle _backgroundPosition;

        // declare instance variables for the sprites
        private Ball _ball;

        // declare a spriteBatch object
        private SpriteBatch _spriteBatch;

        // declare a MouseState object to get mouse information
        private MouseState _mouseOldState;
        private MouseState _mouseNewState;

        // declare a SpriteFont for the on-screen score
        private SpriteFont _scoreFont;

        // declare a variable to store the score
        private int _score;

        // declare a variable for the timer
        private double _timer = TIME_LIMIT;

        private GraphicsDeviceManager _graphics;

        /// <summary>
        /// constructor initializes the game objects
        /// </summary>
        public ClickBall()
        {
            _graphics = new GraphicsDeviceManager(this);

            // set the window size 
            _graphics.PreferredBackBufferWidth = MAP_CELL_COLUMN_COUNT * CELL_WIDTH;
            _graphics.PreferredBackBufferHeight = MAP_CELL_ROW_COUNT * CELL_HEIGHT;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// method allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // set the background's initial position
            _backgroundPosition = new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);

            // create a ball object
            string spriteName = "Ball";
            Vector2 position = new Vector2(300, 200);
            Vector2 velocity = new Vector2(2, 2);
            _ball = new Ball(Content, spriteName, position, velocity);

            // _ball = new Ball(Content, "Ball", new Vector2(300, 200), new Vector2( 2, 2));

            // make the ball active
            _ball.Active = true;

            // make mouse visible on game
            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// method will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // load the background sprite
            // ball sprite loaded when instantiated
            _background = Content.Load<Texture2D>("BackgroundSandyStained");

            // load the font for the score
            _scoreFont = Content.Load<SpriteFont>("ScoreFont");
        }

        /// <summary>
        /// method will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here
        }

        /// <summary>
        /// main game loop
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // player still playing
            if ((_score != WINNING_SCORE) && (_timer > 0))
            {
                HandleKeyboardEvents();
                HandleMouseEvents();
                UpdateBallMovement(_ball);
                UpdateTimer(gameTime);

                base.Update(gameTime);
            }
            // player wins
            else if (_score == WINNING_SCORE)
            {
                DisplayWinScreen();
            }
            // player out of time
            else if (_timer <= 0)
            {
                DisplayOutOfTimeMessage();
            }

        }

        /// <summary>
        /// method to add all of the current sprites to the next game screen
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // draw the background and the ball
            _spriteBatch.Draw(_background, _backgroundPosition, Color.White);
            _ball.Draw(_spriteBatch);

            DrawScoreTimer();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #region HELPER METHODS

        /// <summary>
        /// method to bounce ball of walls
        /// </summary>
        public void BounceOffWalls()
        {
            // ball is at the top or bottom of the window, change the Y direction
            if ((_ball.Position.Y > WINDOW_HEIGHT - CELL_HEIGHT) || (_ball.Position.Y < 0))
            {
                _ball.Velocity = new Vector2(_ball.Velocity.X, -_ball.Velocity.Y);
            }
            // ball is at the left or right of the window, change the X direction
            else if ((_ball.Position.X > WINDOW_WIDTH - CELL_WIDTH) || (_ball.Position.X < 0))
            {
                _ball.Velocity = new Vector2(-_ball.Velocity.X, _ball.Velocity.Y);
            }
        }

        /// <summary>
        /// method to determine if the mouse is on the ball
        /// </summary>
        /// <returns></returns>
        private bool MouseClickOnBall()
        {
            bool mouseClickedOnBall = false;

            // get the current state of the mouse
            _mouseNewState = Mouse.GetState();

            // left mouse button was a click
            if (_mouseNewState.LeftButton == ButtonState.Pressed && _mouseOldState.LeftButton == ButtonState.Released)
            {
                // mouse over ball
                if ((_mouseNewState.X > _ball.Position.X) &&
                    (_mouseNewState.X < (_ball.Position.X + 64)) &&
                    (_mouseNewState.Y > _ball.Position.Y) &&
                    (_mouseNewState.Y < (_ball.Position.Y + 64)))
                {
                    mouseClickedOnBall = true;

                    _score++;
                }
            }

            // store the current state of the mouse as the old state
            _mouseOldState = _mouseNewState;

            return mouseClickedOnBall;
        }

        /// <summary>
        /// spawn the ball at a random location on the screen
        /// </summary>
        /// <param name="ball">ball object</param>
        private void Spawn(Ball ball)
        {
            // find a valid location to spawn the ball
            int ballXPosition = _randomNumbers.Next(WINDOW_WIDTH - CELL_WIDTH);
            int ballYPosition = _randomNumbers.Next(WINDOW_HEIGHT - CELL_HEIGHT);

            // set ball's new position and reverse direction
            ball.Position = new Vector2(ballXPosition, ballYPosition);
            ball.Velocity = -ball.Velocity;
        }

        /// <summary>
        /// method to catch and handle keyboard events
        /// </summary>
        private void HandleKeyboardEvents()
        {
            // detect an Escape key press to end the game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                // demonstrate the use of a Window's message box to display information
                MessageBox(new IntPtr(0), "Escape key pressed Click OK to exit.", "Debug Message", 0);
                Exit();
            }
        }

        /// <summary>
        /// method to catch and handle mouse events
        /// </summary>
        private void HandleMouseEvents()
        {
            // if the mouse is over the ball and left button is clicked, destroy and spawn the ball
            if (MouseClickOnBall())
            {
                Spawn(_ball);
            }
        }

        /// <summary>
        /// method to move the ball forward and bounce off the sites of the screen
        /// </summary>
        /// <param name="_ball">ball object</param>
        private void UpdateBallMovement(Ball _ball)
        {
            if (_ball.Active)
            {
                BounceOffWalls();
                _ball.Position += _ball.Velocity;
            }
        }

        /// <summary>
        /// method to draw the current score and the timer on the game screen
        /// </summary>
        private void DrawScoreTimer()
        {
            _spriteBatch.DrawString(_scoreFont, "Score: " + _score, new Vector2(SCORE_X_POSTION, SCORE_Y_POSTION), Color.Black);
            _spriteBatch.DrawString(_scoreFont, "Time: " + _timer.ToString("000"), new Vector2(SCORE_X_POSTION, SCORE_Y_POSTION + 25), Color.Black);
        }

        /// <summary>
        /// method to display the Win screen
        /// </summary>
        private void DisplayWinScreen()
        {
            MessageBox(new IntPtr(0), "You have won the game!\n Press any key to exit.", "Debug Message", 0);
            Exit();
        }

        /// <summary>
        /// method to up the game timer in seconds
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateTimer(GameTime gameTime)
        {
            _timer -= gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// method to display an out of time message
        /// </summary>
        private void DisplayOutOfTimeMessage()
        {
            MessageBox(new IntPtr(0), "Sorry, you ran out of time.\n Press any key to exit.", "Debug Message", 0);
            Exit();
        }

        #endregion


    }
}
