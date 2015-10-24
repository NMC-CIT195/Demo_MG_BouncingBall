using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Demo_MG_BouncingBall
{
    /// <summary>
    /// initial layout of game including walls and a ball
    /// </summary>
    public class BouncingBall : Game
    {
        // TODO 01a - set the cell size in pixels
        private const int CELL_WIDTH = 64;
        private const int CELL_HEIGHT = 64;

        // TODO 01b - set the map size in cells
        private const int MAP_CELL_ROW_COUNT = 8;
        private const int MAP_CELL_COLUMN_COUNT = 8;

        // TODO 03b - declare instance variables for for the sprites
        private Texture2D _ball;
        private Texture2D _wall;
        private Vector2 _ballPosition;




        // TODO 02a - declare a spriteBatch object
        private SpriteBatch _spriteBatch;

        private GraphicsDeviceManager _graphics;
        /// <summary>
        /// game constructor intitializes the major game objects
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
        /// Allows the game to perform any initialization it needs to before starting to run.
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
        /// LoadContent will be called once per game and is the place to load
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
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO 04b - draw the sprites on the screen using the spriteBatch object
            _spriteBatch.Begin();

            _spriteBatch.Draw(_ball, _ballPosition, Color.White);

            BuildMap();
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #region HELPER METHODS

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
                int wallYPos = (row + 1) * MAP_CELL_COLUMN_COUNT;
                int leftWallXPos = 0;
                int rightWallXPos = (MAP_CELL_COLUMN_COUNT - 1) * MAP_CELL_ROW_COUNT;

                Vector2 leftWallCellPosition = new Vector2();
                Vector2 rightWallCellPosition = new Vector2();

                _spriteBatch.Draw(_wall, new Vector2(leftWallXPos, wallYPos), Color.White);
                _spriteBatch.Draw(_wall, new Vector2(rightWallXPos, wallYPos), Color.White);
            }

        }

        #endregion
    }
}
