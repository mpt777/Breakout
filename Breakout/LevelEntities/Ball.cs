using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

using Breakout.Physics;
using Breakout.LevelEntities;
using Breakout.Views;

namespace Breakout.LevelEntities
{
    public class Ball : Entity
    {
        public Level level;
        public int radius = 10;
        public CirclePhysicsEntity physicsEntity;
        public PhysicsEntity previousPhysicsEntity;
        private Vector2 _velocityModifier = new Vector2(1, 1);
        private Vector2 _velocity = new Vector2(10, -400);
        private int _bricksBroken = 0;

        public Ball(Game1 game, Level level) : base(game)
        {
            this.level = level;
            _prevPosition = _position;
            physicsEntity = new CirclePhysicsEntity(game, radius);
            physicsEntity.parent = this;
            physicsEntity.velocity = _velocity;
            this.Initialize();
        }
        protected override void LoadContent()
        {
            //this._sprite = new Texture2D(this.game.GraphicsDevice, 1, 1);
            //this._sprite.SetData(new[] { Color.White });
            _sprite = game.Content.Load<Texture2D>("Images/beachball");
        }

        override public void Update(GameTime gameTime)
        {
            ProcessInput();
            _position += Vector2.Multiply(physicsEntity.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds, this._velocityModifier);
            _prevPosition = _position;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, new Rectangle((int)_position.X, (int)_position.Y, radius * 2, radius * 2), Color.White);
        }
        public void BreakBrick()
        {
            _bricksBroken += 1;
            this.SetVelocityModifier();
        }
        public void SetVelocityModifier()
        {
            float modifier = 1.0f;
            if (_bricksBroken < 4)
            {
                modifier = 1.0f;
            }
            else if (_bricksBroken < 12)
            {
                modifier = 1.1f;
            }
            else if (_bricksBroken < 36)
            {
                modifier = 1.2f;
            }
            else if (_bricksBroken < 62)
            {
                modifier = 1.3f;
            }
            else
            {
                modifier = 1.4f;
            }
            this._velocityModifier = new Vector2(modifier, modifier);


            //this._ballScore += score;
            //float doubleRate = 100f;
            //float modifier = 1 + (this._ballScore / doubleRate);
            //this._velocityModifier = new Vector2(modifier, modifier);
        }
        public void Reflect(RectanglePhysicEntity physicsEntity)
        {
            if (physicsEntity != previousPhysicsEntity)
            {
                Vector2 direction = Physics.Physics.Direction(physicsEntity, this.physicsEntity);
                this.physicsEntity.velocity *= Physics.Physics.Reflect(direction);
                previousPhysicsEntity = physicsEntity;
            }
        }
        public void SetBounce(RectanglePhysicEntity physicsEntity)
        {
            if (physicsEntity != previousPhysicsEntity)
            {
                this.physicsEntity.velocity = Physics.Physics.BounceVector(this.physicsEntity, physicsEntity);
                previousPhysicsEntity = physicsEntity;
            }

        }
    }
}
