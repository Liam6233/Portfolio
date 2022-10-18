using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    // a static object is a child of GameObject that represents an object 
    // that does not move at all
    class StaticObject : GameObject
    {
        // FIELDS

        // PROPERTIES

        // CONSTRUCTORS

        /// <summary>
        /// A StaticObject constructor that uses the default GameObject constructor and then changes
        /// the variables it needs to make itself unique.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="baseTexture"></param>
        public StaticObject(Vector2 position, Texture2D texture) : base(texture) // add variables on top of base constructor
        {
            this.position = position;
            this.texture = texture;
            CollisionManager.AddStaticObject(this);
        }

        // METHODS
        /*
         * COMMENTING OUT FOR THE TIME BEING TO TEST OUT IF A COLLISION MANAGER WOULD BE MORE EFFECTIVE
        public void CheckCollision(DynamicObject dynamic)
        {
           if (bounds.Intersects(dynamic.Bounds))
           {
                dynamic.YPosition = dynamic.YPosition + bounds.Y;
           }
        }
        */
    }
}
