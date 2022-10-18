using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace DefenseOfRa
{
    static class CollisionManager
    {
            //FIELDS
        private static List<StaticObject> allStatics;
        private static List<Enemy> allEnemies;
        private static Player player;
        private static Spear spear;
        private static Ra ra;
        private static Platform currentPlatform;

            //CONSTRUCTOR
        static CollisionManager()
        {
            allStatics = new List<StaticObject>();
            allEnemies = new List<Enemy>();
        }

            //Method for ensuring all enemies are being handled by the collision manager
            //Called within enemy manager
        public static void UpdateEnemyList(List<Enemy> updatedList)
        {
            allEnemies = updatedList;
        }

            //Adds enemies to the list when they're spawned
            //Called within enemy manager
        public static void AddEnemies(Enemy e)
        {
            allEnemies.Add(e);
        }
        public static void ResetEnemies()
        {
            allEnemies = new List<Enemy>();
        }
            //Adds static objects upon game initialization
        public static void AddStaticObject(StaticObject objectToAdd)
        {
            allStatics.Add(objectToAdd);
        }

            //Instantiates ra object
        public static void AddRa(Ra r)
        {
            ra = r;
        }

            //Instantiates player object
        public static void AddPlayer(Player p)
        {
            player = p;
        }

            //Instantiates spear object
        public static void AddSpear(Spear s)
        {
            spear = s;
        }

            //Called within Game1 update
            //Runs every method to handle every possible collision
        public static void Update()
        {
            for (int i = 0; i < allStatics.Count; i++)
            {
                if(!(allStatics[i] is Platform))
                {
                    allStatics[i].Bounds = new Rectangle((int)allStatics[i].XPosition, (int)allStatics[i].YPosition, (int)allStatics[i].Texture.Width, (int)allStatics[i].Texture.Height);
                }
                else
                {
                    allStatics[i].Bounds = new Rectangle((int)allStatics[i].XPosition, (int)allStatics[i].YPosition, (int)allStatics[i].Texture.Width, 1);
                }
                //allStatics[i].Bounds = new Rectangle((int)allStatics[i].XPosition, (int)allStatics[i].YPosition, (int)allStatics[i].Texture.Width, (int)allStatics[i].Texture.Height);
                //allStatics[i].Bounds = new Rectangle((int)allStatics[i].XPosition, (int)allStatics[i].YPosition, 20, 80);
            }
            CollisionManager.CheckPlayerStaticCollision();
            CollisionManager.CheckEnemyAttackCollision();
            CollisionManager.CheckEnemyStaticCollision();
            CollisionManager.CheckSpearCollision();
        }

            //Method for player interactions with static objects
        public static void CheckPlayerStaticCollision()
        {
            for(int i = 0; i < allStatics.Count; i++)
            {
                    //if the player intersects with a static object
                if (player.Bounds.Intersects(allStatics[i].Bounds))
                {
                        //and it's y position is less than the stat's plus its height,
                        //then it will be drawn back to the proper position
                    if (player.YPosition + player.Bounds.Height > allStatics[i].YPosition
                        && player.PState == PlayerStates.Falling
                        && allStatics[i].XPosition < player.GetCenter().X
                        && allStatics[i].XPosition + allStatics[i].Bounds.Width > player.GetCenter().X)
                    {
                        player.YPosition = allStatics[i].Position.Y - player.Bounds.Height;
                        if (player.SprtEffct == SpriteEffects.FlipHorizontally && player.XVelocity < 0)
                        {
                            player.PState = PlayerStates.MovingLeft;
                        }
                        else if (player.SprtEffct == SpriteEffects.FlipHorizontally && player.XVelocity == 0)
                        {
                            player.PState = PlayerStates.LookingLeft;
                        }
                        else if (player.XVelocity == 0)
                        {
                            player.PState = PlayerStates.LookingRight;
                        }
                        else
                        {
                            player.PState = PlayerStates.MovingRight;
                        }
                    }
                }
                    //and the static object is a boat boundary
                if (allStatics[i].Bounds.Height > allStatics[i].Bounds.Width)
                {
                        //prevent the player from moving past it
                        //left wall
                    if (allStatics[i].SprtEffct == SpriteEffects.None)
                    {
                        if (player.Position.X <= allStatics[i].Position.X + allStatics[i].Bounds.Width)
                        {
                            player.XPosition = allStatics[i].Position.X + allStatics[i].Bounds.Width;
                            player.IsAgainstLeftWall = true;
                        }
                        else
                        {
                            player.IsAgainstLeftWall = false;
                        }
                    }
                        //right wall
                    else
                    {
                        if (player.Position.X + player.Bounds.Width >= allStatics[i].Position.X)
                        {
                            player.XPosition = allStatics[i].Position.X - player.Bounds.Width;
                            player.IsAgainstRightWall = true;
                        }
                        else
                        {
                            player.IsAgainstRightWall = false;
                        }
                    }
                }

                    //and the static object is Ra
                if (allStatics[i] is Ra /*|| allStatics[i] is Platform*/)
                {
                        //allow it to fall off the platform if it walks off the edge
                    if ((player.Position.X +  player.Bounds.Width < allStatics[i].Position.X || player.Position.X > (allStatics[i].Position.X + allStatics[i].Bounds.Width))
                        && player.Position.Y == allStatics[i].Position.Y - player.Bounds.Height)
                    {
                        player.PState = PlayerStates.Falling;
                    }
                }
                
                // if on a platform, check based around that...
                if(currentPlatform != null)
                {

                        Game1.isOnPlatform = "on a platform";
                        if (player.Position.X > currentPlatform.Position.X + currentPlatform.Texture.Width || player.Position.X + player.Texture.Width < currentPlatform.Position.X)
                        {
                            Game1.isOnPlatform = "out of platform bounds";
                            currentPlatform = null;
                            player.PState = PlayerStates.Falling;
                        }

                }
                // else let's see if there is a platform to use...
                else if(allStatics[i] is Platform)
                {
                    if((player.Position.Y == allStatics[i].Position.Y - player.Bounds.Height)
                        && player.Position.X + player.Texture.Width > allStatics[i].Position.X
                        && player.Position.X < allStatics[i].Position.X + allStatics[i].Texture.Width)
                    {
                        //Game1.isOnPlatform = "true";
                        currentPlatform = allStatics[i] as Platform;
                    }
                    else
                    {
                        Game1.isOnPlatform = "no platform";
                    }
                }
            }
        }

            //Method for player interactions with enemy objects
        public static void CheckEnemyAttackCollision()
        {
            for (int i = 0; i < allEnemies.Count; i++)
            {
                // checks to see if enemy is attacking
                if (allEnemies[i].EState == EnemyState.Attacking)
                {
                    // then checks to see its collision with player and ra
                    if (allEnemies[i].Bounds.Intersects(player.Bounds))
                    {
                        player.Health--;
                    }
                    if (allEnemies[i].Bounds.Intersects(ra.Bounds))
                    {
                        ra.Health--;
                    }
                }
            }
        }

            //Method for handling enemy interactions with static objects
            //Virtually the same code as player collision
        public static void CheckEnemyStaticCollision()
        {
            for(int i = 0; i <  allStatics.Count; i++)
            {
                for(int j = 0; j < allEnemies.Count; j++)
                {
                    if (allEnemies[j].Bounds.Intersects(allStatics[i].Bounds))
                    {
                        if (allEnemies[j].YPosition + allEnemies[j].Bounds.Height > allStatics[i].YPosition
                        && allEnemies[j].EState == EnemyState.Spawning
                        && allStatics[i].XPosition < allEnemies[j].GetCenter().X
                        && allStatics[i].XPosition + allStatics[i].Bounds.Width > allEnemies[j].GetCenter().X)
                        {

                            allEnemies[j].YPosition = allStatics[i].YPosition - allEnemies[j].Bounds.Height;
                            //allEnemies[j].EState = EnemyState.Moving;
                        }
                        /*
                        if (allStatics[i].XPosition + allStatics[i].Bounds.Width > player.XPosition &&
                            player.YPosition + player.Bounds.Height == allStatics[i].YPosition + allStatics[i].Bounds.Height)
                        {
                            player.XPosition = allStatics[i].XPosition + allStatics[i].Bounds.Width;
                        }

                        */
                        
                        //if (allEnemies[j].Position.X <= 224)
                        //{
                        //    allEnemies[j].XPosition = 224;
                        //}
                        //if (allEnemies[j].Position.X + allEnemies[j].Bounds.Width >= 940)
                        //{
                        //    allEnemies[j].XPosition = 940 - allEnemies[j].Bounds.Width;
                        //}
                    }
                }
            }
        }

            //Method for assessing spear collision with all objects
        public static void CheckSpearCollision()
        {
                //Statics
            for(int i = 0; i < allStatics.Count; i++)
            {
                    //locks spear collision if intersecting with a static object
                if (spear.Intersects(allStatics[i]) && spear.State == SpearState.Thrown && !(allStatics[i] is Ra))
                {
                    spear.State = SpearState.InGround;
                }
            }
                //Player
            if(spear.Intersects(player) && spear.State != SpearState.Thrown && spear.State != SpearState.Attacking
                && spear.State != SpearState.InEnemy)
            {
                    //locks spear to player if he intersecs with it
                spear.State = SpearState.WithPlayer;
            }
                //Enemies
            for(int i = 0; i < allEnemies.Count; i++)
            {
                    //damages enemies if they intersect with the spear
                if(spear.Intersects(allEnemies[i]) && spear.State != SpearState.WithPlayer && spear.State != SpearState.InGround)
                {
                    if (allEnemies[i] is Mummy && spear.State == SpearState.Attacking)
                    {
                        // if the enemy is a mummy and the spear is thrusting,
                        //  check to see if the mummy hasn't been attacked yet
                        if (!((Mummy)allEnemies[i]).AttackedBySpear)
                        {
                            allEnemies[i].TakeDamage();
                            ((Mummy)allEnemies[i]).AttackedBySpear = true;
                        }
                    }
                    else
                    {
                        allEnemies[i].TakeDamage();
                    }
                }
                else if (allEnemies[i] is Mummy)
                {
                    ((Mummy)allEnemies[i]).AttackedBySpear = false;
                }

                // checks to see if it is a mummy that had a spear impaled but didn't die
                if (allEnemies[i] is Mummy && ((Mummy)allEnemies[i]).IsSpearStuck == true && 
                    spear.State != SpearState.InEnemy)
                {
                    allEnemies[i].TakeDamage();
                }
            }
            
        }
    }
}
