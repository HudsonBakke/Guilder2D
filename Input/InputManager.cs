using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Guilder2D;

/// <summary>
/// Describes all raw hardware inputs
/// </summary>
public enum RawInput
{
    // TO DO: define all raw inputs
    K_W,
    K_S,
    K_A,
    K_D,

    K_UP,
    K_DOWN,
    K_RIGHT,
    K_LEFT,

    MOUSE_LEFT,
    MOUSE_RIGHT
}

/// <summary>
/// Describes all semantic game inputs
/// </summary>
public enum SemanticInput
{
    // TO DO: define all semantic inputs
    MoveUp,
    MoveDown,
    MoveRight,
    MoveLeft,

    Select
}

/// <summary>
/// <list type="bullet">
/// <item><description>Up: Not pressed on either current or previous frame</description></item>
/// <item><description>Pressed: Not pressed on previous frame; pressed on current frame</description></item>
/// <item><description>Released: Pressed on previous frame; not pressed on current frame</description></item>
/// <item><description>Held: Pressed on current and previous frame</description></item>
/// </list>
/// </summary>
public enum PressState
{
    Up,
    Pressed,
    Released,
    Held
}

/// <summary>
/// Manages game input, and the conversion between raw input and semantic input through bindings
/// </summary>
public class InputManager
{
    private Dictionary<SemanticInput, List<RawInput>> _bindings = [];
    private Dictionary<RawInput, PressState> _currentState = [];

    private KeyboardState _currentKeyboardState;
    private MouseState _currentMouseState;
    private GamePadState _currentGamePadState;

    private KeyboardState _previousKeyboardState;
    private MouseState _previousMouseState;
    private GamePadState _previousGamePadState;

    private void SetDefaultButtonStates()
    {
        foreach (RawInput input in Enum.GetValues<RawInput>())
        {
            _currentState[input] = PressState.Up;
        }
    }

    private bool IsRawInputDown(RawInput input, bool currentFrame)
    {
        KeyboardState keyboard = currentFrame ? _currentKeyboardState : _previousKeyboardState;
        MouseState mouse = currentFrame ? _currentMouseState : _previousMouseState;
        GamePadState gamePad = currentFrame ? _currentGamePadState : _previousGamePadState;

        return input switch
        {
            RawInput.K_W => keyboard.IsKeyDown(Keys.W),
            RawInput.K_S => keyboard.IsKeyDown(Keys.S),
            RawInput.K_A => keyboard.IsKeyDown(Keys.A),
            RawInput.K_D => keyboard.IsKeyDown(Keys.D),

            RawInput.K_UP => keyboard.IsKeyDown(Keys.Up),
            RawInput.K_DOWN => keyboard.IsKeyDown(Keys.Down),
            RawInput.K_RIGHT => keyboard.IsKeyDown(Keys.Right),
            RawInput.K_LEFT => keyboard.IsKeyDown(Keys.Left),

            RawInput.MOUSE_LEFT => mouse.LeftButton == ButtonState.Pressed,
            RawInput.MOUSE_RIGHT => mouse.RightButton == ButtonState.Pressed,

            _ => false
        };
    }

    private static PressState ToPressState(bool currentDown, bool previousDown)
    {
        if (currentDown && previousDown) return PressState.Held;
        if (currentDown && !previousDown) return PressState.Pressed;
        if (!currentDown && previousDown) return PressState.Released;
        return PressState.Up;
    }

    
    private PressState GetPressState(RawInput input)
    {
        return ToPressState(
            IsRawInputDown(input, true),
            IsRawInputDown(input, false)
        );
    }

    private static PressState MergePressStates(IEnumerable<PressState> states)
    {
        bool anyPressed = false;
        bool anyHeld = false;
        bool anyReleased = false;

        foreach (PressState state in states)
        {
            if (state == PressState.Pressed) anyPressed = true;
            else if (state == PressState.Held) anyHeld = true;
            else if (state == PressState.Released) anyReleased = true;
        }
        
        if (anyPressed) return PressState.Pressed;
        if (anyHeld) return PressState.Held;
        if (anyReleased) return PressState.Released;
        return PressState.Up;
    }

    private PressState GetSemanticState(SemanticInput semanticInput)
    {
        return MergePressStates(
            _bindings[semanticInput].ConvertAll(raw => _currentState[raw])
        );
    }

    private Point GetVirtualMousePosition(Rectangle presentationRect)
    {
        if (!presentationRect.Contains(_currentMouseState.Position))
            return new Point(-1, -1);

        return new Point(
            (_currentMouseState.X - presentationRect.X) * Guilder2D.VirtualWidth / presentationRect.Width,
            (_currentMouseState.Y - presentationRect.Y) * Guilder2D.VirtualHeight / presentationRect.Height
        );
    }

    public InputManager()
    {
        ResetBindings();
        SetDefaultButtonStates();
    }

    /// <summary>
    /// Call this to read the MonoGame hardware states and update internal state
    /// </summary>
    public void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _previousMouseState = _currentMouseState;
        _previousGamePadState = _currentGamePadState;

        _currentKeyboardState = Keyboard.GetState();
        _currentMouseState = Mouse.GetState();
        _currentGamePadState = GamePad.GetState(PlayerIndex.One);

        foreach (RawInput input in Enum.GetValues<RawInput>())
        {
            _currentState[input] = GetPressState(input);
        }
    }

    /// <summary>
    /// Update an input binding
    /// </summary>
    /// <param name="semanticInput">The semantic input to be updated</param>
    /// <param name="rawInputs">The list of raw inputs to correspond to the semantic input</param>
    public void UpdateBindings(SemanticInput semanticInput, List<RawInput> rawInputs)
    {
        _bindings[semanticInput] = rawInputs;
    }

    /// <summary>
    /// Reset all bindings to predefined defaults
    /// </summary>
    public void ResetBindings()
    {
        _bindings = new Dictionary<SemanticInput, List<RawInput>>
        {
            { SemanticInput.MoveUp, [RawInput.K_W, RawInput.K_UP] },
            { SemanticInput.MoveDown, [RawInput.K_S, RawInput.K_DOWN] },
            { SemanticInput.MoveRight, [RawInput.K_D, RawInput.K_RIGHT] },
            { SemanticInput.MoveLeft, [RawInput.K_A, RawInput.K_LEFT] },

            { SemanticInput.Select, [RawInput.MOUSE_LEFT] }
        };
    }

    /// <summary>
    /// Used to generate a GameInput object
    /// </summary>
    /// <param name="presentationRect">The visible game area</param>
    /// <returns>GameInput object</returns>
    public GameInput Generate(Rectangle presentationRect)
    {
        return new GameInput(

            moveUp: GetSemanticState(SemanticInput.MoveUp),
            moveDown: GetSemanticState(SemanticInput.MoveDown),
            moveRight: GetSemanticState(SemanticInput.MoveRight),
            moveLeft: GetSemanticState(SemanticInput.MoveLeft),

            select: GetSemanticState(SemanticInput.Select),

            mousePos: GetVirtualMousePosition(presentationRect)
        );
    }
}

/// <summary>
/// Used to represent semantic game input. Designed to be passed down and read
/// </summary>
public readonly struct GameInput(

    PressState moveUp,
    PressState moveDown,
    PressState moveRight,
    PressState moveLeft,

    PressState select,

    Point mousePos
    )
{
    public PressState MoveUp { get; } = moveUp;
    public PressState MoveDown { get; } = moveDown;
    public PressState MoveRight { get; } = moveRight;
    public PressState MoveLeft { get; } = moveLeft;

    public PressState Select { get; } = select;

    public Point MousePos { get; } = mousePos;
}