using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Omega;

namespace VectorArena
{
    public class MultiplierManager : Actor
    {
        public List<Multiplier> Multipliers;

        public MultiplierManager(int multiplierCapacity) : base()
        {
            Multipliers = new List<Multiplier>(multiplierCapacity);
        }

        public void CreateMultipliers()
        {
            for (int i = 0; i < Multipliers.Capacity; i++)
            {
                Multiplier m = new Multiplier();
                Multipliers.Add(m);
                AddChild(m);
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

        public override void Update(GameTimerEventArgs e)
        {
            foreach (Multiplier m in Multipliers)
            {
                m.HandleCollisions();
            }

            base.Update(e);
        }
    }
}
