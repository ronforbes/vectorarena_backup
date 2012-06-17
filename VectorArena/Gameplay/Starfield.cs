using System;
using Microsoft.Xna.Framework;
using Omega;

namespace VectorArena
{
    public class Starfield : Actor
    {
        Random random;
        Vector3[] points;
        int[] sizes;
        float[] brightnesses;

        const float starRadius = 5.0f;
        const int starCount = 1000;
        BoundingBox bounds = new BoundingBox(new Vector3(-1000, -1000, -1000), new Vector3(1000, 1000, -500));

        public Starfield() : base(null)
        {
            random = new Random();

            points = new Vector3[starCount];
            sizes = new int[starCount]; 
            brightnesses = new float[starCount];

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Vector3(random.Next((int)bounds.Min.X, (int)bounds.Max.X),
                                        random.Next((int)bounds.Min.Y, (int)bounds.Max.Y),
                                        random.Next((int)bounds.Min.Z, (int)bounds.Max.Z));
                sizes[i] = 1;
                brightnesses[i] = (float)random.NextDouble();
            }
        }

        public override void Update() { }

        public override void Draw(Camera3D camera)
        {
            PointBatch.Begin(Matrix.Identity, camera);

            for (int i = 0; i < starCount; i++)
            {
                PointBatch.Draw(points[i], starRadius, new Color(brightnesses[i], brightnesses[i], brightnesses[i]));
            }

            PointBatch.End();
        }
    }
}
