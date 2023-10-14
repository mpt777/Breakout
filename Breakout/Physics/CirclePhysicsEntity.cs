using Breakout.LevelEntities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Physics
{
    public class CirclePhysicsEntity : PhysicsEntity
    {
        public Circle shape;
        public BoundingSphere bounds;
        public CirclePhysicsEntity(Game1 game, float radius) : base(game)
        {
            shape = new Circle(new Vector2(0, 0), radius);
            bounds = new BoundingSphere(new Vector3(0, 0, 0), radius);
        }

        public Vector2 GlobalCenter()
        {
            return Position() + new Vector2(shape.radius, shape.radius);
        }

        public float TopBound()
        {
            return _position.Y - shape.radius;
        }
        public float RightBound()
        {
            return _position.X + shape.radius;
        }
        public float BottomBound()
        {
            return _position.Y + shape.radius;
        }
        public float LeftBound()
        {
            return _position.X - shape.radius;
        }

        public float GlobalTopBound()
        {
            Vector2 pos = Position();
            return pos.Y - shape.radius;
        }
        public float GlobalRightBound()
        {
            Vector2 pos = Position();
            return pos.X + shape.radius;
        }
        public float GlobalBottomBound()
        {
            Vector2 pos = Position();
            return pos.Y + shape.radius;
        }
        public float GlobalLeftBound()
        {
            Vector2 pos = Position();
            return pos.X - shape.radius;
        }

    }
}
