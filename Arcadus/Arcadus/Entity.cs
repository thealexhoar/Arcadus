using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Arcadus {
    public class Entity {
        public Texture2D texture;
        public Vector2 pos, speed;
        public Rectangle rect;
        public String asset;
        public ContentManager content;
        public Color color = Color.White;

        public Entity(Vector2 pos, Vector2 speed,String asset, ContentManager content) {
            this.pos = pos;
            this.content = content;
            this.speed = speed;
            this.rect = new Rectangle(0, 0, 5, 5);
            this.asset = asset;
            GV.EntityList.Add(this);
        }

        protected virtual void get_Rect() {
            this.rect.X = (int)this.pos.X;
            this.rect.Y = (int)this.pos.Y;
            this.rect.Width = this.texture.Width;
            this.rect.Height = this.texture.Height;
        }
        public void LoadContent() {
            if (this.asset != "") { this.texture = GV.content.Load<Texture2D>(this.asset); }
            this.get_Rect();
        }
        public event EventHandler OnUpdate;
        public virtual void Update() {
            this.pos += this.speed;
            this.get_Rect();
            if (this.OnUpdate != null) { this.OnUpdate(this, new EventArgs()); }
        }
        public virtual void Draw(SpriteBatch spritebatch) {
            Rectangle rect = new Rectangle((int)(this.pos - Main.camera_pos).X, (int)(this.pos - Main.camera_pos).Y, this.rect.Width, this.rect.Height);
            if (this.texture != null) { spritebatch.Draw(this.texture, rect, null, this.color, 0.0f, new Vector2(), SpriteEffects.None, 0.1f); }
        }
    }
}
