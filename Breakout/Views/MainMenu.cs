using Breakout.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Views
{
    public class MainMenu : GameFrame
    {
        private TextDisplay startGame;
        private TextDisplay viewHighScore;
        private TextDisplay viewCredit;
        private TextDisplay title;
        private TextDisplay quit;
        public MainMenu(Game1 game) : base(game)
        {
            this.Initialize();
        }
        protected override void Initialize()
        {
            this.title = new TextDisplay(this.game, "Beach Breakout", new Vector2(200, 50));
            this.startGame = new TextDisplay(this.game, "New Game", new Vector2(200, 150));
            this.viewHighScore = new TextDisplay(this.game, "High Scores", new Vector2(200, 200));
            this.viewCredit = new TextDisplay(this.game, "Credits", new Vector2(200, 250));
            this.quit = new TextDisplay(this.game, "Quit", new Vector2(200, 350));
        }
        public override void Update(GameTime gameTime)
        {
            if (this.game.keyboard.JustPressed(Keys.Escape))
            {
                this.game.Exit();
            }

            if (this.game.keyboard.IsOver(this.startGame.bounds))
            {
                if (this.game.keyboard.JustLeftMouseDown())
                {
                    this.game.StartGame();
                    this.active = false;
                }
            }

            if (this.game.keyboard.IsOver(this.viewHighScore.bounds))
            {
                if (this.game.keyboard.JustLeftMouseDown())
                {
                    this.game.AddFrame(new HighScoreView(this.game));
                    this.active = false;
                }
            }

            if (this.game.keyboard.IsOver(this.viewCredit.bounds))
            {
                if (this.game.keyboard.JustLeftMouseDown())
                {
                    this.game.AddFrame(new CreditView(this.game));
                    this.active = false;
                }
            }
            if (this.game.keyboard.IsOver(this.quit.bounds))
            {
                if (this.game.keyboard.JustLeftMouseDown())
                {
                    this.game.Exit();
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            this.title.Draw(spriteBatch);
            this.startGame.Draw(spriteBatch);
            this.viewHighScore.Draw(spriteBatch);
            this.viewCredit.Draw(spriteBatch);
            this.quit.Draw(spriteBatch);
        }
    }
}
