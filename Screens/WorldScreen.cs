using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public class WorldScreen : Screen
{
    private readonly Player _player;
    // private readonly Texture2D _mapTexture;
    private readonly Map _map;
    private readonly Camera _camera;

    public WorldScreen(ContentManager content, Player player)
    {
        // _mapTexture = content.Load<Texture2D>("test_map");
        _player = player;
        _camera = new Camera();

        // FOR TESTING
        _map = MapLoader.LoadMap(content, "Maps/PlacedMaps/test_map.json");
    }

    public override void SendInput(GameInput gameInput)
    {
        _gameInput = gameInput;
    }

    public override void Update(GameTime gameTime)
    {
        _player.SendInput(_gameInput);
        _player.Update(gameTime, _map);
        _camera.Follow(
            _player.Center,
            Guilder2D.VirtualWidth,
            Guilder2D.VirtualHeight,
            _map.WidthInPixels,
            _map.HeightInPixels
        );
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        /* Vector2 mapScreenPos = _camera.WorldToScreen(Vector2.Zero);

        spriteBatch.Draw(
            texture: _mapTexture,
            position: mapScreenPos,
            sourceRectangle: new Rectangle(
                0,
                0,
                _mapTexture.Width,
                _mapTexture.Height
            ),
            color: Color.White
        );*/

        _map.Draw(spriteBatch, _camera);
        _player.Draw(spriteBatch, _camera);

        spriteBatch.End();
    }
}