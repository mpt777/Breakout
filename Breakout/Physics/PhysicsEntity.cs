using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Breakout.LevelEntities;

namespace Breakout.Physics
{
    abstract public class PhysicsEntity : Entity
    {
        public Vector2 velocity = new Vector2(0, 0);
        public Rectangle rectangle;
        public Circle circle;
        public PhysicsEntity(Game1 game) : base(game)
        {

        }
    }
}
