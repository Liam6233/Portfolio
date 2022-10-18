using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    class OptionsMenu : Menu
    {
        // FIELDS

        string[] prompts;

        // PROPERTIES

        // CONSTRUCTORS

        public OptionsMenu(Texture2D graphic, Button[] buttons, string text) : base(graphic, buttons, text)
        {
            prompts = text.Split(",");
        }

        // METHODS

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch); // makes the background

            Viewport viewport = Game1.Graphics.GraphicsDevice.Viewport;

            int buttonSpace = (int)(viewport.Height * 0.05f);

            // position the buttons
            int lastButtonPos = 50;

            Vector2 buttonPos;  // temp variable

            // how much space between check boxes and prompt
            int spaceInCenter = 50;

            for (int i = 0; i < buttons.Length; i++)
            {
                // only move over check boxes
                if (i != buttons.Length - 1)
                    buttonPos.X = (viewport.Width * 0.5f) - (buttons[i].Texture.Width * 0.5f) - spaceInCenter;
                else
                    buttonPos.X = (viewport.Width * 0.5f) - (buttons[i].Texture.Width * 0.5f);

                //buttonPos.Y = bottomOfGraphic + buttonSpace + (i) * (buttonSpace + buttons[i].Texture.Height);
                buttonPos.Y = lastButtonPos + buttonSpace + (i) * (buttonSpace + buttons[i].Texture.Height);

                buttons[i].Position = buttonPos;


            }

            for (int i = 0; i < prompts.Length; i++)
            {
                spriteBatch.DrawString(Game1.Arial,
                                       prompts[i].Trim(),
                                       buttons[i].Position + new Vector2(spaceInCenter, 5),
                                       Color.White);
            }

        }
    }

}
