using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System; // TODO 00b - add to allow Windows message box
using System.Runtime.InteropServices; // TODO 00c - add to allow Windows message box

namespace Demo_MG_ClickBall.Sprint1
{
    /// <summary>
    /// initial layout of game including walls and a ball
    /// </summary>
    public class BouncingBall : Game
    {
        // TODO 00a - add code to allow Windows message boxes when running in a Windows environment
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        // TODO 01a - set the cell size in pixels
        private const int CELL_WIDTH = 64;
        private const int CELL_HEIGHT = 64;

        // TODO 01b - set the map size in cells
        private const int MAP_CELL_ROW_COUNT = 8;
        private const int MAP_CELL_COLUMN_COUNT = 8;

        // TODO 03b - declare instance variables for the sprites
        private Texture2D _ball;
        private Texture2D _wall;
        private Vector2 _ballPosition;

        // TODO 02a - declare a spriteBatch object
        private SpriteBatch _spriteBatch;

        private GraphicsDeviceManager _graphics;

        /// <summary>
        /// constructor initializes the game objects
        /// </summary>
        public BouncingBall()
        {
            _graphics = new GraphicsDeviceManager(this);

            // TODO 01c - set the window size as a function of cell size and cell count
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
            // TODO 04a - set the ball's initial position
            _ballPosition.X = 100;
            _ballPosition.Y = 200;

            base.Initialize();
        }

        /// <summary>
        /// method will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO 02b - instantiate the spriteBatch object
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO 03a - add sprite files to Content folder 
            // TODO 03c - load images into the game
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
            // TODO 06a - detect an Escape key press to end the game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                // TODO 06b -demonstrate the use of a Window's message box to display information
                MessageBox(new IntPtr(0), "Escape key pressed. Click OK to exit.", "Debug Message", 0);
                Exit();
            }

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

            // TODO 04b - draw the ball on the screen using the spriteBatch object
            _spriteBatch.Draw(_ball, _ballPosition, Color.White);

            // TODO 05b - call the method to draw the wall sprites
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

                _spriteBatch.Draw(_wall, topWallCellPosition, Color.White);
                _spriteBatch.Draw(_wall, bottonWallCellPosition, Color.White);
            }

            // Draw side walls
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

        #endregion
    }
}
