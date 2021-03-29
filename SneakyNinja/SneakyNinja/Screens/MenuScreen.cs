// taken from the GameArchitecture assignment and modified as needed
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SneakyNinja.Screens;



namespace SneakyNinja.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        private SneakyNinjas game;
        public MainMenuScreen(SneakyNinjas game) : base("Sneaky Ninjas", game)
        {
            this.game = game;
            var playGameMenuEntry = new MenuEntry("Play Game");
            var optionsMenuEntry = new MenuEntry("Options");
            var instructionsMenuEntry = new MenuEntry("Hints");
            var exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            instructionsMenuEntry.Selected += HintMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(instructionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }
        public static void Load(ScreenManager screenManager, SneakyNinjas Game)
        {
            var m = new MainMenuScreen(Game);
            screenManager.AddScreen(m);
        }
        private void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
            TopLeftCornerRoomScreen.Load(ScreenManager, game, new PlayerSprite(game, new Vector2(0,0), new Vector2(64, 64)));
        }
        private void HintMenuEntrySelected(object sender, EventArgs e)
        {
            HintScreen.Load(ScreenManager, game);
        }
        private void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            // ScreenManager.AddScreen(new OptionsMenuScreen());
            OptionsScreen.Load(ScreenManager, game);
        }

        protected override void OnCancel()
        {
            ScreenManager.Game.Exit();
        }
    }
    public class MenuScreen : GameScreen
    {
        private readonly List<MenuEntry> _menuEntries = new List<MenuEntry>();
        private int _selectedEntry;
        private readonly string _menuTitle;

        private readonly InputAction _menuUp;
        private readonly InputAction _menuDown;
        private readonly InputAction _menuSelect;
        private readonly InputAction _menuCancel;
        private SneakyNinjas game;

        // Gets the list of menu entries, so derived classes can add or change the menu contents.
        protected IList<MenuEntry> MenuEntries => _menuEntries;

        public MenuScreen(string menuTitle, SneakyNinjas game)
        {
            _menuTitle = menuTitle;
            this.game = game;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _menuUp = new InputAction(
                new[] { Keys.Up, Keys.W }, true);
            _menuDown = new InputAction(
                new[] { Keys.Down, Keys.S }, true);
            _menuSelect = new InputAction(
                new[] { Keys.Enter, Keys.Space }, true);
            _menuCancel = new InputAction(
                new[] { Keys.Back }, true);
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < _menuEntries.Count; i++)
            {
                bool isSelected = IsActive && i == _selectedEntry;
                _menuEntries[i].Update(this, isSelected, gameTime);
            }
        }
        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }
        protected virtual void OnCancel()
        {
            ExitScreen();
        }
        // Responds to user input, changing the selected entry and accepting or cancelling the menu.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            // For input tests we pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;

            if (_menuUp.Occurred(input))
            {
                _selectedEntry--;

                if (_selectedEntry < 0)
                    _selectedEntry = _menuEntries.Count - 1;
            }

            if (_menuDown.Occurred(input))
            {
                _selectedEntry++;

                if (_selectedEntry >= _menuEntries.Count)
                    _selectedEntry = 0;
            }

            if (_menuSelect.Occurred(input))
                OnSelectEntry(_selectedEntry);
            else if (_menuCancel.Occurred(input))
                ExitScreen();
        }
        protected virtual void OnSelectEntry(int entryIndex)
        {
            _menuEntries[entryIndex].OnSelectEntry();
        }
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            var position = new Vector2(0f, 175f);

            // update each menu entry's location in turn
            foreach (var menuEntry in _menuEntries)
            {
                // each entry is to be centered horizontally
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this);
            }
        }
        public void DrawNonSpriteBatch(GameTime gameTime)
        {
            UpdateMenuEntryLocations();

            var graphics = ScreenManager.GraphicsDevice;
            var spriteBatch = ScreenManager.SpriteBatch;
            var font = ScreenManager.Font;


            for (int i = 0; i < _menuEntries.Count; i++)
            {
                var menuEntry = _menuEntries[i];
                bool isSelected = IsActive && i == _selectedEntry;
                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            var titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            var titleOrigin = font.MeasureString(_menuTitle) / 2;
            var titleColor = new Color(192, 192, 192) * TransitionAlpha;
            const float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, _menuTitle, titlePosition, titleColor,
                0, titleOrigin, titleScale, SpriteEffects.None, 0);

        }
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            var graphics = ScreenManager.GraphicsDevice;
            var spriteBatch = ScreenManager.SpriteBatch;
            var font = ScreenManager.Font;

            spriteBatch.Begin();

            for (int i = 0; i < _menuEntries.Count; i++)
            {
                var menuEntry = _menuEntries[i];
                bool isSelected = IsActive && i == _selectedEntry;
                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            var titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            var titleOrigin = font.MeasureString(_menuTitle) / 2;
            var titleColor = new Color(192, 192, 192) * TransitionAlpha;
            const float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, _menuTitle, titlePosition, titleColor,
                0, titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
    public class MenuEntry
    {
        private string text;
        private Vector2 position;
        private float selectionFade;    // Entries transition out of the selection effect when they are deselected
        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public string Text => text;

        public event EventHandler<EventArgs> Selected;
        protected internal virtual void OnSelectEntry()
        {
            Selected?.Invoke(this, null);
        }
        public MenuEntry(string text)
        {
            this.text = text;
        }
        public MenuEntry(string text, Vector2 pos)
        {
            this.text = text;
            position = pos;
        }
        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            Selected?.Invoke(this, new EventArgs());
        }
        public void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            var color = isSelected ? Color.Yellow : Color.White;
            var screenManager = screen.ScreenManager;
            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 1 + pulsate * 0.05f * selectionFade;
            // Modify the alpha to fade text out during transitions.

            // Draw text, centered on the middle of each line.
            var spriteBatch = screenManager.SpriteBatch;
            var font = screenManager.Font;

            var origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, color, 0,
                origin, scale, SpriteEffects.None, 0);
        }
        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }

        public virtual int GetWidth(MenuScreen screen)
        {
           return (int)screen.ScreenManager.Font.MeasureString(Text).X;
       
        }
    }

    public class InputAction
    {
        private readonly Keys[] _keys;
        private readonly bool _firstPressOnly;

        // These delegate types map to the methods on InputState. We use these to simplify the Occurred method
        // by allowing us to map the appropriate delegates and invoke them, rather than having two separate code paths.
        private delegate bool ButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex player);
        private delegate bool KeyPress(Keys key);

        /// <summary>
        /// Constructs a new InputMapping, binding the suppled triggering input options to the action
        /// </summary>
        /// <param name="triggerButtons">The buttons that trigger this action</param>
        /// <param name="triggerKeys">The keys that trigger this action</param>
        /// <param name="firstPressOnly">If this action only triggers on the initial key/button press</param>
        public InputAction(Keys[] triggerKeys, bool firstPressOnly)
        {
            // Store the buttons and keys. If the arrays are null, we create a 0 length array so we don't
            // have to do null checks in the Occurred method
            _keys = triggerKeys != null ? triggerKeys.Clone() as Keys[] : new Keys[0];
            _firstPressOnly = firstPressOnly;
        }

        /// <summary>
        /// Determines if he action has occured. If playerToTest is null, the player parameter will be the player that performed the action
        /// </summary> 
        /// <param name="stateToTest">The InputState object to test</param>
        /// <param name="playerToTest">If not null, specifies the player (0-3) whose input should be tested</param>
        /// <param name="player">The player (0-3) who triggered the action</param>
        public bool Occurred(InputState stateToTest)
        {
            // Figure out which delegate methods to map from the state which takes care of our "firstPressOnly" logic
            KeyPress keyTest;

            if (_firstPressOnly)
            {
                keyTest = stateToTest.IsNewKeyPress;
            }
            else
            {
                keyTest = stateToTest.IsKeyPressed;
            }

            // Now we simply need to invoke the appropriate methods for each button and key in our collections
            foreach (var key in _keys)
            {
                if (keyTest(key))
                    return true;
            }

            // If we got here, the action is not matched
            return false;
        }
    }
}
