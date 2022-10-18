using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    // Child of Enemy Class, has more health, moves slower
    // when hit by a spear projectile the spear will become stuck in the mummy
    class Mummy : Enemy
    {
        // FIELDS
        // checks to see if the spear was thrown into it
        private bool isSpearStuck;
        // checks to see if the spear was thrusted into it
        private bool attackedBySpear;

        // PROPERTIES
        public bool IsSpearStuck
        {
            get { return isSpearStuck; }
            set
            {
                isSpearStuck = value;
            }
        }

        public bool AttackedBySpear
        {
            get { return attackedBySpear; }
            set
            {
                attackedBySpear = value;
            }
        }

        //CONSTRUCTOR
        public Mummy(Vector2 position, Texture2D texture) : base(position, texture, 1.0f, 1.0f)
        {
            isSpearStuck = false;
            health = 3;
            // this does not work
            this.bounds = new Rectangle((int)this.position.X, (int)this.position.Y, texture.Width, texture.Height);
        }

    }
}
