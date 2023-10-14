using Breakout.LevelEntities;
using Breakout.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Views
{
    public class Level : GameFrame
    {
        public Row[] rows;

        public HashSet<Ball> balls = new();
        public HashSet<Ball> ballsToKill = new();
        public HashSet<Ball> ballsToAdd = new();
        public HashSet<Brick> walls = new();
        public List<LifeDisplay> lives = new();
        public List<String> countdownMessages = new(){"3", "2", "1", "Go"};
        public List<String> currentCountdownMessages = new();
        private Animation<Level> countdownAnimation;

        public Paddle paddle;

        public Brick deathWall;

        public int score;
        public int wallSize = 20;
        private int livesCount = 2;
        private TextDisplay scoreDisplay;
        private TextDisplay countdownDisplay;
        private int _everyOneHundred = 0;
        private bool _canShrink = true;

        private Song _music; 

        public Level(Game1 game) : base(game)
        {
            rows = new Row[]
            {
                new Row(this.game, Color.Green, 5),
                new Row(this.game, Color.Green, 5),
                new Row(this.game, Color.Blue, 3),
                new Row(this.game, Color.Blue, 3),
                new Row(this.game, Color.Orange, 2),
                new Row(this.game, Color.Orange, 2),
                new Row(this.game, Color.Yellow, 1),
                new Row(this.game, Color.Yellow, 1),
            };
            Initialize();
        }
        public void InitializeBricks()
        {
            int bricks = 14;
            int playSize = game.width - wallSize * 2;
            int playHeight = game.height / 3;

            float heightBuffer = 8;
            float widthBuffer = 8;

            Vector2 playOffset = new Vector2(wallSize + heightBuffer / 2, game.height / 6);

            float sizeWidth = playSize / bricks;
            float sizeHeight = playHeight / rows.Count();


            float brickWidth = sizeWidth - widthBuffer;
            float brickHeight = sizeHeight - heightBuffer;

            float halfHeightBuffer = heightBuffer / 2;
            float halfWidthBuffer = widthBuffer / 2;

            for (int rowI = 0; rowI < rows.Count(); rowI++)
            {
                Row row = rows[rowI];
                for (int i = 0; i < bricks; i++)
                {
                    row.Add(new Brick(game, playOffset + new Vector2(sizeWidth * i + halfWidthBuffer, sizeHeight * rowI + halfHeightBuffer), (int)brickWidth, (int)brickHeight, row.points, row.color));
                }
            }
        }
        
        protected override void Initialize()
        {
            this.LoadContent();
            for (int i = 0; i < livesCount; i++)
            {
                lives.Add(new LifeDisplay(game, new Vector2(wallSize * (i + 1) * 2, game.height - wallSize * 2)));
            }
            scoreDisplay = new TextDisplay(game, "Score: 0", new Vector2(game.width - wallSize * 10, game.height - wallSize * 2));
            this.AddPaddle();
            this.StartCountDown();

            InitializeBricks();

            walls.Add(new Brick(game, new Vector2(0, 0), game.width, wallSize, 0, Color.Black));
            walls.Add(new Brick(game, new Vector2(0, 0), wallSize, game.height, 0, Color.Black));
            walls.Add(new Brick(game, new Vector2(game.width - wallSize, 0), wallSize, game.height, 0, Color.Black));

            deathWall = new Brick(game, new Vector2(0, game.height + wallSize), game.width, wallSize, 0, Color.Black);
            MediaPlayer.Play(this._music);
        }
        protected override void LoadContent()
        {
            this._music = this.game.Content.Load<Song>("Audio/summer");
        }

        private void CheckBallToBrick(Ball ball)
        {
            for (int rowI = 0; rowI < rows.Count(); rowI++)
            {
                Row row = rows[rowI];
                for (int i = 0; i < row.bricks.Count; i++)
                {
                    if (Physics.Physics.Intersection(ball.physicsEntity, row.bricks[i].physicsEntity))
                    {
                        ball.Reflect(row.bricks[i].physicsEntity);
                        this.UpdateScore(row.bricks[i].points);
                        row.Remove(row.bricks[i]);

                        if (row.bricks.Count <= 0)
                        {
                            this.UpdateScore(25);
                        }

                        foreach (Ball b in balls)
                        {
                            b.BreakBrick();
                        }

                        if (rowI == 0 && _canShrink) // top level brick
                        {
                            this.paddle.StartShrinking(this.paddle.width / 2);
                            this._canShrink = false;
                        }

                    }
                }
            }
        }
        private void CheckBallToWall(Ball ball)
        {
            foreach (Brick wall in walls)
            {
                if (Physics.Physics.Intersection(ball.physicsEntity, wall.physicsEntity))
                {
                    ball.Reflect(wall.physicsEntity);
                }
            }
        }
        private void CheckBallToPaddle(Ball ball)
        {
            if (Physics.Physics.Intersection(ball.physicsEntity, paddle.physicsEntity))
            {

                ball.SetBounce(paddle.physicsEntity);
            }
        }
        private void AddBall(Paddle paddle = null)
        {
            Ball ball = new Ball(game, this);
            ballsToAdd.Add(ball);
            if (paddle != null)
            {
                paddle.ball = ball;
            }
        }
        public void GameOver()
        {
            this.game.AddHighScore(new State.GameState((uint)this.score, 1));
            this.game.RemoveFrame();
            MediaPlayer.Stop();
        }
        private void RemoveBall(Ball ball)
        {
            balls.Remove(ball);

            if (this.balls.Count <= 0)
            {
                this.paddle.StartShrinking(0);
            }
        }
        public void AddPaddle()
        {
            this.paddle = new Paddle(game, new Vector2(game.width / 2, game.height - 75));
            this.paddle.SetBounds(wallSize, game.width - wallSize);
            AddBall(this.paddle);
            this._canShrink = true;
        }
        public void CheckPaddleOver()
        {
            if (this.paddle.width <= 0)
            {
                if (lives.Count > 0)
                {
                    lives.RemoveAt(lives.Count - 1);
                    this.AddPaddle();
                    this.StartCountDown();
                }
                else
                {
                    GameOver();
                    return;
                }
            }
        }
        public void StartCountDown()
        {
            currentCountdownMessages = new List<String>(countdownMessages);
            countdownDisplay = new TextDisplay(game, currentCountdownMessages[0], new Vector2(game.width / 2, game.height / 2), "LargeFont", Color.MediumSeaGreen);
            countdownDisplay.Center();

            countdownAnimation = new Animation<Level>(this, "CountDown", new TimeSpan(0, 0, 0, 1));
        }
        public void CountDown()
        {
            currentCountdownMessages.RemoveAt(0);
            if (currentCountdownMessages.Count == 1)
            {
                this.paddle.LaunchBall();
            }
            if (currentCountdownMessages.Count == 0)
            {
                this.countdownAnimation.Reset();
                this.countdownAnimation.Stop();
                countdownDisplay.SetString("");
                
                return;
            }
            countdownDisplay.SetString(currentCountdownMessages[0]);
            countdownDisplay.Center();
        }
        private void IsBallDead(Ball ball)
        {
            if (Physics.Physics.Intersection(ball.physicsEntity, deathWall.physicsEntity))
            {
                ballsToKill.Add(ball);
            }
        }
        private void UpdateScore(int score)
        {
            int newBallRate = 100;
            this.score += score;
            scoreDisplay.SetString($"Score: {this.score}");

            this._everyOneHundred += score;
            while (this._everyOneHundred >= newBallRate)
            {
                this.AddBall();
                this._everyOneHundred -= newBallRate;
            }
        }
        private void Pause()
        {
            this.game.AddFrame(new PauseMenu(this.game, this));
            this.paused = true;
        }
        public override void Update(GameTime gameTime)
        {
            this.countdownAnimation.Update(gameTime);
            foreach (Ball ball in ballsToAdd)
            {
                this.balls.Add(ball);
                ball.SetPosition(this.paddle.LaunchBallPosition(ball));
            }
            this.ballsToAdd.Clear();

            paddle.Update(gameTime);
            this.CheckPaddleOver();
            ballsToKill.Clear();

            if (this.game.keyboard.JustPressed(Keys.Escape))
            {
                Pause();
            }

            for (int i = 0; i < rows.Count(); i++)
            {
                rows[i].Update(gameTime);
            }
            foreach (Ball ball in balls)
            {
                ball.Update(gameTime);

                CheckBallToPaddle(ball);
                CheckBallToBrick(ball);
                CheckBallToWall(ball);
                IsBallDead(ball);
            }
            foreach (Ball ball in ballsToKill)
            {
                RemoveBall(ball);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < rows.Count(); i++)
            {
                rows[i].Draw(spriteBatch);
            }
            foreach (Ball ball in balls)
            {
                ball.Draw(spriteBatch);
            }
            foreach (Brick wall in walls)
            {
                wall.Draw(spriteBatch);
            }
            for (int i = 0; i < lives.Count(); i++)
            {
                lives[i].Draw(spriteBatch);
            }
            scoreDisplay.Draw(spriteBatch);
            countdownDisplay.Draw(spriteBatch);
            deathWall.Draw(spriteBatch);
            paddle.Draw(spriteBatch);
        }
    }
}
