using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcadus
{
   public class Mob:Entity {
       protected int i;
        protected string step;
        protected string direction;
        protected string name;
        public int atk;
        Point targetTile;
        public int hp;

        public Mob(Vector2 pos, String asset, ContentManager content, String name, int atk, int hp)
            :base(pos, new Vector2 (0.0f),asset, content)
        {
            this.hp = hp;
            this.name = name;
            this.atk = atk;
            this.step = "1";
            this.direction = "right";
            i = 0;
            step = "1";
            this.targetTile = new Point((int)(pos.X / 40), (int)(pos.Y / 40));
        }

        public override void Update() {
            i %= 30;
            //float bulletx = 0.0f;
            //float bullety = 0.0f;
            float dx = 0.0f;
            float dy = 0.0f;
            Random random = new Random();
            int rand = random.Next(1, 4);


            Point hero = new Point();
            foreach (Entity m in GV.EntityList) 
            {
                if (m is Hero) {
                    hero = (m.rect.Center);
                    }
            }
            if (i % 15 == 0) {
                this.step = "1";
            }
            else if (i % 15 == 8) {
                this.step = "2";
            }
            if (i % 2 == 0) {
                if (this.atk == 0) {
                    if (Math.Abs(((this.rect.X - hero.X) * (this.rect.X - hero.X) + (this.rect.Y - hero.Y) * (this.rect.Y - hero.Y))) <= 280 * 280) {

                        
                        if (hero.X < this.rect.X) {
                            dx -= 4;
                            this.direction = "left";
                        }
                        else if (hero.X > this.rect.X) {
                            dx += 4;
                            this.direction = "right";
                        }
                        if (hero.Y < this.rect.Y) {
                            dy -= 4;
                        }
                        else if (hero.Y > this.rect.Y) {
                            dy += 4;
                        }

                    }
                    else {
                        if (i % 10 == 0) {
                            if (rand == 1) {
                                dx += 4;
                                this.direction = "right";

                            }
                            if (rand == 2) {
                                dx -= 4;
                                this.direction = "left";

                            }
                            if (rand == 3) {
                                dy -= 4;

                            }
                            if (rand == 4) {
                                dy += 4;

                            }
                        }
                    }

                }
                else if (this.atk == 1) {

                }

            }
            //new Bullet(new Vector2((this.rect.X + 20), (this.rect.Y + 20)), new Vector2(bulletx*7, bullety*7), "bullet2", this.content);

            bool go = true;
            this.rect.X += (int)(dx);
            this.rect.Y += (int)(dy);
            for (int x = 0; x < (Main.map.grid.GetLength(0)); x++) {
                for (int y = 0; y < (Main.map.grid.GetLength(1)); y++) {
                    if (Main.map.grid[x, y].tile_type == 1) {
                        if (this.rect.Intersects(Main.map.grid[x, y].rect)) {
                            go = false;
                        }
                    }

                }
            }
            this.rect.X -= (int)(dx);
            this.rect.Y -= (int)(dy);
            this.texture = this.content.Load<Texture2D>(this.name+"_" + this.step + "_" + this.direction);

            if (go) { this.speed = new Vector2(dx, dy); }
            else { this.speed = new Vector2(0, 0); }

            base.Update();
        }
    }
}

