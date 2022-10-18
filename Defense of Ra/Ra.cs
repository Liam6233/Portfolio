using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    class Ra : StaticObject
    {
        //FIELDS
        private int health;
        // variable for ra's invincibility frames
        private int iFrames;
        private Dictionary<string, Animation> raAnimations;
        private Color color;

        //PARAMETERS
        public int Health
        {
            get { return health; }
            set
            {
                // if the ra is not invincible, they can take damage
                if (iFrames <= 0 && value < health)
                {
                    health = value;
                    iFrames = 30;
                }
                else if (value > health)
                {
                    health = value;
                }
            }
        }

        public int IFrames
        {
            get { return iFrames; }
            set
            {
                iFrames = value;
            }
        }

        //CONSTRUCTORS
        public Ra(Vector2 position, Texture2D baseTexture) : base( position,  baseTexture) 
        {
            health = 25;
            color = Color.White;
            CollisionManager.AddRa(this);
            raAnimations = new Dictionary<string, Animation>();
        }

        //METHODS
        public void Update(GameTime gameTime)
        {
            iFrames--;
            if(iFrames > 0)
            {
                color = Color.OrangeRed;
            }
            else
            {
                color = Color.White;
            }
            raAnimations["idle"].UpdateAnimation(gameTime);
        }

        public void DrawRa(SpriteBatch sb)
        {
            DrawRaAnimation("idle", SpriteEffects.None, sb);
        }

        public void AddAnimation(string key, Animation a)
        {
            raAnimations.Add(key, a);
        }

        public void DrawRaAnimation(string animKey,SpriteEffects flip, SpriteBatch sb)
        {
            sb.Draw(
                raAnimations[animKey].SpriteSheet,
                new Vector2(localPosition.X, localPosition.Y),
                new Rectangle(raAnimations[animKey].SpriteWidth * raAnimations[animKey].CurrentFrame, 0,
                raAnimations[animKey].SpriteWidth, raAnimations[animKey].SpriteHeight),
                color,
                rotation,
                axisOfRotation,
                1,
                flip,
                0) ;
        }
    }
}
