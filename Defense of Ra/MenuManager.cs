using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    /// <summary>
    /// The MenuManager is a stack that controls which menus and buttons are rendered.
    /// </summary>
    static class MenuManager
    {
        // FIELDS

        static private List<Menu> data;

        // PROPERTIES

        public static int Count
        {
            get { return data.Count; }
        }

        // CONSTRUCTORS

        static MenuManager()
        {
            data = new List<Menu>();

        }

        // METHODS

        public static void Draw(SpriteBatch sb)
        {
            if(data.Count > 0)
                Peek().Draw(sb);
        }

        public static Menu Pop()
        {
            Menu lastMenu = Peek();

            data.Remove(lastMenu);

            if(data.Count > 0)
                data[data.Count - 1].IsOpen = true;

            for(int i = 0; i < lastMenu.Buttons.Length; i++)
            {
                lastMenu.Buttons[i].IsVisible = false;
            }

            return lastMenu;
        }

        public static void Push(Menu menu)
        {
            data.Add(menu);

            menu.IsOpen = true;
            
            for(int i = 0; i < data.Count - 1; i++)
            {
                data[i].IsOpen = false;

                for(int c = 0; c < data[i].Buttons.Length; c++)
                {
                    data[i].Buttons[c].IsVisible = false;
                }
            }
        }

        public static Menu Peek()
        {
            if(data.Count > 0)
            {
                return data[Count - 1];
            }
            else
            {
                throw new Exception("No menus on stack.");
            }
        }
    }
}
