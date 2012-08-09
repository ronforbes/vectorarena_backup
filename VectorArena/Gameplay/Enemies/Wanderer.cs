using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace VectorArena
{
    public class Wanderer : Enemy
    {
        int timer = 0;
        static Random random = new Random();

        public Wanderer()
            : base()
        {
            vertices = new List<Vector3>();
            vertices.Add(new Vector3(-15.0f, 15.0f, -500.0f));
            vertices.Add(new Vector3(15.0f, 15.0f, -500.0f));
            vertices.Add(new Vector3(15.0f, -15.0f, -500.0f));
            vertices.Add(new Vector3(-15.0f, -15.0f, -500.0f));

            lineColor = new Color(0.5f, 0.5f, 1.0f, 1.0f);
            lightColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);

            SetRandomVelocity();
        }

        public void SetRandomVelocity()
        {
            float angle = MathHelper.ToRadians(random.Next(360));
            Velocity = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0.0f);
        }

        public override void HandleAI()
        {
            if (Alive)
            {
                timer++;

                if (timer >= 100 || Position.X < -500.0f || Position.X > 500.0f || Position.Y < -500.0f || Position.Y > 500.0f)
                {
                    SetRandomVelocity();

                    timer = 0;
                }
            }
        }
    }
}
