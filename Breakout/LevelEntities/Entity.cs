using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.LevelEntities
{
    public class Entity
    {
        protected Texture2D _sprite;
        public Game1 game;
        protected Vector2 _position;
        protected Vector2 _prevPosition;

        public int width;
        public int height;

        public Entity parent;

        public Entity(Game1 game)
        {
            this.game = game;
        }
        virtual protected void Initialize()
        {
            LoadContent();
        }
        virtual protected void LoadContent()
        {
        }
        virtual protected void ProcessInput()
        {

        }
        public virtual void Update(GameTime gameTime)
        {

        }
        virtual public void Draw(SpriteBatch spriteBatch)
        {

        }
        public void OnIntersection(Entity intersectWith)
        {

        }
        public void SetPosition(Vector2 pos)
        {
            _position = pos;
        }
        public void SetWidth(int width)
        {
            this.width = width;
        }
        public void SetHeight(int height)
        {
            this.height = height;
        }
        virtual public Vector2 Position()
        {
            if (parent != null)
            {
                return _position + parent.Position();
            }
            return _position;
        }
    }
}
