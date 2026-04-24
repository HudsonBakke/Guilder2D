using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

/// <summary>
/// This enum is used for screens to communicate with ScreenManager
/// </summary>
public enum ScreenSwitch
{
    NoSwitch,
    Menu,
    NewWorld,
    Settings,
    Sub,
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
    /// Filled with a value that is read by ScreenManager
    /// </summary>
    public Screen? SubScreen { get; protected set; } = null;

    /// <summary>
    /// Resets ScreenSwitch to NoSwitch. To be called when this screen is returned to after a subscreen is closed
    /// </summary>
    public void ResetSwitch()
    {
        NextScreen = ScreenSwitch.NoSwitch;
        SubScreen = null;
    }

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