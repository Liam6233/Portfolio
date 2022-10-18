using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    class ParallaxObject : GameObject
    {
        // FIELDS

        private float speed;
        private Vector2 startPos;
        private Vector2 minPos;

        // PROPERTIES

        // CONSTRUCTORS

        public ParallaxObject(Texture2D texture, Vector2 startPos, Vector2 minPos, float speed) : base(texture)
        {
            this.speed = speed;
            this.startPos = startPos;
            this.minPos = minPos;
            this.position = startPos;
        }

        // METHODS

        public void Update()
        {
            //Translate(new Vector2(-speed, 0));
            this.position += new Vector2(-speed, 0);

            if(position.X < minPos.X)
            {
                position = startPos;
            }
        }


    }
}
