using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    /// <summary>
    /// GameObject is the parent for almost all things in the game. All GameObjects have position
    /// and rotation, and they are set to visible or not so the DrawManager knows to render it or
    /// not.
    /// </summary>
    abstract class GameObject
    {
        // FIELDS

        protected Vector2 position;         // world coords
        protected Vector2 localPosition;    // coords on camera
        protected float rotation;
        protected Vector2 axisOfRotation;
        protected Texture2D texture;
        protected Rectangle bounds;
        protected bool isVisible;
        protected bool isAbsolute;          // determines if this gameobject moves with the world or the screen
        protected Animation animation;
        protected Vector2 center;
        protected SpriteEffects spriteEffects;

        // PROPERTIES

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float XPosition
        {
            get { return localPosition.X;  }
            set { position.X = value; }
        }

        public float YPosition
        {
            get { return localPosition.Y;  }
            set { position.Y = value; }
        }

        public Vector2 LocalPosition
        {
            get { return localPosition; }
        }

        public Rectangle Bounds
        {
            get { return bounds;  }
            set
            {
                bounds = value;
            }
        }

        public SpriteEffects SprtEffct
        {
            get { return spriteEffects; }
            set { spriteEffects = value; }
        }

        // CONSTRUCTORS

        /// <summary>
        /// Default GameObject constructor. Spawns the GameObject in the top left of the screen with no texture.
        /// The individual fields should be overriden in the children classes to change the data.
        /// </summary>
        public GameObject(Texture2D texture)
        {
            position = new Vector2(0, 0);
            localPosition = position - Camera.Position;
            rotation = 0f;
            this.texture = texture; // ALTERNATIVE new Texture2D(Game1.Graphics.GraphicsDevice, 25, 25);
            axisOfRotation = position + new Vector2(bounds.Width / 2, bounds.Height / 2);
            bounds = new Rectangle((int) position.X, (int) position.Y, texture.Width, texture.Height);
            isVisible = true;
            isAbsolute = false;

            DrawManager.GameObjects.Add(this);
        }

        // METHODS

        public void Translate(Vector2 translation)
        {
            position += translation;
        }

        public void Rotate(float rotation)
        {
            // stub
        }

        public virtual void Draw(SpriteBatch sb)
        {
            // logic should be handled in update, but this is imperative to
            // draw things in the right position, and it is relevant to drawing
            if (!isAbsolute)
                localPosition = position - Camera.Position;
            else
                localPosition = position;

            sb.Draw(texture,                // texture
                    localPosition,          // position (top left)
                    null,                   // source rectangle
                    Color.White,            // color
                    rotation,               // rotation
                    axisOfRotation,         // where to rotate about (center)
                    1f,                     // scale
                    spriteEffects,          // SpriteEffects
                    1);                     // layer depth
        }

        public void Hide()
        {
            isVisible = false;
        }

        public void Show()
        {
            isVisible = true;
        }

        public Vector2 GetCenter()
        {
            center.X = (bounds.Width / 2) + localPosition.X;
            center.Y = (bounds.Height / 2) +  localPosition.Y;
            return center;
        }
    }
}
