using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Omega;

namespace VectorArena
{
    public class DroneManager : Actor
    {
        public List<Drone> Drones;
        public ShipManager ShipManager;
        public bool Active;

        int droneSpawnTimer;

        static Random random = new Random();

        public DroneManager(int droneCapacity) : base(null)
        {
            Drones = new List<Drone>(droneCapacity);

            for (int i = 0; i < Drones.Capacity; i++)
            {
                Drone d = new Drone(this);
                Drones.Add(d);
                AddChild(d);
            }

            droneSpawnTimer = 0;

            Active = true;
        }

        public void Initialize(ref ShipManager shipManager, ref BulletManager bulletManager, ref MultiplierManager multiplierManager, ref ParticleManager particleManager, ref AudioManager audioManager)
        {
            foreach (Drone d in Drones)
            {
                d.ShipManager = shipManager;
                d.BulletManager = bulletManager;
                d.MultiplierManager = multiplierManager;
                d.ParticleManager = particleManager;
                d.AudioManager = audioManager;
            }
        }

        public void SpawnDrones(int droneCount)
        {
            for (int i = 0; i < droneCount; i++)
            {
                for (int d = 0; d < Drones.Capacity; d++)
                {
                    if (!Drones[d].Alive)
                    {
                        Drones[d].Spawn(new Vector3(random.Next(-500, 500), random.Next(-500, 500), 0.0f));

                        droneCount--;
                        if (droneCount == 0)
                            return;
                    }
                }
            }
        }

        public void KillDrones()
        {
            foreach (Drone d in Drones)
            {
                if(d.Alive)
                    d.Die();
            }
        }

        public override void Update()
        {
            if (Active)
            {
                int livingDroneCount = 0;
                foreach (Drone d in Drones)
                {
                    d.HandleCollisions();
                    if (d.Alive)
                        livingDroneCount++;
                }

                if (ShipManager.PlayerShip.Alive)
                {
                    droneSpawnTimer++;

                    if (livingDroneCount == 0 || droneSpawnTimer >= 300)
                    {
                        SpawnDrones(5 + ShipManager.PlayerShip.Multiplier / 10);
                        droneSpawnTimer = 0;
                    }
                }

                base.Update();
            }
        }
    }
}
