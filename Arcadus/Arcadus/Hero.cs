﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Arcadus
{
    public class Hero:Entity
    {
        protected int i;
        protected int shoot;
        protected string step;
        protected string direction;
        protected string aim;
        public int lives = 4;
        Point targetTile;
        public Hero(Vector2 pos, Vector2 speed, String asset, ContentManager content)
            : base(pos, speed, asset,content)
        {
            this.step = "1";
            this.direction = "right";
            this.aim = "e";
            this.shoot = 0;
            this.targetTile = new Point((int)(pos.X / 40), (int)(pos.Y / 40));
        }
        public override void Update(){
            // set up variables
            int xdif = 0;
            int ydif = 0;
            int collidex = 0;
            int collidey = 0;
            float bulletx = 0.0f;
            float bullety = 0.0f;
            float dx = 0.0f;
            float dy = 0.0f;
            bool go = true;
            if (i % 15 == 0){
                this.step = "1";
            }
            else if (i % 15 == 8){
                this.step = "2";
            }
            

            if (Keyboard.GetState().IsKeyDown(Keys.K)){
                this.shoot = 1;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.K)){
                if (this.shoot == 1)
                {
                    this.shoot = 2;
                }
                else
                {
                    this.shoot = 0;
                }
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A)){dx -= 5;}
            if (Keyboard.GetState().IsKeyDown(Keys.D)){dx += 5;}
            if (Keyboard.GetState().IsKeyDown(Keys.W)){dy -= 5;}
            if (Keyboard.GetState().IsKeyDown(Keys.S)){dy += 5;}

            this.rect.X += (int)(dx);
            this.rect.Y += (int)(dy);
            this.rect.Width = this.texture.Width;
            this.rect.Height = this.texture.Height;
            for(int x = 0; x < (Main.map.grid.GetLength(0)); x ++)
            {
                for (int y = 0; y < (Main.map.grid.GetLength(1)); y++)
                {
                    if (Main.map.grid[x, y].tile_type == 1) 
                    {
                        if (this.rect.Intersects(Main.map.grid[x, y].rect))
                        {
                            go = false;
                            xdif = this.rect.Center.X - Main.map.grid[x, y].rect.Center.X;
                            ydif = this.rect.Center.Y - Main.map.grid[x, y].rect.Center.Y;
                            if (Math.Abs(xdif) > Math.Abs(ydif)) {
                                if (xdif < 0) { collidex = Main.map.grid[x, y].rect.Left; collidey = 0; }
                                if (xdif > 0) { collidex = Main.map.grid[x, y].rect.Right; collidey = 0; }
                            }
                            else {
                                if (ydif < 0) { collidey = Main.map.grid[x, y].rect.Top; collidex = 0; }
                                if (ydif > 0) { collidey = Main.map.grid[x, y].rect.Bottom; collidex = 0; }
                            }
                        }
                    }
                    if (Main.map.grid[x, y].tile_type == 2) {
                        Rectangle origRect = Main.map.grid[x, y].rect;
                        if (this.rect.Intersects(new Rectangle(origRect.X + 8, origRect.Y + 8, 24, 24))) {
                            Main.map.grid[x, y].InvokeDoorLink();
                            y = Main.map.grid.GetLength(1) - 1;
                            x = Main.map.grid.GetLength(0) - 1;

                        }
                    }
                }
            }
            this.rect.X -= (int)(dx);
            this.rect.Y -= (int)(dy);
            this.rect.Width = this.texture.Width;
            this.rect.Height = this.texture.Height;



            if (dy > 0)
            {
                this.aim = "s";
            }
            else if (dy < 0)
            {
                this.aim = "n";
            }
            if ((dx > 0) && (dy < 0))
            {
                this.direction = "right";
                this.aim = "ne";
            }
            else if ((dx < 0) && (dy < 0))
            {
                this.direction = "left";
                this.aim = "nw";
            }
            else if ((dx > 0) && (dy > 0))
            {
                this.direction = "right";
                this.aim = "se";
            }
            else if ((dx < 0) && (dy > 0))
            {
                this.direction = "left";
                this.aim = "sw";
            }
            else if (dx < 0) {
                this.direction = "left";
                this.aim = "w";
            }
            else if (dx > 0) {
                this.direction = "right";
                this.aim = "e";
            }
            if ((dx != 0) || (dy != 0)) {
                this.i++;
                this.i %= 30;
            }

           

            if (this.shoot == 2)
            {
                if (this.aim == "n") { bulletx = 0.0f; bullety = -1.0f; }
                if (this.aim == "ne") { bulletx = 1.0f; bullety = -1.0f; }
                if (this.aim == "e") { bulletx = 1.0f; bullety = 0.0f; }
                if (this.aim == "se") { bulletx = 1.0f; bullety = 1.0f; }
                if (this.aim == "s") { bulletx = 0.0f; bullety = 1.0f; }
                if (this.aim == "sw") { bulletx = -1.0f; bullety = 1.0f; }
                if (this.aim == "w") { bulletx = -1.0f; bullety = 0.0f; }
                if (this.aim == "nw") { bulletx = -1.0f; bullety = -1.0f; }
          
                //TODO: make bullets come out of gun instead of center of sprite
                new Bullet(new Vector2((this.rect.X + 20), (this.rect.Y + 20)), new Vector2(bulletx*9, bullety*9), "bullet1", this.content);
            }

            this.texture = this.content.Load<Texture2D>("Man_" + this.step + "_" + this.direction + "_" + this.aim);
            if (go) { this.speed = new Vector2(dx, dy); }
            else {
                this.speed = new Vector2(0, 0);
                if (collidey != 0) {
                    if (ydif < 0) { this.rect.Y = collidey - this.rect.Height;}
                    if (ydif > 0) { this.rect.Y = collidey; }
                }
                if (collidex != 0) {
                    if (xdif < 0) { this.rect.X = collidex - this.rect.Width;}
                    if (xdif > 0) { this.rect.X = collidex;}
                }
            }
  
            base.Update();
        }
    }
}
