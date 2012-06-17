using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Omega;

namespace VectorArena
{
    public class Drone : Actor
    {
        public ShipManager ShipManager;
        public BulletManager BulletManager;
        public MultiplierManager MultiplierManager;
        public ParticleManager ParticleManager;
        public AudioManager AudioManager;

        List<Vector3> vertices;
        Vector3 lightPosition;

        static Color lineColor = new Color(1.0f, 0.5f, 0.5f, 1.0f);
        static Color lightColor = new Color(0.5f, 0.0f, 0.0f, 1.0f);

        public Drone(Actor parent) : base(parent)
        {
            Radius = 15.0f;

            vertices = new List<Vector3>();
            vertices.Add(new Vector3(0.0f, 15.0f, -500.0f));
            vertices.Add(new Vector3(-15.0f, 0.0f, -500.0f));
            vertices.Add(new Vector3(0.0f, -15.0f, -500.0f));
            vertices.Add(new Vector3(15.0f, 0.0f, -500.0f));

            lightPosition = new Vector3(0.0f, 0.0f, -500.0f);
        }

        public void Spawn(Vector3 position)
        {
            Position = position;
            Alive = true;
        }

        public void Die()
        {
            // drop multipliers
            MultiplierManager.SpawnMultipliers(3, Position);

            ParticleManager.CreateParticleEffect(50, Position, 15, lineColor);

            Alive = false;

            AudioManager.PlaySound("Cymbal", Position, Velocity);
        }

        public void HandleCollisions()
        {
            if (Alive)
            {
                Acceleration = Vector3.Zero;

                // find the closest ship
                float closestDistance = 10000.0f;
                Ship closestShip = null;

                foreach (Ship s in ShipManager.Ships)
                {
                    if (s.Active && s.Alive)
                    {
                        float distance = (s.Position - Position).Length();
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestShip = s;
                        }
                    }
                }

                if (closestShip != null)
                {
                    Acceleration = closestShip.Position - Position;
                    Acceleration.Normalize();
                }

                foreach (Ship s in ShipManager.Ships)
                {
                    if (Math.Abs((Position - s.Position).Length()) <= Radius + s.Radius)
                    {
                        if (s.Alive)
                        {
                            Die();

                            s.Damage(400);
                        }
                    }
                }
            }
        }

        public override void Update()
        {
            if (Alive)
            {
                Velocity += Acceleration;
                Velocity.X = MathHelper.Clamp(Velocity.X, -1.0f, 1.0f);
                Velocity.Y = MathHelper.Clamp(Velocity.Y, -1.0f, 1.0f);
                Position += Velocity;
            }
        }

        public override void Draw(Camera3D camera)
        {
            if (Alive)
            {
                Matrix translationMatrix = Matrix.CreateTranslation(Position);

                LineBatch.Begin(translationMatrix, camera);

                for (int i = 0; i < vertices.Count; i++)
                {
                    LineBatch.Draw(vertices[i], vertices[(i + 1) % vertices.Count], 5.0f, lineColor);
                }

                LineBatch.End();

                PointBatch.Begin(translationMatrix, camera);
                PointBatch.Draw(lightPosition, 25.0f, lightColor);
                PointBatch.End();
            }
        }
    }
}
