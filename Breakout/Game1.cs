using Breakout.State;
using Breakout.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Breakout
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public CustomKeyboard keyboard;
        public Persistance persistance;

        public int width = 1280;
        public int height = 720;

        public List<GameState> highScores = new List<GameState>();
        public Stack<GameFrame> frames = new Stack<GameFrame>();
        public Stack<GameFrame> framesToAdd = new Stack<GameFrame>();

        private Texture2D _texture;
        public Level level;

        private int _shouldPopFrame = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.keyboard = new CustomKeyboard();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            this.persistance = new Persistance();
            this.frames.Push(new MainMenu(this));

            this.persistance.LoadGameState();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            this._texture = Content.Load<Texture2D>("Images/dark_hawaii");

            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();

            // TODO: use this.Content to load your game content here
        }
        public void AddHighScore(GameState highScore)
        {
            this.highScores = this.persistance.states;
            this.highScores.Add(highScore);
            this.persistance.SaveGameState(this.highScores);
        }
        public void StartGame()
        {
            this.framesToAdd.Push(new Level(this));
        }
        public void AddFrame(GameFrame frame)
        {
            this.framesToAdd.Push(frame);
        }
        public void RemoveFrame()
        {
            this._shouldPopFrame += 1;
        }
        private void ProcessFrames(GameTime gameTime)
        {
            while(this._shouldPopFrame > 0)
            {
                this.frames.Pop();
                this.frames.Peek().active = true;
                this.frames.Peek().paused = false;
                this._shouldPopFrame -= 1;

            }

            foreach (GameFrame frame in framesToAdd)
            {
                this.frames.Push(frame);
            }
            this.framesToAdd.Clear();


            foreach (GameFrame frame in frames)
            {
                if (frame.active && !frame.paused)
                {
                    frame.Update(gameTime);
                }
            }

        }
        protected override void Update(GameTime gameTime)
        {


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) //  || Keyboard.GetState().IsKeyDown(Keys.Escape)
                Exit();


            // TODO: Add your update logic here

            this.keyboard.GetKeyboardState();

            this.ProcessFrames(gameTime);


            //base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            this._spriteBatch.Begin();
            this._spriteBatch.Draw(this._texture, new Rectangle(0, 0, width, height), Color.White);
            foreach (GameFrame frame in frames.Reverse())
            {
                if (frame.active) 
                {
                    frame.Draw(this._spriteBatch);
                }
            }
            base.Draw(gameTime);

            this._spriteBatch.End();
            
        }
    }
}