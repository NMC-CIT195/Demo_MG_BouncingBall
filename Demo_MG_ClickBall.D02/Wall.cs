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
    public class Wall
    {

        #region FIELDS

        private ContentManager _contentManager;
        private string _wallSpriteName;
        private Texture2D _wallSprite;
        private Vector2 _wallPosition;
        private bool _active;

        #endregion

        #region PROPERTIES

        public ContentManager ContentManager
        {
            get { return _contentManager; }
            set { _contentManager = value; }
        }

        public string WallSpriteName
        {
            get { return _wallSpriteName; }
            set { _wallSpriteName = value; }
        }

        public Vector2 WallPosition
        {
            get { return _wallPosition; }
            set { _wallPosition = value; }
        }

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// instantiate a new Wall
        /// </summary>
        /// <param name="contentManager">game content manager object</param>
        /// <param name="WallSpriteName">file name of sprite</param>
        /// <param name="WallPositiion">vector position of Wall</param>
        public Wall(
            ContentManager contentManager,
            string WallSpriteName,
            Vector2 WallPositiion
            )
        {
            _contentManager = contentManager;
            _wallSpriteName = WallSpriteName;
            _wallPosition = WallPositiion;

            // load the Wall image into the Texture2D for the Wall sprite
            _wallSprite = _contentManager.Load<Texture2D>(_wallSpriteName);
        }

        #endregion

        #region METHODS
        /// <summary>
        /// add Wall sprite to the SpriteBatch object
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // only draw the Wall if it is active
            if (_active)
            {
                spriteBatch.Draw(_wallSprite, _wallPosition, Color.White);
            }
        }

        #endregion

    }
}
