using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tranquility
{
    class VictoryScreen
    {
        MenuComponent menuComponent;
        Texture2D image;
        Rectangle imageRec;
        Game1 game;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
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

        public VictoryScreen(Game1 game, SpriteBatch spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            
            spriteFont = game.Content.Load<SpriteFont>("menufont");
            image = game.Content.Load<Texture2D>("door");
            string[] menuItems = { "Restart Game", "End Game" };
            menuComponent = new MenuComponent(game, spriteBatch, spriteFont, menuItems);
            game.Components.Add(menuComponent);
            imageRec = new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
        }

        public void Update(GameTime gameTime)
        {
            keyboardstate = Keyboard.GetState();
            if (CheckKey(Keys.Enter))
            {
                if (SelectedItem == 0)
                {
                    ((Game1)game).Disable(menuComponent);
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
