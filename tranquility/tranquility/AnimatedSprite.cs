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
    public class AnimatedSprite 
    {
        
        Texture2D spriteTexture;
        float timer = 0f;
        float interval = 200f;
        int currentFrame = 0;
        int spriteWidth = 32;
        int spriteHeight = 31;

        Rectangle sourceRect;
        Vector3 position;
        Vector2 origin;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }


        public Texture2D Texture
        {
            get { return spriteTexture; }
            set { spriteTexture = value; }
        }


        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        public AnimatedSprite(Texture2D texture, int currentFrame, int spriteWidth, int spriteHeight)
        {
            this.spriteTexture = texture;
            this.currentFrame = currentFrame;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.origin = new Vector2(spriteWidth / 2, spriteHeight / 2);
            this.sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
        }

        public void HandleSpriteMovement(GameTime gameTime)
        {
            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
        }

        public void AnimateCircle(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {

            }

        }

        public void AnimateRight(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 14 || currentFrame < 10)
                {
                    currentFrame = 10;
                }
                timer = 0f;
            }
        }


        public void AnimateBackward(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 19 || currentFrame < 15)
                {
                    currentFrame = 15;
                }
                timer = 0f;
            }
        }

        public void AnimateForward(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 4)
                {
                    currentFrame = 0;
                }
                timer = 0f;
            }
        }

        public void AnimateLeft(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 9 || currentFrame < 5)
                {
                    currentFrame = 5;
                }
                timer = 0f;
            }
        }
    }
}
