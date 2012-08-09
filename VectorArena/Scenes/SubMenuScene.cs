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
    public class SubMenuScene : Scene
    {
        ParticleManager particleManager;
        Random random;
        int particleEffectDelay;

        public SubMenuScene(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            particleManager = new ParticleManager(10000);

            AddActor(new Starfield());
            AddActor(new Grid());
            AddActor(particleManager);

            particleManager.CreateParticles();

            AddPostprocess(new Bloom(new SpriteBatch(graphicsDevice), graphicsDevice));
            
            random = new Random();
        }

        public override void Update(Microsoft.Xna.Framework.GameTimerEventArgs e)
        {
            if (particleEffectDelay++ >= 10)
            {
                particleEffectDelay = 0;
                particleManager.CreateParticleEffect(10, new Vector3(random.Next(-400, 400), random.Next(-200, 200), random.Next(0, 0)), random.Next(10), new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()));
            }

            base.Update(e);
        }
    }
}
