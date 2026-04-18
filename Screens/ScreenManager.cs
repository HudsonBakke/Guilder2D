using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public class ScreenManager
{
    private readonly ScreenStack _screenStack = new();
    private readonly ContentManager _content;
    private GameInput _gameInput;

    public ScreenManager(GraphicsDevice graphicsDevice, ContentManager content)
    {
        // This constructor will eventually load a start game animation, but for now it will just load directly into the main menu
        _content = content;
        _screenStack.Push(new MenuScreen(_content));
    }

    /// <summary>
    /// Set to false when the game should stop running
    /// </summary>
    public bool IsGameRunning { get; private set; } = true;

    /// <summary>
    /// Used to send GameInput into the ScreenManager
    /// </summary>
    /// <param name="gameInput">GameInput object</param>
    public void SendInput(GameInput gameInput)
    {
        _gameInput = gameInput;
    }

    /// <summary>
    /// Used to update the game
    /// </summary>
    /// <param name="gameTime">Current game time</param>
    public void Update(GameTime gameTime)
    {
        Screen? currentScreen = _screenStack.Peek();
        currentScreen?.SendInput(_gameInput);
        currentScreen?.Update(gameTime);
        ScreenSwitch? nextScreen = currentScreen?.NextScreen;

        if (nextScreen != null) 
        {
            switch (nextScreen)
            {
                case ScreenSwitch.Exit:
                    _screenStack.Pop();
                    break;

                case ScreenSwitch.Menu:
                    _screenStack.Clear();
                    _screenStack.Push(new MenuScreen(_content));
                    break;

                case ScreenSwitch.NewWorld:
                    _screenStack.Clear();
                    _screenStack.Push(
                        new WorldScreen(
                            _content, 
                            new Player(_content)
                        )
                    );
                    break;                    
            }
        }

        IsGameRunning = !_screenStack.IsEmpty;
    }

    /// <summary>
    /// Used to draw all screens
    /// </summary>
    /// <param name="spriteBatch">MonoGame SpriteBatch object</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Screen screen in _screenStack)
        {
            screen.Draw(spriteBatch); // Should draw each screen on the stack from bottom to top
        }
    }
    
    private class ScreenStack : IEnumerable<Screen>
    {
        private readonly List<Screen> _stack = [];

        public bool IsEmpty => _stack.Count == 0;

        public IEnumerator<Screen> GetEnumerator() => _stack.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public ScreenStack Push(Screen screen)
        {
            _stack.Add(screen);
            return this;
        }

        public Screen? Pop()
        {
            if (_stack.Count == 0) return null;

            Screen lastScreen = _stack[^1];
            _stack.RemoveAt(_stack.Count - 1);
            return lastScreen;
        }

        public Screen? Peek()
        {
            if (_stack.Count == 0) return null;

            return _stack[^1];
        }

        public void Clear()
        {
            _stack.Clear();
        }
    }
}