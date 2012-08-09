using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Omega;

namespace VectorArena
{
    public class ShipManager : Actor
    {
        public List<Ship> Ships;
        public Ship PlayerShip;

        public ShipManager(int capacity) : base()
        {
            Ships = new List<Ship>(capacity);
        }

        public void CreateShips()
        {
            for (int i = 0; i < Ships.Capacity; i++)
            {
                Ship s = new Ship();
                Ships.Add(s);
                AddChild(s);
            }
        }

        public void ActivateShip(bool playerShip)
        {
            for (int i = 0; i < Ships.Capacity; i++)
            {
                if (!Ships[i].Active)
                {
                    Ships[i].Activate();
                    if (playerShip)
                        PlayerShip = Ships[i];
                    Ships[i].Spawn();
                    return;
                }
            }
        }

        public override void Update(GameTimerEventArgs e)
        {
            if (VirtualThumbsticks.LeftThumbstick.Length() > 0.0f)
                PlayerShip.Thrust(VirtualThumbsticks.LeftThumbstick);
            else
                PlayerShip.Acceleration = Vector3.Zero;

            if (VirtualThumbsticks.RightThumbstick.Length() > 0.5f)
                PlayerShip.Fire(ref PlayerShip, VirtualThumbsticks.RightThumbstick);

            base.Update(e);
        }
    }   
}
