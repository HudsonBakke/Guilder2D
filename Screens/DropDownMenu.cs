using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public class DropDownMenu : Screen
{
    private readonly List<MenuButton> _buttons;
    private readonly Vector2 _pos;

    public DropDownMenu(List<MenuButton> buttons, Vector2 pos)
    {
        _buttons = [.. buttons];
        _pos = pos;
    }

    /// <summary>
    /// A list representing the WasClicked state of every button in the menu.
    /// It is the caller's job to keep track of what each button should do
    /// (I could not think of a good way to have the MenuButton or this class own onclick changes)
    /// </summary>
    public List<bool> ButtonClickedStates { get; private set; }

    public override void SendInput(GameInput gameInput)
    {
        _gameInput = gameInput;
    }

    public override void Update(GameTime gameTime)
    {
        throw new System.NotImplementedException();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        throw new System.NotImplementedException();
    }
}