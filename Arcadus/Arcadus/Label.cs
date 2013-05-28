using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Arcadus {
    public class Label : Entity {
        public string text;
        public SpriteFont font;
        public Label(Vector2 pos, String text, ContentManager content)
            :base(pos, new Vector2(0.0f), "", content) {
            this.text = text;
            this.font = content.Load<SpriteFont>("Text");
        }

        public override void Draw(SpriteBatch spritebatch) {
            spritebatch.DrawString(font, this.text, this.pos, this.color);
        }

        protected override void get_Rect() {
            this.rect.X = (int)this.pos.X;
            this.rect.Y = (int)this.pos.Y;
            this.rect.Width = (int)this.font.MeasureString(this.text).X;
            this.rect.Height = (int)this.font.MeasureString(this.text).Y;
        }
    }
}
