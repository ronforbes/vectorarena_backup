using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Omega;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VectorArena
{
    public class MainMenuScene : Scene
    {
        ParticleManager particleManager;
        AudioManager audioManager;
        Random random;
        int particleEffectDelay;

        public MainMenuScene(GraphicsDevice graphicsDevice, AudioManager audioManager)
            : base(graphicsDevice)
        {
            particleEffectDelay = 0;

            particleManager = new ParticleManager(10000);
            this.audioManager = audioManager;

            AddActor(new Starfield());
            AddActor(new Grid());
            AddActor(particleManager);
            AddActor(audioManager);

            particleManager.CreateParticles();

            AddPostprocess(new Bloom(new SpriteBatch(graphicsDevice), graphicsDevice));

            random = new Random();
        }

        public override void Update(Microsoft.Xna.Framework.GameTimerEventArgs e)
        {
            if (particleEffectDelay++ >= 1)
            {
                particleEffectDelay = 0;
                particleManager.CreateParticleEffect(25, new Vector3(random.Next(-400, 400), random.Next(-200, 200), random.Next(0, 0)), random.Next(50), new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()));
            }

            audioManager.PlaySong("BasicDrumBeat", true);

            base.Update(e);
        }
    }
}
