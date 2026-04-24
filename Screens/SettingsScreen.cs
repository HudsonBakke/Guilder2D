using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public class SettingsScreen : Screen
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly MenuButton _changeRes;
    private readonly ContentManager _content;

    private void UpdateResolution(int newWidth, int newHeight)
    {
        _graphics.PreferredBackBufferWidth = newWidth;
        _graphics.PreferredBackBufferHeight = newHeight;
        _graphics.ApplyChanges();
    }

    public SettingsScreen(GraphicsDeviceManager graphics, ContentManager content)
    {
        _graphics = graphics;
        _content = content;

        _changeRes = new(
            _content.Load<Texture2D>("change_resolution_button"),
            new Vector2(0, 0) // Will update this later once I have the screen layout decided
        );
    }

    public override void SendInput(GameInput gameInput)
    {
        _gameInput = gameInput;
    }

    public override void Update(GameTime gameTime)
    {
        _changeRes.SendInput(_gameInput);
        _changeRes.Update();

        if (_changeRes.WasClicked)
        {
            SubScreen = new DropDownMenu(
                [
                    new MenuButton(
                        _content.Load<Texture2D>("720x1280_res"),
                        new Vector2(0,0)
                    ),
                    new MenuButton(
                        _content.Load<Texture2D>("1080x1920_res"),
                        new Vector2(0,20)
                    ),
                    new MenuButton(
                        _content.Load<Texture2D>("1440x2560_res"),
                        new Vector2(0,40)
                    )
                ],
                new Vector2(
                    _gameInput.MousePos.X,
                    _gameInput.MousePos.Y
                )
            );

            NextScreen = ScreenSwitch.Sub;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        throw new System.NotImplementedException();
    }
}