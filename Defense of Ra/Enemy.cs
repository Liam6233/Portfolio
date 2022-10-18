using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace DefenseOfRa
{
    public enum EnemyState
    {
        Moving,
        Attacking,
        Dead,
        Spawning,
    }

    class Enemy : DynamicObject
    {
        // FIELDS
        protected EnemyState enemyState;
        protected double attackTimerMax;
        protected double attackTimer;
        protected double attackDurationMax;
        protected double attackDuration;
        protected double attackRange;
        protected double distanceToPlayer;
        protected double distanceToRa;
        protected Vector2 distanceToTarget;
        protected float movementSpeed;
        protected float deathTimer;
        // variables to see if the enemy has attacked the player or ra during their attack state
        protected bool attackedPlayer;
        protected bool attackedRa;

        // PROPERTIES
        public EnemyState EState
        {
            get { return enemyState; }
            set { enemyState = value;  }
        }

        // CONSTRUCTORS
        public Enemy(Vector2 position, Texture2D texture, float movementSpeed, float attackDurationMax) : base(position, texture)
        {
            enemyState = EnemyState.Moving;
            CollisionManager.AddEnemies(this);
            EnemyManager.AddEnemy(this);
            this.movementSpeed = movementSpeed;
            attackRange = 100;
            attackTimerMax = 2;
            this.attackDurationMax = attackDurationMax;
            this.attackDuration = attackDurationMax;
            deathTimer = 0;
            attackTimer = attackTimerMax;
            attackedRa = false;
            attackedPlayer = false;
        }

        // METHODS
        public void Update(Player player, Ra ra, GameTime gameTime)
        {
            switch (enemyState)
            {
                case EnemyState.Moving:
                    // get target
                    GameObject target = DeterminePath(player, ra);
                    // move to target
                    MoveTo(target);
                    // determine distance to target
                    distanceToTarget.Y = (float)(target.YPosition - (this.YPosition + this.Bounds.Height + attackRange / 2));
                    if (spriteEffects == SpriteEffects.None)
                    {
                        distanceToTarget.X = (float)(target.XPosition - (this.XPosition + this.Bounds.Width + attackRange / 2));
                    }
                    else
                    {
                        distanceToTarget.X = (float)(this.XPosition - (target.XPosition + target.Bounds.Width + attackRange / 2));
                    }
                    // check to see if the target is within attack range
                            // POTENTIAL HAZARD // // Will probably attack even if the target is in the air
                    if (distanceToTarget.X <= attackRange && distanceToTarget.Y <= attackRange)
                    {
                        // while the target is within attack range, count down a timer
                        attackTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                        // if the timer reaches 0, attack and reset the timer
                        if (attackTimer <= 0)
                        {
                            attackTimer = attackTimerMax;
                            enemyState = EnemyState.Attacking;
                        }
                    }
                    // if the target gets out of range, reset the attack timer
                    if (distanceToTarget.X > attackRange || distanceToTarget.Y > attackRange)
                    {
                        attackTimer = attackTimerMax;
                    }
                    break;
                case EnemyState.Attacking:
                    //Attack(DeterminePath(player,ra), gameTime);
                    attackDuration -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (attackDuration <= 0)
                    {
                        attackedRa = false;
                        attackedPlayer = false;
                        attackDuration = attackDurationMax;
                        enemyState = EnemyState.Moving;
                    }
                    break;
            }
            if (health <= 0)
            {
                enemyState = EnemyState.Dead;
            }
            bounds = new Rectangle((int)XPosition, (int)YPosition, texture.Width, texture.Height);
        }

        /// <summary>
        /// Determine whether the player or ra is closer then call the MoveTo method
        /// </summary>
        /// <param name="player"> player </param>
        /// <param name="ra"> ra </param>
        public GameObject DeterminePath(Player player, Ra ra)
        {
            /*// determine distance to player using pythagorean theorem
            distanceToPlayer =  Math.Sqrt(
                Math.Pow(player.GetCenter().X - this.GetCenter().X, 2) + 
                Math.Pow(player.GetCenter().Y - this.GetCenter().Y, 2));
            // determine distance to ra using pythagorean theorem
            distanceToRa = Math.Sqrt(
                Math.Pow(ra.GetCenter().X - this.GetCenter().X, 2) +
                Math.Pow(ra.GetCenter().Y - this.GetCenter().Y, 2));
            */

            distanceToPlayer = Vector2.Distance(player.GetCenter(), this.GetCenter());
            distanceToRa = Vector2.Distance(ra.GetCenter(), this.GetCenter());

            // choose a path depending on which is closer
            if (distanceToPlayer < distanceToRa)
            {
                if (this.YPosition - (player.YPosition + player.Bounds.Height) < player.Bounds.Height && 
                    (player.YPosition + player.Bounds.Height) - (this.YPosition + this.Bounds.Height) < this.Bounds.Height + player.Bounds.Height)
                {
                    // if the player is within one player length of the enemy, chase them
                    return player;
                }
                else if (this is Locust)
                {
                    // if the enemy is a locust, ignore the previous if statement
                    return player;
                }
            }
            // defaults to ra if the distances are equal
            return ra;
        }

        /// <summary>
        /// Moves the enemy towards an object
        /// </summary>
        /// <param name="obj"> object that the enemy moves toward </param>
        public virtual void MoveTo(GameObject obj)
        {
            // check where the object is for the purposes of movement
            if (obj.GetCenter().X < this.XPosition)
            {
                // if object is to the left, move left
                if (Math.Abs(obj.GetCenter().X - this.XPosition) < movementSpeed)
                {
                    velocity.X = -Math.Abs(obj.GetCenter().X - this.XPosition);
                }
                else
                {
                    velocity.X = -movementSpeed;
                }
            }
            else if (obj.GetCenter().X > this.XPosition + this.bounds.Width)
            {
                // if object is to the right, move right
                if (Math.Abs(obj.GetCenter().X - (this.XPosition + this.bounds.Width)) < movementSpeed)
                {
                    velocity.X = +Math.Abs(obj.GetCenter().X - (this.XPosition + this.bounds.Width));
                }
                else
                {
                    velocity.X = movementSpeed;
                }
            }
            else
            {
                velocity.X = 0;
            }

            // check wich side the object is on for the purposes of drawing
            if (obj.GetCenter().X < this.GetCenter().X)
            {
                // face the enemy to the left
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else if (obj.GetCenter().X > this.GetCenter().X)
            {
                // face the enemy to the right
                spriteEffects = SpriteEffects.None;
            }

            base.Update();
        }

        ///// <summary>
        ///// if an attack target is within range, count down an attack timer
        /////   if the attack timer reaches zero, attack it
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <param name="gameTime"></param>
        //public void Attack(GameObject target, GameTime gameTime)
        //{
        //    // check to see if the target is within attack range
        //    // POTENTIAL HAZARD // // Will probably attack even if the target is in the air
        //    if (Math.Abs(target.GetCenter().X - this.GetCenter().X) <= attackRange)
        //    {
        //        // while the target is within attack range, count down a timer
        //        attackTimer -= gameTime.ElapsedGameTime.TotalSeconds;
        //        // if the timer reaches 0, attack and reset the timer
        //        if (attackTimer <= 0)
        //        {
        //            attackTimer = attackTimerMax;
        //        }
        //    }
        //    // if the target gets out of range, reset the attack timer
        //    if (Math.Abs(target.GetCenter().X - this.GetCenter().X) > attackRange)
        //    {
        //        attackTimer = attackTimerMax;
        //    }
        //}

        /// <summary>
        /// Lowers health
        /// </summary>
        public void TakeDamage()
        {
            health--;
        }

        /// <summary>
        /// Enemy draw method
        /// </summary>
        /// <param name="sb"></param>
        public void EnemyDraw(SpriteBatch sb)
        {
            switch (EState)
            {
                case EnemyState.Moving:
                    break;
                case EnemyState.Attacking:
                    // plays attack animation
                    break;
                case EnemyState.Dead:
                    this.position.Y += 10;
                    rotation += .1f;
                    deathTimer += 1;
                    if(deathTimer >= 60)
                    {
                        this.Hide();
                    }
                    break;
            }
            base.Draw(sb);
        }
    }
}
