using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    class CheckBox : Button
    {
        // FIELDS

        private Texture2D checkedTexture;
        private bool isChecked;

        // PROPERTIES

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        // CONSTRUCTORS

        public CheckBox(Texture2D baseTexture, Texture2D checkTexture, bool determinator) : base(baseTexture, baseTexture)
        {
            checkedTexture = checkTexture;
            isChecked = determinator;   // the variable to control

            if (isChecked)
                this.texture = checkedTexture;
            else
                this.texture = baseTexture;
        }

        // METHODS

        public bool ToggleCheck()
        {

            if (isChecked)
                this.texture = baseTexture;
            else
                this.texture = checkedTexture;
            isChecked = !isChecked;

            return isChecked;
        }

        public override bool CheckHover()
        {
            MouseState mousePos = Mouse.GetState();

            if (this.isVisible)
            {
                if (mousePos.X < position.X + hoverTexture.Width && mousePos.X > position.X &&
                mousePos.Y < position.Y + hoverTexture.Height && mousePos.Y > position.Y)
                {
                    //this.texture = hoverTexture;
                    return true;
                }
                else
                {
                    //this.texture = baseTexture;
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

    }
}
