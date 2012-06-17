using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Omega;

namespace VectorArena
{
    public class MultiplierManager : Actor
    {
        public List<Multiplier> Multipliers;

        public MultiplierManager(int multiplierCapacity) : base(null)
        {
            Multipliers = new List<Multiplier>(multiplierCapacity);

            for (int i = 0; i < Multipliers.Capacity; i++)
            {
                Multiplier m = new Multiplier(this);
                Multipliers.Add(m);
                AddChild(m);
            }
        }

        public void Initialize(ref ShipManager shipManager, ref DroneManager droneManager, ref BulletManager bulletManager, ref ParticleManager particleManager, ref AudioManager audioManager)
        {
            foreach (Multiplier m in Multipliers)
            {
                m.ShipManager = shipManager;
                m.DroneManager = droneManager;
                m.BulletManager = bulletManager;
                m.ParticleManager = particleManager;
                m.AudioManager = audioManager;
            }
        }

        public void SpawnMultipliers(int multiplierCount, Vector3 position)
        {
            for (int i = 0; i < Multipliers.Capacity; i++)
            {
                if (!Multipliers[i].Alive)
                {
                    Multipliers[i].Spawn(position, 10);

                    multiplierCount--;
                    if (multiplierCount == 0)
                        return;
                }
            }
        }

        public void KillMultipliers()
        {
            foreach (Multiplier m in Multipliers)
            {
                if (m.Alive)
                    m.Die();
            }
        }

        public override void Update()
        {
            foreach (Multiplier m in Multipliers)
            {
                m.HandleCollisions();
            }

            base.Update();
        }
    }
}
