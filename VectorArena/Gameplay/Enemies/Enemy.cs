using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Omega;
using System;

namespace VectorArena
{
    public class Enemy : Actor
    {
        protected List<Vector3> vertices;
        protected Color lineColor;
        protected Color lightColor;

        static protected Vector3 lightPosition = new Vector3(0.0f, 0.0f, -500.0f);

        public Enemy()
            : base()
        {
            Radius = 15.0f;
        }

        public void Spawn(Vector3 position)
        {
            Position = position;
            Alive = true;
        }

        public void Die()
        {
            // drop multipliers
            ((GameplayScene)Scene).MultiplierManager.SpawnMultipliers(3, Position);

            ((GameplayScene)Scene).ParticleManager.CreateParticleEffect(50, Position, 15, lineColor);

            Alive = false;

            ((GameplayScene)Scene).AudioManager.PlaySound("Cymbal", Position, Velocity);
        }

        public virtual void HandleAI() { }

        public void HandleCollisions()
        {
            if (Alive)
            {
                foreach (Ship s in ((GameplayScene)Scene).ShipManager.Ships)
                {
                    if (Math.Abs((Position - s.Position).Length()) <= Radius + s.Radius)
                    {
                        if (s.Alive)
                        {
                            Die();

                            s.Die();
                        }
                    }
                }
            }
        }

        public override void Update(GameTimerEventArgs e)
        {
            if (Alive)
            {
                Velocity += Acceleration;
                Velocity.X = MathHelper.Clamp(Velocity.X, -1.0f, 1.0f);
                Velocity.Y = MathHelper.Clamp(Velocity.Y, -1.0f, 1.0f);
                Position += Velocity;

                Acceleration = Vector3.Zero;
            }

            base.Update(e);
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

            base.Draw(camera);
        }
    }
}
