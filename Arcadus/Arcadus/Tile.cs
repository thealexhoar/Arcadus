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
    public class Tile:Entity {

        public int tile_type;
        public string tileSet = "dirt";
        public event EventHandler<StartLevelEventArgs> onPlayerTouch;
        public string doorURL;

        public Tile(Vector2 pos, Vector2 speed, String asset, ContentManager content, int tile_type)
            : base(pos, speed, asset, content) {
            this.tile_type = tile_type;
        }

        public void InvokeDoorLink() {
            this.onPlayerTouch.Invoke(this, new StartLevelEventArgs(this.doorURL));
            Console.WriteLine("player touch");
        }

        public void DetermineTileGraphic(Tile[,] grid, int myX, int myY) {
            string first = "c";
            string second = "c";
            bool all = false;
            bool noneVert = false;
            bool noneHoriz = false;

            int up = myY - 1;
            int down = myY + 1;
            int left = myX - 1;
            int right = myX + 1;

            up = Math.Max(Math.Min(up, grid.GetLength(1) - 1), 0);
            down = Math.Max(Math.Min(down, grid.GetLength(1) - 1), 0);
            left = Math.Max(Math.Min(left, grid.GetLength(0) - 1), 0);
            right = Math.Max(Math.Min(right, grid.GetLength(0) - 1), 0);

            if (grid[myX, myY].tile_type == 1) {
                if (grid[right, myY].tile_type == 0 && grid[left, myY].tile_type == 0) { all = true; }
                else if (grid[right, myY].tile_type == 0) { second = "l"; }
                else if (grid[left, myY].tile_type == 0) { second = "r"; }
                else { noneHoriz = true; }
                if (grid[myX, up].tile_type == 0 && grid[myX, down].tile_type == 0) { all = true; }
                else if (grid[myX, up].tile_type == 0) { first = "b"; }
                else if (grid[myX, down].tile_type == 0) { first = "t"; }
                else { noneVert = true; }
            }
            if (grid[myX, myY].tile_type == 2) {
                if (myY == 0) {
                    if (grid[Math.Min(myX + 1, grid.GetLength(0) - 1), myY].tile_type == 2) {
                        this.texture = this.content.Load<Texture2D>("tile_ethernet_tl");
                    }
                    else { this.texture = this.content.Load<Texture2D>("tile_ethernet_tr"); }
                }
                if (myX == 0) {
                    if (grid[myX, Math.Min(myY + 1, grid.GetLength(1) - 1)].tile_type == 2) {
                        this.texture = this.content.Load<Texture2D>("tile_ethernet_rt");
                    }
                    else { this.texture = this.content.Load<Texture2D>("tile_ethernet_rb"); }
                }
                if (myY == grid.GetLength(1) - 1) {
                    if (grid[Math.Min(myX + 1, grid.GetLength(0) - 1), myY].tile_type == 2) {
                        this.texture = this.content.Load<Texture2D>("tile_ethernet_bl");
                    }
                    else { this.texture = this.content.Load<Texture2D>("tile_ethernet_br"); }
                }
                if (myX == grid.GetLength(0) - 1) {
                    if (grid[myX, Math.Min(myY + 1, grid.GetLength(1) - 1)].tile_type == 2) {
                        this.texture = this.content.Load<Texture2D>("tile_ethernet_lt");
                    }
                    else { this.texture = this.content.Load<Texture2D>("tile_ethernet_lb"); }
                }
            }
                else {
                if (!all && (!noneHoriz || !noneVert)) { this.texture = this.content.Load<Texture2D>("tile_" + this.tileSet + "_" + first + second); }
                else if (all) { this.texture = this.content.Load<Texture2D>("tile_" + this.tileSet + "_all"); }
                else { this.texture = this.content.Load<Texture2D>("tile_" + this.tileSet + "_none"); }
            }
        }

    }
}
