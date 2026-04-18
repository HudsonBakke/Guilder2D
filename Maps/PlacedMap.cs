using Microsoft.Xna.Framework.Graphics;

namespace Guilder2D;

public class PlacedMap : Map
{
    public PlacedMap()
    {
        _objects = [];
    }

    public override void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        foreach (MapObject obj in _objects)
        {
            obj.Draw(spriteBatch, camera);
        }
    }
}