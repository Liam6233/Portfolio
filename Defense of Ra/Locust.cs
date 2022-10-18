using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    // Child of Enemy Class, can fly
    class Locust : Enemy
    {
        // FIELDS

        // PROPERTIES

        // CONSTRUCTORS

        public Locust(Vector2 position, Texture2D texture) : base(position, texture, 1.0f, .75f)
        {
            health = 1;
            // this does not work
            this.bounds = new Rectangle((int)this.position.X, (int)this.position.Y, texture.Width, texture.Height);
        }

        // METHODS
        /// <summary>
        /// Moves the Locust towards an object
        /// </summary>
        /// <param name="obj"> object that the enemy moves toward </param>
        public override void MoveTo(GameObject obj)
        {
            Vector2 distanceToTarget = obj.GetCenter() - this.GetCenter();
            Vector2 speed = new Vector2((float)(movementSpeed * Math.Cos(Math.Atan(distanceToTarget.Y / distanceToTarget.X))),
                (float)(movementSpeed * Math.Sin(Math.Atan(distanceToTarget.Y / distanceToTarget.X))));
            // checks to see that the cursor location and speed match up
            if ((distanceToTarget.X < 0 && speed.X > 0) || (distanceToTarget.X > 0 && speed.X < 0))
            {
                speed.X = speed.X * -1;
            }
            if ((distanceToTarget.Y < 0 && speed.Y > 0) || (distanceToTarget.Y > 0 && speed.Y < 0))
            {
                speed.Y = speed.Y * -1;
            }

            // check where the object is for the purposes of movement on the x axis
            if (obj.GetCenter().X < this.XPosition)
            {
                // if object is to the left, move left
                if (Math.Abs(obj.GetCenter().X - this.XPosition) < Math.Abs(speed.X))
                {
                    velocity.X = -Math.Abs(obj.GetCenter().X - this.XPosition);
                }
                else
                {
                    velocity.X = speed.X;
                    //velocity.X = -(float)(movementSpeed * Math.Cos(Math.Atan(distanceToTarget.Y / distanceToTarget.X)));
                    //velocity.X = -movementSpeed;
                }
            }
            else if (obj.GetCenter().X > this.XPosition + this.bounds.Width)
            {
                // if object is to the right, move right
                if (Math.Abs(obj.GetCenter().X - (this.XPosition + this.bounds.Width)) < Math.Abs(speed.X))
                {
                    velocity.X = +Math.Abs(obj.GetCenter().X - (this.XPosition + this.bounds.Width));
                }
                else
                {
                    velocity.X = speed.X;
                    //velocity.X = (float)(movementSpeed * Math.Cos(Math.Atan(distanceToTarget.Y / distanceToTarget.X)));
                    //velocity.X = movementSpeed;
                }
            }
            else
            {
                velocity.X = 0;
            }

            // check where the object is for the purposes of movement on the y axis
            if (obj.GetCenter().Y < this.YPosition + this.bounds.Height)
            {
                // if object is down, move down
                if (Math.Abs(obj.GetCenter().Y - (this.YPosition + this.bounds.Height)) < Math.Abs(speed.Y))
                {
                    velocity.Y = Math.Abs(obj.GetCenter().Y - (this.YPosition + this.bounds.Height));
                }
                else
                {
                    velocity.Y = speed.Y;
                    //velocity.Y = -(float)(movementSpeed * Math.Sin(Math.Atan(distanceToTarget.Y / distanceToTarget.X)));
                    //velocity.Y = -movementSpeed;
                }
            }
            else if (obj.GetCenter().Y > this.YPosition)
            {
                // if object is up, move up
                if (Math.Abs(obj.GetCenter().Y - this.YPosition) < Math.Abs(speed.Y))
                {
                    velocity.Y = -Math.Abs(obj.GetCenter().Y - this.YPosition);
                }
                else
                {
                    velocity.Y = speed.Y;
                    //velocity.Y = (float)(movementSpeed * Math.Sin(Math.Atan(distanceToTarget.Y / distanceToTarget.X)));
                    //velocity.Y = movementSpeed;
                }
            }
            else
            {
                velocity.Y = 0;
            }
            
            position += velocity;

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
        }
    }
}
