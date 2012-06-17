using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Omega;

namespace VectorArena
{
    public class Ship : Actor
    {
        public bool Active;
        public int ID;
        public int Energy;
        public int FireDelay;
        public const int FireSpeed = 5;
        public const int FireEnergyCost = 10;
        public const int MaxEnergy = 1000;
        public int Score;
        public int Multiplier;
        public GamePage.GameType GameType;
        public int Lives;
        public TimeSpan Timer;
        public bool GameOver;

        public DroneManager DroneManager;
        public BulletManager BulletManager;
        public MultiplierManager MultiplierManager;
        public ParticleManager ParticleManager;
        public AudioManager AudioManager;

        int respawnDelay;
        List<Vector3> vertices;
        Vector3 lightPosition;
    
        static int counter = 0;
        static Random random = new Random();
        static Color lineColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        static float lineWidth = 5.0f;
        static Color lightColor = new Color(0.25f, 0.25f, 0.25f, 1.0f);
        static float lightRadius = 50.0f;

        public Ship(Actor parent) : base(parent)
        {
            Radius = 15.0f;

            Active = false;

            vertices = new List<Vector3>();
            vertices.Add(new Vector3(15.0f, 0.0f, -500.0f));
            vertices.Add(new Vector3(-15.0f, -15.0f, -500.0f));
            vertices.Add(new Vector3(-10.0f, 0.0f, -500.0f));
            vertices.Add(new Vector3(-15.0f, 15.0f, -500.0f));

            lightPosition = new Vector3(0.0f, 0.0f, -500.0f);
        }

        public void Activate(GamePage.GameType gameType)
        {
            ID = counter++;
            GameType = gameType;
            if (GameType == GamePage.GameType.Marathon)
                Lives = 3;
            else if (GameType == GamePage.GameType.TimeAttack)
                Lives = 1;
            FireDelay = 0;
            Score = 0;
            Multiplier = 1;
            respawnDelay = 0;
            Active = true;
        }

        public void Spawn()
        {
            if (Active)
            {
                Position = new Vector3(random.Next(-500, 500), random.Next(-500, 500), 0);
                Velocity = Vector3.Zero;
                Acceleration = Vector3.Zero;
                Alive = true;
                Energy = MaxEnergy;
                Rotation = 0.0f;
            }
        }

        public void Thrust(Vector2 acceleration)
        {
            if (Alive)
            {
                Acceleration = new Vector3(acceleration, 0.0f);
                Rotation = (float)Math.Atan2(acceleration.Y, acceleration.X);
            }
        }

        public void Fire(ref Ship ship, Vector2 velocity)
        {
            if (Alive)
            {
                if (FireDelay <= 0 && Energy > FireEnergyCost)
                {
                    velocity.Normalize();
                    float angle = (float)Math.Atan2(velocity.Y, velocity.X);
                    BulletManager.CreateBullet(ref ship, new Vector2(Position.X, Position.Y) + velocity * Radius * 2, velocity * Bullet.Speed);
                    BulletManager.CreateBullet(ref ship, new Vector2(Position.X, Position.Y) + velocity * Radius * 2, new Vector2((float)Math.Cos(angle + MathHelper.ToRadians(5.0f)), (float)Math.Sin(angle + MathHelper.ToRadians(5.0f))) * Bullet.Speed);
                    BulletManager.CreateBullet(ref ship, new Vector2(Position.X, Position.Y) + velocity * Radius * 2, new Vector2((float)Math.Cos(angle - MathHelper.ToRadians(5.0f)), (float)Math.Sin(angle - MathHelper.ToRadians(5.0f))) * Bullet.Speed);
                    Energy -= Ship.FireEnergyCost;
                    FireDelay = Ship.FireSpeed;

                    AudioManager.PlaySound("ClosedHihat", Position, Velocity);
                }
            }
        }

        public void Damage(int damage)
        {
            if (Alive)
            {
                Energy -= damage;
                if (Energy <= 0)
                    Die();

                Energy = Math.Max(Energy, 0);
            }
        }

        public void Die()
        {
            Alive = false;
            respawnDelay = 100;
            if(GameType == GamePage.GameType.Marathon)
                Lives--;

            DroneManager.KillDrones();

            MultiplierManager.KillMultipliers();

            ParticleManager.CreateParticleEffect(50, Position, 20, lineColor);
        }

        public override void Update()
        {
            if (Alive)
            {
                Velocity += Acceleration;
                Velocity.X = MathHelper.Clamp(Velocity.X, -10.0f, 10.0f);
                Velocity.Y = MathHelper.Clamp(Velocity.Y, -10.0f, 10.0f);
                Velocity *= 0.95f;
                Position += Velocity;

                if (Position.X < -500 || Position.X > 500)
                    Velocity.X *= -1;
                if (Position.Y < -500 || Position.Y > 500)
                    Velocity.Y *= -1;

                if (Energy < 0)
                {
                    Die();
                }
                else if (Energy < MaxEnergy)
                {
                    Energy++;
                }

                if (FireDelay > 0)
                    FireDelay--;
            }
            else
            {
                respawnDelay--;

                if (respawnDelay <= 0)
                {
                    if (Lives > 0)
                        Spawn();
                    else
                        GameOver = true;
                }
            }
        }

        public override void Draw(Camera3D camera)
        {
            // draw ship
            if (Alive)
            {
                Matrix rotationMatrix = Matrix.CreateRotationZ(Rotation);
                Matrix translationMatrix = Matrix.CreateTranslation(Position);

                LineBatch.Begin(rotationMatrix * translationMatrix, camera);

                for (int i = 0; i < vertices.Count; i++)
                {
                    LineBatch.Draw(vertices[i], vertices[(i + 1) % vertices.Count], lineWidth, lineColor);
                }

                LineBatch.End();

                PointBatch.Begin(translationMatrix, camera);
                PointBatch.Draw(lightPosition, lightRadius, lightColor);
                PointBatch.End();
            }
        }
    }
}
