using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Guilder2D;

/// <summary>
/// This enum is used for screens to communicate with ScreenManager
/// </summary>
public enum ScreenSwitch
{
    NoSwitch,
    Menu,
    NewWorld,
    Exit
}

public abstract class Screen
{
    protected GameInput _gameInput;
    protected GameTime _prevTick;

    /// <summary>
    /// Signals to the ScreenManager to switch screens. Defaults to ScreenSwitch.NoSwitch
    /// </summary>
    public ScreenSwitch NextScreen { get; protected set; } = ScreenSwitch.NoSwitch;

    /// <summary>
    /// Used to send player input to the screen
    /// </summary>
    /// <param name="gameInput">GameInput object</param>
    public abstract void SendInput(GameInput gameInput);

    /// <summary>
    /// Used to update the map and all its child objects
    /// </summary>
    /// <param name="gameTime">Current game time</param>
    public abstract void Update(GameTime gameTime);

    /// <summary>
    /// Used to draw the map and all its child objects to the screen
    /// </summary>
    /// <param name="spriteBatch">The MonoGame sprite batch</param>
    public abstract void Draw(SpriteBatch spriteBatch);
}