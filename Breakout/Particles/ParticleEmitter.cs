﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Particles
{
    public class ParticleEmitter
    {

        private Dictionary<int, Particle> m_particles = new Dictionary<int, Particle>();
        private Texture2D _texture;
        private TimeSpan _emittingTime;
        private Rectangle _rectangle;
        private MyRandom m_random = new MyRandom();

        private TimeSpan _rate;
        private int _sarticleSize;
        private int _speed;
        private TimeSpan _lifetime;

        public Vector2 Gravity { get; set; }

        public ParticleEmitter(ContentManager content, TimeSpan rate, Rectangle rect, int size, int speed, TimeSpan lifetime, Texture2D tex, TimeSpan emittingTime)
        {
            _rate = rate;
            _rectangle = rect;
            _sarticleSize = size;
            _speed = speed;
            _lifetime = lifetime;
            _texture = tex;
            _emittingTime = emittingTime;

            Gravity = new Vector2(0, 0);
        }
        public int ParticleCount
        {
            get { return m_particles.Count; }
        }

        private TimeSpan m_accumulated = TimeSpan.Zero;
        private TimeSpan _totalAccumulated = TimeSpan.Zero;

        /// <summary>
        /// Generates new particles, updates the state of existing ones and retires expired particles.
        /// </summary>
        /// 
        public void CreateParticles()
        {
            while (m_accumulated > _rate)
            {
                m_accumulated -= _rate;

                float sourceX = m_random.nextRange(this._rectangle.X, this._rectangle.X + this._rectangle.Width);
                float sourceY = m_random.nextRange(this._rectangle.Y, this._rectangle.Y + this._rectangle.Height);
                Particle p = new Particle(
                    m_random.Next(),
                    new Vector2(sourceX, sourceY),
                    m_random.nextCircleVector(),
                    (float)(Physics.Physics.GetDistance(sourceX, sourceY, this._rectangle.Center.X, this._rectangle.Center.Y) / Math.Max(this._rectangle.Width, this._rectangle.Height)) * _speed,
                    _lifetime);

                if (!m_particles.ContainsKey(p.name))
                {
                    m_particles.Add(p.name, p);
                }

                //(float)m_random.nextGaussian(_speed, 1),
            }
        }
        public void Update(GameTime gameTime)
        {
            //
            // Generate particles at the specified rate
            m_accumulated += gameTime.ElapsedGameTime;
            _totalAccumulated += gameTime.ElapsedGameTime;
               
            if (_totalAccumulated <= this._emittingTime)
            {
                this.CreateParticles();
            }

            //
            // For any existing particles, update them, if we find ones that have expired, add them
            // to the remove list.
            List<int> removeMe = new List<int>();
            foreach (Particle p in m_particles.Values)
            {
                p.lifetime -= gameTime.ElapsedGameTime;
                if (p.lifetime < TimeSpan.Zero)
                {
                    //
                    // Add to the remove list
                    removeMe.Add(p.name);
                }
                //
                // Update its position
                p.position += p.direction * p.speed;
                //
                // Have it rotate proportional to its speed
                p.rotation += p.speed / 50.0f;
                //
                // Apply some gravity
                p.direction += Gravity;
            }

            //
            // Remove any expired particles
            foreach (int Key in removeMe)
            {
                m_particles.Remove(Key);
            }
        }

        /// <summary>
        /// Renders the active particles
        /// </summary>
        /// 
        public bool Finished()
        {
            return (_totalAccumulated <= this._emittingTime) && (m_particles.Count == 0);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle(0, 0, _sarticleSize, _sarticleSize);
            foreach (Particle p in m_particles.Values)
            {

                r.X = (int)p.position.X;
                r.Y = (int)p.position.Y;
                spriteBatch.Draw(
                    _texture,
                    r,
                    null,
                    Color.White,
                    p.rotation,
                    new Vector2(_texture.Width / 2, _texture.Height / 2),
                    SpriteEffects.None,
                    0);
            }
        }
    }
}
