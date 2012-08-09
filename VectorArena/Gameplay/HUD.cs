using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Omega;

namespace VectorArena
{
    public class HUD : Actor
    {
        public Ship PlayerShip;
        public TimeSpan TimeAttackTimer = TimeSpan.Zero;

        int screenWidth, screenHeight;
        
        const int padding = 5;

        public HUD(int screenWidth, int screenHeight) : base()
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        public override void Draw(Camera3D camera)
        {
            Vector2 scoreLabelSize = SpriteFont.MeasureString("Score");
            Vector2 scoreSize = SpriteFont.MeasureString(PlayerShip.Score.ToString("#,#"));
            Vector2 multiplierLabelSize = SpriteFont.MeasureString("Multiplier");
            Vector2 multiplierSize = SpriteFont.MeasureString(PlayerShip.Multiplier.ToString("#,#") + "x");
            Vector2 timerLabelSize = SpriteFont.MeasureString("Time");
            Vector2 timerSize = SpriteFont.MeasureString(TimeAttackTimer.Minutes.ToString() + ":" + TimeAttackTimer.Seconds.ToString());

            SpriteBatch.Begin();

            SpriteBatch.DrawString(SpriteFont, "Multiplier", new Vector2(padding, padding), Color.White);
            SpriteBatch.DrawString(SpriteFont, PlayerShip.Multiplier.ToString("#,#") + "x", new Vector2(padding, padding + multiplierLabelSize.Y / 2), Color.White);
            SpriteBatch.DrawString(SpriteFont, "Score", new Vector2(screenWidth - padding - scoreLabelSize.X, padding), Color.White);
            SpriteBatch.DrawString(SpriteFont, PlayerShip.Score.ToString("#,#"), new Vector2(screenWidth - padding - scoreSize.X, padding + scoreLabelSize.Y / 2), Color.White);

            if (TimeAttackTimer != TimeSpan.Zero)
            {
                SpriteBatch.DrawString(SpriteFont, "Time", new Vector2(screenWidth / 2 - timerLabelSize.X / 2, padding), Color.White);
                SpriteBatch.DrawString(SpriteFont, TimeAttackTimer.Minutes.ToString() + ":" + TimeAttackTimer.Seconds.ToString(), new Vector2(screenWidth / 2 - timerSize.X / 2, padding + timerLabelSize.Y / 2), Color.White);
            }
            
            SpriteBatch.End();
        }

        public void DrawTimer()
        {
            
        }
    }
}
