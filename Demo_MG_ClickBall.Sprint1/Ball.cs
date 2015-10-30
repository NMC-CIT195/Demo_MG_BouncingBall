using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo_MG_ClickBall
{
    public class Ball
    {
        #region ENUMERABLES



        #endregion


        #region FIELDS

        private ContentManager _contentManager;
        private Texture2D _ballSprite;
        private Vector2 _ballPosition;

        #endregion

        #region PROPERTIES

        public ContentManager ContentManager
        {
            get { return _contentManager; }
            set { _contentManager = value; }
        }

        public Texture2D BallSprint
        {
            get { return _ballSprite; }
            set { _ballSprite = value; }
        }

        public Vector2 BallPosition
        {
            get { return _ballPosition; }
            set { _ballPosition = value; }
        }

        #endregion

        #region CONSTRUCTORS

        public Ball(
            ContentManager contentManager,
            Texture2D spriteName,
            Vector2 ballPositiion
            )
        {
            _contentManager = contentManager;
            _ballSprite = spriteName;
            _ballPosition = ballPositiion;
        }

        #endregion

        #region METHODS



        #endregion
    }
}
