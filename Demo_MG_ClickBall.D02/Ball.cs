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
        private string _spriteName;
        private Texture2D _sprite;
        private Vector2 _position;
        private Vector2 _velocity;
        private bool _active;

        #endregion

        #region PROPERTIES

        public ContentManager ContentManager
        {
            get { return _contentManager; }
            set { _contentManager = value; }
        }

        public string SpriteName
        {
            get { return _spriteName; }
            set { _spriteName = value; }
        }

        public Texture2D Sprite
        {
            get { return _sprite; }
            set { _sprite = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
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
        /// <param name="spriteName">file name of sprite</param>
        /// <param name="positiion">vector position of ball</param>
        public Ball(
            ContentManager contentManager,
            string spriteName,
            Vector2 positiion,
            Vector2 velocity
            )
        {
            _contentManager = contentManager;
            _spriteName = spriteName;
            _position = positiion;
            _velocity = velocity;

            // load the ball image into the Texture2D for the ball sprite
            _sprite = _contentManager.Load<Texture2D>(_spriteName);
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
                spriteBatch.Draw(_sprite, _position, Color.White);
            }
        }


        #endregion

    }
}
