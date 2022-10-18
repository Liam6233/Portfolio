using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    /// <summary>
    /// A static class responsible for taking all GameObjects and rendering them every frame.
    /// </summary>
    static class DrawManager
    {
        // FIELDS

        private static List<GameObject> allObjects;
        private static List<Button> allUI;

        // PROPERTIES

        /// <summary>
        /// Allows allObjects to be acccessed so GameObjects can Add() and Remove() themselves from the list.
        /// </summary>
        public static List<GameObject> GameObjects
        {
            get { return allObjects; }
        }

        public static List<Button> UI
        {
            get { return allUI; }
        }

        // CONSTRUCTORS

        static DrawManager()
        {
            allObjects = new List<GameObject>();
            allUI = new List<Button>();
        }

        // METHODS

        /// <summary>
        /// Loops through all GameObjects and draws them if the isVisible field is
        /// set to true.
        /// </summary>
        /// <param name="sb"></param>
        public static void DrawGameObjs(SpriteBatch sb)
        {
            // loop through allObjects and called the Draw method if the indexed object isVisible
            
            for(int i = 0; i < allObjects.Count; i++)
            {
                if(allObjects[i].IsVisible)
                {
                    if (allObjects[i] is Player)
                    {
                        Player tempPlayer = (Player)allObjects[i];
                        tempPlayer.DrawPlayer(sb);
                    }
                    if (allObjects[i] is Enemy)
                    {
                        Enemy tempEnemy = (Enemy)allObjects[i];
                        tempEnemy.EnemyDraw(sb);
                    }
                   else if(allObjects[i] is Ra)
                    {
                        Ra tempRa = (Ra)allObjects[i];
                        tempRa.Draw(sb);
                        tempRa.DrawRa(sb);
                    }
                    else
                    {
                        allObjects[i].Draw(sb);
                    }
                }
            }
        }

        public static void DrawUI(SpriteBatch sb)
        {
            for (int i = 0; i < allUI.Count; i++)
            {
                if (allUI[i].IsVisible)
                {
                    allUI[i].Draw(sb);
                }
            }
        }

        public static void EraseEnemies()
        {
            for(int i = 0; i < allObjects.Count; i++)
            {
                if(allObjects[i] is Enemy)
                {
                    allObjects.RemoveAt(i);
                    i--;
                }
            }
        }
        
    }
}
