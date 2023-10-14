using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Breakout.Particles;

namespace Breakout.LevelEntities
{
    public class Row : Entity
    {
        public Color color;
        public int points;
        public List<ParticleEmitter> emitters = new List<ParticleEmitter>();
        public Row(Game1 game, Color color, int points) : base(game)
        {
            this.color = color;
            this.points = points;
        }
        public List<Brick> bricks = new List<Brick>();

        public void Add(Brick brick)
        {
            brick.row = this;
            brick.parent = this;
            bricks.Add(brick);
        }

        public void Remove(Brick brick)
        {
            bricks.Remove(brick);
            emitters.Add(brick.DestroyedEmitter());
        }
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < bricks.Count; i++)
            {
                bricks[i].Update(gameTime);
            }
            for (int i = 0; i < emitters.Count; i++)
            {
                emitters[i].Update(gameTime);
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < bricks.Count; i++)
            {
                bricks[i].Draw(spriteBatch);
            }
            for (int i = 0; i < emitters.Count; i++)
            {
                emitters[i].Draw(spriteBatch);
            }
        }
    }
}
