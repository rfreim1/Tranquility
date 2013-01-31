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
using System.Xml.Linq;


namespace tranquility
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class LevelComponent : Microsoft.Xna.Framework.GameComponent
    {
        public string WallsFile { get; set; }

        public static Vector3 Scaler = new Vector3(2, 2, 2);

        public List<Wall> Walls { 
            get { return walls;} 
            set {walls = value;} }
        private List<Wall> walls;

        public List<Coin> Coins {
            get { return coins; }
            set { coins = value;}
        }
        private List<Coin>  coins;

        public List<Trap> Traps
        {
            get { return traps; }
            set { traps = value; }
        }

        public Goal Goal;

        private List<Trap> traps;

        GameMode gameMode;

        public LevelComponent(Game1 game, GameMode gameMode, string wallsFile)
            : base((Game)game)
        {
            this.gameMode = gameMode;
            WallsFile = wallsFile;
            System.IO.Stream stream = TitleContainer.OpenStream(wallsFile);
            XDocument doc = XDocument.Load(stream);
            
            Walls = ReadWalls(doc);
            Coins = ReadCoins(doc);
            Traps = ReadTraps(doc);

            Goal = ReadGoal(doc);

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override abstract void Initialize();

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override abstract void Update(GameTime gameTime);

        List<Wall> ReadWalls(XDocument doc)
        {
            List<Wall> walls = new List<Wall>();
            walls = (from wall in doc.Descendants("wall")
                     select new Wall(Game, gameMode)
                     {
                         Position = (new Vector3((float)Convert.ToDouble(wall.Element("positionX").Value),
                                             (float)Convert.ToDouble(wall.Element("positionY").Value),
                                             (float)Convert.ToDouble(wall.Element("positionZ").Value))) * LevelComponent.Scaler,

                         Size = new Vector3((float)Convert.ToDouble(wall.Element("sizeX").Value),
                                         (float)Convert.ToDouble(wall.Element("sizeY").Value),
                                         (float)Convert.ToDouble(wall.Element("sizeZ").Value)) * LevelComponent.Scaler
                     }).ToList();
            return walls;
        }

        List<Coin> ReadCoins(XDocument doc)
        {
            List<Coin> coins;

            coins = (from coin in doc.Descendants("coin")
                     select new Coin(Game, gameMode)
                     {
                         Position = new Vector3(
                             (float)Convert.ToDouble(coin.Element("positionX").Value),
                             (float)Convert.ToDouble(coin.Element("positionY").Value),
                             (float)Convert.ToDouble(coin.Element("positionZ").Value)
                        ) * LevelComponent.Scaler
                     }).ToList();
            return coins;
        }


        List<Trap> ReadTraps(XDocument doc)
        {
            List<Trap> traps;

            traps = (from trap in doc.Descendants("trap")
                     select new Trap(Game, gameMode)
                     {
                         Position = new Vector3(
                             (float)Convert.ToDouble(trap.Element("positionX").Value),
                             (float)Convert.ToDouble(trap.Element("positionY").Value),
                             (float)Convert.ToDouble(trap.Element("positionZ").Value)
                         ) * LevelComponent.Scaler
                     }).ToList();
            return traps;
        }

        Goal ReadGoal(XDocument doc)
        {
            List<Goal> goals;
            goals = (from goal in doc.Descendants("goal")
                     select new Goal(
                         (Game1)Game,
                         gameMode,
                         new Vector3(
                             (float)Convert.ToDouble(goal.Element("positionX").Value),
                             (float)Convert.ToDouble(goal.Element("positionY").Value),
                             (float)Convert.ToDouble(goal.Element("positionZ").Value)
                         ) * LevelComponent.Scaler,
                         new Vector3(
                              (float)Convert.ToDouble(goal.Element("sizeX").Value),
                              (float)Convert.ToDouble(goal.Element("sizeY").Value),
                              (float)Convert.ToDouble(goal.Element("sizeZ").Value)
                         ) * LevelComponent.Scaler
                    )).ToList();

            return goals.First();
        }
    }
}
