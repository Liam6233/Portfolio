using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    class Button
    {
        // FIELDS

        //private protected bool isClicked;
        private protected Texture2D hoverTexture;
        private protected Texture2D baseTexture;

        // migrated from GameObject (unparented it but still useful fields)
        protected Vector2 position;
        protected Texture2D texture;
        protected bool isVisible;

        // PROPERTIES

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Texture2D Texture
        {
            get { return baseTexture; }
        }

        // CONSTRUCTORS

        public Button(Vector2 position, Texture2D baseTexture, Texture2D hoverTexture)
        {
            this.position = position;
            this.texture = baseTexture;

            this.baseTexture = baseTexture;
            this.hoverTexture = hoverTexture;

            isVisible = false;
            //this.isAbsolute = true;

            DrawManager.UI.Add(this);
        }

        public Button(Texture2D baseTexture, Texture2D hoverTexture)
        {
            this.position = new Vector2(0, 0);
            this.texture = baseTexture;

            this.baseTexture = baseTexture;
            this.hoverTexture = hoverTexture;

            isVisible = false;
            //this.isAbsolute = true;

            DrawManager.UI.Add(this);
        }

        // METHODS

        /// <summary>
        /// Abstract method that is implemented in children of Button. It dictates
        /// what happens when a button is clicked.
        /// </summary>
        //public abstract void OnClick();

        public virtual bool CheckHover()
        {
            MouseState mousePos = Mouse.GetState();

            if(this.isVisible)
            {
                if (mousePos.X < position.X + hoverTexture.Width && mousePos.X > position.X &&
                mousePos.Y < position.Y + hoverTexture.Height && mousePos.Y > position.Y)
                {
                    this.texture = hoverTexture;
                    return true;
                }
                else
                {
                    this.texture = baseTexture;
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

        public void Draw(SpriteBatch sb)
        {
            if(isVisible)
            {
                sb.Draw(texture,
                        position,
                        Color.White);
            }
        }
    }
}
