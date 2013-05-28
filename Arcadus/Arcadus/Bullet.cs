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
    class Bullet: Entity
    {

        public Bullet(Vector2 pos, Vector2 speed, String asset, ContentManager content)
            :base(pos, speed, asset, content)
        {
            this.asset = asset;
            base.LoadContent();

        }


        public Point boundToGrid(Point pt, Tile[,] grid) {
            return new Point(Math.Max(Math.Min(pt.X, grid.GetLength(0) - 1), 0), Math.Max(Math.Min(pt.Y, grid.GetLength(1) - 1), 0));
        }
        public override void Update() 
        {
            bool kill = false;
            Entity enemy = null;
            Point worldPos = new Point((int)this.pos.X, (int)this.pos.Y);

            foreach (Entity m in GV.EntityList) {
                if (m is Mob)
                {
                    if (this.rect.Intersects(m.rect)) {
                        kill = true;
                        enemy = m;
                    }
                }
            }

            Point tl = new Point(worldPos.X / 40, worldPos.Y / 40);
            Point tr = new Point((worldPos.X + this.rect.Width) / 40, worldPos.Y / 40);
            Point bl = new Point(worldPos.X / 40, (worldPos.Y + this.rect.Height) / 40);
            Point br = new Point((worldPos.X + this.rect.Width) / 40, (worldPos.Y + this.rect.Height) / 40);
            Point center = new Point((worldPos.X + this.rect.Width / 2) / 40, (worldPos.Y + this.rect.Height / 2) / 40);

            tl = boundToGrid(tl, Main.map.grid);
            tr = boundToGrid(tr, Main.map.grid);
            bl = boundToGrid(bl, Main.map.grid);
            br = boundToGrid(br, Main.map.grid);

            bool isTL = Main.map.grid[tl.X, tl.Y].tile_type == 1;
            bool isTR = Main.map.grid[tr.X, tr.Y].tile_type == 1;
            bool isBL = Main.map.grid[bl.X, bl.Y].tile_type == 1;
            bool isBR = Main.map.grid[br.X, br.Y].tile_type == 1;

            if (isTL) {
                GV.EntityList.Remove(this);
            }
            else if (isTR) {
                GV.EntityList.Remove(this);
            }
            else if (isBL) {
                GV.EntityList.Remove(this);
            }
            else if (isBR) {
                GV.EntityList.Remove(this);
            }

            if (kill) {
                GV.EntityList.Remove(this);
                GV.EntityList.Remove(enemy);
            }

            base.Update();

        }


    }
}
