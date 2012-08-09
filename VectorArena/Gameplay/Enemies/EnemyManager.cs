using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Omega;

namespace VectorArena
{
    public class EnemyManager : Actor
    {
        public List<Enemy> Enemies;
        public bool Active;

        int enemySpawnTimer;

        static Random random = new Random();

        public EnemyManager(int capacity) : base()
        {
            Enemies = new List<Enemy>(capacity);
            enemySpawnTimer = 0;
            Active = true;
        }

        public void CreateEnemies()
        {
            for (int i = 0; i < Enemies.Capacity; i++)
            {
                Enemy e = new Enemy();
                Enemies.Add(e);
                AddChild(e);
                e.LoadContent(Scene.ContentManager);
            }
        }

        public void SpawnDrones(int count)
        {
            for (int e = 0; e < Enemies.Capacity; e++)
            {
                if (!Enemies[e].Alive)
                {
                    Drone d = new Drone();
                    RemoveChild(Enemies[e]);
                    Enemies[e] = d;
                    AddChild(d);
                    d.LoadContent(Scene.ContentManager);
                    Enemies[e].Spawn(new Vector3(random.Next(-500, 500), random.Next(-500, 500), 0.0f));

                    count--;
                    if (count <= 0)
                        return;
                }
            }
        }

        public void SpawnWanderers(int count)
        {
            for (int e = 0; e < Enemies.Capacity; e++)
            {
                if (!Enemies[e].Alive)
                {
                    Wanderer w = new Wanderer();
                    RemoveChild(Enemies[e]);
                    Enemies[e] = w;
                    AddChild(w);
                    w.LoadContent(Scene.ContentManager);
                    Enemies[e].Spawn(new Vector3(random.Next(-500, 500), random.Next(-500, 500), 0.0f));

                    count--;
                    if (count <= 0)
                        return;
                }
            }
        }

        public void SpawnKamikazes(int count)
        {
            for (int e = 0; e < Enemies.Capacity; e++)
            {
                if (!Enemies[e].Alive)
                {
                    Kamikaze k = new Kamikaze();
                    RemoveChild(Enemies[e]);
                    Enemies[e] = k;
                    AddChild(k);
                    k.LoadContent(Scene.ContentManager);
                    Enemies[e].Spawn(new Vector3(random.Next(-500, 500), random.Next(-500, 500), 0.0f));

                    count--;
                    if (count <= 0)
                        return;
                }
            }
        }

        public void KillEnemies()
        {
            foreach (Enemy e in Enemies)
            {
                if(e.Alive)
                    e.Die();
            }
        }

        public override void Update(GameTimerEventArgs e)
        {
            if (Active)
            {
                int livingEnemyCount = 0;
                foreach (Enemy enemy in Enemies)
                {
                    enemy.HandleAI();
                    enemy.HandleCollisions();
                    if (enemy.Alive)
                        livingEnemyCount++;
                }

                if (((GameplayScene)Scene).ShipManager.PlayerShip.Alive)
                {
                    enemySpawnTimer++;

                    if (livingEnemyCount == 0 || enemySpawnTimer >= 300)
                    {
                        SpawnDrones(3 + ((GameplayScene)Scene).ShipManager.PlayerShip.Multiplier / 20);
                        SpawnWanderers(5 + ((GameplayScene)Scene).ShipManager.PlayerShip.Multiplier / 10);
                        SpawnKamikazes(1 + ((GameplayScene)Scene).ShipManager.PlayerShip.Multiplier / 30);

                        enemySpawnTimer = 0;
                    }
                }

                base.Update(e);
            }
        }
    }
}
