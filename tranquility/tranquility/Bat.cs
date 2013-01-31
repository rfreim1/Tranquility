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
    /// 

    //public enum BatState { None, Grounded } 

    public class Bat : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public static Texture2D SpriteSheet { get; private set; }
        SpriteBatch spriteBatch;
        public Vector2 Velocity;
        public Rectangle initRect;
        public Vector2 Position { get; set; }

        public Vector2 cohereTraj { get; set; }
        public Vector2 alignTraj { get; set; }
        public Vector2 sepTraj { get; set; }

        public float speed { get; set; }

        private Random rand;
        AnimatedSprite Sprite;
        public List<Bat> batList;
        Game game;

        public Bat(Game g, SpriteBatch sb, AnimatedSprite asp)
            : base(g)
        {
            spriteBatch = sb;
            Sprite = asp;

            Position = Vector2.Zero;
            speed = 50000;
            game = g;
            rand = new Random();
        }


        /// Gets or sets the forward.
        /// </summary>
        
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Draw(Sprite.Texture, Position, Sprite.SourceRect, Color.White, 0f, Sprite.Origin, 1.0f, SpriteEffects.None, 0);
        }

        public bool IsInBoidNeighborhood(Bat other, float minDistance, float maxDistance, float cosMaxAngle)
        {
            if (other == this)
            {
                return false;
            }
            else
            {
                Vector2 offset = other.Position - this.Position;
                float distanceSquared = offset.LengthSquared();

                // definitely in neighborhood if inside minDistance circle
                if (distanceSquared < (minDistance * minDistance))
                {
                    return true;
                }
                else
                {
                    // definitely not in neighborhood if outside maxDistance circle
                    if (distanceSquared > (maxDistance * maxDistance))
                    {
                        return false;
                    }
                    else
                    {
                        // otherwise, test angular offset from forward axis
                        Vector2 unitOffset = offset / (float)Math.Sqrt(distanceSquared);
                        float forwardness = Vector2.Dot((cohereTraj + sepTraj + alignTraj), unitOffset);
                        return forwardness > cosMaxAngle;
                    }
                }
            }
        }


        public Vector2 Cohere(float maxDistance, float cosMaxAngle, List<Bat> flock)
        {
            // steering accumulator and count of neighbors, both initially zero
            Vector2 steering = Vector2.Zero;
            int neighbors = 0;

            // for each of the other vehicles...
            
            Bat other = this;
            for (int j = 0; j < flock.Count; j++)
            {
                if (flock[j] == other)
                    continue;
                Bat otherj = flock[j];
                if (other.IsInBoidNeighborhood(otherj, this.Sprite.SourceRect.Height * 0.3f, maxDistance, cosMaxAngle))
                {
                    // accumulate sum of neighbor's positions
                    steering += otherj.Position;

                    // count neighbors
                    neighbors++;
                }

            }
            // divide by neighbors, subtract off current position to get error-
            // correcting direction, then normalize to pure direction
            if (neighbors > 0)
            {
                steering = ((steering / (float)neighbors) - other.Position);
                steering.Normalize();
            }

            other.cohereTraj = steering;
            //}

            return steering;
        }

        public Vector2 Align(float maxDistance, float cosMaxAngle, List<Bat> flock)
        {
            // steering accumulator and count of neighbors, both initially zero
            Vector2 steering = Vector2.Zero;
            int neighbors = 0;

            // for each of the other vehicles...
            //for (int i = 0; i < flock.Count; i++)
            //{
            Bat other = this;
            for (int j = 0; j < flock.Count; j++)
            {
                if (flock[j] == this)
                    continue;

                Bat otherj = flock[j];
                if (other.IsInBoidNeighborhood(otherj, this.Sprite.SourceRect.Height * 3, maxDistance, cosMaxAngle))
                {
                    // accumulate sum of neighbor's heading
                    steering += (otherj.alignTraj + otherj.cohereTraj + otherj.sepTraj);

                    // count neighbors
                    neighbors++;
                }
            }

            // divide by neighbors, subtract off current heading to get error-
            // correcting direction, then normalize to pure direction
            if (neighbors > 0)
            {
                steering = ((steering / (float)neighbors) - (other.alignTraj + other.cohereTraj + other.sepTraj));
                steering.Normalize();
            }

            other.alignTraj = steering;
            //}

            return steering;
        }

        public Vector2 Separate(float maxDistance, float cosMaxAngle, List<Bat> flock)
        {
            // steering accumulator and count of neighbors, both initially zero
            Vector2 steering = Vector2.Zero;
            int neighbors = 0;

            // for each of the other vehicles...
            //for (int i = 0; i < flock.Count; i++)
            //{
            Bat other = this;
            for (int j = 0; j < flock.Count; j++)
            {
                if (flock[j] == this)
                    continue;

                Bat otherj = flock[j];
                if (other.IsInBoidNeighborhood(otherj, this.Sprite.SourceRect.Height * 3, maxDistance, cosMaxAngle))
                {
                    // add in steering contribution
                    // (opposite of the offset direction, divided once by distance
                    // to normalize, divided another time to get 1/d falloff)
                    Vector2 offset = otherj.Position - other.Position;
                    float distanceSquared = Vector2.Dot(offset, offset);
                    steering += (offset / -distanceSquared);

                    // count neighbors
                    neighbors++;
                }
            }

            // divide by neighbors, then normalize to pure direction
            if (neighbors > 0)
            {
                steering = (steering / (float)neighbors);
                steering.Normalize();
            }

            other.sepTraj = steering;

            //}

            return steering;
        }



        public override void Update(GameTime gameTime)
        {


            float timeMs = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            Cohere(5.0f, 1.1f, batList);
            Separate(5.0f, 1.1f, batList);
            Align(5.0f, 1.1f, batList);


            Bat batty = this;
            Vector2 traj = batty.cohereTraj + batty.sepTraj + batty.alignTraj;
            Position += traj * batty.speed * timeMs;

            Sprite.HandleSpriteMovement(gameTime);
            Sprite.AnimateForward(gameTime);

            base.Update(gameTime);
        }
    }
}  