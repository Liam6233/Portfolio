using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace DefenseOfRa
{
    class Platform : StaticObject
    {
        public Platform(Vector2 position, Texture2D texture) : base(position, texture)
        {
            bounds.Height = 1;
        }
    }
}
