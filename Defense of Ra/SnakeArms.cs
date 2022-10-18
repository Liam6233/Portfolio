using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    class SnakeArms : Enemy
    {
        //FIELDS

        //PROPERTIES

        //CONSTRUCTORS
        public SnakeArms(Vector2 position, Texture2D texture) : base(position, texture, 2f, 2)
        {
            this.position = position;
            this.texture = texture;
            health = 1;
            // this does not work
            this.bounds = new Rectangle((int)this.position.X, (int)this.position.Y, texture.Width, texture.Height);
        }

        //METHODS

    }
}
