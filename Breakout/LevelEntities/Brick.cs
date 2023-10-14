using Breakout.Particles;
using Breakout.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.LevelEntities
{
    public class Brick : Entity
    {
        public int points = 0;
        public Color color;
        public Row row = null;
        public RectanglePhysicEntity physicsEntity;

        public Brick(Game1 game, Vector2 position, int width, int height, int points = 0, Color? color = null) : base(game)
        {
            _position = position;
            this.width = width;
            this.height = height;
            this.points = points;
            this.color = color != null ? (Color)color : Color.White;

            physicsEntity = new RectanglePhysicEntity(game, this.width, this.height);
            physicsEntity.parent = this;
            this.Initialize();
        }
        override protected void Initialize()
        {
            LoadContent();
        }
        override protected void LoadContent()
        {
            _sprite = new Texture2D(game.GraphicsDevice, 1, 1);
            _sprite.SetData(new[] { this.color });

            //this._sprite = this.game.Content.Load<Texture2D>("hawaii new");
        }
        override protected void ProcessInput()
        {
            
        }
        override public void Update(GameTime gameTime)
        { 
        }
        public ParticleEmitter DestroyedEmitter()
        {
            Vector2 pos = this.Position();
            ParticleEmitter emitter = new ParticleEmitter(
                this.game.Content,
                    new TimeSpan(0, 0, 0, 0, 2),
                    new Rectangle((int)pos.X, (int)pos.Y, this.width, this.height),
                    5,
                    5,
                    new TimeSpan(0, 0, 0, 0, 600),
                    _sprite,
                    new TimeSpan(0, 0, 0, 0, 100)
                 ); ;
            emitter.Gravity = new Vector2(0, 0.1f);
            return emitter;
        }
        override public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 pos = Position();
            spriteBatch.Draw(_sprite, new Rectangle((int)pos.X, (int)pos.Y, width, height), Color.White);
        }

    }
}
