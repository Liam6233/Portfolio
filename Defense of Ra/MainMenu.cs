using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    class MainMenu : Menu
    {
        // FIELDS


        // PROPERTIES

        // CONSTRUCTORS

        public MainMenu(Texture2D graphic, Button[] buttons, string text) : base(graphic, buttons, text)
        {
            
        }

        // METHODS

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch); // makes the background

            Viewport viewport = Game1.Graphics.GraphicsDevice.Viewport;

            float percentToSpace = 0.1f;
            int buttonSpace = (int)(viewport.Height * 0.05f);

            // position the graphic     the x is centered       the y is down % of screen

            spriteBatch.Draw(graphic,
                             new Vector2((viewport.Width * 0.5f) - (graphic.Width * 0.5f), viewport.Height * percentToSpace),
                             Color.White);

            int bottomOfGraphic = (int)(viewport.Height * percentToSpace) + graphic.Height + (int)(viewport.Height * percentToSpace) - buttonSpace;

            // position the buttons

            Vector2 buttonPos;  // temp variable

            for(int i = 0; i < buttons.Length; i++)
            {
                buttonPos.X = (viewport.Width * 0.5f) - (buttons[i].Texture.Width * 0.5f);

                buttonPos.Y = bottomOfGraphic + buttonSpace + (i)*(buttonSpace + buttons[i].Texture.Height);

                buttons[i].Position = buttonPos;
            }

            // draw the text

            // stub
        }
    }
}
