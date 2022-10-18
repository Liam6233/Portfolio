using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    /// <summary>
    /// Author: Chris Stehlar
    /// Date: 2/25/2022
    /// 
    /// Description: The Camera class dictates what is seen on the screen. It has a world position that is accessible by all objects
    /// who calculate their local position (spot on screen) based off this Camera's world position and their world position.
    /// </summary>
    static class Camera
    {
        // FIELDS

        private static Vector2 position;

        // PROPERTIES

        public static Vector2 Position
        {
            get { return position; }
        }

        // CONSTRUCTORS

        static Camera()
        {
            position = new Vector2(0, 0);
        }

        // METHODS

        public static void Translate(Vector2 translation)
        {
            position -= translation;
        }

        public static void Follow(GameObject target, Vector2 offset)
        {
            position = target.Position;
            position -= offset;
        }
    }
}
