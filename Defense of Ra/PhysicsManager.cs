using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace DefenseOfRa
{
    class PhysicsManager
    {
        //FIELDS

        //PROPERTIES

        //CONSTRUCTORS

        //METHODS
        /// <summary>
        /// Adds the velocity vector to the position vector
        /// </summary>
        /// <param name="position"> position </param>
        /// <param name="velocity"> velocity </param>
        /// <returns> updated position </returns>
        public Vector2 AddVelocity(Vector2 position, Vector2 velocity)
        {
            return Vector2.Add(position, velocity);
        }

        /// <summary>
        /// Adds the acceleration vector to the velocity vector
        /// </summary>
        /// <param name="velocity"> velocity </param>
        /// <param name="acceleration"> acceleration </param>
        /// <returns> updated velocity </returns>
        public Vector2 AddAcceleration(Vector2 velocity, Vector2 acceleration)
        {
            return Vector2.Add(velocity, acceleration);
        }
    }
}
