using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DefenseOfRa
{
    // enum that represents the players state during gameplay
    public enum PlayerStates
    {
        MovingLeft,
        LookingLeft,
        MovingRight,
        LookingRight,
        Falling,
        SingleJump,
        DoubleJump,
        AimingLeft,
        AimingRight,
        Throwing,
        ThrustingLeft,
        ThrustingRight,
        RecallingSpear,
        Dead,   
    }
    // the player object is a child of Dynamic object meaning that it
    // has the ability to move on the screen
    // The player is what the user controls on the screen
    // it has the ability to move left and right
    // jump, thrust with the spear, aim and throw the spear
    // They have a health field and a bool that tells us if the player has the spear
    class Player : DynamicObject
    {
        // FIELDS
        // the amount of hits the player can take before dying
        // whether or not the player has the spear on them
        private bool hasSpear;
        // how much to update the velocity by per frame
        private float speed;
        // thrust timer to track how long thrust has been going for
        private double thrustTimer;
        // how long thrusts last
        private double thrustDuration;

        // variable to keep track of the current player state,
        // and the player state during the last frame
        private PlayerStates pState;
        private PlayerStates prevPState;

        // dictionary of animations, uses string as key, stores animations to be used
        // in player's draw method
        private Dictionary<string, Animation> playerAnimations;

        // varables to represent if player is in contact with one
        // of the 2 walls
        private bool isAgainstLeftWall;
        private bool isAgainstRightWall;

        // variable for the player's invincibility frames
        private int iFrames;
        private Color color;

        // PROPERTIES
        
        // get/set for the state of the player
        public PlayerStates PState
        {
            get { return pState; }
            set { pState = value; }
        }

        // set properties for whether player is against the wall
        // used inside of collision manager
        public bool IsAgainstLeftWall
        {
            set { isAgainstLeftWall = value; }
        }
        public bool IsAgainstRightWall
        {
            set { isAgainstRightWall = value; }
        }

        // get/set for how many times the player can be hit
        public int Health
        {
            get { return health; }
            set
            {
                // if the player is not invincible, they can take damage
                if (iFrames <= 0 && value < health)
                {
                    health = value;
                    iFrames = 60;
                }
                else if (value > health)
                {
                    health = value;
                }
            }
        }

        // get property for amount of Iframes remaining
        // only used in debug mode
        public int IFrames
        {
            get { return iFrames; }
            set
            {
                iFrames = value;
            }
        }

        // get/set for whether the player is holding the spear or not
        public bool HasSpear
        {
            get { return hasSpear; }
            set { hasSpear = value; }
        }
        
        // get for the players sprite effect which is a field inherited from
        // game object. None means sprite looks to the right and Flipped means looking to left
        public SpriteEffects SpriteEffects
        {
            get { return spriteEffects; }
        }


        // CONSTRUCTORS
        public Player(Vector2 position, Texture2D texture) : base(position, texture)
        {
            // player has 10 health
            health = 10;
            hasSpear = true;
            pState = PlayerStates.Falling;
            // player's horizontal movement speed
            speed = 16f;
            // the length of time it takes to thrust the spear
            thrustTimer = 0.0;
            thrustDuration = .15;
            this.texture = texture;
            // automatically adds itself to the collision manager
            CollisionManager.AddPlayer(this);
            // checks to ensure player does not spaz out when against a wall
            isAgainstLeftWall = false;
            isAgainstRightWall = false;
            iFrames = 0;
            playerAnimations = new Dictionary<string, Animation>();
            color = Color.White;
        }

        // METHODS
        public void Update(KeyboardState kbState, MouseState mState,GameTime gameTime, Spear spear)
        {
            // Finite State machine that changes the players state
            // based on what is going on 
            switch (pState)
            {
                // all code for looking, aiming, and moving, is identical
                // except reversed
                // as such not going to comment right states since it would be
                // the same thing
                
                // looking states set velocity to 0
                // pressing A or D puts player into a moving state
                // pressing W puts player in jumping state
                // if the player has spear and holds right click
                // puts them in an aiming state based on if mouse is to 
                // left or right of player
                // pressing left click puts player into a thrusting state
                // while in a looking state, the idle animation is updated
                case PlayerStates.LookingLeft:
                    velocity.X = 0;
                    velocity.Y = 0;
                    if (Game1.SinglePress(Keys.A))
                    {
                        pState = PlayerStates.MovingLeft;
                    }
                    if (Game1.SinglePress(Keys.D))
                    {
                        pState = PlayerStates.MovingRight;
                    }
                    if (Game1.SinglePress(Keys.W))
                    {
                        this.velocity.Y -= 40f;
                        pState = PlayerStates.SingleJump;
                    }
                    if(hasSpear && mState.RightButton == ButtonState.Pressed && mState.X < XPosition)
                    {
                        pState = PlayerStates.AimingLeft;
                    }
                    else if(hasSpear && mState.RightButton == ButtonState.Pressed)
                    {
                        pState = PlayerStates.AimingRight;
                    }
                    if (Game1.SingleLeftClick())
                    {
                        thrustTimer = thrustDuration;
                        pState = PlayerStates.ThrustingLeft;
                    }
                    playerAnimations["standing"].UpdateAnimation(gameTime);
                    break;
                // in a moving state, their speed is added to their velocity
                // to moving them in a direction,
                // releasing the key that put you into the moving state
                // sets you back to the looking state
                // pressing the key that would send you in the opposite 
                // direction puts player in opposite looking state
                // holding right click still sends player to an aiming state
                // pressing W still sends player to jumping state
                // player can also left click to thrust
                // moving animation is updated
                case PlayerStates.MovingLeft:
                    //this.position.X -= 5;                        OLD
                    this.velocity = new Vector2(-speed, 0);    // NEW
                    velocity.Y = 0;
                    if (kbState.IsKeyUp(Keys.A))
                    {
                        pState = PlayerStates.LookingLeft;
                    }
                    if (Game1.SinglePress(Keys.D))
                    {
                        pState = PlayerStates.MovingRight;
                    }
                    if(mState.RightButton == ButtonState.Pressed)
                    {
                        pState = PlayerStates.AimingLeft;
                    }
                    if (Game1.SinglePress(Keys.W))
                    {
                        this.velocity.Y -= 40f;
                        pState = PlayerStates.SingleJump;
                    }
                    // this presents an intersting bug/feature where
                    // if the player thrusts from a moving state, their velocity
                    // is constant until the end of the thrust
                    if (Game1.SingleLeftClick())
                    {
                        thrustTimer = thrustDuration;
                        pState = PlayerStates.ThrustingLeft;
                    }
                    playerAnimations["moving"].UpdateAnimation(gameTime);
                    break;
                // aiming states make it so player cannot moving
                // if left clicks why in aiming state, the spear is thrown
                // if they move the mouse to the other side of the player, the 
                // state will flip from left to right and vice versa
                // letting go of right mouse button sets player to 
                // a looking state
                // throwing animation is updated
                case PlayerStates.AimingLeft:
                    velocity.X = 0;
                    if(hasSpear && Game1.SingleLeftClick())
                    {
                        pState = PlayerStates.Throwing;
                    }
                    if(hasSpear && mState.RightButton == ButtonState.Pressed && mState.X > XPosition)
                    {
                        pState = PlayerStates.AimingRight;
                    }
                    if (!hasSpear || mState.RightButton == ButtonState.Released)
                    {
                        pState = PlayerStates.LookingLeft;
                    }
                    playerAnimations["throwing"].UpdateAnimation(gameTime);
                    break;
                // see lines 155-162
                case PlayerStates.LookingRight:
                    velocity.X = 0;
                    velocity.Y = 0;
                    if (Game1.SinglePress(Keys.A))
                    {
                        pState = PlayerStates.MovingLeft;
                    }
                    if (Game1.SinglePress(Keys.D))
                    {
                        pState = PlayerStates.MovingRight;
                    }
                    if (Game1.SinglePress(Keys.W))
                    {
                        this.velocity.Y -= 40f;
                        pState = PlayerStates.SingleJump;
                    }
                    if (hasSpear && mState.RightButton == ButtonState.Pressed && mState.X > XPosition) 
                    {
                        pState = PlayerStates.AimingRight;
                    }
                    else if(hasSpear && mState.RightButton == ButtonState.Pressed)
                    {
                        pState = PlayerStates.AimingLeft;
                    }
                    if (Game1.SingleLeftClick())
                    {
                        thrustTimer = thrustDuration;
                        pState = PlayerStates.ThrustingRight;
                    }
                    playerAnimations["standing"].UpdateAnimation(gameTime);
                    break;
                // see lines 194-203
                case PlayerStates.MovingRight:
                    //this.position.X += 5;                       OLD
                    this.velocity = new Vector2(speed, 0);    // NEW
                    velocity.Y = 0;

                    if (kbState.IsKeyUp(Keys.D))
                    {
                        pState = PlayerStates.LookingRight;
                    }
                    if (Game1.SinglePress(Keys.A))
                    {
                        pState = PlayerStates.MovingLeft;
                    }
                    if (mState.RightButton == ButtonState.Pressed)
                    {
                        pState = PlayerStates.AimingRight;
                    }
                    if (Game1.SinglePress(Keys.W))
                    {
                        this.velocity.Y -= 40f;
                        pState = PlayerStates.SingleJump;
                    }
                    if (Game1.SingleLeftClick())
                    {
                        thrustTimer = thrustDuration;
                        pState = PlayerStates.ThrustingRight;
                    }
                    playerAnimations["moving"].UpdateAnimation(gameTime);
                    break;
                // see lines 235-241
                case PlayerStates.AimingRight:
                    velocity.X = 0;
                    if (hasSpear && Game1.SingleLeftClick())
                    {
                        pState = PlayerStates.Throwing;
                    }
                    if (hasSpear && mState.RightButton == ButtonState.Pressed && mState.X < XPosition)
                    {
                        pState = PlayerStates.AimingLeft;
                    }
                    if (!hasSpear || mState.RightButton == ButtonState.Released)
                    {
                        pState = PlayerStates.LookingRight;
                    }
                    playerAnimations["throwing"].UpdateAnimation(gameTime);
                    break;
                // this state handles what the player is able to do in the air
                // player can press W again to go to double jump state
                // otherwise after velocity goes down to 0, player
                // automatically goes to falling state which pushes them back down to the
                // ground
                case PlayerStates.SingleJump:
                    // allows for spear to be recalled while in air
                    if (!hasSpear && Game1.SingleLeftClick())
                    {
                        spear.State = SpearState.Recalled;
                    }
                    velocity.Y += 3;
                    if (Game1.SinglePress(Keys.W))
                    {
                        pState = PlayerStates.DoubleJump;
                    }
                    
                    if(velocity.Y >= 0)
                    {
                        pState = PlayerStates.Falling;
                    }
                    playerAnimations["falling"].UpdateAnimation(gameTime);
                    break;
                // the double jump adds a small amount of height
                // but it moreso pushs the player horizontally to act as a midair
                // dash to get from one side of the boat to the other quickly
                case PlayerStates.DoubleJump:
                    // allows for spear to be recalled
                    if (!hasSpear && Game1.SingleLeftClick())
                    {
                        spear.State = SpearState.Recalled;
                    }

                    velocity.Y += 1.5f;
                    velocity.X = 0;
                    if (spriteEffects == SpriteEffects.FlipHorizontally)
                    {
                        this.velocity.X = -20f;
                    }
                    else
                    {
                        this.velocity.X = 20f;
                    }
                    if (velocity.Y > 0)
                    {
                        pState = PlayerStates.Falling;
                    }
                    playerAnimations["falling"].UpdateAnimation(gameTime);
                    break;
                // this state simulates gravity with it pushing the player down
                // however, in this state the player is able to influence
                // their trajectory somewhat, you are able to double jump in this state
                // however it is not enough to keep the player permanantly in air
                case PlayerStates.Falling:
                    velocity.Y += 3;
                    // allows for spear to be recalled
                    if (!hasSpear && Game1.SingleLeftClick())
                    {
                        spear.EnemyStuckTo = null;
                        spear.State = SpearState.Recalled;
                    }

                    // this code allows for minor mid air movements
                    if (velocity.X >= 0 &&  kbState.IsKeyDown(Keys.A))
                    {
                        velocity.X -= speed / 2;
                        spriteEffects = SpriteEffects.FlipHorizontally;
                    }
                    if (velocity.X <= 0 && kbState.IsKeyDown(Keys.D))
                    {
                        velocity.X += speed / 2;
                        spriteEffects = SpriteEffects.None;
                    }
                    if (Game1.SinglePress(Keys.W))
                    {
                        pState = PlayerStates.DoubleJump;
                    }
                    playerAnimations["falling"].UpdateAnimation(gameTime);
                    break;
                // interacts with spear class
                // this states only purpose is to update call the throw spear
                // method and reset player state aftwards
                case PlayerStates.Throwing:
                    spear.EnemyStuckTo = null;
                    ThrowSpear(mState, spear);
                    hasSpear = false;
                    if(prevPState == PlayerStates.AimingLeft)
                    {
                        pState = PlayerStates.LookingLeft;
                    }
                    else
                    {
                        pState = PlayerStates.LookingRight;
                    }
                    playerAnimations["throwing"].UpdateAnimation(gameTime);
                    break;
                // identical to thrusting right state so I will only comment on this state
                // if the player has the spear, then a timer will kick off,
                // the player will stay in the thrusting state until the timer ends
                // while it is going, the spear moves to the thrusting position
                // after the timer has ended, the player will revert to a looking position
                // if the player does not have the spear it will set player to recalling state
                case PlayerStates.ThrustingLeft:
                    if (hasSpear)
                    {
                        // thrust the spear for a set duration
                        if (thrustTimer > 0.0)
                        {
                            thrustTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else if (velocity.X == 0)
                        {
                            pState = PlayerStates.LookingLeft;
                        }
                        else
                        {
                            pState = PlayerStates.MovingLeft;
                        }
                        
                    }
                    else
                    {
                        pState = PlayerStates.RecallingSpear;
                    }
                    playerAnimations["thrusting"].UpdateAnimation(gameTime);
                    break;
                // see lines 433-438
                case PlayerStates.ThrustingRight:
                    if (hasSpear)
                    {
                        // thrust the spear for a set duration
                        if (thrustTimer > 0.0)
                        {
                            thrustTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else if (velocity.X == 0)
                        {
                            pState = PlayerStates.LookingRight;
                        }

                        else
                        {
                            pState = PlayerStates.MovingRight;
                        }
                    }
                    else
                    {
                        pState = PlayerStates.RecallingSpear;
                    }
                    playerAnimations["thrusting"].UpdateAnimation(gameTime);
                    break;
                // this state is what triggers the spear to go to its own
                // recalling state which runs a method in the spear class
                // that then gets the spear back to the player, afterwards
                // depending on the sprite effected, it will revert the player to a looking state
                case PlayerStates.RecallingSpear:
                    spear.State = SpearState.Recalled;
                    if(SprtEffct == SpriteEffects.FlipHorizontally)
                    {
                        pState = PlayerStates.LookingLeft;
                    }
                    else
                    {
                        pState = PlayerStates.LookingRight;
                    }
                    break;
            }
            // sets previous player state to current player state
            prevPState = pState;

            // if the player is next to a wall, don't let them move that way
            if(isAgainstLeftWall == true && velocity.X < 0)
            {
                velocity.X = 0;
            }
            if (isAgainstRightWall == true && velocity.X > 0)
            {
                velocity.X = 0;
            }

            // iFrames ensure that after being attacked by an enemy, the player
            // will be invincible for a short time in order to not get hit
            // 60 times a second and insta killed. iFrames work sort of on a timer
            // if the player has iFrames, remove them
            if (iFrames > 0)
            {
                iFrames--;
            }

            // bounds of the player updated each frame so that hitbox properly matches the spritea
            // being displayed on the screen position wise
            bounds = new Rectangle((int)XPosition, (int)YPosition, texture.Width, texture.Height);
            base.Update(); // applys velocity and acceleration
        }

        // this draw method looks at the state of the player, then plays an
        // animation which using the DrawPlayerAnimation method, by passing in the key for
        // whatever animation needs to be drawn.
        public void DrawPlayer(SpriteBatch sb)
        {
            switch (pState)
            {
                case PlayerStates.LookingLeft:
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    // code to convert to using animation spritesheet
                    DrawPlayerAnimation("standing", spriteEffects, sb);
                    break;
                case PlayerStates.MovingLeft:
                    // in the future this will be an actual running animation
                    // for now it just flips the sprite
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    DrawPlayerAnimation("moving", SpriteEffects, sb);
                    break;
                case PlayerStates.LookingRight:
                    // code to convert to using animation spritesheet
                    spriteEffects = SpriteEffects.None;
                    DrawPlayerAnimation("standing", spriteEffects, sb);
                    break;
                case PlayerStates.MovingRight:
                    // in the future this will be an actual running animation
                    spriteEffects = SpriteEffects.None;
                    DrawPlayerAnimation("moving", SpriteEffects, sb);
                    break;
                case PlayerStates.Dead:
                    rotation = 90f;
                    break;
                // since there is only 1 animation for jumping 
                // uses falldown cases to play the same animation for all 3 
                // mid air states
                case PlayerStates.Falling:
                case PlayerStates.SingleJump:
                case PlayerStates.DoubleJump:
                    DrawPlayerAnimation("falling", SpriteEffects, sb);
                    break;
                // same idea as jumping but with the throwing state
                case PlayerStates.Throwing:
                case PlayerStates.AimingLeft:
                case PlayerStates.AimingRight:
                    DrawPlayerAnimation("throwing", SpriteEffects, sb);
                    break;
                // animation is for both thrusting is the same
                // so uses falldown case
                case PlayerStates.ThrustingLeft:
                case PlayerStates.ThrustingRight:
                    DrawPlayerAnimation("thrusting", SpriteEffects, sb);
                    break;
                    
            }
            if(IFrames > 0)
            {
                color = Color.OrangeRed;
            }
            else
            {
                color = Color.White;
            }
            // using animations so this line introduces bugs so it is commented out
            //base.Draw(sb);
        }


        /// <summary>
        /// Throws the spear, updating its state and velocity
        /// </summary>
        /// <param name="mState"> mouse position</param>
        /// <param name="spear"> spear</param>
        public void ThrowSpear(MouseState mState, Spear spear)
        {
            // calculates angle to throw spear based on mouse position using trigonometry
            double distanceToCursorX = mState.X - this.GetCenter().X;
            double distanceToCursorY = mState.Y - this.GetCenter().Y;
            double throwingAngle = Math.Atan(distanceToCursorY / distanceToCursorX);
            double xSpeed = 40 * Math.Cos(throwingAngle);
            double ySpeed = 40 * Math.Sin(throwingAngle);
            // checks to see that the cursor location and speed match up
            if ((distanceToCursorX < 0 && xSpeed > 0) || (distanceToCursorX > 0 && xSpeed < 0))
            {
                xSpeed = xSpeed * -1;
            }
            if ((distanceToCursorY < 0 && ySpeed > 0) || (distanceToCursorY > 0 && ySpeed < 0))
            {
                ySpeed = ySpeed * -1;
            }
            // after finding angle, adds velocity to the spear in x and y direction
            spear.Velocity = new Vector2((float)xSpeed, (float)ySpeed);
            spear.YPosition = spear.Position.Y + 75;
            // Updates the spears state to the thrown state 
            spear.State = SpearState.Thrown;
        }

        // used to add animations to the playeAnimation dictionary
        // since animations are created in Game1, need some what to get them into player class
        // method takes string for key and the animation to add, then uses built in dictionary
        // add method
        public void AddAnimation(string key, Animation a)
        {
            playerAnimations.Add(key, a);
        }

        // this helper method is responsible for drawing all the animations
        // a key is passed in to tell method what animation to use from the dictionary
        // the sprite effect is given so that animations can be drawn left or right
        // then uses draw method and code from Eric Cascioli's Mario Walking PE (source rectangle)
        // to draw each frame of a particular animation
        public void DrawPlayerAnimation(string animKey, SpriteEffects flip, SpriteBatch sb)
        {
            sb.Draw(
                playerAnimations[animKey].SpriteSheet,
                localPosition,
                new Rectangle(playerAnimations[animKey].SpriteWidth * playerAnimations[animKey].CurrentFrame, 0,
                playerAnimations[animKey].SpriteWidth, playerAnimations[animKey].SpriteHeight),
                color,
                rotation,
                Vector2.Zero,
                1.0f,
                flip,
                0f);
        }

        // these methods were used a certain points during developement but are now
        // not needed
        #region Unused Methods
        // this code does work
        // method is no longer in use as we realized that 
        // we did not need a seperate method to handle jumping 
        // and it could all be handled inside players update method
        /*
        public void Jump(float yVel)
        {
            switch (pState)
            {
                case PlayerStates.Falling:
                    velocity.Y -= yVel;
                    acceleration.Y += 1;
                    break;

                case PlayerStates.SingleJump:
                    velocity.Y -= yVel;
                    acceleration.Y += 1;
                    break;

                case PlayerStates.DoubleJump:
                    // checks X velocity here to see if player is moving left or right
                    // if x velocity is positive it means he is moving to the right
                    // otherwise he is moving to the left
                    if (velocity.X >= 0)
                    {
                        velocity += new Vector2(10, -yVel / 2);
                        acceleration += new Vector2(-1, 1);
                    }
                    else
                    {
                        velocity += new Vector2(-10, -yVel / 2);
                        acceleration += new Vector2(1, 1);
                    }
                    
                    break;
            }
        }
        */
        // helper methods to draw player in different states
        /*
        public void DrawPlayerStanding(SpriteEffects flip, SpriteBatch sb)
        {

            sb.Draw(
                playerAnimations["standing"].SpriteSheet,
                LocalPosition,
                new Rectangle(playerAnimations["standing"].SpriteWidth * playerAnimations["standing"].CurrentFrame, 
                0, playerAnimations["standing"].SpriteWidth, playerAnimations["standing"].SpriteHeight), 
                Color.White, 
                0f, 
                Vector2.Zero,
                1.0f,
                flip, 
                0f);
        }

        public void DrawPlayerMoving(SpriteEffects flip, SpriteBatch sb)
        {
            sb.Draw(
                playerAnimations["moving"].SpriteSheet,
                LocalPosition,
                new Rectangle(playerAnimations["moving"].SpriteWidth * playerAnimations["moving"].CurrentFrame, 0, 
                playerAnimations["moving"].SpriteWidth, playerAnimations["moving"].SpriteHeight),
                Color.White,
                0f,
                Vector2.Zero,
                1.0f,
                flip,
                0f);
        }

        public void DrawPlayerFalling(SpriteEffects flip, SpriteBatch sb)
        {
            sb.Draw(
                playerAnimations["falling"].SpriteSheet,
                LocalPosition,
                new Rectangle(playerAnimations["falling"].SpriteWidth * playerAnimations["falling"].CurrentFrame, 0,
                playerAnimations["falling"].SpriteWidth, playerAnimations["falling"].SpriteHeight),
                Color.White,
                0f,
                Vector2.Zero,
                1.0f,
                flip,
                0f);
        }

        public void DrawPlayerThrowing(SpriteEffects flip, SpriteBatch sb)
        {
            sb.Draw(
                playerAnimations["throwing"].SpriteSheet,
                LocalPosition,
                new Rectangle(playerAnimations["throwing"].SpriteWidth * playerAnimations["throwing"].CurrentFrame, 0,
                playerAnimations["throwing"].SpriteWidth, playerAnimations["throwing"].SpriteHeight),
                Color.White,
                0f,
                Vector2.Zero,
                1.0f,
                flip,
                0f);
        }
        */
        #endregion

       
        
    }
}