using System;
using Microsoft.Xna.Framework.Graphics;
using Omega;
using Microsoft.Xna.Framework;

namespace VectorArena
{
    public class TimeAttackScene : GameplayScene
    {
        TimeSpan gameTimer;

        public TimeAttackScene(GraphicsDevice graphicsDevice, AudioManager audioManager)
            : base(graphicsDevice, audioManager)
        {
            gameTimer = TimeSpan.FromMinutes(2);
            Hud.TimeAttackTimer = gameTimer;
        }

        public override void Update(GameTimerEventArgs e)
        {
            if (gameTimer.TotalSeconds > 0)
            {
                gameTimer -= e.ElapsedTime;
                Hud.TimeAttackTimer = gameTimer;
            }

            if (gameTimer.TotalSeconds <= 0)
            {
                gameOver = true;
            }

            base.Update(e);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
