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
    public class RedLens : Lens
    {
        public RedLens(Game game, GameMode gameMode)
            : base(game, gameMode)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            DieRate = 1f / (20 * 1000);           // It takes 5 seconds for this lens to die.
            RechargeRate = 0;// 1f / (10 * 1000);     // 10 seconds to recharge

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
