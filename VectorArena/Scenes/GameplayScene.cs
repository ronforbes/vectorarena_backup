using Microsoft.Xna.Framework.Graphics;
using Omega;
using System;
using Microsoft.Xna.Framework;

namespace VectorArena
{
    public class GameplayScene : Scene
    {
        public bool EndScene;
        public ShipManager ShipManager;
        public BulletManager BulletManager;
        public EnemyManager EnemyManager;
        public MultiplierManager MultiplierManager;
        public ParticleManager ParticleManager;
        public AudioManager AudioManager;
        public HUD Hud;

        protected bool gameOver;

        TimeSpan postgameTimer;

        public GameplayScene(GraphicsDevice graphicsDevice, AudioManager audioManager) : base(graphicsDevice)
        {
            EndScene = false;

            ShipManager = new ShipManager(10);
            BulletManager = new BulletManager(100);
            EnemyManager = new EnemyManager(50);
            MultiplierManager = new MultiplierManager(100);
            ParticleManager = new ParticleManager(10000);
            Hud = new HUD(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            AudioManager = audioManager;

            postgameTimer = TimeSpan.FromSeconds(3);
            gameOver = false;

            AddActor(new Starfield());
            AddActor(new Grid());
            AddActor(ShipManager);
            AddActor(BulletManager);
            AddActor(EnemyManager);
            AddActor(MultiplierManager);
            AddActor(ParticleManager);
            AddActor(Hud);
            AddActor(audioManager);

            ShipManager.CreateShips();
            BulletManager.CreateBullets();
            EnemyManager.CreateEnemies();
            MultiplierManager.CreateMultipliers();
            ParticleManager.CreateParticles();

            AddPostprocess(new Bloom(new SpriteBatch(graphicsDevice), graphicsDevice));

            ShipManager.ActivateShip(true);
            Camera.TargetActor = ShipManager.PlayerShip;
            Hud.PlayerShip = ShipManager.PlayerShip;

            AudioManager.TargetListener = ShipManager.PlayerShip;
            AudioManager.Stop();
        }

        public void OnNavigatedFrom()
        {
            AudioManager.Stop();
        }

        public override void Update(GameTimerEventArgs e)
        {
            if (gameOver)
            {
                EnemyManager.KillEnemies();
                EnemyManager.Active = false;

                postgameTimer -= e.ElapsedTime;

                if (postgameTimer.TotalSeconds < 0)
                    EndScene = true;
            }

            AudioManager.PlaySong("ParticleFusion", true);

            base.Update(e);
        }
    }
}
