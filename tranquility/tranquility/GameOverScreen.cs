using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace tranquility
{
    class GameOverScreen
    {
        MenuComponent menuComponent;
        Texture2D image;
        Rectangle imageRec;
        Game1 game;
        SpriteBatch spriteBatch;
        public AnimatedSprite Sprite;
        SpriteFont spriteFont;
        List<Bat> ListOBats = new List<Bat>();
        private Random rand;

        KeyboardState keyboardstate;
        KeyboardState oldkeyboardstate;

        public int SelectedItem
        {
            get { return menuComponent.selectedItem; }
            set { menuComponent.selectedItem = value; }
        }

        public bool CheckKey(Keys theKey)
        {
            return keyboardstate.IsKeyUp(theKey) && oldkeyboardstate.IsKeyDown(theKey);
        }

        public GameOverScreen(Game1 game, SpriteBatch spriteBatch, AnimatedSprite Sp)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.Sprite = Sp;

            spriteFont = game.Content.Load<SpriteFont>("menufont");
            image = game.Content.Load<Texture2D>("blood");
            string[] menuItems = { "Restart Game", "End Game" };
            menuComponent = new MenuComponent(game, spriteBatch, spriteFont, menuItems);
            game.Components.Add(menuComponent);
            imageRec = new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            
            // Start Bat placement into list
            rand = new Random();

            for (int i = 0; i < 60; i++)
            {
                Bat b = new Bat(game, spriteBatch, Sprite);
                b.Position = new Vector2(400 + (float)(-50 + rand.NextDouble() * 100),
                                         200 + (float)(-50 + rand.NextDouble() * 100));
                b.speed = 20f + (float)rand.NextDouble() * 40.0f;
                ListOBats.Add(b);
                game.Components.Add(b);
            }
            for (int j = 0; j < 60; j++)
            {
                ListOBats[j].batList = ListOBats;
            }
            // end bat placement into list  
        }

        public void Update(GameTime gameTime)
        {
            keyboardstate = Keyboard.GetState();
            if (CheckKey(Keys.Enter))
            {
                if (SelectedItem == 0)
                {
                    ((Game1)game).Disable(menuComponent);
                    foreach (Bat b in ListOBats)
                        ((Game1)game).Disable(b);
                    game.startGameMode();
                }
                if (SelectedItem == 1)
                    game.Exit();
            }
            oldkeyboardstate = keyboardstate;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(image, imageRec, Color.White);
        }
    }
}
