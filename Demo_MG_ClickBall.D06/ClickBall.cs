using System; // add to allow Windows message box
using System.Runtime.InteropServices; // add to allow Windows message box
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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
        private const int SCORE_Y_POSTION = 5;

        // set the winning score
        private const int WINNING_SCORE = 5;

        // set the time limit seconds
        private const double TIME_LIMIT = 50;

        // create a random number set
        private Random _randomNumbers = new Random();

        // declare instance variables for the background
        private Texture2D _background;
        private Rectangle _backgroundPosition;

        // declare instance variables for the sprites
        private List<Ball> _balls;
        private List<Wall> _walls;

        // declare a flag to indicate when the map needs to be redrawn
        private bool _modifyMap = true;

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

        // declare a SoundEffect for the explosion
        private SoundEffect _explosion;

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

            // create a list of Ball objects
            _balls = new List<Ball>();

            // create a list of Wall objects
            _walls = new List<Wall>();

            // add balls to the list
            Ball ball01 = new Ball(Content, "Ball", 32, new Vector2(200, 200), new Vector2(4, 4));
            _balls.Add(ball01);
            Ball ball02 = new Ball(Content, "Ball", 32, new Vector2(100, 300), new Vector2(-2, -2));
            _balls.Add(ball02);
            Ball ball03 = new Ball(Content, "small_ball", 16, new Vector2(300, 300), new Vector2(4, -4));
            _balls.Add(ball03);

            // make the balls active
            ball01.Active = true;
            ball02.Active = true;
            ball03.Active = true;

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

            // load the explosion
            _explosion = Content.Load<SoundEffect>("Explosion01");
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
                UpdateBallMovement(_balls);
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

            // draw the balls
            foreach (var ball in _balls)
            {
                ball.Draw(_spriteBatch);
            }

            // draw the walls
            foreach (var wall in _walls)
            {
                wall.Draw(_spriteBatch);
            }



            // call the BuildMap method to add all of the walls
            BuildMap();

            DrawScoreTimer();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #region HELPER METHODS


        /// <summary>
        /// method to move the ball forward and bounce off the sites of the screen
        /// </summary>
        /// <param name="_ball">ball object</param>
        private void UpdateBallMovement(List<Ball> balls)
        {
            foreach (var ball in balls)
            {
                if (ball.Active)
                {
                    BounceOffWalls(ball);
                    BounceOffBalls(balls, ball);
                    ball.Position += ball.Velocity;
                }
            }

        }

        /// <summary>
        /// method to bounce ball of walls
        /// </summary>
        //public void BounceOffWalls(Ball ball)
        //{
        //    // ball is at the top or bottom of the window, change the Y direction
        //    if ((ball.Position.Y > (WINDOW_HEIGHT - CELL_HEIGHT) - ball.Radius * 2) || (ball.Position.Y < CELL_HEIGHT))
        //    {
        //        ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
        //    }
        //    // ball is at the left or right of the window, change the X direction
        //    else if ((ball.Position.X > (WINDOW_WIDTH - CELL_WIDTH) - ball.Radius * 2) || (ball.Position.X < CELL_HEIGHT))
        //    {
        //        ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
        //    }
        //}

        public void BounceOffWalls(Ball ball)
        {
            Rectangle ballRectangle = new Rectangle((int)ball.Position.X, (int)ball.Position.Y, (int)(ball.Radius * 2), (int)(ball.Radius * 2));
            foreach (Wall wall in _walls)
            {
                Rectangle wallRectangle = new Rectangle((int)wall.Position.X, (int)wall.Position.Y, CELL_WIDTH, CELL_HEIGHT);
                if (Rectangle.Intersect(ballRectangle, wallRectangle) != null)
                {
                    MessageBox(new IntPtr(0), "I hit a wall!!", "Debug Message", 0);
                }
            }



            //// ball is at the top or bottom of the window, change the Y direction
            //if ((ball.Position.Y > (WINDOW_HEIGHT - CELL_HEIGHT) - ball.Radius * 2) || (ball.Position.Y < CELL_HEIGHT))
            //{
            //    ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
            //}
            //// ball is at the left or right of the window, change the X direction
            //else if ((ball.Position.X > (WINDOW_WIDTH - CELL_WIDTH) - ball.Radius * 2) || (ball.Position.X < CELL_HEIGHT))
            //{
            //    ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
            //}
        }



        /// <summary>
        /// method to change the velocity of the balls when they collide
        /// </summary>
        /// <param name="balls">list of all balls</param>
        /// <param name="checkBall">ball being check</param>
        public void BounceOffBalls(List<Ball> balls, Ball checkBall)
        {
            foreach (Ball ball in balls)
            {
                if (ball != checkBall)
                {
                    if (DistanceBetweenBalls(checkBall, ball) < checkBall.Radius + ball.Radius)
                    {
                        checkBall.Velocity = -checkBall.Velocity;
                    }
                }

            }
        }

        /// <summary>
        /// method to calculate the distance between the centers of two balls
        /// </summary>
        /// <param name="ball1">first ball</param>
        /// <param name="ball2">second ball</param>
        /// <returns></returns>
        public double DistanceBetweenBalls(Ball ball1, Ball ball2)
        {
            double distance;

            distance = Vector2.Distance(ball1.Center, ball2.Center);

            return distance;
        }

        /// <summary>
        /// method to catch and handle mouse events
        /// </summary>
        private void HandleMouseEvents()
        {
            // get the current state of the mouse
            _mouseNewState = Mouse.GetState();

            // left mouse button was a click
            if (_mouseNewState.LeftButton == ButtonState.Pressed && _mouseOldState.LeftButton == ButtonState.Released)
            {
                // check all balls
                foreach (var ball in _balls)
                {
                    // if the mouse is over the ball and left button is clicked, destroy and spawn the ball
                    if (MouseOnBall(ball))
                    {
                        _explosion.CreateInstance().Play();
                        Spawn(ball);
                        _score++;
                    }
                }
            }

            // store the current state of the mouse as the old state
            _mouseOldState = _mouseNewState;
        }

        /// <summary>
        /// method to determine if the mouse is on the ball
        /// </summary>
        /// <returns></returns>
        private bool MouseOnBall(Ball ball)
        {
            bool mouseClickedOnBall = false;

            // get the current state of the mouse
            MouseState mouseState = Mouse.GetState();

            // mouse over ball
            if ((_mouseNewState.X > ball.Position.X) &&
                (_mouseNewState.X < (ball.Position.X + 64)) &&
                (_mouseNewState.Y > ball.Position.Y) &&
                (_mouseNewState.Y < (ball.Position.Y + 64)))
            {
                mouseClickedOnBall = true;
            }

            return mouseClickedOnBall;
        }

        /// <summary>
        /// spawn the ball at a random location on the screen
        /// </summary>
        /// <param name="ball">ball object</param>
        private void Spawn(Ball ball)
        {
            // find a valid location to spawn the ball
            int ballXPosition = _randomNumbers.Next(WINDOW_WIDTH - (2 * CELL_WIDTH));
            int ballYPosition = _randomNumbers.Next(WINDOW_HEIGHT - (2 * CELL_HEIGHT));

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


        /// <summary>
        /// method to add the starting walls to the map
        /// </summary>
        private void BuildMap()
        {
            // draw top and bottom walls
            for (int column = 0; column < MAP_CELL_COLUMN_COUNT; column++)
            {
                int wallCellXPos = column * CELL_WIDTH;
                int topWallYPos = 0;
                int bottomWallYPos = (MAP_CELL_ROW_COUNT - 1) * CELL_HEIGHT;

                Vector2 topWallCellPosition = new Vector2(wallCellXPos, topWallYPos);
                Vector2 bottonWallCellPosition = new Vector2(wallCellXPos, bottomWallYPos);

                Wall topWallSection = new Wall(Content, "Wall", topWallCellPosition);
                topWallSection.Active = true;
                //topWallSection.Draw(_spriteBatch);
                _walls.Add(topWallSection);

                Wall bottomWallSection = new Wall(Content, "Wall", bottonWallCellPosition);
                bottomWallSection.Active = true;
                //bottomWallSection.Draw(_spriteBatch);
                _walls.Add(bottomWallSection);
            }

            // draw side walls
            for (int row = 0; row < MAP_CELL_ROW_COUNT - 2; row++)
            {
                int wallYPos = (row + 1) * CELL_HEIGHT;
                int leftWallXPos = 0;
                int rightWallXPos = (MAP_CELL_COLUMN_COUNT - 1) * CELL_HEIGHT;

                Vector2 leftWallCellPosition = new Vector2(leftWallXPos, wallYPos);
                Vector2 rightWallCellPosition = new Vector2(rightWallXPos, wallYPos);

                Wall leftWallSection = new Wall(Content, "Wall", leftWallCellPosition);
                leftWallSection.Active = true;
                //leftWallSection.Draw(_spriteBatch);
                _walls.Add(leftWallSection);

                Wall rightWallSection = new Wall(Content, "Wall", rightWallCellPosition);
                rightWallSection.Active = true;
                //rightWallSection.Draw(_spriteBatch);
                _walls.Add(rightWallSection);
            }

            // draw the scoreboard
            Content.Load<Texture2D>("ScoreBoard");
            _spriteBatch.Draw(
                Content.Load<Texture2D>("ScoreBoard"),
                new Vector2(WINDOW_WIDTH - 192, 0),
                Color.White);
        }


        #endregion
    }
}
