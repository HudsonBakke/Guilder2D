using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public class WorldScreen : Screen
{
    private readonly Player _player;
    private readonly Map _map;
    private readonly Camera _camera;

    public WorldScreen(AssetManager assets)
    {
        _player = new Player(assets);
        _camera = new Camera();

        // FOR TESTING
        _map = MapLoader.LoadMap(assets, "Maps/PlacedMaps/test_map.json");

        _map.Entities.Spawn(_player);
        _map.Entities.Spawn(new TestEnemy(assets));
    }

    public override void SendInput(GameInput gameInput)
    {
        _gameInput = gameInput;
    }

    public override void Update(GameTime gameTime)
    {
        _player.SendInput(_gameInput);
        _map.UpdateEntities(gameTime, _player, _gameInput.MousePos, _gameInput.Select == PressState.Held, false);
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