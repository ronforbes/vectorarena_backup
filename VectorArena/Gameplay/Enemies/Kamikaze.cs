using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace VectorArena
{
    public class Kamikaze : Enemy
    {
        bool charging = false;
        int chargingCounter = 0;

        static Random random = new Random();

        public Kamikaze()
            : base()
        {
            vertices = new List<Vector3>();
            vertices.Add(new Vector3(-15.0f, 15.0f, -500.0f));
            vertices.Add(new Vector3(-15.0f, -15.0f, -500.0f));
            vertices.Add(new Vector3(15.0f, 15.0f, -500.0f));
            vertices.Add(new Vector3(15.0f, -15.0f, -500.0f));

            lineColor = new Color(0.5f, 1.0f, 1.0f, 1.0f);
            lightColor = new Color(0.0f, 0.5f, 0.5f, 1.0f);
        }

        public override void HandleAI()
        {
            if (Alive)
            {
                if (!charging)
                {
                    // find the closest ship
                    float closestDistance = 10000.0f;
                    Ship closestShip = null;

                    foreach (Ship s in ((GameplayScene)Scene).ShipManager.Ships)
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
                        if (closestDistance < 100.0f)
                        {
                            charging = true;
                            Velocity = Vector3.Zero;
                        }
                        else
                        {
                            Acceleration = closestShip.Position - Position;
                            Acceleration.Normalize();
                        }
                    }
                }
                else
                {
                    chargingCounter++;

                    if (chargingCounter > 100)
                    {
                        Die();

                        for (int i = 0; i < 5; i++)
                        {
                            Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
                            ((GameplayScene)Scene).ParticleManager.CreateParticleEffect(10, Position, 100, color);
                        }

                        foreach (Ship s in ((GameplayScene)Scene).ShipManager.Ships)
                        {
                            if (Math.Abs((Position - s.Position).Length()) <= 100 + s.Radius)
                            {
                                if (s.Alive)
                                {
                                    s.Die();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
