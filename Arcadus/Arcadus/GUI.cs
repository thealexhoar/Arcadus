using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Arcadus {
    public class GameGUI : Entity {
        
        Label TitleLabel;
        Label ScoreLabel;
        Entity[] LivesLabels;

        public int lives { get; private set; }
        public int score { get; private set; }
        public GameGUI(Vector2 pos, ContentManager content) : base(pos, new Vector2(0, 0), "heart", content) {
            this.visible = false;
            TitleLabel = new Label(pos, " ", content);
            TitleLabel.pos = new Vector2(pos.X + 400 - (TitleLabel.font.MeasureString(TitleLabel.text).X / 2), pos.Y + 4);
            LivesLabels = new Entity[5];
            ScoreLabel = new Label(new Vector2(0, 0), "Score: " + this.score.ToString(), content);
            ScoreLabel.pos = new Vector2( 796 - (ScoreLabel.font.MeasureString(score.ToString()).X), 20);
            ScoreLabel.OnUpdate += update_score;
            for (int i = 0; i < LivesLabels.Length; i++) {
                LivesLabels[i] = new Entity(new Vector2(0, 0), new Vector2(0, 0), "heart", content);
                LivesLabels[i].pos = new Vector2(pos.X + 4 + (i * 17), pos.Y + 4);
                LivesLabels[i].inWorld = false;
            }
        }

        public override void LoadContent() {
            base.LoadContent();
            TitleLabel.LoadContent();
            ScoreLabel.LoadContent();
            for (int i = 0; i < LivesLabels.Length; i++) {
                LivesLabels[i].LoadContent();
            }
        }

        private void update_score(object sender, EventArgs e) {
            Label label = ((Label)sender);
            label.text = "Score: " + score.ToString();
            label.pos = new Vector2(796 - label.font.MeasureString(label.text).X, 4);
        }

        public void SetScore(int score) {
            this.score = score;
        }
        public void SetLives(int numLives) {
            this.lives = numLives;
        }
        public void SetTitleText(string text) {
            this.TitleLabel.text = text;
        }

        public override void Update() {
            base.Update();
            TitleLabel.pos = new Vector2(pos.X + 400 - (TitleLabel.font.MeasureString(TitleLabel.text).X / 2), pos.Y + 4);
            for (int i = 0; i < LivesLabels.Length; i++) {
                if (i < this.lives) {
                    LivesLabels[i].visible = true;
                }
                else { LivesLabels[i].visible = false; }
            }
        }
    }
}
