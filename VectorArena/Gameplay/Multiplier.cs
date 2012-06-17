using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Omega;

namespace VectorArena
{
    public class Multiplier : Actor
    {
        public ShipManager ShipManager;
        public DroneManager DroneManager;
        public BulletManager BulletManager;
        public ParticleManager ParticleManager;
        public AudioManager AudioManager;

        int Life;

        List<Vector3> vertices;
        Vector3 lightPosition;

        static Random random = new Random();
        static Color lineColor = new Color(0.5f, 1.0f, 0.5f, 1.0f);
        static Color lightColor = new Color(0.0f, 0.5f, 0.0f, 1.0f);

        public Multiplier(Actor parent) : base(parent)
        {
            vertices = new List<Vector3>();
            vertices.Add(new Vector3(0.0f, 5.0f, -500.0f));
            vertices.Add(new Vector3(-5.0f, 0.0f, -500.0f));
            vertices.Add(new Vector3(0.0f, -5.0f, -500.0f));
            vertices.Add(new Vector3(5.0f, 0.0f, -500.0f));

            lightPosition = new Vector3(0.0f, 0.0f, -500.0f);
        }

        public void Spawn(Vector3 position, int radius)
        {
            Position = position;
            float angle = MathHelper.ToRadians(random.Next(0, 360));
            int speed = random.Next(radius);
            Velocity = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0.0f) * speed;
            Radius = 5.0f;
            Life = 1000;

            Alive = true;
        }

        public void Die()
        {
            Alive = false;
        }

        public void HandleCollisions()
        {
            if (Alive)
            {
                foreach (Ship s in ShipManager.Ships)
                {
                    if (Math.Abs((Position - s.Position).Length()) <= Radius + s.Radius * 5)
                    {
                        if (s.Alive)
                        {
                            Vector3 acceleration = s.Position - Position;
                            acceleration.Normalize();
                            Acceleration = acceleration * 3;
                        }
                    }

                    if (Math.Abs((Position - s.Position).Length()) <= Radius + s.Radius)
                    {
                        if (s.Alive)
                        {
                            s.Multiplier++;

                            Die();
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
                Velocity *= 0.9f;
                Position += Velocity;
                Life--;
                if (Life <= 0)
                    Die();

                Acceleration = Vector3.Zero;
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
                    LineBatch.Draw(vertices[i], vertices[(i + 1) % vertices.Count], 2.5f, lineColor);
                }

                LineBatch.End();

                PointBatch.Begin(translationMatrix, camera);
                PointBatch.Draw(lightPosition, 15.0f, lightColor);
                PointBatch.End();
            }
        }
    }
}
