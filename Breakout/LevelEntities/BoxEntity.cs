using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.LevelEntities
{
    public class BoxEntity : Entity
    {
        public Rectangle rect;
        public BoxEntity(Game1 game, Vector2 position, int width, int height) : base(game)
        {
            _position = position;
            this.width = width;
            this.height = height;
            rect = new Rectangle(0, 0, width, height);
        }
    }
}
