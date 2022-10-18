using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    class Menu
    {
        // FIELDS

        private protected Texture2D graphic;
        private protected Button[] buttons;
        private protected string text;

        private protected bool isOpen;
        private protected Texture2D background;

        // PROPERTIES

        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
        }

        public Button[] Buttons
        {
            get { return buttons; }
        }

        // CONSTRUCTORS

        public Menu(Texture2D graphic, Button[] buttons, string text)
        {
            this.graphic = graphic;
            this.buttons = buttons;
            this.text = text;

            background = Game1.Placeholder;
            isOpen = false;
        }

        // METHODS


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(isOpen)
            {
                // Makes background

                Viewport viewport = Game1.Graphics.GraphicsDevice.Viewport;

                spriteBatch.Draw(background,
                                new Rectangle(0, 0, viewport.Width, viewport.Height),
                                new Color(0, 0, 0, 100));

                spriteBatch.Draw(background,
                                new Rectangle(viewport.Width / 4, 0, viewport.Width / 2, viewport.Height),
                                new Color(0, 0, 0, 50));

                // Open the buttons

                for (int i = 0; i < buttons.Length; i++)
                {
                    //buttons[i].Draw(spriteBatch);
                    buttons[i].IsVisible = true;
                }
            }
            
        }

    }
}
