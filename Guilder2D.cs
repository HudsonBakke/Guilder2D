/******************************************************************************
 * NAME: Hudson Bakke
 * FILE: Guilder2D.cs
 * DATE: 4/3/2026
 * DESC: This is the main driver code for Guilder2D. It is based on the
 *       code that MonoGame gives you when you start a new project.
 ******************************************************************************/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

/// <summary>
/// This is the main driver class for the whole game. Implementation details are handled by MonoGame.
/// </summary>
public class Guilder2D : Game
{
    public const int VirtualWidth = 480;
    public const int VirtualHeight = 270;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private ScreenManager _screenManager;
    private InputManager _inputManager;
    private RenderTarget2D _sceneTarget;

    private Rectangle GetPresentationRectangle()
    {
        int backBufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
        int backBufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
        
        float scaleX = (float)backBufferWidth / VirtualWidth;
        float scaleY = (float)backBufferHeight / VirtualHeight;
        float overallScale = MathF.Min(scaleX, scaleY);

        int width = (int)(VirtualWidth * overallScale);
        int height = (int)(VirtualHeight * overallScale);

        int x = (backBufferWidth - width) / 2;
        int y = (backBufferHeight - height) / 2;

        return new Rectangle(x, y, width, height);
    }

    public Guilder2D()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;

        
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new(GraphicsDevice);
        _sceneTarget = new(
            GraphicsDevice,
            VirtualWidth,
            VirtualHeight
        );

        _screenManager = new(GraphicsDevice, Content);
        _inputManager = new();
    }

    protected override void Update(GameTime gameTime)
    {
        _inputManager.Update();
        _screenManager.SendInput(_inputManager.Generate(GetPresentationRectangle()));
        _screenManager.Update(gameTime);

        if (!_screenManager.IsGameRunning)
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Draw to virtual screen
        GraphicsDevice.SetRenderTarget(_sceneTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _screenManager.Draw(_spriteBatch); 

        // Draw to physical screen
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(
            _sceneTarget,
            GetPresentationRectangle(),
            Color.White
        );
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
