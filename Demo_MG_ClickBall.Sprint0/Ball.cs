using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Demo_MG_ClickBall
{
    public class Ball
    {

        #region FIELDS

        private ContentManager _contentManager;
        private string _ballSpriteName;
        private Texture2D _ballSprite;
        private Vector2 _ballPosition;
        private bool _active;

        #endregion

        #region PROPERTIES

        public ContentManager ContentManager
        {
            get { return _contentManager; }
            set { _contentManager = value; }
        }

        public string ballSpriteName
        {
            get { return _ballSpriteName; }
            set { _ballSpriteName = value; }
        }

        public Vector2 BallPosition
        {
            get { return _ballPosition; }
            set { _ballPosition = value; }
        }

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// instantiate a new ball
        /// </summary>
        /// <param name="contentManager">game content manager object</param>
        /// <param name="ballSpriteName">file name of sprite</param>
        /// <param name="ballPositiion">vector position of ball</param>
        public Ball(
            ContentManager contentManager,
            string ballSpriteName,
            Vector2 ballPositiion
            )
        {
            _contentManager = contentManager;
            _ballSpriteName = ballSpriteName;
            _ballPosition = ballPositiion;
            
            // load the ball image into the Texture2D for the ball sprite
            _ballSprite = _contentManager.Load<Texture2D>(_ballSpriteName);
        }

        #endregion

        #region METHODS
        /// <summary>
        /// add ball sprite to the SpriteBatch object
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // only draw the ball if it is active
            if (_active)
            {
                spriteBatch.Draw(_ballSprite, _ballPosition, Color.White);
            }
        }
        
        #endregion


    }
}
