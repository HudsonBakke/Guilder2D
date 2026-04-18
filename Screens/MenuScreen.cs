using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public class MenuScreen : Screen
{
    private readonly MenuButton _startGameButton;
    private readonly MenuButton _exitGameButton;
    private readonly Texture2D _backgroundTexture;

    public MenuScreen(ContentManager content)
    {
        _startGameButton = new(
            content.Load<Texture2D>("start_game_button"),
            new Vector2(100,100)
        );
        _exitGameButton = new(
            content.Load<Texture2D>("exit_game_button"),
            new Vector2(100,220)
        );
        _backgroundTexture = content.Load<Texture2D>("menu_background");
    }

    public override void SendInput(GameInput gameInput)
    {
        _gameInput = gameInput;
    }

    public override void Update(GameTime gameTime)
    {
        if (_startGameButton.IsInside(_gameInput.MousePos)
            && _gameInput.Select == PressState.Released)
        {
            _startGameButton.Press();
        }

        else if (_exitGameButton.IsInside(_gameInput.MousePos)
            && _gameInput.Select == PressState.Released)
        {
            _exitGameButton.Press();
        }

        if (_startGameButton.Pressed)
        {
            NextScreen = ScreenSwitch.NewWorld;
        }

        else if (_exitGameButton.Pressed)
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
        
        _startGameButton.Draw(spriteBatch, _startGameButton.IsInside(_gameInput.MousePos));
        _exitGameButton.Draw(spriteBatch, _exitGameButton.IsInside(_gameInput.MousePos));

        spriteBatch.End();
    }

    private class MenuButton
    {
        private Texture2D _texture;
        private Vector2 _pos;

        public MenuButton(Texture2D texture, Vector2 pos)
        {
            _texture = texture;
            _pos = pos;
        }
        
        public bool Pressed { get; private set; }

        public bool IsInside(Point mousePos)
        {
            return new Rectangle(
                (int)_pos.X,
                (int)_pos.Y,
                _texture.Width,
                _texture.Height
            ).Contains(mousePos);
        }

        public void Press()
        {
            Pressed = true;
        }

        public void Draw(SpriteBatch spriteBatch, bool hovered = false)
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
                color: hovered
                       ? Color.Gray
                       : Color.White
            );
        }
    }
}