using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using GetLevel;

namespace Arcadus
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    
    public class Main : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Vector2 camera_pos;
        public static Map map;
        public static Random random = new Random();
        public Color BackgroundColor;
        public int score;
        public Main() {

            GV.content = Content;
            GV.MainInstance = this;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
        }

        public Hero ex;
        Mob[] mobs = new Mob[10];
        Entity splash;
        Label pushToStart;
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            splash = new Entity(new Vector2(200, 125), new Vector2(0, 0), "splash", Content);
            splash.OnUpdate += splash_update;
            pushToStart = new Label(new Vector2(270, 380), "Press <spacebar> to start.", Content);
            base.Initialize();
            this.BackgroundColor = Color.Black;
        }

        bool flashingin = false;
        private void splash_update(object sender, EventArgs e) {
            if (flashingin) {
                if (pushToStart.color.A < 255) {
                    pushToStart.color.A += 2;
                    pushToStart.color.R = pushToStart.color.G = pushToStart.color.B = pushToStart.color.A;
                }
                else { flashingin = false; }
            }
            else {
                if (pushToStart.color.A > 128) {
                    pushToStart.color.A-= 2;
                    pushToStart.color.R = pushToStart.color.G = pushToStart.color.B = pushToStart.color.A;
                }
                else { flashingin = true; }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) {

                GV.EntityList.Remove(splash);
                GV.EntityList.Remove(pushToStart);
                StartLevel();

            }
        }

        GameGUI gameGUI;
        public void StartLevel(object sender, StartLevelEventArgs e) {
            this.StartLevel(e.url);
        }
        protected void StartLevel() {
            this.StartLevel("google.com");
        }
        protected void StartLevel(string url) {
            GV.EntityList.Clear();
            this.BackgroundColor = new Color(10, 10, 10);
            Console.WriteLine("Loading remote site...");
            map = new Map(url, "type", Content);
            Console.WriteLine("Finished loading remote site");
            if (map.hasError) { map.title = "Unable to retrieve remote data"; }
            ex = new Hero(new Vector2(40.0f), new Vector2(1.0f), "Man_1_right_e", Content);
            for (int x = 0; x < 3; x++) {
                mobs[x] = new Mob(new Vector2((float)(random.Next(0,(map.grid.GetLength(0)*40)) ), (float)(random.Next(0, (map.grid.GetLength(1)*40)) )), "Troll_1_right", Content, "Troll", 0, 1);
            }
            ex.LoadContent();
            map.DetermineTileGraphics();
            gameGUI = new GameGUI(new Vector2(0, 0), Content);
            gameGUI.SetLives(ex.lives);
            gameGUI.SetScore(score);
            gameGUI.SetTitleText(map.title);
            gameGUI.LoadContent();
        }

        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (Entity e in GV.EntityList){
                e.LoadContent();
            }
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (ex != null) { camera_pos = new Vector2(ex.pos.X - 400, ex.pos.Y - 350); }
            else { camera_pos = new Vector2(0, 0); }
            for (int e = 0; e < GV.EntityList.Count; e++)
            {
                GV.EntityList[e].Update();
            }
            if (gameGUI != null) {
                if (ex != null && ex.lives != gameGUI.lives) { gameGUI.SetLives(ex.lives); }
                gameGUI.SetScore(score);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(this.BackgroundColor);
            
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (Entity e in GV.EntityList) {
                e.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
