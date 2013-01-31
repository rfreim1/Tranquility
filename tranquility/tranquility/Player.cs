using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace tranquility
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        public Vector3 Position { get; private set; }
        private Vector3 Velocity;
        private const float MaxSpeed = 6;
        private bool fixedCollision;
        private Vector3 Widths;
        private float EyeHeight;
        private MouseState previousMouseState;
        public float xAng { get; private set; }
        public float yAng { get; private set; }
        private bool _mouseLock;

        public int Health { get; private set; }

        /// <summary>
        /// EyePosition returns the position of the player's eyes.
        /// </summary>
        public Vector3 EyePosition { get; private set; }

        /// <summary>
        /// BoundingBox is the player's BoundingBox.
        /// </summary>
        public BoundingBox BoundingBox { get; private set; }

        /// <summary>
        /// LookPoint returns a Vector3 that represents a point of unit length away from the EyePoint where the player is looking.
        /// </summary>
        public Vector3 LookPoint { get; private set; }

        /// <summary>
        /// Creates the player.
        /// </summary>
        /// <param name="position">Position of the player.</param>
        /// <param name="widths">Widths of the player's bounding box.</param>
        /// <param name="eyeHeight">y-height from player's position where the player's eyes are</param>
        public Player(Game game,Vector3 position, Vector3 widths, float eyeHeight,float facingAng)
            : base(game)
        {
            // TODO: Construct any child components here
            Position = position;
            Widths = widths;
            EyeHeight = eyeHeight;
            yAng = facingAng;
            xAng = 0;
            previousMouseState = Mouse.GetState();
            Velocity = Vector3.Zero;
            fixedCollision = false;
            EyePosition = Position + new Vector3(0, EyeHeight, 0);
            Health = 3;
            _mouseLock = true;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            // Code for looking around
            MouseState currentMouseState = Mouse.GetState();
            int deltaX = currentMouseState.X - previousMouseState.X;
            int deltaY = currentMouseState.Y - previousMouseState.Y;
            Point windowCenter = Game.Window.ClientBounds.Center;
            if(_mouseLock && Game.IsActive)
                Mouse.SetPosition(windowCenter.X, windowCenter.Y);
            previousMouseState = Mouse.GetState();
            yAng += ((float) deltaX)/2f; //change in the mouse's x postiion means change in rotation about y axis
            xAng -= ((float) deltaY)/2f; //change in the mouse's y position means change in rotation about x axis
            if (xAng > 90) xAng = 90;
            if (xAng < -90) xAng = -90;
           
            // code for moving around
            KeyboardState keyboardState = Keyboard.GetState();
            Vector3 velocity = Vector3.Zero;
            float goalSpeed = 0;
            if (keyboardState.IsKeyDown(Keys.W))
            {
                velocity += new Vector3((float) Math.Cos(MathHelper.ToRadians(yAng)), 0,
                                       (float) Math.Sin(MathHelper.ToRadians(yAng)));
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                velocity += new Vector3((float)Math.Cos(MathHelper.ToRadians(yAng+180)), 0,
                                       (float)Math.Sin(MathHelper.ToRadians(yAng+180)));
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                velocity += new Vector3((float)Math.Cos(MathHelper.ToRadians(yAng-90)), 0,
                                       (float)Math.Sin(MathHelper.ToRadians(yAng-90)));
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                velocity += new Vector3((float)Math.Cos(MathHelper.ToRadians(yAng+90)), 0,
                                       (float)Math.Sin(MathHelper.ToRadians(yAng+90)));
            }
            if(velocity.LengthSquared()>0)
            {
                velocity.Normalize();
                goalSpeed = MaxSpeed;
            }
            Velocity += (goalSpeed*velocity - Velocity) * .1f;
            Position += Velocity * gameTime.ElapsedGameTime.Milliseconds / 1000;

            //update eyeposition
            EyePosition = Position + new Vector3(0, EyeHeight, 0);

            //update eye look at point
            LookPoint = new Vector3((float)Math.Cos(MathHelper.ToRadians(yAng)) * (float)Math.Cos(MathHelper.ToRadians(xAng)), 
                                    (float)Math.Sin(MathHelper.ToRadians(xAng)),
                                    (float)Math.Sin(MathHelper.ToRadians(yAng)) * (float)Math.Cos(MathHelper.ToRadians(xAng))) + EyePosition;

            //update bounding box
            Vector3 minPoint = Position - new Vector3(Widths.X / 2, 0, Widths.Z / 2);
            BoundingBox = new BoundingBox(minPoint, minPoint + Widths);

            //reset collision stuff
            fixedCollision = false;
            

            base.Update(gameTime);
        }
        public void Collided(GameTime gameTime)
        {
            if(!fixedCollision)
            {
                Position -= Velocity*gameTime.ElapsedGameTime.Milliseconds/1000;
                fixedCollision = true;
            }
        }
        public void Damage(int damage)
        {
            Health -= damage;
        }
        public void MouseLocked(bool locked)
        {
            _mouseLock = locked;
        }
    }
}
