using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Breakout.UI;
using Microsoft.Xna.Framework.Input;

namespace Breakout.Views
{
    public class PauseMenu : GameFrame
    {
        private TextDisplay _mainMenu;
        private TextDisplay _continueGame;
        private TextDisplay _title;
        private Level _level;
        private Texture2D _sprite;
        public PauseMenu(Game1 game, Level level) : base(game) 
        {
            this._level = level;
            this._title = new TextDisplay(this.game, "-- Pause Menu --", new Vector2(this.game.width/2, 250));
            this._mainMenu = new TextDisplay(this.game, "Main Menu", new Vector2(this.game.width/2, 450));
            this._continueGame = new TextDisplay(this.game, "Continue Game", new Vector2(this.game.width / 2, 350));
            this._title.Center();
            this._mainMenu.Center();
            this._continueGame.Center();

            this.LoadContent();
        }
        override protected void LoadContent()
        {
            _sprite = new Texture2D(game.GraphicsDevice, 1, 1);
            _sprite.SetData(new[] { Color.White });
        }

        private void UnPause()
        {
            this.game.RemoveFrame();
            this.active = false;
        }
        public override void Update(GameTime gameTime)
        {

            if (this.game.keyboard.IsOver(this._mainMenu.bounds))
            {
                if (this.game.keyboard.JustLeftMouseDown())
                {
                    this.game.RemoveFrame();
                    this._level.GameOver();
                }
            }

            if (this.game.keyboard.IsOver(this._continueGame.bounds))
            {
                if (this.game.keyboard.JustLeftMouseDown())
                {
                    this.UnPause();
                    return;
                }
            }

            if (this.game.keyboard.JustPressed(Keys.Escape))
            {
                this.UnPause();
                return;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, new Rectangle(this.game.width/2-200, 150, 400, 400), Color.Black);
            this._title.Draw(spriteBatch);
            this._mainMenu.Draw(spriteBatch);
            this._continueGame.Draw(spriteBatch);
        }
    }
}
