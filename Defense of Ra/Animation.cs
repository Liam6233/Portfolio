using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace DefenseOfRa
{
    class Animation
    {
        //FIELDS
        private int numFrames;
        private Texture2D spriteSheet;
        private int spriteWidth;
        // variables for animation
        private int currentFrame;
        private float fps;
        private float secondsPerFrame;
        private double timeCounter;
        
        // PROPERTIES
        public int SpriteWidth
        {
            get { return spriteWidth; }
        }

        public int SpriteHeight
        {
            get { return spriteSheet.Height; }
        }

        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
        }

        public int CurrentFrame
        {
            get { return currentFrame; }
        }
        //CONSTRUCTORS
        public Animation(Texture2D spriteSheet, int numFrames, int fps)
        {
            this.spriteSheet = spriteSheet;
            this.numFrames = numFrames;
            currentFrame = 1;
            this.fps = fps;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
            spriteWidth = this.spriteSheet.Width / numFrames;
        }
        //METHODs
        /// <summary>
		/// Updates the animation time, Credit to Erin Cascioli
		/// </summary>
		/// <param name="gameTime">Game time information</param>
		public void UpdateAnimation(GameTime gameTime)
        {
            // Add to the time counter (need TOTALSECONDS here)
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Has enough time gone by to actually flip frames?
            if (timeCounter >= secondsPerFrame)
            {
                // Update the frame and wrap
                currentFrame++;
                if (currentFrame >= numFrames)
                    currentFrame = 1;

                // Remove one "frame" worth of time
                timeCounter -= secondsPerFrame;
            }

        }

    }
}
