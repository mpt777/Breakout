using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Physics
{
    public class RectanglePhysicEntity : PhysicsEntity
    {
        public Rectangle shape;
        public BoundingBox bounds;

        public RectanglePhysicEntity(Game1 game, int width, int height) : base(game)
        {
            shape = new Rectangle(0, 0, width, height);
            bounds = new BoundingBox(new Vector3(0, 0, 0), new Vector3(width, height, 0));
        }

        public Vector2 GlobalCenter()
        {
            Vector2 pos = Position();
            return new Vector2(pos.X + shape.Width / 2, pos.Y + shape.Height / 2);
        }

        public float TopBound()
        {
            return _position.Y;
        }
        public float RightBound()
        {
            return _position.X + shape.Width;
        }
        public float BottomBound()
        {
            return _position.Y + shape.Height;
        }
        public float LeftBound()
        {
            return _position.X;
        }

        public Vector2 GlobalTopLeft()
        {
            return Position();
        }
        public Vector2 GlobalTopRight()
        {
            return Position() + new Vector2(shape.Width, 0);
        }
        public Vector2 GlobalBottomRight()
        {
            return Position() + new Vector2(shape.Width, shape.Height);
        }
        public Vector2 GlobalBottomLeft()
        {
            return Position() + new Vector2(0, shape.Height);
        }
    }
}
