using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    public enum SpawnState
    {
        Snake,
        MummySnake,
        LocustMummySnake
    }

    static class EnemyManager
    {
        // FIELDS

        private static List<Enemy> allEnemies;
        private static Enemy[] enemyTypes;
        private static Random rand;
        private static SpawnState spawnState;
        // allows for the EnemyManager to spawn enemies
        private static Texture2D snakeSprite;
        private static Texture2D mummySprite;
        private static Texture2D locustSprite;

        // CONSTRUCTORS

        static EnemyManager()
        {
            allEnemies = new List<Enemy>();
            spawnState = SpawnState.Snake;

            // enemyTypes = new Enemy[]{new SnakeWithArms(),
            //                          new Locust(),
            //                          new Mummy()}
        }

        // METHODS

        /// <summary>
        /// Adds an enemy to the list
        /// </summary>
        /// <param name="e"></param>
        public static void AddEnemy(Enemy e)
        {
            allEnemies.Add(e);
        }

        /// <summary>
        /// Gets the sprites of the enemies
        /// </summary>
        /// <param name="snakeSpriteG"> snake sprite </param>
        /// <param name="mummySpriteG"> mummy sprite </param>
        /// <param name="rng"> random </param>
        public static void InitializeEnemies(Texture2D snakeSpriteG, Texture2D mummySpriteG, Texture2D locustSpriteG, Random rng)
        {
            snakeSprite = snakeSpriteG;
            mummySprite = mummySpriteG;
            locustSprite = locustSpriteG;
            rand = rng;
        }

        /// <summary>
        /// Updates the spawning state of the enemy manager
        /// </summary>
        /// <param name="currentTime"> current time of the game </param>
        /// <param name="maxTime"> max amount of time of the game </param>
        public static void UpdateSpawning(double currentTime, double maxTime)
        {
            if (currentTime >= (maxTime / 3) * 2)
            {
                spawnState = SpawnState.Snake;
            }
            else if (currentTime >= maxTime / 3)
            {
                spawnState = SpawnState.MummySnake;
            }
            else
            {
                spawnState = SpawnState.LocustMummySnake;
            }
        }

        /// <summary>
        /// Spawns an enemy depending on the spawnState
        /// </summary>
        /// <param name="position"></param>
        public static void SpawnEnemy(float groundY)
        {
            int spawnPositionX;
            // 50% chance to spawn on left side of boat, 50% chance to spawn on right
            if (rand.NextDouble() >= .5)
            {
                spawnPositionX =200;
            }
            else
            {
                spawnPositionX = 1800;
            }

            // update later depending on enemy weights
            switch (spawnState)
            {
                case SpawnState.Snake:
                    allEnemies.Add(new SnakeArms(new Vector2(spawnPositionX, groundY - snakeSprite.Height), snakeSprite));
                    break;
                case SpawnState.MummySnake:
                    // choose the enemy to spawn
                    if (rand.NextDouble() < .75)
                    {
                        allEnemies.Add(new SnakeArms(new Vector2(spawnPositionX, groundY - snakeSprite.Height), snakeSprite));
                    }
                    else
                    {
                        allEnemies.Add(new Mummy(new Vector2(spawnPositionX, groundY - mummySprite.Height), mummySprite));
                    }
                    break;
                case SpawnState.LocustMummySnake:
                    double spawnChance = rand.NextDouble();
                    if (spawnChance < .65)
                    {
                        allEnemies.Add(new SnakeArms(new Vector2(spawnPositionX, groundY - snakeSprite.Height), snakeSprite));
                    }
                    else if (spawnChance < .85)
                    {
                        allEnemies.Add(new Mummy(new Vector2(spawnPositionX, groundY - mummySprite.Height), mummySprite));
                    }
                    else
                    {
                        // spawn locust at any point in the air
                        spawnPositionX = rand.Next(0, 2001);
                        allEnemies.Add(new Locust(new Vector2(spawnPositionX, -300), locustSprite));
                    }
                    break;
            }

            //Enemy enemyToSpawn = enemyTypes[choice];

            // stub for manual placement
            // enemyToSpawn.Position = new Vector2(1000, 310);

            //allEnemies.Add(enemyToSpawn);

        }


        /// <summary>
        /// goes through the list of enemies and checks if they are dead or not
        /// if the enemy is dead it is removed from the list
        /// </summary>
        public static void ClearDeadEnemies()
        {
            for(int i = 0; i < allEnemies.Count; i++)
            {
                if (allEnemies[i].EState == EnemyState.Dead)
                {
                    allEnemies.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Calls the enemy update methods
        /// </summary>
        /// <param name="player"></param>
        /// <param name="ra"></param>
        /// <param name="gameTime"></param>
        public static void UpdateEnemies(Player player, Ra ra, GameTime gameTime)
        {
            for(int i = 0; i < allEnemies.Count; i++)
            {
                allEnemies[i].Update(player, ra, gameTime);
            }
        }

        /// <summary>
        /// Removes all the enemies fromt the screen
        /// </summary>
        public static void RemoveEnemies()
        {
            allEnemies = new List<Enemy>();
        }
        
        /// <summary>
        /// Returns the count of the allEnemies list
        /// </summary>
        /// <returns></returns>
        public static int GetEnemyCount()
        {
            return allEnemies.Count;
        }
    }
}
