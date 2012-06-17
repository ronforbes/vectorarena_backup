using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Omega;

namespace VectorArena
{
    public class ShipManager : Actor
    {
        public List<Ship> Ships;
        public Ship PlayerShip;

        public ShipManager(int shipCapacity) : base(null)
        {
            Ships = new List<Ship>(shipCapacity);

            for(int i = 0; i < Ships.Capacity; i++)
            {
                Ship s = new Ship(this);
                Ships.Add(s);
                AddChild(s);
            }
        }

        public void Initialize(ref DroneManager droneManager, ref BulletManager bulletManager, ref MultiplierManager multiplierManager, ref ParticleManager particleManager, ref AudioManager audioManager)
        {
            foreach (Ship s in Ships)
            {
                s.DroneManager = droneManager;
                s.BulletManager = bulletManager;
                s.MultiplierManager = multiplierManager;
                s.ParticleManager = particleManager;
                s.AudioManager = audioManager;
            }
        }

        public void ActivateShip(bool playerShip, GamePage.GameType gameType)
        {
            for (int i = 0; i < Ships.Capacity; i++)
            {
                if (!Ships[i].Active)
                {
                    Ships[i].Activate(gameType);
                    if (playerShip)
                        PlayerShip = Ships[i];
                    Ships[i].Spawn();
                    return;
                }
            }
        }

        public override void Update()
        {
            if (VirtualThumbsticks.LeftThumbstick.Length() > 0.0f)
                PlayerShip.Thrust(VirtualThumbsticks.LeftThumbstick);
            else
                PlayerShip.Acceleration = Vector3.Zero;

            if (VirtualThumbsticks.RightThumbstick.Length() > 0.5f)
                PlayerShip.Fire(ref PlayerShip, VirtualThumbsticks.RightThumbstick);

            base.Update();
        }
    }   
}
