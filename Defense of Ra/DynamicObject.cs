using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    abstract class DynamicObject : GameObject
    {
        // FIELDS
        protected Vector2 velocity;
        protected Vector2 acceleration;
        // the amount of hits the object can take before dying
        protected int health;

        // PROPERTIES
        public float XVelocity
        {
            get { return velocity.X; }
            set { velocity.X = value; }
        }

        public float YVelocity
        {
            get { return velocity.Y; }
            set { velocity.Y = value; }
        }
        // CONSTRUCTORS

        // commenting for debug to run Game1 without making constructors for all children of GameObject
        public DynamicObject(Vector2 position, Texture2D texture) : base(texture)
        {
            this.position = position;
            this.texture = texture;
            this.velocity = new Vector2(0, 0);
            this.acceleration = new Vector2(0, 0);
        }

        // METHODS
        public virtual void Update()
        {
            this.position += velocity;
            this.velocity += acceleration;

            // reset velocity every frame
            // this line of code is causing problems when I am trying to jump
            // since velocity is reset each frame, after applying a force in the y direction
            // and an opposing force in the acceleration, the acceleration force is not applied 
            // commenting this line out makes player move like they are on ice and I am not sure
            // how to get around that
            // velocity = new Vector2(0 ,0); ---> OLD

            // after working on other code this solution came to me. 
            // instead of reseting velocity every frame, these if statements function almost like
            // a speed limit, if the player is moving to fast in a certain direction,
            // it stops it from continuing to increase
            // this removes the feelings of moving on ice, and lets jumping work
            // NEW
            /*
            if (velocity.X > 10)
            {
                velocity.X = 5;
            }
            if (velocity.X < -10)
            {
                velocity.X = -5;
            }
            */
            if (velocity.Y > 10)
            {
                velocity.Y = 5;
            }
            if(velocity.Y < -50)
            {
                velocity.Y = -5;
            }
            
        }

        public void TakeDamage()
        {
            health--;
        }

    }
}
