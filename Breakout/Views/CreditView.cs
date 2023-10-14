using Breakout.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace Breakout.Views
{
    public class CreditView : GameFrame
    {
        public List<TextDisplay> textDisplays = new List<TextDisplay>();
        private Texture2D _background;
        public CreditView(Game1 game) : base(game) 
        {
            this.textDisplays.Add(new TextDisplay(this.game, "-- Credits --", new Vector2(this.game.width/2,100), Color.White));
            this.textDisplays.Add(new TextDisplay(this.game, "Created by: Marshal Taylor", new Vector2(this.game.width/2,200), Color.White));
            this.textDisplays.Add(new TextDisplay(this.game, "Music: Bensound.com/royalty-free-music\r\nLicense code: 0E2RSPRDR3M87JII", new Vector2(this.game.width / 2, 300), Color.White));
            for (int i = 0; i < this.textDisplays.Count; i++)
            {
                this.textDisplays[i].Center();
            }
            this.LoadContent();
        }
        protected override void LoadContent()
        {
            this._background = this.game.Content.Load<Texture2D>("Images/sunset");
        }
        public override void Update(GameTime gameTime)
        {
            if (this.game.keyboard.JustPressed(Keys.Escape))
            {
                this.game.RemoveFrame();
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._background, new Rectangle(0, 0, this.game.width, this.game.height), Color.White);
            for (int i = 0; i < this.textDisplays.Count; i++)
            {
                this.textDisplays[i].Draw(spriteBatch);
            }
        }
    }
}
