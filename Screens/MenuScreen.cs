using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

/// <summary>
/// This class represents menu buttons
/// </summary>
public class MenuButton
{
    private Texture2D _texture;
    private Vector2 _pos;
    private GameInput _input;
    private bool _armed = false;

    private bool IsMouseHovering => new Rectangle(
            (int)_pos.X,
            (int)_pos.Y,
            _texture.Width,
            _texture.Height
        ).Contains(_input.MousePos);

    public MenuButton(Texture2D texture, Vector2 pos)
    {
        _texture = texture;
        _pos = pos;
    }

    /// <summary>
    /// Returns true if the button has been clicked
    /// </summary>
    public bool WasClicked { get; private set; }

    /// <summary>
    /// Reset WasClicked to false
    /// </summary>
    public void Reset()
    {
        WasClicked = false;
    }

    /// <summary>
    /// Send input to the button
    /// </summary>
    /// <param name="input">GameInput object</param>
    public void SendInput(GameInput input)
    {
        _input = input;
    }

    /// <summary>
    /// Update the button's internal state
    /// </summary>
    public void Update()
    {
        WasClicked = false;

        if (IsMouseHovering && _input.Select == PressState.Pressed)
        {
            _armed = true;
        }

        if (_input.Select == PressState.Released)
        {
            if (_armed && IsMouseHovering)
            {
                WasClicked = true;
            }

            _armed = false;
        }

        if (_input.Select == PressState.Up && !IsMouseHovering)
        {
            _armed = false;
        }
    }

    /// <summary>
    /// Draw the button to the screen
    /// </summary>
    /// <param name="spriteBatch">MonoGame spriteBatch object</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            texture: _texture,
            position: _pos,
            sourceRectangle: new(
                0,
                0,
                _texture.Width,
                _texture.Height
            ),
            color: IsMouseHovering
                ? Color.Gray
                : Color.White
        );
    }
}

public class MenuScreen : Screen
{
    private readonly MenuButton _startGameButton;
    private readonly MenuButton _settingsButton;
    private readonly MenuButton _exitGameButton;
    private readonly Texture2D _backgroundTexture;

    public MenuScreen(ContentManager content)
    {
        _startGameButton = new(
            content.Load<Texture2D>("start_game_button"),
            new Vector2(150,50)
        );

        _settingsButton = new(
            content.Load<Texture2D>("exit_game_button"), // I have not made a unique texture for the settings button yet
            new Vector2(150,120)
        );

        _exitGameButton = new(
            content.Load<Texture2D>("exit_game_button"),
            new Vector2(150,190)
        );

        _backgroundTexture = content.Load<Texture2D>("menu_background");
    }

    public override void SendInput(GameInput gameInput)
    {
        _gameInput = gameInput;
    }

    public override void Update(GameTime gameTime)
    {
        _startGameButton.SendInput(_gameInput);
        _startGameButton.Update();

        _settingsButton.SendInput(_gameInput);
        _settingsButton.Update();

        _exitGameButton.SendInput(_gameInput);
        _exitGameButton.Update();

        if (_startGameButton.WasClicked)
        {
            NextScreen = ScreenSwitch.NewWorld;
        }

        else if (_settingsButton.WasClicked)
        {
            NextScreen = ScreenSwitch.Settings;
        }

        else if (_exitGameButton.WasClicked)
        {
            NextScreen = ScreenSwitch.Exit;
        }
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        spriteBatch.Draw(
            texture: _backgroundTexture,
            position: Vector2.Zero,
            sourceRectangle: new(
                0,
                0,
                Guilder2D.VirtualWidth,
                Guilder2D.VirtualHeight
            ),
            color: Color.White
        );
        
        _startGameButton.Draw(spriteBatch);
        _settingsButton.Draw(spriteBatch);
        _exitGameButton.Draw(spriteBatch);

        spriteBatch.End();
    }
}