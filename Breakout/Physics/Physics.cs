using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Breakout.LevelEntities;

namespace Breakout.Physics
{
    public class Physics
    {
        public static double GetDistance(float x1, float y1, float x2, float y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }
        public static bool Intersection(RectanglePhysicEntity rect1, RectanglePhysicEntity rect2)
        {
            bool intersects = false;
            rect1.shape.X += (int)rect1.Position().X;
            rect1.shape.Y += (int)rect1.Position().Y;

            rect2.shape.X += (int)rect2.Position().X;
            rect2.shape.Y += (int)rect2.Position().Y;

            if (rect1.shape.Intersects(rect2.shape))
            {
                intersects = true;
            }

            rect1.shape.X -= (int)rect1.Position().X;
            rect1.shape.Y -= (int)rect1.Position().Y;

            rect2.shape.X -= (int)rect2.Position().X;
            rect2.shape.Y -= (int)rect2.Position().Y;

            return intersects;
        }
        public static bool Intersection(CirclePhysicsEntity circle, RectanglePhysicEntity rect)
        {
            return Intersection(rect, circle);
        }
        public static bool Intersection(RectanglePhysicEntity rectEnt, CirclePhysicsEntity circleEnt)
        {
            //return rectEnt.bounds.Contains(circleEnt.bounds) != ContainmentType.Disjoint;

            Rectangle rect = rectEnt.shape;
            Circle circle = circleEnt.shape;

            Vector2 circleDistance = new Vector2(0, 0);

            circleDistance.X = Math.Abs(circleEnt.GlobalCenter().X - rectEnt.GlobalCenter().X);
            circleDistance.Y = Math.Abs(circleEnt.GlobalCenter().Y - rectEnt.GlobalCenter().Y);

            if (circleDistance.X > rect.Width / 2 + circle.radius) { return false; }
            if (circleDistance.Y > rect.Height / 2 + circle.radius) { return false; }

            if (circleDistance.X <= rect.Width / 2) { return true; }
            if (circleDistance.Y <= rect.Height / 2) { return true; }

            float cornerDistance = (circleDistance.X - rect.Width / 2) * (circleDistance.X - rect.Width / 2) +
                                 (circleDistance.Y - rect.Height / 2) * (circleDistance.Y - rect.Height / 2);

            return cornerDistance <= circle.radius * circle.radius;
        }

        public static Vector2 Direction(RectanglePhysicEntity rectEnt, CirclePhysicsEntity circleEnt)
        {
            return Direction(circleEnt, rectEnt) * new Vector2(-1, -1);
        }

        public static bool IntersectionHelper(Vector2 pointA, Vector2 pointB, Vector2 pointC)
        {
            return (pointC.Y - pointA.Y) * (pointB.X - pointA.X) > (pointB.Y - pointA.Y) * (pointC.X - pointA.X);
        }

        public static bool LineIntersection(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD)
        {
            return IntersectionHelper(pointA, pointC, pointD) != IntersectionHelper(pointB, pointC, pointD) && IntersectionHelper(pointA, pointB, pointC) != IntersectionHelper(pointA, pointB, pointD);
        }

        public static Vector2 BounceVector(CirclePhysicsEntity circleEnt, RectanglePhysicEntity rectEnt)
        {
            float range = rectEnt.shape.Width;
            float center = rectEnt.GlobalCenter().X;

            float velocity = (float)Math.Sqrt(circleEnt.velocity.X * circleEnt.velocity.X + circleEnt.velocity.Y * circleEnt.velocity.Y);
            float ratio = (circleEnt.GlobalCenter().X - center) / (range / 2);
            const float factor = 0.75f;

            Vector2 vec = new Vector2(0, 0);
            vec.X = velocity * ratio * factor;
            vec.Y = (float)Math.Sqrt(velocity * velocity - vec.X * vec.X) * (circleEnt.velocity.Y > 0 ? -1 : 1);
            return vec;
        }

        public static Vector2 Direction(CirclePhysicsEntity circleEnt, RectanglePhysicEntity rectEnt)
        {
            Vector2 circleCenter = circleEnt.GlobalCenter();
            Vector2 rectCenter = rectEnt.GlobalCenter();

            Vector2 rectTopLeft = rectEnt.GlobalTopLeft();
            Vector2 rectTopRight = rectEnt.GlobalTopRight();
            Vector2 rectBottomRight = rectEnt.GlobalBottomRight();
            Vector2 rectBottomLeft = rectEnt.GlobalBottomLeft();

            if (LineIntersection(circleCenter, rectCenter, rectTopLeft, rectTopRight))
            {
                return new Vector2(0, 1);
            }

            if (LineIntersection(circleCenter, rectCenter, rectBottomRight, rectTopRight))
            {
                return new Vector2(-1, 0);
            }

            if (LineIntersection(circleCenter, rectCenter, rectBottomRight, rectBottomLeft))
            {
                return new Vector2(0, -1);
            }

            if (LineIntersection(circleCenter, rectCenter, rectBottomLeft, rectTopLeft))
            {
                return new Vector2(1, 0);
            }
            return new Vector2(0, 0);
        }

        public static Vector2 Direction(RectanglePhysicEntity circleEnt, RectanglePhysicEntity rectEnt)
        {

            Vector2 circleCenter = circleEnt.GlobalCenter();
            Vector2 rectCenter = rectEnt.GlobalCenter();

            Vector2 rectTopLeft = rectEnt.GlobalTopLeft();
            Vector2 rectTopRight = rectEnt.GlobalTopRight();
            Vector2 rectBottomRight = rectEnt.GlobalBottomRight();
            Vector2 rectBottomLeft = rectEnt.GlobalBottomLeft();


            if (LineIntersection(circleCenter, rectCenter, rectTopLeft, rectTopRight))
            {
                return new Vector2(0, 1);
            }

            if (LineIntersection(circleCenter, rectCenter, rectBottomRight, rectTopRight))
            {
                return new Vector2(-1, 0);
            }

            if (LineIntersection(circleCenter, rectCenter, rectBottomRight, rectBottomLeft))
            {
                return new Vector2(0, -1);
            }

            if (LineIntersection(circleCenter, rectCenter, rectBottomLeft, rectTopLeft))
            {
                return new Vector2(1, 0);
            }
            return new Vector2(0, 0);
        }

        public static Vector2 Reflect(Vector2 direction)
        {
            //direction *= new Vector2(-1, -1);
            direction.X = direction.X == 0 ? 1 : -1;
            direction.Y = direction.Y == 0 ? 1 : -1;

            return direction;
        }
    }
}
