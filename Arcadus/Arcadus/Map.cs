using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GetLevel;

namespace Arcadus
{
    public class Map {
        protected Texture2D texture;
        protected Vector2 pos, speed;
        protected Rectangle rect;
        protected String asset;
        protected ContentManager content;
        public Tile[,] grid;
        public string title;
        public bool hasError;

        public Map(string url, string type, ContentManager content)
        {
            string tile =  "Tile";
            this.title = url;
            this.content = content;

            Random random = new Random();
            int index = 0;
            LevelGen lvlgen = new LevelGen(url);
            int[,] grid = lvlgen.LevelGrid;
            if (grid == null) {
                hasError = true;
                this.title = "Encountered an error retrieving remote document!";
                grid = new int[50, 50];
            }
            else { 
                this.title = lvlgen.title;
                index = random.Next(lvlgen.colors.Count);
            }
            for (int x = 1; x < grid.GetLength(0) - 2; x++) {
                for (int y = 1; y < grid.GetLength(1); y++) {
                    if (hasError && (x == 0 || x == grid.GetLength(0) - 1 || y == 0 || y == grid.GetLength(0) - 1)) { grid[x, y] = 1; }
                    if (random.Next(0, 50) == 12) { grid[x, y] = 1; }
                }
            }
            this.grid = new Tile[grid.GetLength(0), grid.GetLength(1)];
            for (int y = 0; y < grid.GetLength(1); y++) {
                for (int x = 0; x < grid.GetLength(0); x++) {
                    if (grid[x, y] == 0) { tile = "Tile"; }
                    else if (grid[x, y] == 1) { tile = "Wall"; }
                    else if (grid[x, y] == 2) { 
                        tile = "Door"; 
                    }
                    this.grid[x, y] = new Tile(new Vector2((float)(x * 40), (float)(y * 40)), new Vector2(), tile, GV.content, grid[x, y]);
                    this.grid[x, y].color = lvlgen.colors[index];
                    if (this.grid[x, y].tile_type == 2) {
                        this.grid[x, y].doorURL = lvlgen.dict[new Tuple<int, int>(x, y)];
                        this.grid[x, y].onPlayerTouch += GV.MainInstance.StartLevel;
                    }
                }
            }
            
        }

        public void DetermineTileGraphics() {
            for (int y = 0; y < this.grid.GetLength(1); y++) {
                for (int x = 0; x < this.grid.GetLength(0); x++) {
                    grid[x, y].DetermineTileGraphic(grid, x, y);
                }
            }
        }


        public virtual void Update() {

        }

    }

    public class StartLevelEventArgs : EventArgs {
        public string url = "";
        public StartLevelEventArgs(string url) {
            this.url = url;
        }
    }
}
