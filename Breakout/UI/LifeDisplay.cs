using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Breakout.LevelEntities;

namespace Breakout.UI
{
    public class LifeDisplay : Entity
    {
        public LifeDisplay(Game1 game, Vector2 position) : base(game)
        {
            width = 20;
            height = 20;
            _position = position;
            this.Initialize();
        }
        override protected void LoadContent()
        {
            _sprite = new Texture2D(game.GraphicsDevice, 1, 1);
            _sprite.SetData(new[] { Color.White });
        }
        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, new Rectangle((int)Position().X, (int)Position().Y, width, height), Color.SandyBrown);
        }
    }
}
