using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    public enum SpearState
    {
        WithPlayer,
        Attacking,
        Thrown,
        InGround,
        InEnemy,
        Recalled
    }

    class Spear : DynamicObject
    {
        // FIELDS
        private SpearState state;
        private Mummy enemyStuckTo;

        // PROPERTIES
        public SpearState State
        {
            get { return state; }
            set
            {
                state = value;
            }
        }

        public Mummy EnemyStuckTo
        {
            get { return enemyStuckTo; }
            set
            {
                enemyStuckTo = value;
            }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set
            {
                velocity = value;
            }
        }

        public float Rotation
        {
            get { return rotation; }
        }

        // CONSTRUCTORS
        public Spear(Vector2 position, Texture2D texture): base(position, texture)
        {
            rotation = 0;
            state = SpearState.WithPlayer;
            CollisionManager.AddSpear(this);
        }

        // METHODS
        /// <summary>
        /// Updates the spear using an FMS
        /// </summary>
        /// <param name="player"></param>
        public void Update(Player player)
        {
            // Spear does not update its own fms most of the time
            switch (state)
            {
                case SpearState.WithPlayer:
                    velocity = new Vector2(0, 0);
                    // check if player is facing right or left
                    if (player.SpriteEffects == SpriteEffects.FlipHorizontally)
                    {
                        position = new Vector2(player.Position.X + player.Bounds.Width + 5, player.Position.Y - 50);
                        rotation = 0;
                        spriteEffects = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        position = new Vector2(player.Position.X - this.Bounds.Width, player.Position.Y - 50);
                        rotation = 0;
                        spriteEffects = SpriteEffects.None;
                    }
                    velocity = new Vector2(0, 0);
                    player.HasSpear = true;
                    if (player.PState == PlayerStates.ThrustingLeft || 
                        player.PState == PlayerStates.ThrustingRight)
                    {
                        state = SpearState.Attacking;
                    }
                    break;
                case SpearState.Attacking:
                    velocity = new Vector2(0, 0);
                    // checks to see which thrusting state the player is in
                    //   if the player is not in a thrusting state, return to WithPlayer
                    if (player.PState == PlayerStates.ThrustingLeft)
                    {
                        position = new Vector2(player.Position.X -65,
                            player.Position.Y + 55);
                        rotation = -(float)Math.PI / 2;
                        spriteEffects = SpriteEffects.FlipHorizontally;
                    }
                    else if (player.PState == PlayerStates.ThrustingRight)
                    {
                        position = new Vector2(player.Position.X + 130, 
                            player.Position.Y + 35);
                        rotation = (float)Math.PI / 2;
                        spriteEffects = SpriteEffects.None;
                    }
                    else
                    {
                        state = SpearState.WithPlayer;
                    }
                    break;
                case SpearState.Thrown:
                    // this will have the spear accelerate downwards
                    acceleration = new Vector2(0, .8f);
                    // POTENTIAL HAZARD // // Might not have full 360 degree motion
                    rotation = (float)Math.Atan(velocity.Y / velocity.X);
                        // adds rotation depending on the direction it is going
                    if (velocity.X > 0)
                    {
                        rotation += (float)Math.PI / 2;
                    }
                    else
                    {
                        rotation -= (float)Math.PI / 2;
                    }

                    // speed cap
                    if (velocity.Y > 50)
                    {
                        velocity.Y = 50;
                    }

                    // don't let the spear go too far off screen
                    if (YPosition > Game1.Graphics.GraphicsDevice.Viewport.Height + 1000)
                    {
                        YVelocity = 0;
                        YPosition = Game1.Graphics.GraphicsDevice.Viewport.Height + 900;
                    }
                    if (YPosition < -1000)
                    {
                        YVelocity = 0;
                        YPosition = Game1.Graphics.GraphicsDevice.Viewport.Height + 900;
                    }
                    if (XPosition > Game1.Graphics.GraphicsDevice.Viewport.Width + 1000)
                    {
                        XVelocity = 0;
                        XPosition = Game1.Graphics.GraphicsDevice.Viewport.Width + 900;
                    }
                    if (XPosition < -1000)
                    {
                        XVelocity = 0;
                        XPosition = -900;
                    }
                    break;
                case SpearState.InGround:
                    acceleration = new Vector2(0, 0);
                    velocity = new Vector2(0, 0);
                    break;
                case SpearState.InEnemy:
                    XPosition = enemyStuckTo.Position.X;
                    YPosition = enemyStuckTo.Position.Y;
                    acceleration = new Vector2(0, 0);
                    velocity = new Vector2(0, 0);
                    break;
                case SpearState.Recalled:
                    enemyStuckTo = null;
                    Recall(player);
                    break;
            }
            // update velocity
            velocity += acceleration;
            // update position
            position += velocity;
            // update bounds
            bounds = new Rectangle((int)XPosition, (int)YPosition, texture.Width, texture.Height);
        }

        /// <summary>
        /// Draws the spear
        /// </summary>
        /// <param name="sb"> _spriteBatch </param>
        public void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

        /// <summary>
        /// Accelerates the spear toward the player
        /// </summary>
        public void Recall(Player player)
        {
            Vector2 distanceToPlayer = this.GetCenter() - player.GetCenter();
            float distanceRatio;
                // prevents division by zero
            if (distanceToPlayer.X != 0)
            {
                    // This will give how much the Y acceleration is changing compared to the X
                distanceRatio = Math.Abs(distanceToPlayer.Y / distanceToPlayer.X);
            }
            else
            {
                distanceRatio = 0f;
            }
                // check the direction to player
            if (distanceRatio == 0f)
            {
                // checks to see wether x or y is zero
                if (distanceToPlayer.X == 0)
                {
                    acceleration = new Vector2(0, 1);
                }
                else
                {
                    acceleration = new Vector2(1, 0);
                }
            }
            else if (distanceToPlayer.X < 0 && distanceToPlayer.Y < 0)
            {
                acceleration = new Vector2(1, distanceRatio);
            }
            else if (distanceToPlayer.X > 0 && distanceToPlayer.Y < 0)
            {
                acceleration = new Vector2(-1, distanceRatio);
            }
            else if (distanceToPlayer.X < 0 && distanceToPlayer.Y > 0)
            {
                acceleration = new Vector2(1, -distanceRatio);
            }
            else if (distanceToPlayer.X > 0 && distanceToPlayer.Y > 0)
            {
                acceleration = new Vector2(-1, -distanceRatio);
            }
                // add acceleration to velocity
            velocity += acceleration;
            
                // give the spear rotation
            rotation = (float)Math.Atan(velocity.Y / velocity.X);
                // adds rotation depending on the direction it is going
            if (velocity.X > 0)
            {
                rotation += (float)Math.PI / 2;
            }
            else
            {
                rotation -= (float)Math.PI / 2;
            }

            // cap velocity
            int velocityCap = 50;
            if (velocity.X > velocityCap)
            {
                velocity.X = velocityCap;
                velocity.Y = velocity.X * distanceRatio;
            }
            if (velocity.X < -velocityCap)
            {
                velocity.X = -velocityCap;
                velocity.Y = velocity.X * distanceRatio;
            }
            if (velocity.Y > velocityCap)
            {
                velocity.Y = velocityCap;
                velocity.X = velocity.Y / distanceRatio;
            }
            if (velocity.Y < -velocityCap)
            {
                velocity.Y = -velocityCap;
                velocity.X = velocity.Y / distanceRatio;
            }

            if (Math.Abs(distanceToPlayer.X) < 100 && Math.Abs(distanceToPlayer.Y) < 100)
            {
                state = SpearState.WithPlayer;
            }
        }

        /// <summary>
        /// Checks to see if the spear is touching an object
        ///     Works best with statics
        /// </summary>
        /// <param name="gameObject"> Object that the spear is interacting with </param>
        /// <returns> bool </returns>
        public bool Intersects(GameObject gameObject)
        {
            


            Vector2 radius = new Vector2(0, 0);
                // resets the rotation from how it is visually drawn to help with calculations
            if (velocity.X > 0)
            {
                radius.X = (float)Math.Cos(rotation - (Math.PI / 2));
                radius.Y = (float)Math.Sin(rotation - (Math.PI / 2));
            }
            else
            {
                radius.X = (float)Math.Cos(rotation + (Math.PI / 2));
                radius.Y = (float)Math.Sin(rotation + (Math.PI / 2));
            }

            // SPECIAL COLLISION
            // checks to see if it is a player
            if (Math.Abs(gameObject.GetCenter().X - this.GetCenter().X) < 25 + (this.bounds.Height / 2) &&
                Math.Abs(gameObject.GetCenter().Y - this.YPosition) < 20 + (this.bounds.Height / 2) && gameObject is Player)
            {
                return true;
            }

            // checks to see if it is an enemy
            if (gameObject.GetCenter().X - (this.GetCenter().X) < 35 + (this.bounds.Height / 2) &&
                (this.GetCenter().X) - gameObject.GetCenter().X < 35 + (this.bounds.Height / 2) &&
                gameObject.GetCenter().Y - (this.GetCenter().Y) < 25 + (this.bounds.Width) &&
                (this.GetCenter().Y) - gameObject.GetCenter().Y < 65 + (this.bounds.Width) && 
                gameObject is Enemy && ((Enemy)gameObject).EState != EnemyState.Dead)
            {
                // first checks to see if the enemy is a Mummy and that the spear is thrown
                if (gameObject is Mummy && state != SpearState.Attacking && 
                    state != SpearState.WithPlayer && state != SpearState.InGround)
                {
                    if (state == SpearState.Recalled)
                    {
                        return true;
                    }

                    if (state != SpearState.InEnemy)
                    {
                        // if it is a mummy stick in the mummy
                        ((Mummy)gameObject).IsSpearStuck = true;
                        enemyStuckTo = (Mummy)gameObject;
                        state = SpearState.InEnemy;
                    }

                    return false;
                }
                // potentially makes the spear less broken
                //if (velocity.X > 0)
                //{
                //    velocity.X -= 6;
                //}
                //else if (velocity.X < 0)
                //{
                //    velocity.X += 6;
                //}
                return true;
            }

            // check to see if the tip of the spear intersects the object
            if (this.GetCenter().Y + radius.Y > gameObject.YPosition + 50 &&
                this.GetCenter().Y + radius.Y < gameObject.YPosition + gameObject.Bounds.Height &&
                gameObject.XPosition < this.GetCenter().X + radius.X &&
                gameObject.XPosition + gameObject.Bounds.Width > this.GetCenter().X + radius.X)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}
