using System;
using Microsoft.Xna.Framework;
using Omega;

namespace VectorArena
{
    public class Bullet : Actor
    {
        public Ship Ship;
        public ShipManager ShipManager;
        public DroneManager DroneManager;
        public MultiplierManager MultiplierManager;
        public ParticleManager ParticleManager;
        public AudioManager AudioManager;

        public const int Radius = 5;
        public const int Speed = 20;

        const int explosionParticleCount = 10;
        const int explosionRadius = 5;

        static Color lineColor = new Color(1.0f, 1.0f, 0.5f, 1.0f);
        static Color lightColor = new Color(0.5f, 0.5f, 0.0f, 1.0f);

        public Bullet(Actor parent) : base(parent)
        {
            Alive = false;
        }

        public void Spawn(ref Ship ship, Vector2 position, Vector2 velocity)
        {
            Ship = ship;
            Position = new Vector3(position, 0.0f);
            Velocity = new Vector3(velocity, 0.0f);
            Alive = true;
        }

        public void Die()
        {
            ParticleManager.CreateParticleEffect(explosionParticleCount, Position, explosionRadius, lineColor);

            Alive = false;
        }

        public void HandleCollisions()
        {
            if (Alive)
            {
                foreach (Drone d in DroneManager.Drones)
                {
                    if (d.Alive)
                    {
                        if (Math.Abs((Position - d.Position).Length()) <= Bullet.Radius + d.Radius)
                        {
                            Die();
                            
                            d.Die();

                            Ship.Score += 100 * Ship.Multiplier;
                        }
                    }
                }

                if (Position.X < -500 || Position.X > 500 || Position.Y < -500 || Position.Y > 500)
                {
                    Die();
                }
            }
        }

        public override void Update()
        {
            if (Alive)
            {
                Position += Velocity;
            }
        }

        public void Draw(LineBatch3D lineBatch, PointBatch3D pointBatch)
        {
            if (Alive)
            {
                lineBatch.Draw(Position, Position - Velocity * 2.0f, 10.0f, lineColor);
                pointBatch.Draw(Position - Velocity / 2, 5.0f, lightColor);
            }
        }
    }
}
