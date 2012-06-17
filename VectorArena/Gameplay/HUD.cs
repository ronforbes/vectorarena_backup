using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Omega;

namespace VectorArena
{
    public class HUD : Actor
    {
        public Ship PlayerShip;
        public TimeSpan TimeAttackTimer;

        int screenWidth, screenHeight;
        const int padding = 5;

        GamePage.GameType gameType;

        public HUD(GamePage.GameType gameType, int screenWidth, int screenHeight) : base(null)
        {
            this.gameType = gameType;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        public override void Draw(Camera3D camera)
        {
            Vector2 energyLabelSize = SpriteFont.MeasureString("Energy");
            Vector2 energySize = SpriteFont.MeasureString(PlayerShip.Energy.ToString());
            Vector2 scoreLabelSize = SpriteFont.MeasureString("Score");
            Vector2 scoreSize = SpriteFont.MeasureString(PlayerShip.Score.ToString("#,#"));
            Vector2 multiplierLabelSize = SpriteFont.MeasureString("Multiplier");
            Vector2 multiplierSize = SpriteFont.MeasureString(PlayerShip.Multiplier.ToString("#,#") + "x");
            Vector2 livesLabelSize = SpriteFont.MeasureString("Lives");
            Vector2 livesSize = SpriteFont.MeasureString(PlayerShip.Lives.ToString());
            Vector2 timerLabelSize = SpriteFont.MeasureString("Time");
            Vector2 timerSize = SpriteFont.MeasureString(TimeAttackTimer.Minutes.ToString() + ":" + TimeAttackTimer.Seconds.ToString());

            SpriteBatch.Begin();
            SpriteBatch.DrawString(SpriteFont, "Energy", new Vector2(screenWidth / 2 - energyLabelSize.X / 2, screenHeight - padding - energySize.Y - energyLabelSize.Y / 2 - 10), Color.White);
            SpriteBatch.DrawString(SpriteFont, PlayerShip.Energy.ToString(), new Vector2(screenWidth / 2 - energySize.X / 2, screenHeight - padding - energySize.Y), Color.White);
            SpriteBatch.DrawString(SpriteFont, "Multiplier", new Vector2(padding, padding), Color.White);
            SpriteBatch.DrawString(SpriteFont, PlayerShip.Multiplier.ToString("#,#") + "x", new Vector2(padding, padding + multiplierLabelSize.Y / 2), Color.White);
            SpriteBatch.DrawString(SpriteFont, "Score", new Vector2(screenWidth - padding - scoreLabelSize.X, padding), Color.White);
            SpriteBatch.DrawString(SpriteFont, PlayerShip.Score.ToString("#,#"), new Vector2(screenWidth - padding - scoreSize.X, padding + scoreLabelSize.Y / 2), Color.White);

            if (gameType == GamePage.GameType.Marathon)
            {
                SpriteBatch.DrawString(SpriteFont, "Lives", new Vector2(screenWidth / 2 - livesLabelSize.X / 2, padding), Color.White);
                SpriteBatch.DrawString(SpriteFont, PlayerShip.Lives.ToString(), new Vector2(screenWidth / 2 - livesSize.X / 2, padding + livesLabelSize.Y / 2), Color.White);
            }
            else if (gameType == GamePage.GameType.TimeAttack)
            {
                SpriteBatch.DrawString(SpriteFont, "Time", new Vector2(screenWidth / 2 - timerLabelSize.X / 2, padding), Color.White);
                SpriteBatch.DrawString(SpriteFont, TimeAttackTimer.Minutes.ToString() + ":" + TimeAttackTimer.Seconds.ToString(), new Vector2(screenWidth / 2 - timerSize.X / 2, padding + timerLabelSize.Y / 2), Color.White);
            }

            SpriteBatch.End();

            PrimitiveBatch.Begin(BlendState.Additive, null, Matrix.Identity);
            float x = (float)PlayerShip.Energy / (float)Ship.MaxEnergy;
            PrimitiveBatch.DrawLine(new Vector2(padding + (1.0f - x) * (screenWidth / 2 - 50 - padding), screenHeight - padding - energySize.Y / 2), new Vector2(screenWidth / 2 - 50, screenHeight - padding - energySize.Y / 2), (int)energySize.Y / 3, Color.White);
            PrimitiveBatch.DrawLine(new Vector2(screenWidth / 2 + 50, screenHeight - padding - energySize.Y / 2), new Vector2(screenWidth / 2 + 50 + x * (screenWidth / 2 - 50 - padding), screenHeight - padding - energySize.Y / 2), (int)energySize.Y / 3, Color.White);
            PrimitiveBatch.End();
        }
    }
}
