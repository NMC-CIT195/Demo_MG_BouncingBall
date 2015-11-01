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
        // declare instance variables for the background
        private Texture2D _background;
        private Rectangle _backgroundPosition;

        // declare instance variables for the sprites
        private Ball _ball;

        // declare a spriteBatch object
        private SpriteBatch _spriteBatch;

        private GraphicsDeviceManager _graphics;

        /// <summary>
        /// constructor initializes the game objects
        /// </summary>
        public ClickBall()
        {
            _graphics = new GraphicsDeviceManager(this);

            // set the window size 
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 480;

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

            // create a ball object
            string spriteName = "Ball";
            Vector2 position = new Vector2(300, 200);
            Vector2 velocity = new Vector2(2, 2);
            _ball = new Ball(Content, spriteName, position, velocity);

            // _ball = new Ball(Content, "Ball", new Vector2(300, 200), new Vector2( 2, 2));

            // make the ball active
            _ball.Active = true;

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

            // draw the background and the ball
            _spriteBatch.Draw(_background, _backgroundPosition, Color.White);
            _ball.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #region HELPER METHODS



        #endregion
    }
}
