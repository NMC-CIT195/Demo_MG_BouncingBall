using System;
using System.Runtime.InteropServices;
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

        // declare instance variables for the sprites
        // declare instance variables for the background
        private Texture2D _background;
        private Rectangle _backgroundPosition;

        private string _ballSpriteName;
        private Vector2 _ballPosition;
        private Ball _ball;

        private string _wallSpriteName;
        private Vector2 _wallPosition;
        private Wall _wall;
       
        // declare a spriteBatch object
        private SpriteBatch _spriteBatch;

        private GraphicsDeviceManager _graphics;

        /// <summary>
        /// constructor initializes the game objects
        /// </summary>
        public ClickBall()
        {
            _graphics = new GraphicsDeviceManager(this);

            // set the window size as a function of cell size and cell count
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
            _backgroundPosition = new Rectangle(0, 0, 640, 480);    

            // set the ball's initial values
            _ballPosition.X = 100;
            _ballPosition.Y = 200;
            _ballSpriteName = "Ball";

            // set the wall's initial values
            _wallSpriteName = "Wall";

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
            _background = Content.Load<Texture2D>("BackgroundSandyStained");

            // instantiate a Ball and Wall
            _ball = new Ball(Content, _ballSpriteName, _ballPosition);
            _wall = new Wall(Content, _wallSpriteName, _wallPosition);

            //
            _ball.Draw(_spriteBatch);
            _wall.Draw(_spriteBatch);
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
                MessageBox(new IntPtr(0), "Escape key pressed. Click OK to exit.", "Debug Message", 0);
                Exit();
            }

            _ball.Active = true;

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

            // draw the background and the ball
            _spriteBatch.Draw(_background, _backgroundPosition, Color.White);
            _ball.Draw(_spriteBatch);

            // call the BuildMap method to add all of the walls
            BuildMap();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #region HELPER METHODS

        // TODO 05a - add a method to draw the wall sprites
        // method to draw the wall sprites
        /// <summary>
        /// method to add the starting walls to the map
        /// </summary>
        private void BuildMap()
        {
            // Draw top and bottom walls
            for (int column = 0; column < MAP_CELL_COLUMN_COUNT; column++)
            {
                int wallCellXPos = column * CELL_WIDTH;
                int topWallYPos = 0;
                int bottomWallYPos = (MAP_CELL_ROW_COUNT - 1) * CELL_HEIGHT;

                Vector2 topWallCellPosition = new Vector2(wallCellXPos, topWallYPos);
                Vector2 bottonWallCellPosition = new Vector2(wallCellXPos, bottomWallYPos);

                Wall topWallSection = new Wall(Content, _wallSpriteName, topWallCellPosition);
                topWallSection.Active = true;
                topWallSection.Draw(_spriteBatch);

                Wall bottomWallSection = new Wall(Content, _wallSpriteName, bottonWallCellPosition);
                bottomWallSection.Active = true;
                bottomWallSection.Draw(_spriteBatch);
            }

            // Draw side walls
            for (int row = 0; row < MAP_CELL_ROW_COUNT - 2; row++)
            {
                int wallYPos = (row + 1) * CELL_HEIGHT;
                int leftWallXPos = 0;
                int rightWallXPos = (MAP_CELL_COLUMN_COUNT - 1) * CELL_HEIGHT;

                Vector2 leftWallCellPosition = new Vector2(leftWallXPos, wallYPos);
                Vector2 rightWallCellPosition = new Vector2(rightWallXPos, wallYPos);

                Wall leftWallSection = new Wall(Content, _wallSpriteName, leftWallCellPosition);
                leftWallSection.Active = true;
                leftWallSection.Draw(_spriteBatch);

                Wall rightWallSection = new Wall(Content, _wallSpriteName, rightWallCellPosition);
                rightWallSection.Active = true;
                rightWallSection.Draw(_spriteBatch);
            }
        }

        #endregion
    }
}
