using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace tranquility
{
    public class Level01 : LevelComponent
    {
        public Level01(Game1 game, GameMode gameMode)
            : base(game, gameMode, "Content\\level_01.xml")
        {

        }

        public override void Initialize()
        {

        }

        public override void Update(GameTime gameTime)
        {
            // any level-specific logic might go here
        }
    }
}
