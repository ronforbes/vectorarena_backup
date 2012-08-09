using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Omega;

namespace VectorArena
{
    public class MarathonScene : GameplayScene
    {
        public MarathonScene(GraphicsDevice graphicsDevice, AudioManager audioManager)
            : base(graphicsDevice, audioManager)
        {

        }

        public override void Update(GameTimerEventArgs e)
        {
            if (!ShipManager.PlayerShip.Alive)
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
