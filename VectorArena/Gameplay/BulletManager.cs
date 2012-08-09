using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Omega;

namespace VectorArena
{
    public class BulletManager : Actor
    {
        public List<Bullet> Bullets;

        public BulletManager(int bulletCapacity) : base()
        {
            Bullets = new List<Bullet>(bulletCapacity);
        }

        public void CreateBullets()
        {
            for (int i = 0; i < Bullets.Capacity; i++)
            {
                Bullet b = new Bullet();
                Bullets.Add(b);
                AddChild(b);
            }
        }

        public void SpawnBullet(ref Ship ship, Vector2 position, Vector2 velocity)
        {
            for (int i = 0; i < Bullets.Capacity; i++)
            {
                if (!Bullets[i].Alive)
                {
                    Bullets[i].Spawn(ref ship, position, velocity);
                    return;
                }
            }
        }

        public override void Update(GameTimerEventArgs e)
        {
            foreach (Bullet b in Bullets)
            {
                b.HandleCollisions();
            }

            base.Update(e);
        }

        public override void Draw(Camera3D camera)
        {
            Matrix translation = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, -500.0f));

            LineBatch.Begin(translation, camera);
            PointBatch.Begin(translation, camera);

            foreach (Bullet b in Bullets)
            {
                b.Draw(LineBatch, PointBatch);
            }

            PointBatch.End();
            LineBatch.End();

            base.Draw(camera);
        }
    }
}
