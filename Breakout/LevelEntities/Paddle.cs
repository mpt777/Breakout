using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Tracing;
using Breakout.Physics;

namespace Breakout.LevelEntities
{
    public class Paddle : Entity
    {

        private float _speed = 500f;
        private Vector2 _velocity = new Vector2(0, 0);
        public RectanglePhysicEntity physicsEntity;
        public Ball ball;
        int minX;
        int maxX;

        private int _shrinkToWidth;
        private int _startWidth;
        private TimeSpan _shrinkTime;
        private int startWidth =300;

        public Paddle(Game1 game, Vector2 position) : base(game)
        {
            width = startWidth;
            _shrinkToWidth = startWidth;
            height = 20;
            _position = position;
            _position.X -= width / 2;

            physicsEntity = new RectanglePhysicEntity(game, width, height);
            physicsEntity.parent = this;
            this.Initialize();
        }
        public void SetBounds(int minX, int maxX)
        {
            this.minX = minX;
            this.maxX = maxX;
        }
        override protected void LoadContent()
        {
            _sprite = new Texture2D(game.GraphicsDevice, 1, 1);
            _sprite.SetData(new[] { Color.White });
        }
        override protected void ProcessInput()
        {
            _velocity.X = 0;
            if (game.keyboard.IsKeyDown(Keys.Left))
            {
                _velocity.X = -_speed;
            }
            if (game.keyboard.IsKeyDown(Keys.Right))
            {
                _velocity.X = _speed;
            }
            //if (ball != null)
            //{
            //    if (game.keyboard.IsKeyDown(Keys.Space))
            //    {
            //        LaunchBall();
            //    }
            //}
        }
        public Vector2 LaunchBallPosition(Ball ball)
        {
            return new Vector2(_position.X + width / 2 - ball.radius, _position.Y - ball.radius * 2 - 1);
        }
        public void LaunchBall()
        {
            ball = null;
        }
        public void StartShrinking(int width=0)
        {
            this._startWidth = this.width;
            this._shrinkToWidth = width;
            this._shrinkTime = new TimeSpan(0, 0, 1);
        }
        public void Shrinking(GameTime gameTime)
        {
            int size = (int)((this._startWidth - this._shrinkToWidth) / this._shrinkTime.TotalSeconds * gameTime.ElapsedGameTime.TotalSeconds);
            this.width -= size;
            this.physicsEntity.shape.Width = this.width;
            this._position.X += size / 2;
        }
        public void ProcessShrink(GameTime gameTime)
        {
            if (this._shrinkToWidth < this.width)
            {
                this.Shrinking(gameTime);
            }
        }
        override public void Update(GameTime gameTime)
        {
            _prevPosition = _position;

            this.ProcessShrink(gameTime);

            if (ball != null)
            {
                ball.SetPosition(this.LaunchBallPosition(ball));
            }

            ProcessInput();
            _position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            ProcessCollision();

        }
        public void ProcessCollision()
        {
            _position = Vector2.Clamp(_position, new Vector2(minX, Position().Y), new Vector2(maxX - width, Position().Y));
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, new Rectangle((int)_position.X, (int)_position.Y, width, height), Color.SandyBrown);
        }
    }
}
