﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System; // add to allow Windows message box
using System.Runtime.InteropServices; // add to allow Windows message box

namespace Demo_MG_ClickBall.Sprint3
{
    /// <summary>
    /// initial layout of game including walls and a ball
    /// </summary>
    public class BouncingBall : Game
    {
        // add code to allow Windows message boxes when running in a Windows environment
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        // set the cell size in pixels
        private const int CELL_WIDTH = 64;
        private const int CELL_HEIGHT = 64;

        // TODO 00a - increase map size
        // set the map size in cells
        private const int MAP_CELL_ROW_COUNT = 8;
        private const int MAP_CELL_COLUMN_COUNT = 12;

        // declare a GraphicsDevideManater object to handle the graphics of the game
        private GraphicsDeviceManager _graphics;

        // declare a spriteBatch object 
        private SpriteBatch _spriteBatch;

        // declare instance variables for the sprites
        private Texture2D _ball;
        private Texture2D _wall;
        private Vector2 _ballPosition;
        private bool _ballVisible;

        // TODO 01a - add instance variable to store the ball's speeds; x and y directions
        private int _ballXSpeed;
        private int _ballYSpeed;

        // declare a MouseState object to get mouse information
        private MouseState _mouseState;

        // TODO 05a - declare a random number generator
        Random randomNumber01;

        /// <summary>
        /// constructor initializes the game objects
        /// </summary>
        public BouncingBall()
        {
            _graphics = new GraphicsDeviceManager(this);

            // set the window size as a function of cell size and cell count
            _graphics.PreferredBackBufferWidth = CELL_WIDTH * MAP_CELL_COLUMN_COUNT;
            _graphics.PreferredBackBufferHeight = CELL_HEIGHT * MAP_CELL_ROW_COUNT;

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
            // set the ball's initial position
            _ballPosition.X = 100;
            _ballPosition.Y = 200;

            // TODO 01b - set ball's initial speeds
            _ballXSpeed = 4;
            _ballYSpeed = 4;

            // make mouse visible on game
            this.IsMouseVisible = true;

            // set the balls visibility
            _ballVisible = true;

            // TODO 05b - initialize the random number generator
            randomNumber01 = new Random();

            base.Initialize();
        }

        /// <summary>
        /// method will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // instantiate the spriteBatch object
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // add sprite files to Content folder 
            // load images into the game
            _ball = Content.Load<Texture2D>("ball");
            _wall = Content.Load<Texture2D>("wall");
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
        /// method allowing the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // detect an Escape key press to end the game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                // demonstrate the use of a Window's message box to display information
                MessageBox(new IntPtr(0), "Escape key pressed Click OK to exit.", "Debug Message", 0);
                Exit();
            }

            // if the mouse is over the ball and left button is clicked, make the ball invisible
            if (MouseOnBall() && (_mouseState.LeftButton == ButtonState.Pressed))
            {
                //MessageBox(new IntPtr(0), "Mouse on Ball", "Debug Message", 0);
                _ballVisible = false;

                // TODO 04b - call the method to re-spawn the ball in a random location
                SpawnBall();
            }

            // TODO 02b - call the method to move the ball
            MoveBall();

            base.Update(gameTime);
        }

        /// <summary>
        /// method to add all of the current sprites to the next game screen
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //
            if (_ballVisible)
            {
                //  draw the ball on the screen using the spriteBatch object
                _spriteBatch.Draw(_ball, _ballPosition, Color.White);
            }


            // call the method to draw the wall sprites
            BuildMap();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #region HELPER METHODS

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

                _spriteBatch.Draw(_wall, topWallCellPosition, Color.White);
                _spriteBatch.Draw(_wall, bottonWallCellPosition, Color.White);
            }

            // draw side walls
            for (int row = 0; row < MAP_CELL_ROW_COUNT - 2; row++)
            {
                int wallYPos = (row + 1) * CELL_HEIGHT;
                int leftWallXPos = 0;
                int rightWallXPos = (MAP_CELL_COLUMN_COUNT - 1) * CELL_HEIGHT;

                Vector2 leftWallCellPosition = new Vector2(leftWallXPos, wallYPos);
                Vector2 rightWallCellPosition = new Vector2(rightWallXPos, wallYPos);

                _spriteBatch.Draw(_wall, leftWallCellPosition, Color.White);
                _spriteBatch.Draw(_wall, rightWallCellPosition, Color.White);
            }
        }

        /// <summary>
        /// method to determine if the mouse is on the ball
        /// </summary>
        /// <returns></returns>
        private bool MouseOnBall()
        {
            _mouseState = Mouse.GetState();

            if (_mouseState.X < _ballPosition.X) return false;
            if (_mouseState.X > (_ballPosition.X + _ball.Width)) return false;
            if (_mouseState.Y < _ballPosition.Y) return false;
            if (_mouseState.Y > (_ballPosition.Y + _ball.Height)) return false;

            return true;
        }

        // TODO 02a - add a method to move the ball
        /// <summary>
        /// method to move the ball by changing the X and Y coordinates
        /// </summary>
        private void MoveBall()
        {
            // TODO 03b - call method to detect and handle ball/wall collisions
            CollisionBallWall();

            // add coordinate speeds to the ball's position
            _ballPosition.X = _ballPosition.X + _ballXSpeed;
            _ballPosition.Y = _ballPosition.Y + _ballYSpeed;

            // alternate way to set new ball position
            //_ballPosition = new Vector2(_ballPosition.X + _ballXSpeed, _ballPosition.Y + _ballYSpeed);
        }

        // TODO 03a - add a method to detect and handle ball/wall collisions
        /// <summary>
        /// method to "bounce" off the wall
        /// </summary>
        /// <returns></returns>
        private void CollisionBallWall()
        {
            // collision with left or right wall
            if (_ballPosition.X <= CELL_WIDTH || _ballPosition.X >= (MAP_CELL_COLUMN_COUNT - 2) * CELL_WIDTH)
            {
                // reverse speed in x direction
                _ballXSpeed = -_ballXSpeed;
            }
            
            // collision with top or bottom wall
            if (_ballPosition.Y <= CELL_HEIGHT || _ballPosition.Y >= (MAP_CELL_ROW_COUNT - 2) * CELL_HEIGHT)
            {
                // reverse speed in y direction
                _ballYSpeed = -_ballYSpeed;
            }
        }

        // TODO 04a - add a method to re-spawn the ball in a random location
        /// <summary>
        /// method to re-spawn the ball in a new random location
        /// </summary>
        private void SpawnBall()
        {
            // get randomly generated new cell location for the ball
            int newCellRow = (int)randomNumber01.Next(1, MAP_CELL_ROW_COUNT - 2);
            int newCellColumn = (int)randomNumber01.Next(1, MAP_CELL_COLUMN_COUNT - 2);

            // set the ball's position to the new cell
            _ballPosition = new Vector2(newCellColumn * CELL_WIDTH, newCellRow * CELL_HEIGHT);

            // make the ball visible
            _ballVisible = true;
        }

        #endregion
    }
}
