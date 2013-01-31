using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    public class Lens : Microsoft.Xna.Framework.GameComponent
    {

        // These determine how quickly the lens runs out when activated and how quickly
        // it recharges when inactive.
        GameMode gameMode;

        public float DieRate { get; set; }
        public float RechargeRate { get; set; }

        // A float from 0 to 1 indicating how close the lens is to dead.
        public float Health { get; set; }
        
        public Lens(Game game, GameMode gameMode)
            : base(game)
        {
            // TODO: Construct any child components here
            this.gameMode = gameMode;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Health = 1;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (gameMode.ActiveLens == this)
            {
                
                Health -= DieRate * dt;
                if (Health <= 0)
                {
                    Health = 0;
                    // TODO
                }
            }
            else
            {
                Health += RechargeRate * dt;

                if (Health > 1)
                    Health = 1;
            }

            base.Update(gameTime);
        }
    }
}
