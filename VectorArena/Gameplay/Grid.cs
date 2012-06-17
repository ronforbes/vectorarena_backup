using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Omega;

namespace VectorArena
{
    public class Grid : Actor
    {
        List<Vector3> startPoints;
        List<Vector3> endPoints;
        List<int> widths;
        List<float> gridBrightnesses;

        BoundingBox bounds = new BoundingBox(new Vector3(-500, -500, -500), new Vector3(500, 500, -500));

        public Grid() : base(null)
        {
            startPoints = new List<Vector3>();
            endPoints = new List<Vector3>();
            widths = new List<int>();
            gridBrightnesses = new List<float>();

            for (int x = (int)bounds.Min.X; x <= bounds.Max.X; x += 50)
            {
                startPoints.Add(new Vector3(x, bounds.Min.Y, bounds.Min.Z));
                endPoints.Add(new Vector3(x, bounds.Max.Y, bounds.Max.Z));

                if (Math.Abs(x) % 100 == 0)
                {
                    widths.Add(5);
                    gridBrightnesses.Add(0.05f);
                }
                else if (Math.Abs(x) % 100 == 25)
                {
                    widths.Add(1);
                    gridBrightnesses.Add(0.05f);
                }
                else if (Math.Abs(x) % 100 == 50)
                {
                    widths.Add(3);
                    gridBrightnesses.Add(0.05f);
                }
                else if (Math.Abs(x) % 100 == 75)
                {
                    widths.Add(1);
                    gridBrightnesses.Add(0.05f);
                }
            }
            for (int y = (int)bounds.Min.Y; y <= bounds.Max.Y; y += 50)
            {
                startPoints.Add(new Vector3(bounds.Min.X, y, bounds.Min.Z));
                endPoints.Add(new Vector3(bounds.Max.X, y, bounds.Max.Z));

                if (Math.Abs(y) % 100 == 0)
                {
                    widths.Add(5);
                    gridBrightnesses.Add(0.05f);
                }
                else if (Math.Abs(y) % 100 == 25)
                {
                    widths.Add(1);
                    gridBrightnesses.Add(0.05f);
                }
                else if (Math.Abs(y) % 100 == 50)
                {
                    widths.Add(3);
                    gridBrightnesses.Add(0.05f);
                }
                else if (Math.Abs(y) % 100 == 75)
                {
                    widths.Add(1);
                    gridBrightnesses.Add(0.05f);
                }
            }
        }

        public override void Update() { }

        public override void Draw(Camera3D camera)
        {
            LineBatch.Begin(Matrix.Identity, camera);

            for (int i = 0; i < startPoints.Count; i++)
            {
                LineBatch.Draw(startPoints[i], endPoints[i], widths[i], new Color(gridBrightnesses[i], gridBrightnesses[i], gridBrightnesses[i], 1.0f));
            }

            LineBatch.Draw(new Vector3(bounds.Min.X, bounds.Min.Y, bounds.Min.Z), new Vector3(bounds.Max.X, bounds.Min.Y, bounds.Max.Z), 5.0f, Color.White);
            LineBatch.Draw(new Vector3(bounds.Min.X, bounds.Max.Y, bounds.Min.Z), new Vector3(bounds.Max.X, bounds.Max.Y, bounds.Max.Z), 5.0f, Color.White);
            LineBatch.Draw(new Vector3(bounds.Max.X, bounds.Min.Y, bounds.Min.Z), new Vector3(bounds.Max.X, bounds.Max.Y, bounds.Max.Z), 5.0f, Color.White);
            LineBatch.Draw(new Vector3(bounds.Min.X, bounds.Max.Y, bounds.Min.Z), new Vector3(bounds.Min.X, bounds.Min.Y, bounds.Max.Z), 5.0f, Color.White);

            LineBatch.End();
        }
    }
}
