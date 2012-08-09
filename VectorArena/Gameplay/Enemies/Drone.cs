using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Omega;

namespace VectorArena
{
    public class Drone : Enemy
    {
        public Drone() : base()
        {
            vertices = new List<Vector3>();
            vertices.Add(new Vector3(0.0f, 15.0f, -500.0f));
            vertices.Add(new Vector3(-15.0f, 0.0f, -500.0f));
            vertices.Add(new Vector3(0.0f, -15.0f, -500.0f));
            vertices.Add(new Vector3(15.0f, 0.0f, -500.0f));

            lineColor = new Color(1.0f, 0.5f, 0.5f, 1.0f);
            lightColor = new Color(0.5f, 0.0f, 0.0f, 1.0f);
        }

        public override void HandleAI()
        {
            if (Alive)
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
                    Acceleration = closestShip.Position - Position;
                    Acceleration.Normalize();
                }
            }
        }
    }
}
